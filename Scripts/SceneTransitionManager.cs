using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void LoadTo(string sceneName)
    {
        FadeIOManager.instance.FadeOutToIn(()=>Load(sceneName));
    }

    void Load(string sceneName)
    {
        SoundManager.instance.PlayBGM(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void TitleToExplanation()
    {
        SceneManager.LoadScene("Explanation");
    }

    public void ExplanationToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
