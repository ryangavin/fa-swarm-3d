using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelData : ScriptableObject {
    public string scene;
    public Vector3 spawnPoint;
}