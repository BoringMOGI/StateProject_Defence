using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine(Process("AAA"));
        StartCoroutine(Process("BBB"));
        StartCoroutine(Process("CCC"));

        coroutines = new Coroutine[3];

        coroutines[0] = StartCoroutine(nameof(Process), "DDD");
        coroutines[1] = StartCoroutine(nameof(Process), "EEE");
        coroutines[2] = StartCoroutine(nameof(Process), "FFF");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(coroutines[1]);
        }
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
