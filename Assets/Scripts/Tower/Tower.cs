using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour, ISelect
{
    [System.Serializable]
    public enum TYPE
    {
        Projectile,
        Explode,
        Spark,
    }

    private enum STATE
    {
        Ready,      // ���.
        Search,     // Ž��.
        Attack,     // ����.
    }

    [Header("Info")]
    public Sprite towerSprite;
    public string towerName;
    public int towerPrice;
    public TYPE towerType;      // Ÿ���� Ÿ��.
    public int towerLevel;      // Ÿ���� ����.

    [Header("Weapon")]
    public float attackRange;   // ���� ����.
    public float attackRate;    // ���� �ֱ�.
    public float attackPower;   // ���ݷ�.
    public LayerMask attackMask;// ���� ��� ����ũ.

    [Header("Etc")]
    public LineRenderer lineRenderer;
    public TowerGround ground;          // ��ġ�� ��.

    // Mathf.Rount : float (�ݿø�)
    // Mathf.Ceil : float (�ø�)
    // Mathf.Floor : float (����)
    public int sellPrice => Mathf.RoundToInt(towerPrice * 0.4f);

    private STATE state;        // ����.
    private bool isSet;         // ������ �Ǿ���.
    private Enemy target;       // ���� ���.
    

    protected float nextAttackTime;   // ���� ���� �ð�.


    private void Start()
    {        
        int segmenet = 360;
        lineRenderer.useWorldSpace = false;     // ���� �������� ������� �ʰڴ�.
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.loop = true;               // ������ �ݺ� (�������� ó���� ����)
        lineRenderer.positionCount = segmenet;  // segment��ŭ ������ ī��Ʈ ����.

        Vector3[] points = new Vector3[segmenet];   // ����.
        for(int i = 0; i<points.Length; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segmenet);
            points[i] = new Vector3(Mathf.Sin(rad) * attackRange, Mathf.Cos(rad) * attackRange, 0f);
        }

        lineRenderer.SetPositions(points);      // ����� ������ ��ġ�� ����.
    }

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
    public void Setup(TowerGround ground)
    {
        this.ground = ground;
        state = STATE.Ready;
        isSet = true;
        target = null;

        lineRenderer.enabled = false;

        // ������ ���� ���϶� ��ġ�ϸ� �ٷ� Ž�� ���� ����.
        if (GameManager.Instance.isWaving)
            state = STATE.Search;

        // �̺�Ʈ ���.
        // �Լ����� ���� ID�� �ֱ� ������ ������ �����ϴ�.
        GameManager.Instance.onStartWave += OnStartWave;
        GameManager.Instance.onEndWave += OnEndWave;
    }
    private void OnDestroy()
    {
        // ������Ʈ�� �����Ǿ��� �� �Ҹ��� �̺�Ʈ �Լ�.
        GameManager.Instance.onStartWave -= OnStartWave;
        GameManager.Instance.onEndWave -= OnEndWave;
    }

    private void OnStartWave()
    {
        // ���̺갡 ���۵Ǿ����� Ž���� �����Ѵ�.
        state = STATE.Search;
    }
    private void OnEndWave()
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

        OnRotate(); 

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            OnAttack(target);           // �����϶�!
        }
    }

    protected virtual void OnRotate()
    {
        // Ÿ���� ȸ��.
        Vector3 dir = (target.transform.position - transform.position).normalized;  // ����.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                    // 2���� �������� dir������ ����.
        Quaternion lookAt = Quaternion.AngleAxis(angle, Vector3.forward);           // ���� �� �������� angle��ŭ ������ ���� ���ʹϾ� ��.

        // Lerp�� ���ؼ� �ڿ������� �� ��ȭ�� ����.
        // A���� B������ ��ȭ�� ������ ��ȭ.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, 30f * Time.deltaTime);
    }
    protected virtual void OnAttack(Enemy target)
    {
    }

    public void OnSelect()
    {
        if (!isSet)
            return;

        lineRenderer.enabled = true;
        TowerInfoUI.Instance.OnShow(this);
        TowerControlUI.Instance.Open(this);
    }
    public void OnDeselect()
    {
        if (!isSet)
            return;

        lineRenderer.enabled = false;
        TowerInfoUI.Instance.OnClose();
        TowerControlUI.Instance.Close();
    }
}
