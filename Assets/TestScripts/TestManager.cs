using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public TestCube prefab;
    
    void Start()
    {
        Debug.Log(prefab.cubeName);
        Debug.Log(prefab.price);

        Instantiate(prefab);
    }
}
