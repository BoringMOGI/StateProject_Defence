using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCannon : Singleton<TestCannon>
{
    public TestBullet bulletPrefab;     // 복제할 원본 프리팹.
    public int poolCount;               // 풀링 카운트.

    Stack<TestBullet> stack;

    private void Start()
    {
        // 최초에 poolCount만큼 클론을 생성해 Stack에 대입.
        stack = new Stack<TestBullet>();
        for(int i = 0; i<poolCount; i++)
            CreateBullet();
    }

    private void CreateBullet()
    {
        TestBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.SetParent(transform);
        bullet.gameObject.SetActive(false);
        bullet.onReturnBullet += OnReturnBullet;
        stack.Push(bullet);
    }
    private TestBullet GetBullet()
    {
        // 만약 총알을 달라고 했는데 없을 경우에는 하나 만든다.
        if (stack.Count <= 0)
            CreateBullet();

        TestBullet bullet = stack.Pop();        // 스택에서 하나 꺼내온다.
        bullet.transform.SetParent(null);       // 최상위 객체가 된다 (부모 오브젝트 X)
        bullet.gameObject.SetActive(true);      // 게임 오브젝트를 활성화한다.
        return bullet;
    }

    public void OnReturnBullet(TestBullet bullet)
    {
        bullet.transform.SetParent(transform);
        bullet.gameObject.SetActive(false);
        stack.Push(bullet);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Instantiate<T>(T object)
            // Instantiate<T>(T object, Transform parent)
            // Instantiate<T>(T object, Vector3 position, Quaternion rotation)
            TestBullet newBullet = GetBullet();
            newBullet.transform.position = transform.position;
            newBullet.transform.rotation = Quaternion.identity;
            newBullet.Setup(transform.right);
        }
    }

}
