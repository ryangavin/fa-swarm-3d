using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public CharacterData enemy;
    public GameObject characterContainer;
    public float delay = 2000f;

    private float _timeSinceLastSpawn = 0f;

    private void Update() {
        if (_timeSinceLastSpawn > delay) {
            Debug.Log("Spawning");
            Character.Spawn(enemy, characterContainer, transform.position);
            _timeSinceLastSpawn = 0;
        }
        else {
            _timeSinceLastSpawn += Time.deltaTime;
        }
    }

}
