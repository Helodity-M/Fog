using UnityEngine;

public class ScoreTracker : StatTracker
{
    ScoreKeeper scoreKeeper;

    protected override int GetTargetValue()
    {
        return scoreKeeper.GetScore();
    }

    protected override string GetText()
    {
        return CurValue.ToString("000000");
    }

    void Start()
    {
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
    }
}
