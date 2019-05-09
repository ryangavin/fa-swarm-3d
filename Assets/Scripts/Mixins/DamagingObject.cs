using System.Linq;
using UnityEngine;

/// <summary>
/// Adds a damage component to a GameObject which applies DamagePerHit to any Damageable that collides with the GameObject.
/// </summary>
///
/// TODO Add filter 
public class DamagingObject : MonoBehaviour {
    // Start is called before the first frame update
    public int damagePerHit = 1;

    [TagSelector] public string[] tagFilter = { };

    void OnCollisionEnter(Collision collision) {
        // Check if the collision target is a damagable or a child of a damagable
        var damageable = collision.gameObject.GetComponentInParent<Damageable>();
        if (damageable == null) {
            return;
        }

        // Allow different implementations of this class to have a programatic filter
        if (!ShouldDamage()) {
            return;
        }

        // Tag filtering
        bool shouldDamage = !(tagFilter.Length > 0 && !tagFilter.Any(t => collision.gameObject.CompareTag(t)));

        if (!shouldDamage) {
            return;
        }

        // Apply the damage
        damageable.Damage(gameObject, damagePerHit);

        // Call the callback so any super classes can add functionality
        AfterDamage(collision.gameObject);
    }

    protected virtual bool ShouldDamage() {
        return true;
    }

    protected virtual void BeforeDamage(GameObject collision) { }

    protected virtual void AfterDamage(GameObject collision) { }
}