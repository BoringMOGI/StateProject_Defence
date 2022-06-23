using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGround : MonoBehaviour
{
    private bool isSetTower = false; // �� ���� Ÿ���� ��ġ �Ǿ� �ִ°�?

    private void OnMouseUpAsButton()
    {
        if (isSetTower)
            return;

        if (GameManager.Instance.OnUseGold(500))
        {
            // ��ġ �� ��ġ ���ο� true�� �����Ѵ�.
            TowerSpawner.Instance.OnRequestTower(this);
            isSetTower = true;
        }
    }
}
