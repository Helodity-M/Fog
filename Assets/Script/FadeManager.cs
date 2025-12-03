using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : PersistentSingleton<FadeManager>
{
    [SerializeField] Image FadeImage;

    [SerializeField] float FadeDuration;
    bool inSceneTransition = false;

    private void Start()
    {
        StartCoroutine(DoFadeOut());
    }
    public void FadeToScene(string sceneName)
    {
        if (!inSceneTransition)
        {
            inSceneTransition = true;
            StartCoroutine(DoSceneTransition(sceneName));
        }
    }

    IEnumerator DoFadeOut()
    {
        float fadeRemaining = FadeDuration;
        while (fadeRemaining > 0)
        {
            FadeImage.color = new Color(0, 0, 0, fadeRemaining / FadeDuration);
            yield return new WaitForEndOfFrame();
            fadeRemaining -= Time.deltaTime;
        }
        FadeImage.color = Color.clear;
    }

    IEnumerator DoFadeIn()
    {
        float fadeRemaining = FadeDuration;
        while (fadeRemaining > 0)
        {
            FadeImage.color = new Color(0, 0, 0, 1 - fadeRemaining / FadeDuration);
            yield return new WaitForEndOfFrame();
            fadeRemaining -= Time.deltaTime;
        }
        FadeImage.color = Color.black;
    }

    IEnumerator DoSceneTransition(string scene)
    {
        yield return DoFadeIn();
        SceneManager.LoadScene(scene);
        yield return DoFadeOut();

        //Finish up
        inSceneTransition = false;
    }

}
