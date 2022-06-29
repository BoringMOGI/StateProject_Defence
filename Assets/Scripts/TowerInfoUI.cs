using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : Singleton<TowerInfoUI>
{
    public GameObject panel;
    public Image towerImage;
    public Text powerText;
    public Text rateText;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void OnShow(Tower tower)
    {
        towerImage.sprite = tower.towerSprite;
        powerText.text = tower.attackPower.ToString("#,##0");
        rateText.text = tower.attackRate.ToString("#,##0");
        panel.SetActive(true);
    }
    public void OnClose()
    {
        panel.SetActive(false);
    }

}
