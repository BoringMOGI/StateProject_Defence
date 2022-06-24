using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTower : Tower
{
    [Header("Explode")]
    public float explodeRange;

    protected override void OnAttack(Enemy target)
    {
        // target�� �߽����� explode range��ŭ�� �������� ������ ���� üũ.
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.transform.position, explodeRange, attackMask);
        for(int i = 0; i<hits.Length; i++)
        {
            Enemy hit = hits[i].GetComponent<Enemy>();      // i��° �ݶ��̴����Լ� Enemy ������Ʈ �˻�.
            if (hit == null)                                // ���� ������Ʈ�� ���ٸ�.
                continue;                                   // �ش� ���� �ǳʶ�.

            hit.OnDamage(attackPower);
        }
    }

}
