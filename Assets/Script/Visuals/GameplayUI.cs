using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] Image DamageVignette;

    [SerializeField] SpriteRenderer BackgroundRenderer;
    [SerializeField] SpriteRenderer GroundTileRenderer;
    [SerializeField] SpriteRenderer GroundTopRenderer;

    GameplayManager gameManager;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameplayManager>();
        BackgroundRenderer.sprite = SongPlayer.CurrentSong.BackgroundSprite;
        GroundTileRenderer.sprite = SongPlayer.CurrentSong.GroundTileSprite;
        GroundTopRenderer.sprite = SongPlayer.CurrentSong.GroundTopSprite;
    }

    // Update is called once per frame
    void Update()
    {
        DamageVignette.color = new Color(1, 1, 1, 1 - gameManager.playerHealth);
    }
}
