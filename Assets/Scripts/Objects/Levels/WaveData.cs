using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class WaveData : ScriptableObject {

    public enum WaveType {
        Enemy,
        Boss
    }

    public WaveType waveType;
    public List<EnemySpawner> enemySpanwers;
    public List<CharacterData> characterPool; 
}