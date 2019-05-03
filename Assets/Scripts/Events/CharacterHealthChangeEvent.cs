using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthChangeEvent : Event {

    public readonly int previousHealth;
    public readonly int currentHealth;

    public CharacterHealthChangeEvent(GameObject source, GameObject target, int previousHealth, int currentHealth) : base(source, target) {
        this.previousHealth = previousHealth;
        this.currentHealth = currentHealth;
    }
}
