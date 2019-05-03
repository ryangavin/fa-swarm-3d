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
[RequireComponent(typeof(Damageable))]
public class Character : MonoBehaviour {

    public int Health = 3;
       
    // TODO split into a scriptable object
    public Shader DeathShader;
    public Weapon DefaultWeapon;
    public GameObject CharacterObject;
    public float MoveSpeed = 15f;

    private Animator animator;
    private Rigidbody rb;

    private GameObject characterContainer;
    private GameObject currentPlanet;
    private List<GameObject> planetsInScene;
    private bool alive = true;
    private Weapon currentWeapon;
    private GameObject weaponSlot;


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

        // Set up the character container and spawn the character object
        characterContainer = Instantiate(new GameObject(), parent: transform, position: transform.TransformPoint(Vector3.zero), rotation: Quaternion.identity);
        characterContainer.name = "CharacterContainer";
        GameObject characterObjectInstance = Instantiate(CharacterObject, parent: characterContainer.transform, position: characterContainer.transform.TransformPoint(Vector3.zero), rotation: Quaternion.identity);
        characterObjectInstance.name = "Character";

        // Change the weapon to the default weapon
        ChangeWeapon(DefaultWeapon);

        // Get the rest of the compnoents that we might need
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        // Find all the planets in the scene so we can decide which one we're standing on
        planetsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        currentPlanet = planetsInScene[0];

        Debug.DrawRay(transform.position, transform.forward*100, Color.red);
        Debug.DrawRay(transform.position, characterContainer.transform.forward*100, Color.green);


    }

    public void ChangeWeapon(Weapon weapon) {

        // Destroy the existing physical weapon
        if (weaponSlot != null) {
            Destroy(weaponSlot);
        }

        // Create the game object that physically represents the weapon
        Vector3 weaponSlotOffset = characterContainer.transform.TransformPoint(new Vector3(.05f, .1f, .01f));
        weaponSlot = Instantiate(weapon.WeaponObject, parent: characterContainer.transform, position: weaponSlotOffset, rotation: Quaternion.identity);
        weaponSlot.name = "Weapon";

        // Update the current weapon data
        this.currentWeapon = weapon;
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

    public void Rotate(float localAngle) {
        characterContainer.transform.localRotation = Quaternion.Euler(0, localAngle, 0);
    }

    public void UseWeapon() {

        // Spawn the projectile
        Vector3 projectileSpawnPosition = characterContainer.transform.TransformPoint(new Vector3(0, .2f, .3f));
        GameObject projectileInstance = Instantiate(currentWeapon.ProjectileObject, projectileSpawnPosition, characterContainer.transform.rotation);

        // Modify tthe values of the OrbitalProjectile component
        // TODO is there a better way to get access to components when we know they will be there
        OrbitalProjectile orbitalProjectile = projectileInstance.GetComponent<OrbitalProjectile>();
        orbitalProjectile.Target = currentPlanet;
        orbitalProjectile.Axis = characterContainer.transform.right;    // Orbit around the players right axis -> shoot from the front and orbit the target using the player's forward as the origin.
        orbitalProjectile.Lifetime = currentWeapon.Lifetime;
        orbitalProjectile.ProjectileVelocity = currentWeapon.ProjectileVelocity;

    }


    protected virtual void OnDamaged(object eventArgs) {
        // Ignore any damage events that might come in while the character is dead and still in the game world
        if (alive == false) {
            return;
        }

        DamageEvent damageEvent = (DamageEvent)eventArgs;
        int previousHealth = Health;
        Health -= damageEvent.Damage;
        if (Health <= 0) {
            alive = false;
            OnDeath();
        }

        // Let the world know that this character's health changed
        EventBus.Publish(new CharacterHealthChangeEvent(gameObject, gameObject, previousHealth, Health));
    }

    protected virtual void OnDeath() {
        alive = false;

        // Turn off the collider
        GetComponent<Collider>().enabled = false;

        // Find the renderers associated with this object and change their shaders to the death shader
        List<Renderer> renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        foreach (Renderer renderer in renderers) {
            renderer.material.shader = DeathShader;
            renderer.material.SetFloat("_StartTime", Time.timeSinceLevelLoad);
        }

        // Destroy the object 2 seconds later
        GameObject.Destroy(gameObject, .8f);

        // TODO publish an on death event
        // Perhaps move to OnDamaged so no one can opt out of publishing the event by overriding OnDeath()
    }
}
