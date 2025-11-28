using TMPro;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI scoreText;
   ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();

    }

    void Update()
    {
        scoreText.text = scoreKeeper.GetScore().ToString("0000000");
    }
}
