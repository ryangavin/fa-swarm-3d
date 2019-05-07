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
    void Update() {
        var cameraTargetLocalPosition = new Vector3(0, cameraHeight, -cameraDistance);
        transform.position = _target.transform.TransformPoint(cameraTargetLocalPosition);
        
        transform.LookAt(_target.transform, _target.transform.up);
        // Calculate the angle to look down
        //var angle = Mathf.Atan2(cameraDistance, cameraHeight) * Mathf.Rad2Deg;
        //transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
        
    }
}
