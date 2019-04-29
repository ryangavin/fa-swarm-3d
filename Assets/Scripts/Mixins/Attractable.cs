using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour {
    void Start() {
        EventBus.Publish(new AttractableStartEvent(gameObject));
    }
}
