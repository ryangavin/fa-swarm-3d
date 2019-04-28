using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {

    private GameObject source;  // The generator of this event, never null
    private GameObject target; // Some events may pertain to specific target, can be null

    public Event(GameObject source, GameObject target) {
        this.source = source;
        this.target = target;
    }

    public GameObject Source { get => source; }
    public GameObject Target { get => target; }
}
