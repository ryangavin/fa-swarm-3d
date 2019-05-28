using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(Image))]
public class CharacterHealthBar : MonoBehaviour, ICharacterContainerAware {
    private int _maxHealth;
    private int _currentHealth;

    private Image _image;
    private GameObject _parent;

    private Camera _camera;

    // Since this behavior needs to register listeners that target the character container,
    // those listeners must be registered in SetParent().
    public void SetParent(GameObject parent) {
        _parent = parent;
        EventBus.Register<CharacterHealthChangeEvent>(OnHealthChange, target: _parent);
        EventBus.Register<CharacterSpawnEvent>(OnCharacterSpawn, target: _parent);
    }

    public void Awake() {
        if (Camera.main) {
            _camera = Camera.main;
        }
        else {
            Debug.Log("No main camera in scene, CharacterHealthBar will display incorrectly");
        }

        _image = GetComponent<Image>();
    }

    // TODO Register listeners
    public void OnDisable() {
        EventBus.DeRegister<CharacterHealthChangeEvent>(OnHealthChange, target: _parent);
        EventBus.DeRegister<CharacterSpawnEvent>(OnCharacterSpawn, target: _parent);
    }

    private void OnHealthChange(object eventArgs) {
        var healthChangeEvent = (CharacterHealthChangeEvent) eventArgs;
        _currentHealth = healthChangeEvent.currentHealth;
        UpdateHealthBar();
    }

    private void OnCharacterSpawn(object eventArgs) {
        var characterSpawnEvent = (CharacterSpawnEvent) eventArgs;
        _maxHealth = characterSpawnEvent.CharacterData.health;
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        Debug.Log("Updating health bar");
        _image.fillAmount = (float) _currentHealth / _maxHealth;
        Debug.Log(_image.fillAmount);
    }
}