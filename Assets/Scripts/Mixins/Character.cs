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

    private CharacterScriptableObject _character;
    private Animator _animator;
    private Rigidbody _rb;

    private GameObject _characterContainer;
    private GameObject _currentPlanet;
    private List<GameObject> _planetsInScene;    // TODO this needs to move out into a shared registry
    private bool _alive = true;
    private Weapon _currentWeapon;
    private GameObject _weaponSlot;
    private int _currentHealth;


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
        
        // Get the player character data out of the gamestate
        _character = GameStateManager.Instance.Gamestate.playerCharacter;

        // Set up the character container and spawn the character object
        _characterContainer = Instantiate(new GameObject(), parent: transform, position: transform.TransformPoint(Vector3.zero), rotation: Quaternion.identity);
        _characterContainer.name = "CharacterContainer";
        GameObject characterObjectInstance = Instantiate(_character.characterModel, parent: _characterContainer.transform, position: _characterContainer.transform.TransformPoint(Vector3.zero), rotation: Quaternion.identity);
        characterObjectInstance.name = "Character";

        // Change the weapon to the default weapon
        ChangeWeapon(_character.defaultWeapon);

        // Get the rest of the compnoents that we might need
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();

        // Find all the planets in the scene so we can decide which one we're standing on
        _planetsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        _currentPlanet = _planetsInScene[0];

        Debug.DrawRay(transform.position, transform.forward*100, Color.red);
        Debug.DrawRay(transform.position, _characterContainer.transform.forward*100, Color.green);


    }

    public void ChangeWeapon(Weapon weapon) {

        // Destroy the existing physical weapon
        if (_weaponSlot != null) {
            Destroy(_weaponSlot);
        }

        // Create the game object that physically represents the weapon
        Vector3 weaponSlotOffset = _characterContainer.transform.TransformPoint(new Vector3(.05f, .1f, .01f));
        _weaponSlot = Instantiate(weapon.WeaponObject, parent: _characterContainer.transform, position: weaponSlotOffset, rotation: Quaternion.identity);
        _weaponSlot.name = "Weapon";

        // Update the current weapon data
        this._currentWeapon = weapon;
    }


    public void MoveDirection(Vector3 moveDirection) {
        // Set the target position so the player is correctly moved when the physics engine ticks
        // Note the Time.deltaTime call which returns the time since call to Update(). This ensures our players movement is adjusted for the irregular intervals of the Update() call.
        if (moveDirection != Vector3.zero) {
            _rb.MovePosition(transform.position + _character.moveSpeed * Time.deltaTime * transform.TransformDirection(moveDirection));
        }

        // Update the animator
        _animator.SetBool("Moving", moveDirection != Vector3.zero);
    }

    public void Rotate(float localAngle) {
        _characterContainer.transform.localRotation = Quaternion.Euler(0, localAngle, 0);
    }

    public void UseWeapon() {

        // Spawn the projectile
        Vector3 projectileSpawnPosition = _characterContainer.transform.TransformPoint(new Vector3(0, .2f, .3f));
        GameObject projectileInstance = Instantiate(_currentWeapon.ProjectileObject, projectileSpawnPosition, _characterContainer.transform.rotation);

        // Modify the values of the OrbitalProjectile component
        // TODO is there a better way to get access to components when we know they will be there
        // Perhaps this would be more efficient with a factory
        // Perhaps it would be better to manually add the OrbitalProjectile mixin via a prepopulated component
        OrbitalProjectile orbitalProjectile = projectileInstance.GetComponent<OrbitalProjectile>();
        orbitalProjectile.Target = _currentPlanet;
        orbitalProjectile.Axis = _characterContainer.transform.right;    // Orbit around the players right axis -> shoot from the front and orbit the target using the player's forward as the origin.
        orbitalProjectile.Lifetime = _currentWeapon.Lifetime;
        orbitalProjectile.ProjectileVelocity = _currentWeapon.ProjectileVelocity;

    }


    protected virtual void OnDamaged(object eventArgs) {
        // Ignore any damage events that might come in while the character is dead and still in the game world
        if (_alive == false) {
            return;
        }

        var damageEvent = (DamageEvent)eventArgs;
        var previousHealth = _currentHealth;
        _currentHealth -= damageEvent.Damage;
        if (_currentHealth <= 0) {
            _alive = false;
            OnDeath();
        }

        // Let the world know that this character's health changed
        EventBus.Publish(new CharacterHealthChangeEvent(gameObject, gameObject, previousHealth, _currentHealth));
    }

    protected virtual void OnDeath() {
        _alive = false;

        // Turn off the collider
        GetComponent<Collider>().enabled = false;

        // Find the renderers associated with this object and change their shaders to the death shader
        List<Renderer> renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        foreach (Renderer renderer in renderers) {
            renderer.material.shader = _character.deathShader;
            renderer.material.SetFloat("_StartTime", Time.timeSinceLevelLoad);
        }

        // Destroy the object 2 seconds later
        GameObject.Destroy(gameObject, .8f);

        // TODO publish an on death event
        // Perhaps move to OnDamaged so no one can opt out of publishing the event by overriding OnDeath()
    }
}
