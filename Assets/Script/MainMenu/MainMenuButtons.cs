using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
