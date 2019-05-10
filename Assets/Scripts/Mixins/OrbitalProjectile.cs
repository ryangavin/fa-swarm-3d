using UnityEngine;

public class OrbitalProjectile : MonoBehaviour {

    public GameObject Target;
    public Vector3 Axis = Vector3.up;
    public float ProjectileVelocity = 80.0f;
    public float ImpactFoce = 30;
    public float Lifetime = 5f;

    // Update is called once per frame
    void Update() {
        if (Lifetime < 0) {
            Destroy(gameObject);
            return;
        }

        transform.RotateAround(Target.transform.position, Axis, ProjectileVelocity * Time.deltaTime);

        Lifetime -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.rigidbody != null) {
            // Apply a force to the rigidbody
            collision.rigidbody.AddForce(transform.forward * ImpactFoce, mode: ForceMode.VelocityChange);

            // TODO add some callback so other things can do stuff

            Destroy(gameObject);
        }
    }
}
