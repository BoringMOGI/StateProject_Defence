using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    public float speed;         // n.
    public float liveTime;      // 2��.
    public Rigidbody2D rigid;

    public delegate void TestBulletEvent(TestBullet t);     // ��ȯ�� X, �Ű������� TestBullet�� �޴� ��������Ʈ.
    public event TestBulletEvent onReturnBullet;            // �� ��������Ʈ �� �̺�Ʈ ����.

    Vector3 dir;

    public void Setup(Vector3 dir)
    {
        this.dir = dir;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        Invoke(nameof(OnDead), liveTime);                   // liveTime �Ŀ� OnDead�Լ��� �Ҹ���.
    }
    private void OnDead()
    {
        //TestCannon.Instance.OnReturnBullet(this);
        onReturnBullet?.Invoke(this);
    }


    /*
    IEnumerator Movement()
    {
        float time = 0.0f;
        while(time < liveTime)          // ���� ����.
        {
            float movmenet = speed * Time.deltaTime;
            transform.position += dir * movmenet;       // ��ǥ �̵�.
            time += Time.deltaTime;                     // Ÿ�̸�.
            yield return null;
        }

        Destroy(gameObject);
    }
    */

}
