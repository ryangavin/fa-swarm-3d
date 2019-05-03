using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for holding the state of the game
// Basically the Dungeon Master
public class GameStateManager : MonoBehaviour {

    // TODO refactor this to be a Character Scriptable Object
    public GameObject PlayerCharacter;
    public Vector3 SpawnPoint;

    public static GameStateManager Instance { get; private set; }

    public GameState Gamestate { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            Gamestate = ScriptableObject.CreateInstance<GameState>();
            DontDestroyOnLoad(gameObject);
        } else {
            // If there is already an instance, then just destroy whatever this is
            Debug.LogWarning("There are more than one GameStateManagers");
            Destroy(gameObject);
        }

        GameObject playerCharacterInstance = Instantiate(PlayerCharacter, position: SpawnPoint, rotation: Quaternion.identity);
        playerCharacterInstance.name = "PlayerCharacter";
        Gamestate.player = playerCharacterInstance;
        Gamestate.currentHealth = 3; // TODO fix this;
    }

    void OnDestroy() {
        Instance = null;    
    }

    void OnEnable() {
        EventBus.Register<DamageEvent>(OnDamage, global: true);
        EventBus.Register<CharacterHealthChangeEvent>(OnPlayerHealthChange, target: Gamestate.player);
    }

    void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamage);
        EventBus.DeRegister<CharacterHealthChangeEvent>(OnPlayerHealthChange, target: Gamestate.player);

    }

    void OnDamage(object eventArgs) { 
        DamageEvent damageEvent = (DamageEvent)eventArgs;

        // Record the score if the source was not an enemey (eg player)
        // TODO refactor this condition
        if (damageEvent.source.GetComponent<Enemy>() == null) {
            int previousScore = Gamestate.score;
            Gamestate.score += damageEvent.Damage;
            EventBus.Publish(new ScoreChangeEvent(gameObject, Gamestate.score - previousScore, Gamestate.score));
        }

    }

    void OnPlayerHealthChange(object args) {
        CharacterHealthChangeEvent eventArgs = (CharacterHealthChangeEvent)args;
        Gamestate.currentHealth = eventArgs.currentHealth;
    }
}
