using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

    public float AttractorMass = PhyiscalConstants.MassOfEarth;

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

    void FixedUpdate() {
        // Iterate over each game object
        // Create a new list to iterate over because attractables might get updated by another call to Update() during iteration
        foreach (GameObject attractable in new List<GameObject>(attractables)) {
            // Check if they should be removed from the set because the object doesn't exist anymore
            if (attractable == null) {
                attractables.Remove(attractable);
                continue;
            }

            // Get the rb
            Rigidbody attractableRb = attractable.GetComponent<Rigidbody>();

            // Apply the attraction
            Vector3 gravityUp = (attractable.transform.position - transform.position).normalized;
            Vector3 attractableUp = attractable.transform.up;
            float distance = Vector3.Distance(transform.position, attractable.transform.position) / 1000;   // Convert to meters

            // Actual gravitation forumula taking into account the masses of the objects and their distances
            float forceToApply = (PhyiscalConstants.G * attractableRb.mass * AttractorMass) / Mathf.Pow(distance, 2);

            // Using force mode Acceleration, we can simulate adding the force in newtons to the rigidbody
            attractableRb.AddForce(-gravityUp * forceToApply, mode: ForceMode.Acceleration);

            Quaternion targetRotation = Quaternion.FromToRotation(attractableUp, gravityUp) * attractable.transform.rotation;
            attractable.transform.rotation = Quaternion.Slerp(attractable.transform.rotation, targetRotation, 50f * Time.fixedDeltaTime);
        }
    }
}
