using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public delegate void OnDamageEventHandler(GameObject source, int damage);
    public event OnDamageEventHandler OnDamaged;

    public void Damage(GameObject from, int damage) {
        EventArgs args = new EventArgs();
        OnDamaged?.Invoke(from, damage);
    }

}
