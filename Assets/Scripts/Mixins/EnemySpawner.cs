using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject Enemy;
    public float Delay;

    private float timeSinceLastSpawn = 0f;

    void Start() {

    }

    private void Update() {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > Delay) {
            Instantiate(Enemy, transform.position, Quaternion.identity);
            timeSinceLastSpawn = 0f;
        }
    }

}
