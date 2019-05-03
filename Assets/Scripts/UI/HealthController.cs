using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    public GameObject HealthIcon;

    private GameObject target;

    void OnDisable() {
        EventBus.DeRegister<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: target);
    }

    void Start() {
        target = GameStateManager.Instance.Gamestate.player;
        EventBus.Register<CharacterHealthChangeEvent>(OnCharacterHealthChange, target: target);
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
            GameObject healthSprite = Instantiate(HealthIcon, parent: transform, position: transform.position + new Vector3(-20 + (-60 * i), 0), rotation: Quaternion.identity);
            healthSprite.name = "HealthIcon" + i;
        }
    }
}
