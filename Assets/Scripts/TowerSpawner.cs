using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public LayerMask towerGroundMask;   // 설치 땅 레이어.
    public TowerGroup[] towerGroups;    // 타입별 타워 그룹 배열.

    private bool isSetMode;             // 타워 설치 중인가?

    private void Start()
    {
        TowerButtonManager.Instance.Setup(towerGroups);

        GameManager.Instance.onStartWave += OnStartWave;
        GameManager.Instance.onEndWave += OnEndWave;
    }

    // 하나의 웨이브가 시작되었다, 끝이 났다.
    private void OnStartWave()
    {

    }
    private void OnEndWave()
    {

    }

    private Tower GetPrefab(Tower.TYPE type)
    {
        // 프리팹 그룹에서 type에 해당하는 그룹을 검색한다.
        // Linq 쿼리문.
        var find = from g in towerGroups        // 어디서부터?
                   where g.type == type         // 무슨 조건으로?
                   select g.firstTower;         // 무슨 값을 가져 갈것인가?

        // 검색 결과 Tower들을 가지고 있지만 어차피 하나만 들고있기 때문에 First로 받아옴.
        return find.First();
    }
    private Tower GetNextPrefab(Tower tower)
    {
        var find = from g in towerGroups            // 어디서부터?
                   where g.type == tower.towerType  // 무슨 조건으로?
                   select g;                        // 무슨 값을 가져 갈것인가?

        TowerGroup group = find.First();

        // 레벨은 1부터, 실제 인덱스는 0부터 시작하기 때문에...
        int nextIndex = tower.towerLevel;

        // 만약 최대 레벨을 넘어선다면 생성할 수 없다.
        if(group.MaxLevel <= nextIndex)
        {
            return null;
        }
        else
        {
            // 내가 원하는 다음 타워.
            return group.towers[nextIndex];
        }
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

    public void OnUpgradeTower(Tower tower)
    {
        Tower nextLevelPrefab = GetNextPrefab(tower);    // 매개변수 tower의 다음 레벨 타워를 검색한다.
        if (nextLevelPrefab == null)
            return;

        if(GameManager.Instance.OnUseGold(nextLevelPrefab.towerPrice))
        {
            TowerGround ground = tower.ground;      // 기존에 설치되어있던 땅 대입.
            tower.OnDeselect();                     // 비선택.
            Destroy(tower.gameObject);              // 삭제.

            Tower newTower = Instantiate(nextLevelPrefab);  // 새로운 클론 생성.
            ground.SetTower(newTower);                      // 땅에 설치.
        }
        else
        {
            MessagePopup.Instance.Show("골드가 부족합니다.");
        }
        
    }
    public void OnSellTower(Tower tower)
    {
        int sellPrice = tower.sellPrice;    // 판매가격.
        tower.OnDeselect();                 // 선택해제.
        Destroy(tower.gameObject);          // 삭제.
        GameManager.Instance.OnAddGold(sellPrice);  // 돈 얻기.
    }
}
