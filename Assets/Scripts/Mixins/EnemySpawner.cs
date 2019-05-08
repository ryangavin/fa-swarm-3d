using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public CharacterData enemy;
    public GameObject characterContainer;
    public float delay = 2000f;

    private float _timeSinceLastSpawn;
    private float _numSpawns;

    private void Update() {
        if (_timeSinceLastSpawn > delay) {
            var enemyInstance = Character.Spawn(enemy, characterContainer, transform.position);
            enemyInstance.name = "Enemy "+_numSpawns++;
            _timeSinceLastSpawn = 0;
        }
        else {
            _timeSinceLastSpawn += Time.deltaTime;
        }
    }

}
