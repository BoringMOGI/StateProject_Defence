using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TowerButton : MonoBehaviour
{
    public Image towerImage;    // Ÿ�� �̹���.
    public Text nameText;       // �̸�.
    public Text priceText;      // ����.
    public Tower.TYPE type;

    public void Setup(Sprite towerSprite, string towerName, int towerPrice, Tower.TYPE type)
    {
        towerImage.sprite = towerSprite;
        nameText.text = towerName;
        priceText.text = towerPrice.ToString("#,##0");
        this.type = type;
    }

    public void OnSelectTower()
    {
        // Ÿ�� �Ŵ������Լ� �ش��ϴ� type�� Ÿ�� ������ �����´�.
        // ��带 �����ϴ� ���ӸŴ������� �ش� ��常ŭ ����� �� �ִ��� �����.
        int towerPrice = TowerSpawner.Instance.GetTowerPrice(type);
        if(GameManager.Instance.IsEnoughGold(towerPrice))
        {
            TowerSpawner.Instance.OnRequestTower(type);     // Ÿ�� �Ŵ������� ��ġ ��û.
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void OnPointerEnter()
    {
        nameText.color = Color.yellow;
        priceText.color = Color.yellow;
    }
    public void OnPointerExit()
    {
        nameText.color = Color.white;
        priceText.color = Color.white;
    }
}
