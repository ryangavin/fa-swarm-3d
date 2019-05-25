using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class WaveData : ScriptableObject {
    public List<CharacterData> characterPool;
    public float spawnRate;

    private float _timeUntilNextSpawn;
    
    public bool ShouldSpawn() {
        // Since ShouldSpawn is called ONCE in Update(), we can use Time.deltaTime here to update _timeUntilNextSpawn
        _timeUntilNextSpawn -= Time.deltaTime;
        return _timeUntilNextSpawn <= 0;
    }

    public void Spawn() {
        Debug.Log("Wave Spawn");
        _timeUntilNextSpawn = spawnRate;
        
        // Find a suitable spawn point
        var targetOffset = Random.insideUnitCircle * 10;
        var localSpawnPoint = new Vector3(targetOffset.x, 0, targetOffset.y);
        var targetSpawnPoint =
            GameStateManager.Instance.gamestate.playerCharacter.transform.TransformPoint(localSpawnPoint);

        // TODO pick a random character
        Character.Spawn(characterPool[0], LevelManager.Instance.enemyCharacterContainer, targetSpawnPoint);
    }
}