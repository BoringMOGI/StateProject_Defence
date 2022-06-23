using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCannon : Singleton<TestCannon>
{
    public TestBullet bulletPrefab;     // ������ ���� ������.
    public int poolCount;               // Ǯ�� ī��Ʈ.

    Stack<TestBullet> stack;

    private void Start()
    {
        // ���ʿ� poolCount��ŭ Ŭ���� ������ Stack�� ����.
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
        // ���� �Ѿ��� �޶�� �ߴµ� ���� ��쿡�� �ϳ� �����.
        if (stack.Count <= 0)
            CreateBullet();

        TestBullet bullet = stack.Pop();        // ���ÿ��� �ϳ� �����´�.
        bullet.transform.SetParent(null);       // �ֻ��� ��ü�� �ȴ� (�θ� ������Ʈ X)
        bullet.gameObject.SetActive(true);      // ���� ������Ʈ�� Ȱ��ȭ�Ѵ�.
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
