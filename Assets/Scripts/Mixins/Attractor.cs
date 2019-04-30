using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

    // TODO move this to configuration somewhere
    private const float gravity = -9.81f;

    public float AttractorMass = 999999;

    private HashSet<GameObject> attractables = new HashSet<GameObject>();

    void OnEnable() {
        EventBus.Register<AttractableStartEvent>(OnAttractableStart, global: true);
    }

    void OnDisable() {
        EventBus.DeRegister<AttractableStartEvent>(OnAttractableStart);
    }

    void OnAttractableStart(object eventArgs) {
        AttractableStartEvent attractableStartEvent = (AttractableStartEvent)eventArgs;
        attractables.Add(attractableStartEvent.Source);
    }

    void FixedUpdate() {
        // Iterate over each game object
        // Create a new list to iterate over because attractables might get updated by another call to Update() during iteration
        foreach (GameObject attractable in new List<GameObject>(attractables)) {
            // Check if they should be removed from the set because the object doesn't exist anymore
            if (attractable.activeInHierarchy == false) {
                attractables.Remove(attractable);
            }

            // Get the rb
            Rigidbody attractableRb = attractable.GetComponent<Rigidbody>();

            // Apply the attraction
            Vector3 gravityUp = (attractable.transform.position - transform.position).normalized;
            Vector3 attractableUp = attractable.transform.up;

            attractableRb.AddForce(gravityUp * gravity * attractableRb.mass);

            Quaternion targetRotation = Quaternion.FromToRotation(attractableUp, gravityUp) * attractable.transform.rotation;
            attractable.transform.rotation = Quaternion.Slerp(attractable.transform.rotation, targetRotation, 50f * Time.fixedDeltaTime);
        }
    }
}
