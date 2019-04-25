using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public ParticleSystem bulletParticleSystem;

    private bool firing = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            bulletParticleSystem.Play();
        } if (Input.GetMouseButtonUp(0)) {
            bulletParticleSystem.Stop();
        }
    }

    void FixedUpdate() {
        
    }
}
    