using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkTower : Tower
{
    public float sparkRange;

    protected override void OnRotate()
    {
        // 스파크 타워는 회전하는 않는다.
    }

    protected override void OnAttack(Enemy target)
    {
        // 나를 중심으로 spark range만큼의 반지름을 가지는 원형 체크.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sparkRange, attackMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Enemy hit = hits[i].GetComponent<Enemy>();      // i번째 콜라이더에게서 Enemy 컴포넌트 검색.
            if (hit == null)                                // 만약 컴포넌트가 없다면.
                continue;                                   // 해당 루프 건너뜀.

            hit.OnDamage(attackPower);
        }
    }

}
