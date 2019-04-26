using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : Event {

    private GameObject damageFrom;
    private Damageable damageable;
    private int damage;

    public DamageEvent(GameObject damageFrom, Damageable damageable, int damage) {
        this.damageFrom = damageFrom;
        this.damageable = damageable;
        this.damage = damage;
    }

    public GameObject DamageFrom { get => damageFrom;}
    public Damageable Damageable { get => damageable;}
    public int Damage { get => damage;}
}
