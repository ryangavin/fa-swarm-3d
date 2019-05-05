using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthController : MonoBehaviour {
    public GameObject healthIcon;
    private GameObject _target;

    void OnDisable() {
        EventBus.DeRegister<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: _target);
    }

    void Start() {
        _target = GameStateManager.Instance.Gamestate.player;
        EventBus.Register<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: _target);
        DrawHealth(GameStateManager.Instance.Gamestate.currentHealth);    
    }

    private void OnCharacterHealthChange(object args) {
        CharacterHealthChangeEvent eventArgs = (CharacterHealthChangeEvent) args;
        DrawHealth(eventArgs.currentHealth);
    }

    private void DrawHealth(int health) {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < health; i ++) {
            Debug.Log("Drawing Health");
            GameObject healthSprite = Instantiate(healthIcon, parent: transform, position: transform.position + new Vector3(-20 + (-60 * i), 0), rotation: Quaternion.identity);
            healthSprite.name = "HealthIcon" + i;
        }
    }
}
