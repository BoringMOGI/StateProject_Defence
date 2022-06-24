using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkTower : Tower
{
    public float sparkRange;

    protected override void OnRotate()
    {
        // ����ũ Ÿ���� ȸ���ϴ� �ʴ´�.
    }

    protected override void OnAttack(Enemy target)
    {
        // ���� �߽����� spark range��ŭ�� �������� ������ ���� üũ.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sparkRange, attackMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Enemy hit = hits[i].GetComponent<Enemy>();      // i��° �ݶ��̴����Լ� Enemy ������Ʈ �˻�.
            if (hit == null)                                // ���� ������Ʈ�� ���ٸ�.
                continue;                                   // �ش� ���� �ǳʶ�.

            hit.OnDamage(attackPower);
        }
    }

}
