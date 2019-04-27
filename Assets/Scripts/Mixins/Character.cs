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

    protected virtual void OnDamaged(GameObject from, int damage) {
        Debug.Log("Hit from: " + from + " for: " + damage);
        Health -= damage;
        if (Health <= 0) {
            OnDeath();
        }
    }

    protected virtual void OnDeath() {
        GameObject.Destroy(gameObject);
    }
}
