using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    public float speed;         // n.
    public float liveTime;      // 2초.
    public Rigidbody2D rigid;

    public delegate void TestBulletEvent(TestBullet t);     // 반환형 X, 매개변수로 TestBullet을 받는 델리게이트.
    public event TestBulletEvent onReturnBullet;            // 그 델리게이트 형 이벤트 변수.

    Vector3 dir;

    public void Setup(Vector3 dir)
    {
        this.dir = dir;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        Invoke(nameof(OnDead), liveTime);                   // liveTime 후에 OnDead함수가 불린다.
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
        while(time < liveTime)          // 종료 조건.
        {
            float movmenet = speed * Time.deltaTime;
            transform.position += dir * movmenet;       // 좌표 이동.
            time += Time.deltaTime;                     // 타이머.
            yield return null;
        }

        Destroy(gameObject);
    }
    */

}
