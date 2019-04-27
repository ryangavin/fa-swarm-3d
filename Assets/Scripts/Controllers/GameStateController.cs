using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

    private GameState gamestate;

    // Start is called before the first frame update
    void Start() {
        gamestate = new GameState();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnemyDestroy() {
        gamestate.Score += 1;
    }
}
