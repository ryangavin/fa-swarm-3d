using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float cameraHeight;
    public float cameraDistance;
    
    private GameObject _target;

    private void Start() {
        _target = GameStateManager.Instance.gamestate.player;
    }

    // Update is called once per frame
    private void LateUpdate() {
        var cameraTargetLocalPosition = new Vector3(0, cameraHeight, -cameraDistance);
        transform.position = _target.transform.TransformPoint(cameraTargetLocalPosition);
        transform.LookAt(_target.transform, _target.transform.up);
    }
}
