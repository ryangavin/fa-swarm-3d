using UnityEngine;

// Responsible for holding the state of the game
// Basically the Dungeon Master
public class GameStateManager : MonoBehaviour {

    public GameObject playerCharacterContainer;
    public GameObject playerCamera;
    public CharacterData playerCharacter;
    public Vector3 spawnPoint;    // TODO move this to some level data object

    public static GameStateManager Instance { get; private set; }

    public GameState gamestate { get; private set; }

    private void Awake() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        if (Instance == null) {
            Instance = this;
            gamestate = ScriptableObject.CreateInstance<GameState>();
            DontDestroyOnLoad(gameObject);
        } else {
            // If there is already an instance, then just destroy whatever this is
            Debug.LogWarning("There is more than one GameStateManager in the scene");
            Destroy(gameObject);
        }

        // Spawn the player character
        // TODO move this into the Spawn method and make spawn static
        var playerCharacterInstance = Character.Spawn(playerCharacter, playerCharacterContainer, spawnPoint);

        // Spawn the camera
        var playerCameraInstance = Instantiate(playerCamera);
        playerCameraInstance.name = "PlayerCamera";
        
        // Update the gamestate
        gamestate.player = playerCharacterInstance;
        gamestate.currentHealth = playerCharacter.health;
    }

    private void OnDestroy() {
        Instance = null;    
    }

    private void OnEnable() {
        EventBus.Register<DamageEvent>(OnDamage, global: true);
        EventBus.Register<CharacterHealthChangeEvent>(OnPlayerHealthChange, target: gamestate.player);
    }

    private void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamage);
        EventBus.DeRegister<CharacterHealthChangeEvent>(OnPlayerHealthChange, target: gamestate.player);

    }

    private void OnDamage(object eventArgs) { 
        DamageEvent damageEvent = (DamageEvent)eventArgs;

        // Record the score if the source was not an enemey (eg player)
        // TODO refactor this condition
        if (damageEvent.source.GetComponent<EnemyController>() == null) {
            var previousScore = gamestate.score;
            gamestate.score += damageEvent.Damage;
            EventBus.Publish(new ScoreChangeEvent(gameObject, gamestate.score - previousScore, gamestate.score));
        }

    }

    private void OnPlayerHealthChange(object args) {
        CharacterHealthChangeEvent eventArgs = (CharacterHealthChangeEvent)args;
        gamestate.currentHealth = eventArgs.currentHealth;
    }
}
