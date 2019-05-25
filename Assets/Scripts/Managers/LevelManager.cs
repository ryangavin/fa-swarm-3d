using UnityEngine;

/// <summary>
/// The LevelManager is responsible for controlling the gameplay of an individual level.
/// The GameStateManager publishes a LevelStartEvent when the LevelManager should begin gameplay.
/// </summary>
public class LevelManager : MonoBehaviour {

    public GameObject enemyCharacterContainer;
    
    private LevelData _currentLevel;
    private WaveData _currentWave;

    public static LevelManager Instance;
    
    private void OnEnable() {
        EventBus.Register<LevelStartEvent>(OnLevelStart, global: true);
    }

    private void OnDisable() {
        EventBus.DeRegister<LevelStartEvent>(OnLevelStart);

    }

    private void Awake() {
        // Set up the singleton
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            // If there is already an instance, then just destroy whatever this is
            Debug.LogWarning("There is more than one GameStateManager in the scene");
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (!_currentLevel) return;
        if (!_currentWave) return;
        
        // TODO check if the wave should end
        
        // Check if the wave should spawn some enemies
        if (_currentWave.ShouldSpawn()) {
            _currentWave.Spawn();
        }
    }

    private void OnLevelStart(object eventArgs) {
        Debug.Log("Staring level");
        var levelStartEvent = (LevelStartEvent) eventArgs;
        _currentLevel = levelStartEvent.LevelData;
        _currentWave = _currentLevel.waves[0];
    }

}