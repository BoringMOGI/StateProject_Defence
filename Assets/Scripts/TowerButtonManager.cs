using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonManager : Singleton<TowerButtonManager>
{
    public TowerButton buttonPrefab;

    public void Setup(TowerGroup[] groups)
    {
        // ������ �ִ� ��ư ����.
        TowerButton[] preButtons = transform.GetComponentsInChildren<TowerButton>();
        for (int i = 0; i < preButtons.Length; i++)
            Destroy(preButtons[i].gameObject);

        // ���� ���� Ÿ�� �迭�� ������ ���� ���ο� ��ư ����.
        foreach(TowerGroup group in groups)
        {
            // ���ο� ��ư�� �����.
            TowerButton newButton = Instantiate(buttonPrefab, transform);
            Tower firstTower = group.firstTower;

            // group���� ù��° Ÿ���� ���� ���� ����.
            Sprite sprite   = firstTower.towerSprite;
            string name     = firstTower.towerName;
            int price       = firstTower.towerPrice;
            Tower.TYPE type = firstTower.towerType;

            // ���ο� ��ư�� ����.
            newButton.Setup(sprite, name, price, type);
        }
    }
}
