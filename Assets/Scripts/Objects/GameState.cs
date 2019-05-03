using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO change package protection in such a way where only the GameStateController can see the members.
// TODO everyone else uses getters
// TODO Basically only the GameStateController should be able to update the GameState
public class GameState : ScriptableObject {

    public GameObject player;
    public int score = 0;
    public int currentHealth;
}
