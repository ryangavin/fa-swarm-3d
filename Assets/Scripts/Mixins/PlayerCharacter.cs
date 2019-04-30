using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Refactor out the bits about controlling the character
// TODO This class should only contain the members and functions that make this differ from a regular character.
// TODO The actual player input should come from another class
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : Character {

    public float MoveSpeed = 10f;
    public GameObject CharacterContainer;


    private Animator Animator;
    private Rigidbody rb;

    private Vector3 TargetDirection = Vector3.zero;
    private float TargetRotation;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Animator = GetComponentInChildren<Animator>();
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
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Debug.DrawRay(ray.origin, ray.direction*100, color: Color.red);
        Plane plane = new Plane(transform.up, transform.position);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = transform.InverseTransformDirection(target);
            TargetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Debug.Log(TargetRotation);
        }
       
    }

    // Runs at a guaranteed interval (something like every .2 seconds)
    // This is where any physics bodies should be modified.
    void FixedUpdate() {
        if (TargetDirection != Vector3.zero) {
            rb.MovePosition(transform.position + transform.TransformDirection(TargetDirection) * MoveSpeed * Time.deltaTime);
        }
        CharacterContainer.transform.localRotation = Quaternion.Euler(0, TargetRotation, 0);
    }

    protected override void OnDeath() {
        Debug.Log("You Died");
    }
}
