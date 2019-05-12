using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Attractor : MonoBehaviour {

    public float attractorMass = PhyiscalConstants.MassOfEarth;

    private HashSet<GameObject> attractables = new HashSet<GameObject>();

    void OnEnable() {
        EventBus.Register<AttractableStartEvent>(OnAttractableStart, global: true);
    }

    void OnDisable() {
        EventBus.DeRegister<AttractableStartEvent>(OnAttractableStart);
    }

    void OnAttractableStart(object eventArgs) {
        AttractableStartEvent attractableStartEvent = (AttractableStartEvent)eventArgs;
        attractables.Add(attractableStartEvent.source);
    }

    private void FixedUpdate() {
        // Iterate over each game object
        // Create a new list to iterate over because attractables might get updated by another call to Update() during iteration
        foreach (GameObject attractable in new List<GameObject>(attractables)) {
            // Check if they should be removed from the set because the object doesn't exist anymore
            if (attractable == null) {
                attractables.Remove(attractable);
                continue;
            }

            // Get the rb
            var attractableRb = attractable.GetComponent<Rigidbody>();

            // Apply the attraction
            var gravityUp = (attractable.transform.position - transform.position).normalized;
            var attractableUp = attractable.transform.up;

            // Using force mode Acceleration, we can simulate adding the force in newtons to the rigidbody
            //attractableRb.AddForce(-gravityUp * forceToApply, mode: ForceMode.Acceleration);
            attractableRb.AddForce(-gravityUp * 20, ForceMode.Acceleration);

            var currentRotation = attractable.transform.rotation;
            var targetRotation = Quaternion.FromToRotation(attractableUp, gravityUp) * currentRotation;
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, 3);
            attractable.transform.rotation = currentRotation;
        }
    }
}
