using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMover : Singleton<SceneMover>
{
    public Image blindImage;

    public void OnLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float fadeOutTime = 1.0f;
        float time = 0.0f;

        blindImage.enabled = true;
        blindImage.SetAlpha(0.0f);

        while ((time += Time.deltaTime) < fadeOutTime)
        {
            blindImage.SetAlpha(time / fadeOutTime);
            yield return null;
        }

        blindImage.SetAlpha(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}

public static class Method
{
    public static void SetAlpha(this Image image, float a)
    {
        Color color = image.color;
        color.a = a;
        image.color = color;
    }
}
