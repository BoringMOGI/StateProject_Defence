using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy target;
    private float moveSpeed;
    private float power;

    // 1. 셋업 시 대상을 받는다.
    // 2. 대상에게 일정 속도로 날아간다.
    // 3. 대상과 충돌하면 사라진다.

    public void Setup(Enemy target, float moveSpeed, float power)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.power = power;
    }

    private void Update()
    {
        // 만약 타겟을 향해 날아가던 중 타겟이 삭제되면 나를 삭제한다.
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 position = transform.position;              // 내 위치.
        Vector3 destination = target.transform.position;    // 목적지.
        float movement = moveSpeed * CTime.deltaTime;        // 이동량.

        // 좌표 이동.
        transform.position = Vector3.MoveTowards(position, destination, movement);
        if(transform.position == target.transform.position)
        {
            target.OnDamage(power);     // 적에게 데미지 전달.
            Destroy(gameObject);        // 오브젝트 삭제.
        }
    }
}
