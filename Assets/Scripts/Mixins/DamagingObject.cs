using UnityEngine;

/// <summary>
/// Adds a damage component to a GameObject which applies DamagePerHit to any Damageable that collides with the GameObject.
/// </summary>
///
/// TODO Add filter 
public class DamagingObject : MonoBehaviour
{
    // Start is called before the first frame update
    public int damagePerHit = 1;

    void OnCollisionEnter(Collision collision) {
        // Check if the collision target is a damagable or a child of a damagable
        var damageable = collision.gameObject.GetComponentInParent<Damageable>();
        if (damageable == null) {
            return;
        }
        
        // Apply the damage
        damageable.Damage(gameObject, damagePerHit);

        // Call the callback so any super classes can add functionality
        AfterDamage(collision.gameObject);
    }
    
    protected virtual void BeforeDamage(GameObject collision) { }

    protected virtual void AfterDamage(GameObject collision) { }
}
