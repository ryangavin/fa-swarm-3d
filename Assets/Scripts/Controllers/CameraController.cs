using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 position = target.transform.position * 2f;
        transform.position = position;
        
    }
}
