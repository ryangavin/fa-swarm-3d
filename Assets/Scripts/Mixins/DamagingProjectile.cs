using UnityEngine;

// TODO Add filter
public class DamagingProjectile : DamagingObject {

    protected override void AfterDamage(GameObject collision) {
        Destroy(gameObject);
    }

}
