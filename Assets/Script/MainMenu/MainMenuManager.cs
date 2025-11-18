using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CutsceneSO StartingCutscene;

    [SerializeField] SongPlayer songPlayer;

    private void Awake()
    {
        songPlayer.BeginPlayback(0);
    }

    public void LoadGameplay()
    {
        CutsceneManager.currentCutscene = StartingCutscene;
        SceneManager.LoadScene("Cutscene");
    }
}
