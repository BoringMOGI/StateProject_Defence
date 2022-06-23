using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy target;
    private float moveSpeed;
    private float power;

    // 1. �¾� �� ����� �޴´�.
    // 2. ��󿡰� ���� �ӵ��� ���ư���.
    // 3. ���� �浹�ϸ� �������.

    public void Setup(Enemy target, float moveSpeed, float power)
    {
        Debug.Log("����!!");

        this.target = target;
        this.moveSpeed = moveSpeed;
        this.power = power;
    }

    private void Update()
    {
        // ���� Ÿ���� ���� ���ư��� �� Ÿ���� �����Ǹ� ���� �����Ѵ�.
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 position = transform.position;              // �� ��ġ.
        Vector3 destination = target.transform.position;    // ������.
        float movement = moveSpeed * Time.deltaTime;        // �̵���.

        // ��ǥ �̵�.
        transform.position = Vector3.MoveTowards(position, destination, movement);
        if(transform.position == target.transform.position)
        {
            target.OnDamage(power);     // ������ ������ ����.
            Destroy(gameObject);        // ������Ʈ ����.
        }
    }
}
