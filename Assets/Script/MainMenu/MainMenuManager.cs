using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CutsceneSO StartingCutscene;
    [SerializeField] SongSO MenuSong;

    [SerializeField] SongPlayer songPlayer;
    [Header("Options")]
    [SerializeField] TransformAnimator optionsPageAnimator;

    [Header("Freeplay")]
    [SerializeField] TransformAnimator freeplayPageAnimator;

    [SerializeField] List<TransformAnimator> onLeaveAnimators;


    private void Start()
    {
        SongPlayer.CurrentSong = MenuSong;
        songPlayer.BeginPlayback(0);
    }

    public void LoadGameplay()
    {
        CutsceneManager.currentCutscene = StartingCutscene;
        SongPlayer.IsFreeplay = false;
        OnStartSceneLeave();
        StartCoroutine(LoadScene("Cutscene", 0.5f));
    }

    IEnumerator LoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadFreeplaySong(SongSO song)
    {
        CloseFreeplayPage(); //I cannot be bothered to make a better implementation
        OnStartSceneLeave();
        SongPlayer.IsFreeplay = true;
        SongPlayer.CurrentSong = song;
        StartCoroutine(LoadScene("Gameplay", 0.5f));
    }


    void OnStartSceneLeave()
    {
        foreach (TransformAnimator anim in onLeaveAnimators)
        {
            anim.StartAnimation(true);
        }
    }

    public void OpenOptionsPage()
    {
        optionsPageAnimator.StartAnimation(true);
    }
    public void CloseOptionsPage()
    {
        optionsPageAnimator.StartAnimation(false);
    }
    public void OpenFreeplayPage()
    {
        freeplayPageAnimator.StartAnimation(true);
    }
    public void CloseFreeplayPage()
    {
        freeplayPageAnimator.StartAnimation(false);
    }
}
