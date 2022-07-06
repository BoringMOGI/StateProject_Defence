using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemySpawner : Singleton<EnemySpawner>
{
    public Enemy enemyPrefab;           // 적 프리팹.
    public int spawnCount;              // 생성 개수.
    public float spawnRate;             // 생성 간격(시간)
    public Transform wayPointParnet;    // 목적지의 부모 오브젝트.

    private Transform[] waypoints;      // 목적지 정보.

    private void Start()
    {
        // 목적지 검색.
        waypoints = new Transform[wayPointParnet.childCount];   // 부모 오브젝트의 자식의 개수만큼 배열 생성.
        for (int i = 0; i < wayPointParnet.childCount; i++)
            waypoints[i] = wayPointParnet.GetChild(i);

    }

    public void Spawn(System.Action onEndWave)
    {
        StartCoroutine(SpawnProcess(onEndWave));
    }
    IEnumerator SpawnProcess(System.Action onEndWave)
    {
        int deadCount = 0;        // 적의 죽은 개수.
        int amount = spawnCount;  // 생성 개수.

        WaitForSeconds waitForRate = new WaitForSeconds(spawnRate);

        while(amount > 0)
        {
            yield return waitForRate; // spawnRate만큼 대기하라.

            Enemy newEnemy = Instantiate(enemyPrefab, transform);
            newEnemy.Setup(waypoints, () => { deadCount += 1; });
            newEnemy.name = string.Concat("Enemy", amount);
            amount -= 1;
        }

        // 죽음 카운트가 생성 카운트보다 적을 경우 대기.
        while (deadCount < spawnCount)
            yield return null;

        // 웨이브가 끝났다.
        onEndWave?.Invoke();
    }

}
