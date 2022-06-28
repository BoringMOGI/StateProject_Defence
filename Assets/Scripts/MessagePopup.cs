using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessagePopup : Singleton<MessagePopup>
{
    public CanvasGroup group;
    public Text msgText;

    public float showTime;     // 출력 시간.
    public float fadeTime;     // 페이드 아웃 시간.

    Coroutine showCoroutime;

    private void Start()
    {
        group.alpha = 0.0f;
    }

    public void Show(string msg)
    {
        msgText.text = msg;
        group.alpha = 1.0f;

        // 기존에 돌아가는 코루틴이 존재한다면 중지.
        if (showCoroutime != null)
            StopCoroutine(showCoroutime);

        // 새로운 코루틴을 생성 후 대입.
        showCoroutime = StartCoroutine(ShowProcess());
    }

    IEnumerator ShowProcess()
    {
        yield return new WaitForSeconds(showTime);  // 대기.

        float time = fadeTime;
        while(time > 0.0f)
        {
            time = Mathf.Clamp(time - Time.deltaTime, 0.0f, fadeTime);
            group.alpha = time / fadeTime;
            yield return null;
        }
    }
}
