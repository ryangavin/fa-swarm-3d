using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public delegate void OnDamageEventHandler(DamageEvent eventArgs);
    public event OnDamageEventHandler OnDamaged;

    public void Damage(GameObject from, int damage) {
        DamageEvent eventArgs = new DamageEvent(from, this, damage);
        OnDamaged?.Invoke(eventArgs);
    }

}
