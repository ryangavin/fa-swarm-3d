using UnityEngine;

public class HealthController : MonoBehaviour {
    public GameObject healthIcon;
    private GameObject _target;
    private Transform _transform;

    private void OnEnable() {
        EventBus.Register<PlayerSpawnEvent>(OnPlayerSpawn, global: true);
    }

    private void OnDisable() {
        EventBus.DeRegister<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: _target);
        EventBus.DeRegister<PlayerSpawnEvent>(OnPlayerSpawn);
    }

    private void Start() {
        _transform = transform;
    }

    private void OnPlayerSpawn(object eventArgs) {
        var playerSpawnEvent = (PlayerSpawnEvent) eventArgs;
        
        if (!_target) {
            EventBus.DeRegister<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: _target);
        }
        _target = playerSpawnEvent.target;
        
        EventBus.Register<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: _target);
        DrawHealth(GameStateManager.Instance.gamestate.playerCharacter.CurrentHealth);
    }

    private void OnCharacterHealthChange(object args) {
        var eventArgs = (CharacterHealthChangeEvent) args;
        DrawHealth(eventArgs.currentHealth);
    }

    private void DrawHealth(int health) {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        for (var i = 0; i < health; i ++) {
            var healthIconObject = Instantiate(healthIcon, parent: _transform, position: _transform.position + new Vector3(-20 + (-60 * i), 0), rotation: Quaternion.identity);
            healthIconObject.name = "HealthIcon" + i;
        }
    }
}
