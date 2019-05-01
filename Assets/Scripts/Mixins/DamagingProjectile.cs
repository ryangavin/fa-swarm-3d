using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingProjectile : DamagingObject {

    protected override void AfterDamage(GameObject collision) {
        //GameObject.Destroy(gameObject);
    }

}
