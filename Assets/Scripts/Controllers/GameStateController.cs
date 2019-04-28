using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

    private GameState gamestate;

    void OnEnable() {
        gamestate = ScriptableObject.CreateInstance<GameState>();
        EventBus.Register<DamageEvent>(OnDamage, global: true);
    }

    void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamage);
    }

    void Update() {

    }

    void OnDamage(object eventArgs) {
        DamageEvent damageEvent = (DamageEvent)eventArgs;
        if (damageEvent.Source.GetComponent<Enemy>() == null) {
            int previousScore = gamestate.score;
            gamestate.score += damageEvent.Damage;
            EventBus.Publish(new ScoreChangeEvent(gameObject, gamestate.score - previousScore, gamestate.score));
        }
    }
}
