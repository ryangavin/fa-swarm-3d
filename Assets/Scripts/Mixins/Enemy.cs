using UnityEngine;

// TODO refactor into a SimpleEnemyController or something
public class Enemy : Character {

    private GameObject _target;
    private Vector3 _targetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start() {

        // Find the player when instantiated
        _target = GameStateManager.Instance.Gamestate.player;

    }

    // Update is called once per frame
    void Update() {

        // Move our position a step closer to the target.
        //float step = MoveSpeed * Time.deltaTime; // calculate distance to move
        //_targetPosition = Vector3.MoveTowards(transform.position, _target.transform.position, step);
    }

    void FixedUpdate() {
        if (_targetPosition != Vector3.zero) {
            transform.position = _targetPosition;
        }
    }

    protected override void OnDamaged(object eventArgs) {
        DamageEvent damagedDevent = (DamageEvent)eventArgs;
        // Filter out damage from other enemies
        if (damagedDevent.source.GetComponent<Enemy>() != null) {
            return;
        }
        base.OnDamaged(eventArgs);
    }
}
