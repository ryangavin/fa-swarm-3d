using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {

    public readonly GameObject source;  // The generator of this event, never null
    public readonly GameObject target; // Some events may pertain to specific target, can be null

    public Event(GameObject source, GameObject target) {
        this.source = source;
        this.target = target;
    }
}
