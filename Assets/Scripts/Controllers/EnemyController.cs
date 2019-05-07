using UnityEngine;
[RequireComponent(typeof(Character))]
public class EnemyController : MonoBehaviour {

    private GameObject _target;
    private Character _character;

    // Start is called before the first frame update
    private void Start() {

        // Find the player when instantiated
        _target = GameStateManager.Instance.gamestate.player;
        
        // Get the character component
        _character = GetComponent<Character>();

    }

    // Update is called once per frame
    void Update() {
        // Find the direction of the player and tell the character component to move that way
        var playerDirection = _target.transform.position - transform.position;
        playerDirection = playerDirection.normalized;
        _character.MoveDirection(playerDirection);
    }
}
