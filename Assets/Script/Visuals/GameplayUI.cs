using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
