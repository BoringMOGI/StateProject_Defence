using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// where GenericType : Type
// => Generic 타입은 적어도 Type이어야 한다.
public class Singleton<ClassType> : MonoBehaviour
    where ClassType : class         
{
    private static ClassType instance;
    public static ClassType Instance => instance;

    protected void Awake()
    {
        // ClassType에 어떤 자료형이 올지 모르겠지만
        // 적어도 class형태이기 때문에 this를 형변환 가능하다.
        instance = this as ClassType;
        Debug.Log("singleton Awake");
    }
}
