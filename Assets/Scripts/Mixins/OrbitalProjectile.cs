using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalProjectile : MonoBehaviour {

    public GameObject Target;

    // Update is called once per frame
    void Update() {
        Vector3 relativePos = Target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
        transform.Translate(0, 0, 3 * Time.deltaTime);
    }
}
