﻿using UnityEngine;
[RequireComponent(typeof(Character))]
public class EnemyController : MonoBehaviour {

    private GameObject _target;
    private Character _character;

    // Start is called before the first frame update
    private void Start() {

        // Find the player when instantiated
        _target = GameStateManager.Instance.gamestate.playerGameObject;
        
        // Get the character component
        _character = GetComponent<Character>();

    }

    // Update is called once per frame
    void Update() {
        if (!_target || GameStateManager.Instance.gamestate.gameOver) {
            return;
        }
        // Find the direction of the player and tell the character component to move that way
        var playerDirection = _target.transform.position - transform.position;
        playerDirection.y = 0;
        playerDirection = playerDirection.normalized;
        _character.MoveDirection(playerDirection);
        _character.Rotate(Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg);
    }
}
