using UnityEngine;

/// <summary>
/// Published when a player character has spawned in the world and the gamestate has been updated to reflect this
/// </summary>
public class PlayerSpawnEvent : Event {
    public PlayerSpawnEvent(GameObject source, GameObject target) : base(source, target) { }
}
