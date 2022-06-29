using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;           // �� ������.
    public int spawnCount;              // ���� ����.
    public float spawnRate;             // ���� ����(�ð�)
    public Transform wayPointParnet;    // �������� �θ� ������Ʈ.
    public UnityEvent OnEndWave;        // (UnityEvent : ������Ʈ�� �����ų �� �ִ� ��������Ʈ)

    private Transform[] waypoints;      // ������ ����.

    private void Start()
    {
        // ������ �˻�.
        waypoints = new Transform[wayPointParnet.childCount];   // �θ� ������Ʈ�� �ڽ��� ������ŭ �迭 ����.
        for (int i = 0; i < wayPointParnet.childCount; i++)
            waypoints[i] = wayPointParnet.GetChild(i);

        // �̺�Ʈ ���.
        GameManager.Instance.onStartWave += Spawn;
    }
    public void Spawn()
    {
        StartCoroutine(SpawnProcess());
    }
    IEnumerator SpawnProcess()
    {
        int deadCount = 0;        // ���� ���� ����.
        int amount = spawnCount;  // ���� ����.

        WaitForSeconds waitForRate = new WaitForSeconds(spawnRate);

        while(amount > 0)
        {
            yield return waitForRate; // spawnRate��ŭ ����϶�.

            Enemy newEnemy = Instantiate(enemyPrefab, transform);
            newEnemy.Setup(waypoints, () => { deadCount += 1; });
            newEnemy.name = string.Concat("Enemy", amount);
            amount -= 1;
        }

        // ���� ī��Ʈ�� ���� ī��Ʈ���� ���� ��� ���.
        while (deadCount < spawnCount)
            yield return null;

        // ���̺갡 ������.
        OnEndWave?.Invoke();
    }

}
