using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public Tower towerPrefab;

    private List<Tower> towerList;
    private bool isStartWave;           // 웨이브가 진행중인가?

    private void Start()
    {
        towerList = new List<Tower>();
    }

    // 타워 생성.
    public void OnRequestTower(TowerGround ground)
    {
        // Instantiate<T>(T, Transform) : T
        // => T형 오브젝트를 복제한 후 Transform의 자식으로 둔다.
        Tower newTower = Instantiate(towerPrefab, transform);
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
}
