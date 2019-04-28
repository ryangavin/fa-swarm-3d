using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChangeEvent : Event {

    private int delta;
    private int currentScore;

    public ScoreChangeEvent(GameObject Source, int Delta, int CurrentScore) : base(Source, null) {
        this.delta = Delta;
        this.currentScore = CurrentScore;
    }

    public int Delta { get => delta; }
    public int CurrentScore { get => currentScore; }
}
