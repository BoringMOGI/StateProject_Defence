using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public Tower.TYPE spawnType;
    public LayerMask towerGroundMask;
    public Tower[] towerPrefabs;

    private List<Tower> towerList;
    private bool isStartWave;           // 웨이브가 진행중인가?
    private bool isSetMode;             // 타워 설치 중인가?

    private void Start()
    {
        towerList = new List<Tower>();

        TowerButtonManager.Instance.Setup(towerPrefabs);
    }

    // 타워 생성.
    public void OnRequestTower(TowerGround ground)
    {
        // 배열중에서 원하는 타워 프리팹.
        Tower prefab = towerPrefabs[(int)spawnType];

        // Instantiate<T>(T, Transform) : T
        // => T형 오브젝트를 복제한 후 Transform의 자식으로 둔다.

        // 실제 클론 생성.
        Tower newTower = Instantiate(prefab, transform);
        newTower.transform.position = ground.transform.position;
        newTower.Setup();

        towerList.Add(newTower);

        // 타워 설치 시 현재 웨이브가 진행중이라면 알려준다.
        if (isStartWave)
            newTower.OnStartWave();
    }

    // 하나의 웨이브가 시작되었다, 끝이 났다.
    public void OnStartWave()
    {
        isStartWave = true;

        foreach (Tower tower in towerList)
            tower.OnStartWave();
    }
    public void OnEndWave()
    {
        isStartWave = false;

        foreach (Tower tower in towerList)
            tower.OnEndWave();
    }

    public void OnSelectTowerType(int index)
    {
        Tower.TYPE type = (Tower.TYPE)index;
        Debug.Log($"타워 타입 변경 : {type}");
        spawnType = type;
    }


    private Tower GetPrefab(Tower.TYPE type)
    {
        // 프리팹 배열에서 type에 해당하는 타워를 검색한다.
        Tower target = null;
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            Tower tower = towerPrefabs[i];
            if (tower.towerType == type)
            {
                target = tower;
                break;
            }
        }
        return target;
    }

    // 타워 설치 요청.
    public void OnRequestTower(Tower.TYPE type)
    {
        if (isSetMode)
            return;

        isSetMode = true;

        Tower newTower = Instantiate(GetPrefab(type));
        StartCoroutine(OnSetMode(newTower)); // 프리팹을 클론으로 복제.
    }
    IEnumerator OnSetMode(Tower newTower)
    {
        Camera cam = Camera.main;
        Transform target = newTower.transform;

        while(true)
        {
            // 마우스 좌표(Screen)을 월드 좌표로 변환. (단, z축은 카메라의 z와 같기 때문에 0으로 조정)
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = mousePos;
            targetPos.z = 0;

            // 새로 만든 타워의 포지션을 마우스 포지션으로 대입.
            target.position = targetPos;

            // 레이를 이용해 타워 그라운드 찾기.
            TowerGround ground = null;
            RaycastHit hit;
            if(Physics.Raycast(mousePos, Vector3.forward, out hit, float.MaxValue, towerGroundMask))
            {
                ground = hit.collider.GetComponent<TowerGround>();
                target.position = ground.transform.position;                                
            }

            // 마우스 버튼 클릭.
            if(Input.GetMouseButtonDown(0))
            {
                // 그라운드가 아닌 빈 공간 클릭. (설치 모드 취소)
                if (ground == null)
                {
                    Destroy(newTower.gameObject);
                    break;
                }
                // 그라운드를 클릭했는데 설치 가능 상태일 경우.
                // 그리고 설치하려는 타워의 
                else if (ground.IsSetTower())
                { 
                    GameManager.Instance.OnUseGold(newTower.towerPrice);
                    ground.SetTower(newTower);
                    break;
                }
            }

            yield return null;
        }

        // 타워 설치 모드 종료.
        isSetMode = false;
    }


    // 타워 가격.
    public int GetTowerPrice(Tower.TYPE type)
    {
        Tower target = GetPrefab(type);

        //return target?.towerPrice ?? 99999;
        return (target != null) ? target.towerPrice : 999999;
    }
}
