using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractable : MonoBehaviour {

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        EventBus.Publish(new AttractableStartEvent(gameObject));
    }
}
