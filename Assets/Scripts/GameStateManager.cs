using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Responsible for holding the state of the game
// Basically the Dungeon Master
public class GameStateManager : MonoBehaviour {

    public GameObject playerCharacterContainer;
    public GameObject playerCamera;
    public CharacterScriptableObject playerCharacter;
    public Vector3 spawnPoint;    // TODO move this to some level data object

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

        // Spawn the player character
        GameObject playerCharacterInstance = Instantiate(playerCharacterContainer, spawnPoint, Quaternion.identity);
        playerCharacterInstance.name = "PlayerCharacter"; 
        
        // Spawn the camera
        var playerCameraInstance = Instantiate(playerCamera);
        playerCameraInstance.name = "PlayerCamera";
        
        // Update the gamestate
        Gamestate.player = playerCharacterInstance;
        Gamestate.currentHealth = playerCharacter.health;
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
