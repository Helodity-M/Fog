using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{

    [SerializeField] SpriteRenderer BackgroundRenderer;
    [SerializeField] SpriteRenderer GroundTileRenderer;
    [SerializeField] SpriteRenderer GroundTopRenderer;
    void Start()
    {
        BackgroundRenderer.sprite = SongPlayer.CurrentSong.BackgroundSprite;
        GroundTileRenderer.sprite = SongPlayer.CurrentSong.GroundTileSprite;
        GroundTopRenderer.sprite = SongPlayer.CurrentSong.GroundTopSprite;
    }
}
