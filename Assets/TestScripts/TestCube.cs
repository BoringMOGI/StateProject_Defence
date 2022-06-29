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
        Debug.Log("큐브 선택 해제");
    }

    public void OnSelect()
    {
        Debug.Log("큐브 선택!!");
    }

    private void Funtion()
    {
        Debug.Log("그냥 함수!");
    }

}
