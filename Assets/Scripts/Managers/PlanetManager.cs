using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The PlanetManager is responsible for keeping track of all the planets in the scene.
///
/// This allows other game objects to query for the planets available in the scene if that is useful.
///
/// The primary intention is for the Player character to figure out what planet they're on when firing a weapon.
/// </summary>
public class PlanetManager : MonoBehaviour {

    public List<GameObject> PlanetsInScene { private set; get; }

    public static PlanetManager Instance;
    
    // Start is called before the first frame update
    private void Start() {
        // Set up the singleton
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            // If there is already an instance, then just destroy whatever this is
            Debug.LogWarning("There is more than one PlanetManager in the scene");
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += PopulatePlanetCache;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= PopulatePlanetCache;
    }

    private void PopulatePlanetCache(Scene scene, LoadSceneMode loadSceneMode) {
        PlanetsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
    }
}