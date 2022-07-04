using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerControlUI : Singleton<TowerControlUI>
{
    public Text upgradeText;
    public Text sellText;

    private Tower tower;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(Tower tower)
    {
        this.tower = tower;

        upgradeText.text = "$0";
        sellText.text = string.Concat('$', tower.sellPrice);

        transform.position = tower.transform.position;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnUpgradeTower()
    {
        TowerSpawner.Instance.OnUpgradeTower(tower);
    }
    public void OnSellTower()
    {
        TowerSpawner.Instance.OnSellTower(tower);
    }

}
