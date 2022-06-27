using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TowerButton : MonoBehaviour
{
    public Image towerImage;    // 타워 이미지.
    public Text nameText;       // 이름.
    public Text priceText;      // 가격.
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
        // 타워 매니저에게서 해당하는 type의 타워 가격을 가져온다.
        // 골드를 관리하는 게임매니저에게 해당 골드만큼 사용할 수 있는지 물어본다.
        int towerPrice = TowerSpawner.Instance.GetTowerPrice(type);
        if(GameManager.Instance.IsEnoughGold(towerPrice))
        {
            TowerSpawner.Instance.OnRequestTower(type);     // 타워 매니저에게 설치 요청.
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
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
