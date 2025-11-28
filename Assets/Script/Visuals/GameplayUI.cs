using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] Image DamageVignette;
    [SerializeField] TMP_Text scoreText;


    [SerializeField] SpriteRenderer BackgroundRenderer;
    [SerializeField] SpriteRenderer GroundTileRenderer;
    [SerializeField] SpriteRenderer GroundTopRenderer;

    GameplayManager gameManager;
    ScoreKeeper scoreKeeper;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameplayManager>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        BackgroundRenderer.sprite = SongPlayer.CurrentSong.BackgroundSprite;
        GroundTileRenderer.sprite = SongPlayer.CurrentSong.GroundTileSprite;
        GroundTopRenderer.sprite = SongPlayer.CurrentSong.GroundTopSprite;
    }

    // Update is called once per frame
    void Update()
    {
        DamageVignette.color = new Color(1, 1, 1, 1 - gameManager.playerHealth);
        scoreText.text = scoreKeeper.GetScore().ToString("0000000");
    }
}
