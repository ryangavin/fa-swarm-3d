using UnityEngine;

// TODO change package protection in such a way where only the GameStateController can see the members.
// TODO everyone else uses getters
// TODO Basically only the GameStateController should be able to update the GameState
public class GameState : ScriptableObject {

    public GameObject playerGameObject;
    public Character playerCharacter;
    public int score;
    public bool gameOver;
}
