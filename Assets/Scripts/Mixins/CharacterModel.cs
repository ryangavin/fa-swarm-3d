using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterModel : MonoBehaviour {

    private Animator _animator;
    public Animator animator => _animator;

    // Start is called before the first frame update
    private void Start() {
        _animator = GetComponent<Animator>();
    }
}