using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessagePopup : Singleton<MessagePopup>
{
    public CanvasGroup group;
    public Text msgText;

    public float showTime;     // ��� �ð�.
    public float fadeTime;     // ���̵� �ƿ� �ð�.

    Coroutine showCoroutime;

    private void Start()
    {
        group.alpha = 0.0f;
    }

    public void Show(string msg)
    {
        msgText.text = msg;
        group.alpha = 1.0f;

        // ������ ���ư��� �ڷ�ƾ�� �����Ѵٸ� ����.
        if (showCoroutime != null)
            StopCoroutine(showCoroutime);

        // ���ο� �ڷ�ƾ�� ���� �� ����.
        showCoroutime = StartCoroutine(ShowProcess());
    }

    IEnumerator ShowProcess()
    {
        yield return new WaitForSeconds(showTime);  // ���.

        float time = fadeTime;
        while(time > 0.0f)
        {
            time = Mathf.Clamp(time - Time.deltaTime, 0.0f, fadeTime);
            group.alpha = time / fadeTime;
            yield return null;
        }
    }
}
