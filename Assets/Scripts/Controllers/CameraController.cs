using UnityEngine;

public class CameraController : MonoBehaviour {

    public float cameraHeight;
    public float cameraDistance;

    private GameObject _target;

    private void Start() {
        _target = GameStateManager.Instance.gamestate.playerGameObject;
    }

    // Update is called once per frame
    private void LateUpdate() {
        // Skip if the target is no longer in the scene
        if (!_target) return;
        
        var cameraTargetLocalPosition = new Vector3(0, cameraHeight, -cameraDistance);
        transform.position = _target.transform.TransformPoint(cameraTargetLocalPosition);
        transform.LookAt(_target.transform, _target.transform.up);
    }

    public void Shake(float seconds, float shakeAmount) {
        
    }
}
