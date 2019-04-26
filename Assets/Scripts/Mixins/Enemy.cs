using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Enemy : MonoBehaviour {

    public float MoveSpeed = 10f;
    public int Health = 3;

    private Damageable damageable;

    private GameObject target;

    private Vector3 TargetPosition = Vector3.zero;

    // Register listeners
    void OnEnable() {
        damageable = GetComponent<Damageable>();
        damageable.OnDamaged += OnDamaged;
    }

    void OnDisable() {
        damageable.OnDamaged -= OnDamaged;
    }

    // Start is called before the first frame update
    void Start() {

        // Find the player when instantiated
        target = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update() {

        // Move our position a step closer to the target.
        float step = MoveSpeed * Time.deltaTime; // calculate distance to move
        TargetPosition = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    void FixedUpdate() {
        if (TargetPosition != Vector3.zero) {
            transform.position = TargetPosition;
        }
    }

    void OnDamaged(GameObject from, int damage) {
        Debug.Log("Hit from: " + from+ " for: "+damage);
        Health -= damage;
        if (Health <= 0) {
            GameObject.Destroy(gameObject);
        }
    }

}
