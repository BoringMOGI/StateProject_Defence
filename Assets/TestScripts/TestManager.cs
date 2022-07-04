using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Person
{
    public string name;
    public int age;
    public float height;

    public override string ToString()
    {
        return string.Format("�̸�:{0}, ����:{1}, Ű:{2}", name, age, height);
    }
}


public class TestManager : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 pos;

    private void Start()
    {
        Person[] persons = new Person[]
        {
            new Person(){name = "����", age = 95, height = 200f },
            new Person(){name = "�����", age = 18, height = 160f },
            new Person(){name = "�ٸ��콺", age = 70, height = 200f },
            new Person(){name = "����", age = 11, height = 155f },
            new Person(){name = "��þ�", age = 30, height = 185f },
            new Person(){name = "ī��", age = 25, height = 170f },
            new Person(){name = "�Ƹ�", age = 900, height = 175f },
            new Person(){name = "���", age = 3000, height = 90f },
            new Person(){name = "������", age = 500, height = 5000f },
        };

        var fine = from person in persons
                   where person.age <= 30
                   select person.age;

        foreach(var person in fine)
        {
            Debug.Log(person);
        }

        
    }

}
