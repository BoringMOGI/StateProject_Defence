using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour, ISelect
{
    public Sprite sprite;
    public int price;
    public string cubeName;

    public void OnDeselect()
    {
        Debug.Log("ť�� ���� ����");
    }

    public void OnSelect()
    {
        Debug.Log("ť�� ����!!");
    }

    private void Funtion()
    {
        Debug.Log("�׳� �Լ�!");
    }

}
