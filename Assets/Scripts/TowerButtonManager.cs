using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonManager : Singleton<TowerButtonManager>
{
    public TowerButton buttonPrefab;

    public void Setup(Tower[] towers)
    {
        // 기존에 있는 버튼 삭제.
        TowerButton[] preButtons = transform.GetComponentsInChildren<TowerButton>();
        for (int i = 0; i < preButtons.Length; i++)
            Destroy(preButtons[i].gameObject);

        // 전달 받은 타워 배열의 정보를 통해 새로운 버튼 생성.
        foreach(Tower tower in towers)
        {
            // 새로운 버튼을 만든다.
            TowerButton newButton = Instantiate(buttonPrefab, transform);

            // tower 프리팹에서 정보 추출.
            Sprite sprite = tower.towerSprite;
            string name = tower.towerName;
            int price = tower.towerPrice;
            Tower.TYPE type = tower.towerType;

            // 새로운 버튼에 세팅.
            newButton.Setup(sprite, name, price, type);
        }
    }
}
