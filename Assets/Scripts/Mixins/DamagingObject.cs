using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds a damage component to a GameObject which applies DamagePerHit to any Damageable that collides with the GameObject.
/// </summary>
public class DamagingObject : MonoBehaviour
{
    // Start is called before the first frame update
    public int DamagePerHit = 1;

    void OnCollisionEnter(Collision collision) {
        // Make sure we only publish events if the collision is with a Damageable. 
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable != null) {
            damageable.Damage(gameObject, DamagePerHit);
        }
    }
}
