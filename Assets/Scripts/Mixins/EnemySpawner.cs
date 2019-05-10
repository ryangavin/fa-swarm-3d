using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public CharacterData enemy;
    public GameObject characterContainer;
    public float delay = 2000f;

    private float _timer;
    private float _numSpawns;

    private void Update() {
        _timer -= Time.deltaTime;
        
        if (!(_timer <= 0)) return;
        
        var enemyInstance = Character.Spawn(enemy, characterContainer, transform.position);
        enemyInstance.name = "Enemy "+_numSpawns++;
        _timer = delay;
    }

}
