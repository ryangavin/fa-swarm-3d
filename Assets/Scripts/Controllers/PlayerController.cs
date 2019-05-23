using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour {

    private float _targetRotation;
    private Camera _camera;
    private Character _character;
    
    void Start() {
        _camera = Camera.main;
        _character = GetComponent<Character>();
    }

    // Update is called once per frame.
    // It is important to do any calculations on user input here so they are processed ASAP.
    // Any updates that need to reflect in the world should be set up here, so they can be used by FixedUpdate().
    // Do NOT update position of any physics bodies in this method as they may be repositioned when FixedUpdate() runs.
    void Update() {

        var leftRightInput = Input.GetAxisRaw("Horizontal");
        var upDownInput = Input.GetAxisRaw("Vertical");
        _character.MoveDirection(new Vector3(leftRightInput, 0, upDownInput).normalized);

        // Figure out which way the character should face based on the relative position of the mouse
        var transformCache = transform;
        var ray = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
        var plane = new Plane(transformCache.up, transformCache.position);
        if (plane.Raycast(ray, out var distance)) {
            var target = ray.GetPoint(distance);
            var direction = transform.InverseTransformPoint(target);
            _character.Rotate(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
        }
        
        // Check for a trigger click and fire the weapon
        if (Input.GetMouseButtonDown(0)) {
            _character.SetFiring(true); // TODO why is this expensive
        }
        if (Input.GetMouseButtonUp(0)) {
            _character.SetFiring(false);
        }
    }
}
