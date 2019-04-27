using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {

    private GameObject source;

    public Event(GameObject source) {
        this.source = source;
    }

    public GameObject Source { get => source; }
}
