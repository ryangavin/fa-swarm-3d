using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character {

    private float TargetRotation;

    // Update is called once per frame.
    // It is important to do any calculations on user input here so they are processed ASAP.
    // Any updates that need to reflect in the world should be set up here, so they can be used by FixedUpdate().
    // Do NOT update position of any physics bodies in this method as they may be repositioned when FixedUpdate() runs.
    void Update() {

        float leftRightInput = Input.GetAxisRaw("Horizontal");
        float upDownInput = Input.GetAxisRaw("Vertical");
        MoveDirection(new Vector3(x: leftRightInput, y: 0, z: upDownInput).normalized);

        // Figure out which way the character should face based on the relative position of the mouse
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Plane plane = new Plane(transform.up, transform.position);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = transform.InverseTransformPoint(target);
            Rotate(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
        }
        
        if (Input.GetMouseButtonDown(0)) {
            UseWeapon();
        }

    }

    protected override void OnDeath() {
        Debug.Log("You Died");
    }
}
