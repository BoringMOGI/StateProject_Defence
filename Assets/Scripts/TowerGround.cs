using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGround : MonoBehaviour
{
    [SerializeField] GameObject lightObject;

    private delegate void TurnEvent();
    private static TurnEvent turnOn;
    private static TurnEvent turnoff;

    private Tower onTower = null;

    private void Start()
    {
        lightObject.SetActive(false);

        // �̺�Ʈ ���.
        turnOn += TurnOn;
        turnoff += TurnOff;
    }
    private void OnDestroy()
    {
        // �̺�Ʈ ��� ����.
        turnOn -= TurnOn;
        turnoff -= TurnOff;
    }


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
    
    private void TurnOn()
    {
        if (IsSetTower())
            lightObject.SetActive(true);
    }
    private void TurnOff()
    {
        lightObject.SetActive(false);
    }

    public static void OnSwitchLight(bool isOn)
    {
        if (isOn)
            turnOn?.Invoke();
        else
            turnoff?.Invoke();
    }
}
