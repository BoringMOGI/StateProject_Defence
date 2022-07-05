using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHpBar
{
    public Transform hpBarPivot;
    public float hp;
    public float maxHp;
    public int gold;
    public float moveSpeed;

    Transform[] waypoints;
    int wayIndex;

    private System.Action onDead;       // �ڽ��� �׾����� �˸��� �ݹ� �Լ�.

    public void Setup(Transform[] waypoints, System.Action onDead)
    {
        this.waypoints = waypoints;
        this.onDead = onDead;
        wayIndex = 0;

        // 0��° ������ (�����)�� �ڽ��� ��ġ�Ѵ�.
        transform.position = waypoints[0].position;

        // ü�¹� �Ŵ������Լ� ü�¹ٸ� ������ �� �����Ѵ�.
        HpBar hpBar = HpBarManager.Instance.GetHpBar();
        hpBar.Setup(this);
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
        Vector3 destination = waypoints[wayIndex].position;     // ������.
        if(transform.position == destination)
        {
            wayIndex++;     // ���� �� ���� ������ ��ȣ ����.
            if(wayIndex >= waypoints.Length)
            {
                OnGoal();
            }
        }
        else
        {
            // MoveTowards : Vector3
            // = A�������� B�������� T��ŭ �������� ���� ��ġ�� ��ȯ.
            Vector3 current = transform.position;               // A
            float movement = moveSpeed * CTime.deltaTime;        // T
            transform.position = Vector3.MoveTowards(current, destination, movement);
        }
    }

    private void OnGoal()
    {
        // ������ Goal�� �����ߴ�.
        GameManager.Instance.OnDamageHp();  // ü���� 1 ��´�.
        OnDead();
    }
    private void OnDead()
    {
        onDead?.Invoke();                   // �ݹ� �Լ� ȣ�� (�����ʰ� ���� ������ ��� ���ؼ�)
        Destroy(gameObject);                // ������Ʈ ����.
    }

    public Transform GetPivot()
    {
        return hpBarPivot;
    }

    public float GetHp()
    {
        return hp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }
}
