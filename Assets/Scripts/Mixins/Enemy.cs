using UnityEngine;

public class Enemy : Character {

    public float MoveSpeed = 10f;

    private GameObject target;

    private Vector3 TargetPosition = Vector3.zero;

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

    protected override void OnDamaged(object eventArgs) {
        DamageEvent damagedDevent = (DamageEvent)eventArgs;
        // Filter out damage from other enemies
        if (damagedDevent.Source.GetComponent<Enemy>() != null) {
            return;
        }
        base.OnDamaged(eventArgs);
    }
}
