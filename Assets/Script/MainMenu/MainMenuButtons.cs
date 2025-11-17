using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] CutsceneSO StartingCutscene;

    public void LoadGameplay()
    {
        CutsceneManager.currentCutscene = StartingCutscene;
        SceneManager.LoadScene("Cutscene");
    }
}
