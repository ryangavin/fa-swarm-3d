using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Persistable))]
public class HUDController : MonoBehaviour {

    private Animator _animator;
   
    private static readonly int AnimatorGameOver = Animator.StringToHash("GameOver");

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        EventBus.Register<GameOverEvent>(OnGameOver, global: true);
    }

    private void OnDisable() {
        EventBus.DeRegister<GameOverEvent>(OnGameOver);
    }

    private void OnGameOver(object eventArgs) {
        _animator.SetBool(AnimatorGameOver, true);
    }
}