using System;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour{

    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private void Update() {
        var cameraRotation = _camera.transform.rotation;
        transform.LookAt(transform.position + cameraRotation * Vector3.back,
            cameraRotation * Vector3.down);
    }
}