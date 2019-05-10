using UnityEngine;
using UnityEngine.SceneManagement;

// Responsible for holding the state of the game
// Basically the Dungeon Master.
// Only to be used inside the GameContainerScene.
public class GameStateManager : MonoBehaviour {

    public GameObject playerCharacterContainer;
    public GameObject playerCamera;
    public CharacterData playerCharacter;
    public GameModeData gameMode;

    private LevelData _currentLevel;

    // TODO refactor this so the Instance is hidden and only the gamestate is exposed
    public static GameStateManager Instance { get; private set; }

    public GameState gamestate { get; private set; }
    
    private void OnEnable() {
        EventBus.Register<DamageEvent>(OnDamage, global: true);
        EventBus.Register<DeathEvent>(OnPlayerDeath, target: gamestate.playerGameObject);
    }

    private void OnDisable() {
        EventBus.DeRegister<DamageEvent>(OnDamage);
        EventBus.DeRegister<DeathEvent>(OnPlayerDeath, target: gamestate.playerGameObject);
    }

    private void Awake() {
        // Set up the singleton
        if (Instance == null) {
            Instance = this;
            gamestate = ScriptableObject.CreateInstance<GameState>();
            DontDestroyOnLoad(gameObject);
        }
        else {
            // If there is already an instance, then just destroy whatever this is
            Debug.LogWarning("There is more than one GameStateManager in the scene");
            Destroy(gameObject);
        }

        // Configure the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    private void Start() {
        LoadLevel(gameMode.levels[0]);
    }

    private void OnDestroy() {
        Instance = null;    
    }

    private void LoadLevel(LevelData level) {
        // Load the scene
        var asyncOperation = SceneManager.LoadSceneAsync(level.scene);
        asyncOperation.completed += OnLoadLevel;
        _currentLevel = level;
    }

    private void OnLoadLevel(AsyncOperation asyncOperation) {
        
        // Spawn the player character
        Debug.Log("Spawning character");
        var playerCharacterInstance = Character.Spawn(playerCharacter, playerCharacterContainer, _currentLevel.spawnPoint);
        playerCharacterInstance.name = "PlayerCharacter";
        //DontDestroyOnLoad(playerCharacterInstance); // TODO Evaluate whether or not we need this

        // Spawn the camera
        var playerCameraInstance = Instantiate(playerCamera);
        playerCameraInstance.name = "PlayerCamera";

        // Update the gamestate
        gamestate.gameOver = false;
        gamestate.playerGameObject = playerCharacterInstance;
        gamestate.playerCharacter = playerCharacterInstance.GetComponent<Character>();
        
        // Publish a player spawn event
        EventBus.Publish(new PlayerSpawnEvent(gameObject, playerCharacterInstance));
    }

    private void OnDamage(object eventArgs) { 
        var damageEvent = (DamageEvent)eventArgs;

        // Record the score if the source was not an enemy (eg player)
        // TODO refactor this condition
        if (damageEvent.source.GetComponent<EnemyController>() != null) return;
        
        // Update the gamestate
        var previousScore = gamestate.score;
        gamestate.score += damageEvent.Damage;
        EventBus.Publish(new ScoreChangeEvent(gameObject, gamestate.score - previousScore, gamestate.score));

    }

    private void OnPlayerDeath(object eventArgs) {
        gamestate.gameOver = true;
        EventBus.Publish(new GameOverEvent(gameObject, null));
    }
}
