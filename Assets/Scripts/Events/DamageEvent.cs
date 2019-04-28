using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : Event {

    private int damage;

    public DamageEvent(GameObject source, GameObject target, int damage) : base(source, target) {
        this.damage = damage;
    }

    public int Damage { get => damage;}
}
