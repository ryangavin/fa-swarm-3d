using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Character class is the base mixin for any characters in the game.
/// 
/// Characters have the following attributes:
/// - Health
/// - Move Speed
/// 
/// Characters expose functionality to:
/// - Move
/// - Rotate
/// 
/// Examples of characters include:
/// - Main character
/// - Any enemies
/// - NPCs
/// </summary>
[RequireComponent(typeof(Damageable), typeof(Animator))]
public class Character : MonoBehaviour {

    public int Health = 3;
    public float MoveSpeed = 10f;
    public GameObject CharacterContainer;   // TODO figure out how to get this consistently and programatically
    public GameObject Bullet;
    public float BulletSpeed = 100f;

    private Animator animator;
    private Rigidbody rb;

    private float targetAngle;
    private GameObject currentPlanet;
    private List<GameObject> planetsInScene;


    // Register listeners
    void OnEnable() {
        EventBus.Register<DamageEvent>(OnDamaged, null, gameObject);
    }

    void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamaged, null, gameObject);
    }

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        planetsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));

    }


    public void MoveDirection(Vector3 moveDirection) {
        // Set the target position so the player is correctly moved when the physics engine ticks
        // Note the Time.deltaTime call which returns the time since call to Update(). This ensures our players movement is adjusted for the irregular intervals of the Update() call.
        if (moveDirection != Vector3.zero) {
            rb.MovePosition(transform.position + transform.TransformDirection(moveDirection) * MoveSpeed * Time.deltaTime);
        }

        // Update the animator
        animator.SetBool("Moving", moveDirection != Vector3.zero);
    }

    public void SetRotation(float localAngle) {
        targetAngle = localAngle;
    }

    public void UseWeapon() {
        Vector3 bulletPosition = new Vector3 {
            y = .1f,
            z = .2f,
        };
        bulletPosition = CharacterContainer.transform.TransformPoint(bulletPosition);
        GameObject bulletInstance = Instantiate(Bullet, bulletPosition, CharacterContainer.transform.rotation);
        Rigidbody bulletInstanceRb = bulletInstance.GetComponent<Rigidbody>();
        OrbitalProjectile bulletInstanceOrbitalProjectile = bulletInstance.GetComponent<OrbitalProjectile>();
        bulletInstanceOrbitalProjectile.Target = currentPlanet;
        //bulletInstanceRb.AddRelativeForce(new Vector3(0, 0, BulletSpeed), mode: ForceMode.VelocityChange);
    }


    protected virtual void OnDamaged(object eventArgs) {
        DamageEvent damageEvent = (DamageEvent)eventArgs;

        Debug.Log("Hit from: " + damageEvent.Source + " for: " + damageEvent.Damage);
        Health -= damageEvent.Damage;
        if (Health <= 0) {
            OnDeath();
        }
    }

    protected virtual void OnDeath() {
        GameObject.Destroy(gameObject);

        // TODO publish an on death event
        // Perhaps move to OnDamaged so no one can opt out of publishing the event by overriding OnDeath()
    }

    // Process any updates that will change something in the physical world
    // TODO I think maybe this should be moved to Update();
    // TODO It's beyond unclear whether or not this is the best practice, but it's the practice we're using for this project!
    void FixedUpdate() {
        // TODO it might be nice to remove the CharacterContainer
        CharacterContainer.transform.localRotation = Quaternion.Euler(0, targetAngle, 0);

    }

    void LateUpdate() {
        // Find the nearest planet and set that to our "current planet"
        // TODO replace this with a planet change event
        float nearestPlanetDistance = 0;
        GameObject nearestPlanet = null;
        foreach (GameObject planet in this.planetsInScene) {
            float distance = Vector3.Distance(transform.position, planet.transform.position);
            if (nearestPlanet == null || distance < nearestPlanetDistance) {
                nearestPlanet = planet;
            }
        }
        currentPlanet = nearestPlanet;
    }
}
