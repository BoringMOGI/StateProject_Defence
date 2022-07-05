using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    public UnityEngine.UI.Image image;

    private void Start()
    {
    }

    [ContextMenu("Fade")]
    public void Fade()
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        float fadeTime = 1.0f;
        float time = 0.0f;

        while(true)
        {
            Debug.Log(Time.deltaTime);
            time = Mathf.Clamp(time + Time.deltaTime, 0.0f, fadeTime);

            Color color = image.color;
            color.a = time / fadeTime;
            image.color = color;
            yield return null;

            if (color.a >= 1.0f)
                break;
        }

        SceneManager.sceneLoaded += OnLoadScene;
        SceneManager.LoadScene("Test");
    }

    void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("씬이 로드가 되면 불리는 함수");
    }


}
