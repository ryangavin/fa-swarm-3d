using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreController : MonoBehaviour {

    private TextMeshProUGUI textMesh;

    void Start() {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() {
        EventBus.Register<ScoreChangeEvent>(OnScore, global:true);
    }

    void OnDisable() {
        EventBus.DeRegister<ScoreChangeEvent>(OnScore);
    }

    void OnScore(object eventArgs) {
        ScoreChangeEvent scoreEvent = (ScoreChangeEvent)eventArgs;
        textMesh.text = "Score: " + scoreEvent.CurrentScore;
    }
}
