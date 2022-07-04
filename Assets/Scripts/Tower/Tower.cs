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
        Ready,      // 대기.
        Search,     // 탐색.
        Attack,     // 공격.
    }

    [Header("Info")]
    public Sprite towerSprite;
    public string towerName;
    public int towerPrice;
    public TYPE towerType;      // 타워의 타입.

    [Header("Weapon")]
    public float attackRange;   // 공격 범위.
    public float attackRate;    // 공격 주기.
    public float attackPower;   // 공격력.
    public LayerMask attackMask;// 공격 대상 마스크.

    [Header("Etc")]
    public LineRenderer lineRenderer;

    // Mathf.Rount : float (반올림)
    // Mathf.Ceil : float (올림)
    // Mathf.Floor : float (내림)
    public int sellPrice => Mathf.RoundToInt(towerPrice * 0.4f);



    private STATE state;        // 상태.
    private bool isSet;         // 세팅이 되었다.
    private Enemy target;       // 공격 대상.

    protected float nextAttackTime;   // 다음 공격 시간.

    private void Start()
    {        
        int segmenet = 360;
        lineRenderer.useWorldSpace = false;     // 월드 기준으로 사용하지 않겠다.
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.loop = true;               // 라인의 반복 (마지막과 처음을 연결)
        lineRenderer.positionCount = segmenet;  // segment만큼 포지션 카운트 세팅.

        Vector3[] points = new Vector3[segmenet];   // 정점.
        for(int i = 0; i<points.Length; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segmenet);
            points[i] = new Vector3(Mathf.Sin(rad) * attackRange, Mathf.Cos(rad) * attackRange, 0f);
        }

        lineRenderer.SetPositions(points);      // 계산한 정점의 위치를 대입.
    }

    void Update()
    {
        if (!isSet)
            return;

        // 상태머신 (state-machine)
        // => 나의 상태에 따라서 특정한 행동을 하게 만든다.
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

    // 타워가 설치된 후 초기화 되는 함수.
    public void Setup()
    {
        state = STATE.Ready;
        isSet = true;
        target = null;

        lineRenderer.enabled = false;

        // 게임이 진행 중일때 설치하면 바로 탐색 모드로 들어간다.
        if (GameManager.Instance.isWaving)
            state = STATE.Search;

        // 이벤트 등록.
        // 함수에도 고유 ID가 있기 때문에 구분이 가능하다.
        GameManager.Instance.onStartWave += OnStartWave;
        GameManager.Instance.onEndWave += OnEndWave;
    }
    private void OnDestroy()
    {
        // 오브젝트가 삭제되었을 때 불리는 이벤트 함수.
        GameManager.Instance.onStartWave -= OnStartWave;
        GameManager.Instance.onEndWave -= OnEndWave;
    }

    private void OnStartWave()
    {
        // 웨이브가 시작되었으니 탐지를 시작한다.
        state = STATE.Search;
    }
    private void OnEndWave()
    {
        // 웨이브가 끝났으니 동작을 쉰다.
        state = STATE.Ready;
    }

    private void Search()
    {
        // attackRange의 반지름을 가지는 원형 범위를 체크한다.
        // 이때, 해당 범위에 들어온 attackMask 콜라이더가 있다면 배열에 추가한다.
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, attackMask);
        if(targets != null && targets.Length > 0)
        {
            // 이 중에서 내 공격범위에 들어온 대상만 체크.
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
        // 만약 공격 대상이 죽어버리면 다시 탐색한다.
        // 만약 공격대상이 나의 공격 범위보다 멀리 갈 경우 다시 탐색한다.
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
            OnAttack(target);           // 공격하라!
        }
    }

    protected virtual void OnRotate()
    {
        // 타워의 회전.
        Vector3 dir = (target.transform.position - transform.position).normalized;  // 방향.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                    // 2차원 기준으로 dir까지의 각도.
        Quaternion lookAt = Quaternion.AngleAxis(angle, Vector3.forward);           // 정면 축 기준으로 angle만큼 돌렸을 때의 쿼터니언 값.

        // Lerp를 통해서 자연스러운 값 변화를 유도.
        // A에서 B까지의 변화를 비율로 변화.
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

        Debug.Log(name);

        lineRenderer.enabled = false;
        TowerInfoUI.Instance.OnClose();
        TowerControlUI.Instance.Close();
    }
}
