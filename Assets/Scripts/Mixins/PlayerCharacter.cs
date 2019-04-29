using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Refactor out the bits about controlling the character
// TODO This class should only contain the members and functions that make this differ from a regular character.
// TODO The actual player input should come from another class
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : Character {

    public float MoveSpeed = 10f;

    private Animator Animator;
    private Rigidbody rb;

    private Vector3 TargetDirection = Vector3.zero;
    private float TargetRotation;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
            TargetDirection = new Vector3(x: leftRightInput, y: 0, z: upDownInput).normalized;
        } else {
            TargetDirection = Vector3.zero;
        }

        // Update the animator
        Animator.SetBool("Moving", (leftRightInput != 0 || upDownInput != 0));

        // Figure out which way the character should face based on the relative position of the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            TargetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
       
    }

    // Runs at a guaranteed interval (something like every .2 seconds)
    // This is where any physics bodies should be modified.
    void FixedUpdate() {
        if (TargetDirection != Vector3.zero) {
            rb.MovePosition(transform.position + TargetDirection * MoveSpeed * Time.deltaTime);
        }
        // TODO add a similar if statement
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation = new Vector3(currentRotation.x, TargetRotation, currentRotation.z);
        transform.rotation = Quaternion.Euler(targetRotation);
    }

    protected override void OnDeath() {
        Debug.Log("You Died");
    }
}
