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

        // 이벤트 등록.
        turnOn += TurnOn;
        turnoff += TurnOff;
    }
    private void OnDestroy()
    {
        // 이벤트 등록 해제.
        turnOn -= TurnOn;
        turnoff -= TurnOff;
    }


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
