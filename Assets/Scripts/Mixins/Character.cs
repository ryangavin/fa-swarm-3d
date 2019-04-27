using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Character : MonoBehaviour {

    public int Health = 3;

    private Damageable damageable;

    // Register listeners
    void OnEnable() {
        damageable = GetComponent<Damageable>();
        damageable.OnDamaged += OnDamaged;
    }

    void OnDisable() {
        damageable.OnDamaged -= OnDamaged;
    }

    protected virtual void OnDamaged(DamageEvent eventArgs) {
        Debug.Log("Hit from: " + eventArgs.Source + " for: " + eventArgs.Damage);
        Health -= eventArgs.Damage;
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
