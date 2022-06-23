using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGround : MonoBehaviour
{
    private bool isSetTower = false; // 내 위에 타워가 설치 되어 있는가?

    private void OnMouseUpAsButton()
    {
        if (isSetTower)
            return;

        if (GameManager.Instance.OnUseGold(500))
        {
            // 설치 후 설치 여부에 true를 대입한다.
            TowerSpawner.Instance.OnRequestTower(this);
            isSetTower = true;
        }
    }
}
