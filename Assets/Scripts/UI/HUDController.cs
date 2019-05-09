using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HUDController : MonoBehaviour {

    private Animator _animator;
   
    private static readonly int AnimatorGameOver = Animator.StringToHash("GameOver");
    
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