using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// where GenericType : Type
// => Generic Ÿ���� ��� Type�̾�� �Ѵ�.
public class Singleton<ClassType> : MonoBehaviour
    where ClassType : class         
{
    private static ClassType instance;
    public static ClassType Instance => instance;

    protected void Awake()
    {
        // ClassType�� � �ڷ����� ���� �𸣰�����
        // ��� class�����̱� ������ this�� ����ȯ �����ϴ�.
        instance = this as ClassType;
        Debug.Log("singleton Awake");
    }
}
