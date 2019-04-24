using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;    // Larger number means a smoother scroll
    public float distance = 15f;    // How far should the camera stay away from the target on the z axis

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z - distance);
        if (transform.position != targetPosition) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1/smoothing);
        }
    }
}
