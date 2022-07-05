using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGround : MonoBehaviour
{
    private Tower onTower = null;
    
    public bool IsSetTower()
    {
        return onTower == null;
    }
    public void SetTower(Tower tower)
    {
        // 타워 설치 후 동작하라고 명령.
        onTower = tower;                
        tower.transform.position = transform.position;
        tower.Setup(this);
    }
}
