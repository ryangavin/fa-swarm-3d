using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Character : MonoBehaviour {

    public int Health = 3;

    // Register listeners
    void OnEnable() {
        EventBus.Register<DamageEvent>(OnDamaged, null, gameObject);
    }

    void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamaged, null, gameObject);
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
}
