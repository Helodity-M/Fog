using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CutsceneSO StartingCutscene;
    [SerializeField] SongSO MenuSong;

    [SerializeField] SongPlayer songPlayer;

    private void Start()
    {
        SongPlayer.CurrentSong = MenuSong;
        songPlayer.BeginPlayback(0);
    }

    public void LoadGameplay()
    {
        CutsceneManager.currentCutscene = StartingCutscene;
        SceneManager.LoadScene("Cutscene");
    }
}
