using UnityEngine;

/// <summary>
/// Published when GAMEPLAY should begin in a given level
/// </summary>
public class LevelStartEvent : Event {
    public readonly LevelData LevelData;

    public LevelStartEvent(GameObject source, LevelData levelData) : base(source, null) {
        LevelData = levelData;
    }
}
