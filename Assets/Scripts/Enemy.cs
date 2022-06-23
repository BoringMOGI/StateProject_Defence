using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public int gold;
    public float moveSpeed;

    Transform[] waypoints;
    int wayIndex;

    private System.Action onDead;       // 자신이 죽었음을 알리는 콜백 함수.
    public void Setup(Transform[] waypoints, System.Action onDead)
    {
        this.waypoints = waypoints;
        this.onDead = onDead;
        wayIndex = 0;

        // 0번째 포지션 (출발점)에 자신을 배치한다.
        transform.position = waypoints[0].position;
    }
    
    public void OnDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0.0f)
        {
            GameManager.Instance.OnAddGold(gold);
            OnDead();
        }
    }


    private void Update()
    {
        Vector3 destination = waypoints[wayIndex].position;     // 목적지.
        if(transform.position == destination)
        {
            wayIndex++;     // 도착 후 다음 목적지 번호 지정.
            if(wayIndex >= waypoints.Length)
            {
                OnGoal();
            }
        }
        else
        {
            // MoveTowards : Vector3
            // = A지점에서 B지점까지 T만큼 움직였을 때의 위치를 반환.
            Vector3 current = transform.position;               // A
            float movement = moveSpeed * Time.deltaTime;        // T
            transform.position = Vector3.MoveTowards(current, destination, movement);
        }
    }

    private void OnGoal()
    {
        // 완전히 Goal에 도작했다.
        GameManager.Instance.OnDamageHp();  // 체력을 1 깍는다.
        OnDead();
    }
    private void OnDead()
    {
        onDead?.Invoke();                   // 콜백 함수 호출 (스포너가 종료 시점을 잡기 위해서)
        Destroy(gameObject);                // 오브젝트 삭제.
    }
}
