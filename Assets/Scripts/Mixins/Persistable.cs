using UnityEngine;

public class Persistable : MonoBehaviour {
    // Start is called before the first frame update
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}