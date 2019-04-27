using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : Event {

    private Damageable damageable;
    private int damage;

    public DamageEvent(GameObject source, Damageable damageable, int damage) : base(source) {
        this.damageable = damageable;
        this.damage = damage;
    }

    public Damageable Damageable { get => damageable;}
    public int Damage { get => damage;}
}
