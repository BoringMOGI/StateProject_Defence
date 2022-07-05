using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionWindow : Singleton<OptionWindow>
{
    public GameObject panel;
    private bool isPause;

    private void Start()
    {
        isPause = false;
        CTime.isPause = false;
        panel.SetActive(false);
    }

    public void SwitchOption()
    {
        isPause = !isPause;
        CTime.isPause = isPause;
        panel.SetActive(isPause);
    }
    public void OnRetry()
    {
        SceneMover.Instance.OnLoadScene("Game");
    }
}
