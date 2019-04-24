using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

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

    // Update is called once per frame
    void Update() {

        float leftRightInput = Input.GetAxisRaw("Horizontal");
        float upDownInput = Input.GetAxisRaw("Vertical");

        // Set the target force so the player is correctly moved when the physics engine ticks
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

    void FixedUpdate() {
        if (TargetPosition != Vector3.zero) {
            transform.position = TargetPosition;
        }
        // TODO add a similar if statement
        transform.rotation = Quaternion.Euler(TargetRotation);
    }
}
