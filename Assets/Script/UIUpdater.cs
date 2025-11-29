using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
   [SerializeField] Slider healthSlider;
    [SerializeField] GameplayManager gameplayManager;
   
   [SerializeField] TextMeshProUGUI scoreText;
   ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
       
    }

    void Update()
    {
        scoreText.text = scoreKeeper.GetScore().ToString("0000000");
        healthSlider.value = gameplayManager.playerHealth;
    }
}

