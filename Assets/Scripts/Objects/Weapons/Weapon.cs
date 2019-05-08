using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Weapon")]
public class Weapon : ScriptableObject {

    public float ProjectileVelocity = 80;
    public float ImpactVeloctiy = 400;
    public float FireRate = 6f;
    public float Lifetime = 2f;
    public GameObject ProjectileObject;
    public GameObject WeaponObject;

}


