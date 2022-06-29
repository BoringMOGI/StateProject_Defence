using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class Class
{
    public int number;
    public int type;
    public int height;
}
struct Struct
{
    public int number;
    public int type;
    public int height;
}

public class TestManager : MonoBehaviour
{
    public GameObject target;

    Coroutine[] coroutines;
    void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(coroutines[1]);
        }

        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    IEnumerator Process(string str)
    {
        while (true)
        {
            Debug.Log(str);
            yield return null;
        }
    }

}
