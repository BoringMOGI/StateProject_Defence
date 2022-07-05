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
        // Ÿ�� ��ġ �� �����϶�� ���.
        onTower = tower;                
        tower.transform.position = transform.position;
        tower.Setup(this);
    }
}
