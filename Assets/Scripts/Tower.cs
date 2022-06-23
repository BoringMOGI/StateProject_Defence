using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private enum STATE
    {
        Ready,      // ���.
        Search,     // Ž��.
        Attack,     // ����.
    }

    public float attackRange;   // ���� ����.
    public float attackRate;    // ���� �ֱ�.
    public float attackPower;   // ���ݷ�.
    public float bulletSpeed;   // ź��.
    public LayerMask attackMask;// ���� ��� ����ũ.
    public Bullet bulletPrefab; // ź.

    private STATE state;        // ����.
    private bool isSet;         // ������ �Ǿ���.
    private Enemy target;       // ���� ���.

    private float nextAttackTime;   // ���� ���� �ð�.

    void Update()
    {
        if (!isSet)
            return;

        // ���¸ӽ� (state-machine)
        // => ���� ���¿� ���� Ư���� �ൿ�� �ϰ� �����.
        switch(state)
        {
            case STATE.Ready:
                break;
            case STATE.Search:
                Search();
                break;
            case STATE.Attack:
                Attack();
                break;
        }
    }

    // Ÿ���� ��ġ�� �� �ʱ�ȭ �Ǵ� �Լ�.
    public void Setup()
    {
        state = STATE.Ready;
        isSet = true;
        target = null;
    }
    public void OnStartWave()
    {
        // ���̺갡 ���۵Ǿ����� Ž���� �����Ѵ�.
        state = STATE.Search;
    }
    public void OnEndWave()
    {
        // ���̺갡 �������� ������ ����.

        state = STATE.Ready;
    }

    private void Search()
    {
        // attackRange�� �������� ������ ���� ������ üũ�Ѵ�.
        // �̶�, �ش� ������ ���� attackMask �ݶ��̴��� �ִٸ� �迭�� �߰��Ѵ�.
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, attackMask);
        if(targets != null && targets.Length > 0)
        {
            // �� �߿��� �� ���ݹ����� ���� ��� üũ.
            foreach(Collider2D target in targets)
            {
                if(Vector3.Distance(transform.position, target.transform.position) <= attackRange)
                {
                    this.target = target.GetComponent<Enemy>();
                    state = STATE.Attack;
                    Debug.Log("target on : " + target.name);
                    return;
                }
            }
        }
    }
    private void Attack()
    {
        // ���� ���� ����� �׾������ �ٽ� Ž���Ѵ�.
        // ���� ���ݴ���� ���� ���� �������� �ָ� �� ��� �ٽ� Ž���Ѵ�.
        if (target == null || Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            target = null;
            state = STATE.Search;
            Debug.Log("target lost.. research!");
            return;
        }

        // Ÿ���� ȸ��.
        Vector3 dir = (target.transform.position - transform.position).normalized;  // ����.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                    // 2���� �������� dir������ ����.
        Quaternion lookAt = Quaternion.AngleAxis(angle, Vector3.forward);           // ���� �� �������� angle��ŭ ������ ���� ���ʹϾ� ��.

        // Lerp�� ���ؼ� �ڿ������� �� ��ȭ�� ����.
        // A���� B������ ��ȭ�� ������ ��ȭ.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, 30f * Time.deltaTime);

        // �����϶�!
        if(Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;            // ���� ���� �ð�.
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // �Ѿ� ������ ����.
            bullet.Setup(target, bulletSpeed, attackPower);     // �Ѿ� ����.
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
