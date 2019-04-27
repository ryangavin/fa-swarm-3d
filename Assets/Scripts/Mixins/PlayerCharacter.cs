using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacter : Character {

    public float MoveSpeed = 10f;

    private Animator Animator;
    private Camera cam;

    private Vector3 TargetPosition = Vector3.zero;
    private Vector3 TargetRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Animator = GetComponent<Animator>();

        cam = Camera.main;
    }

    // Update is called once per frame.
    // It is important to do any calculations on user input here so they are processed ASAP.
    // Any updates that need to reflect in the world should be set up here, so they can be used by FixedUpdate().
    // Do NOT update position of any physics bodies in this method as they may be repositioned when FixedUpdate() runs.
    void Update() {

        float leftRightInput = Input.GetAxisRaw("Horizontal");
        float upDownInput = Input.GetAxisRaw("Vertical");

        // Set the target position so the player is correctly moved when the physics engine ticks
        // Note the Time.deltaTime call which returns the time since call to Update(). This ensures our players movement is adjusted for the irregular intervals of the Update() call.
        // TODO I'm not sure if this supports running up and down hills. It seems like we would try to force the player through the geometry in the x or z direction.
        if (leftRightInput != 0 || upDownInput != 0) {
            TargetPosition = new Vector3 {
                x = transform.position.x + (leftRightInput * MoveSpeed * Time.deltaTime),
                z = transform.position.z + (upDownInput * MoveSpeed * Time.deltaTime),
            };
        } else {
            TargetPosition = Vector3.zero;
        }

        // Update the animator
        Animator.SetBool("Moving", (leftRightInput != 0 || upDownInput != 0));

        // Figure out which way the character should face based on the relative position of the mouse
        Vector3 mousePosition = Input.mousePosition;
        Vector3 characterScreenPosition = cam.WorldToScreenPoint(transform.position);
        Vector2 relativeDirection = mousePosition - characterScreenPosition;
        float angle = Mathf.Atan2(relativeDirection.x, relativeDirection.y) * Mathf.Rad2Deg;
        TargetRotation = new Vector3(0, angle, 0);
    }

    // Runs at a guaranteed interval (something like every .2 seconds)
    // This is where any physics bodies should be modified.
    void FixedUpdate() {
        if (TargetPosition != Vector3.zero) {
            transform.position = TargetPosition;
        }
        // TODO add a similar if statement
        transform.rotation = Quaternion.Euler(TargetRotation);
    }

    protected override void OnDeath() {
        Debug.Log("You Died");
    }
}
