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
            GameObject newEnemy = Instantiate(Enemy, transform.position, Quaternion.identity);
            newEnemy.transform.localScale = new Vector3(4, 4, 4);

            timeSinceLastSpawn = 0f;
        }
    }

}
