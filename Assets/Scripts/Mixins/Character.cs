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
[RequireComponent(typeof(Damageable), typeof(Attractable), typeof(BoxCollider))]
public class Character : MonoBehaviour {

    private static readonly int AnimatorMoving = Animator.StringToHash("Moving");

    private CharacterData _characterData;
    private CharacterModel _characterModel;
    private Rigidbody _rb;
    private GameObject _modelContainer;
    private BoxCollider _boxCollider;
    
    private GameObject _currentPlanet;
    private List<GameObject> _planetsInScene;    // TODO this needs to move out into a shared registry
    
    private Weapon _currentWeapon;
    private GameObject _weaponSlot;

    private Vector3 _targetDirection;
    
    public int CurrentHealth { private set; get; }
    public bool Alive { private set; get; }

    // Register listeners
    private void OnEnable() {
        EventBus.Register<DamageEvent>(OnDamaged, null, gameObject);
    }

    private void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamaged, null, gameObject);
    }

    private void FixedUpdate() {
        if (_targetDirection == Vector3.zero) return;
        
        // convert the target direction to a world space vector
        var t = transform;
        var worldDirection = t.TransformDirection(_targetDirection);
            
        _rb.MovePosition(t.position + _characterData.moveSpeed * Time.fixedDeltaTime * worldDirection);
    }

    public static GameObject Spawn(CharacterData characterData, GameObject container, Vector3 position) {
        var parentContainer = Instantiate(container);
        var characterScript = parentContainer.GetComponent<Character>();
        characterScript._characterData = characterData;
        
        // Move the parent container to the spawn point
        parentContainer.transform.position = position;
        
        // Reset some stuff
        // TODO evaluate this
        characterScript.CurrentHealth = characterData.health;
        characterScript.Alive = true;
        
        // Set up the model container which holds all the game objects related to rendering the character and weapon models
        characterScript._modelContainer = new GameObject("ModelContainer");
        characterScript._modelContainer.transform.parent = parentContainer.transform;
        characterScript._modelContainer.transform.position = parentContainer.transform.TransformPoint(Vector3.zero);

        // Set up the character model and maintain the reference to the CharacterModel script
        var characterModel = Instantiate(characterData.characterModel, parent: characterScript._modelContainer.transform, position: characterScript._modelContainer.transform.TransformPoint(Vector3.zero), rotation: Quaternion.identity);
        characterModel.name = "CharacterModel";
        characterScript._characterModel = characterModel.GetComponent<CharacterModel>();

        // Change the weapon to the default weapon
        // This also creates the "Weapon" container inside the "CharacterContainer"
        characterScript.ChangeWeapon(characterData.defaultWeapon);

        // Get the rest of the components that we might need
        characterScript._rb = parentContainer.GetComponent<Rigidbody>();
        characterScript._boxCollider = parentContainer.GetComponent<BoxCollider>();

        // Update the size of the box collider so it encapsulates the character model
        foreach (var renderer in characterModel.GetComponentsInChildren<Renderer>()) {
            if (!renderer.gameObject.CompareTag(Tags.CharacterModel)) continue;
            var bounds = renderer.bounds;
            characterScript._boxCollider.center = bounds.center - parentContainer.transform.position;
            characterScript._boxCollider.size = bounds.size;
            break;
        }

        // Update the current planet of the character
        // TODO Handle more than one planet in the scene
        // TODO Probably set the planet to the nearest planet based on the spawn point
        if (PlanetManager.Instance && PlanetManager.Instance.PlanetsInScene.Count > 0) {
            characterScript._currentPlanet = PlanetManager.Instance.PlanetsInScene[0];
        }

        return parentContainer;
    }

    public void ChangeWeapon(Weapon weapon) {
        if (!weapon) {
            return;
        }

        // Destroy the existing physical weapon
        if (_weaponSlot) {
            Destroy(_weaponSlot);
        }

        // Create the game object that physically represents the weapon
        var weaponSlotOffset = _modelContainer.transform.TransformPoint(new Vector3(.05f, .1f, .01f));
        _weaponSlot = Instantiate(weapon.WeaponObject, parent: _modelContainer.transform, position: weaponSlotOffset, rotation: Quaternion.identity);
        _weaponSlot.name = "WeaponModel";

        // Update the current weapon data
        _currentWeapon = weapon;
    }


    public void MoveDirection(Vector3 moveDirection) {
        // Set the target direction so the player is correctly moved when the physics engine ticks
        // Note the Time.deltaTime call which returns the time since call to Update().
        // This ensures our players movement is adjusted for the irregular intervals of the Update() call.
        _targetDirection = moveDirection;

        // Update the animator
        _characterModel.animator.SetBool(AnimatorMoving, moveDirection != Vector3.zero);
    }

    public void Rotate(float localAngle) {
        _modelContainer.transform.localRotation = Quaternion.Euler(0, localAngle, 0);
    }

    public void UseWeapon() {

        // Spawn the projectile
        var projectileSpawnPosition = _modelContainer.transform.TransformPoint(new Vector3(0, .2f, .3f));
        var projectileInstance = Instantiate(_currentWeapon.ProjectileObject, projectileSpawnPosition, _modelContainer.transform.rotation);

        // Modify the values of the OrbitalProjectile component
        // Perhaps this would be more efficient with a factory
        var orbitalProjectile = projectileInstance.GetComponent<OrbitalProjectile>();
        orbitalProjectile.Target = _currentPlanet;
        orbitalProjectile.Axis = _modelContainer.transform.right;    // Orbit around the players right axis -> shoot from the front and orbit the target using the player's forward as the origin.
        orbitalProjectile.Lifetime = _currentWeapon.Lifetime;
        orbitalProjectile.ProjectileVelocity = _currentWeapon.ProjectileVelocity;
        orbitalProjectile.ImpactFoce = _currentWeapon.ImpactVeloctiy;

    }


    private void OnDamaged(object eventArgs) {
        // Ignore any damage events that might come in while the character is dead and still in the game world
        if (Alive == false) {
            return;
        }

        var damageEvent = (DamageEvent)eventArgs;
        var previousHealth = CurrentHealth;
        CurrentHealth -= damageEvent.Damage;
        if (CurrentHealth <= 0) {
            OnDeath();
        }

        // Let the world know that this character's health changed
        var g = gameObject;
        EventBus.Publish(new CharacterHealthChangeEvent(g, g, previousHealth, CurrentHealth));
    }

    private void OnDeath() {
        Debug.Log("Character OnDeath()");
        // Mark this character as not alive
        Alive = false;

        // Turn off the collider
        GetComponent<Collider>().enabled = false;

        // Find the renderers associated with this object and change their shaders to the death shader
        var renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        foreach (var r in renderers) {
            r.material.shader = _characterData.deathShader;
            r.material.SetFloat("_StartTime", Time.timeSinceLevelLoad);
        }

        // Destroy the object later
        Destroy(gameObject, .8f);
        
        // TODO Perhaps move to OnDamaged so no one can opt out of publishing the event by overriding OnDeath()
        var g = gameObject;
        EventBus.Publish(new DeathEvent(g, g));
    }
}
