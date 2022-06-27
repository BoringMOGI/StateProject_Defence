using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonManager : Singleton<TowerButtonManager>
{
    public TowerButton buttonPrefab;

    public void Setup(Tower[] towers)
    {
        // ������ �ִ� ��ư ����.
        TowerButton[] preButtons = transform.GetComponentsInChildren<TowerButton>();
        for (int i = 0; i < preButtons.Length; i++)
            Destroy(preButtons[i].gameObject);

        // ���� ���� Ÿ�� �迭�� ������ ���� ���ο� ��ư ����.
        foreach(Tower tower in towers)
        {
            // ���ο� ��ư�� �����.
            TowerButton newButton = Instantiate(buttonPrefab, transform);

            // tower �����տ��� ���� ����.
            Sprite sprite = tower.towerSprite;
            string name = tower.towerName;
            int price = tower.towerPrice;
            Tower.TYPE type = tower.towerType;

            // ���ο� ��ư�� ����.
            newButton.Setup(sprite, name, price, type);
        }
    }
}
