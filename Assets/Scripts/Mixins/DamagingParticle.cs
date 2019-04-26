using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// Assigns a damage value to a particle system which will be applied on collision to any Damageables.
/// </summary>
/// <see cref="Damageable"/>
public class DamagingParticle : MonoBehaviour {

    public int DamagePerHit = 1;

    void OnParticleCollision(GameObject other) {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null) {

            // TODO It is possible that multiple particles can hit in the frame
            // TODO possibly pass along hit velocity and trajectory
            damageable.Damage(gameObject, DamagePerHit);
        }
    }
}
