using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public Tower.TYPE spawnType;
    public Tower[] towerPrefabs;

    private List<Tower> towerList;
    private bool isStartWave;           // ���̺갡 �������ΰ�?

    private void Start()
    {
        towerList = new List<Tower>();
    }

    // Ÿ�� ����.
    public void OnRequestTower(TowerGround ground)
    {
        // �迭�߿��� ���ϴ� Ÿ�� ������.
        Tower prefab = towerPrefabs[(int)spawnType];

        // Instantiate<T>(T, Transform) : T
        // => T�� ������Ʈ�� ������ �� Transform�� �ڽ����� �д�.

        // ���� Ŭ�� ����.
        Tower newTower = Instantiate(prefab, transform);
        newTower.transform.position = ground.transform.position;
        newTower.Setup();

        towerList.Add(newTower);

        // Ÿ�� ��ġ �� ���� ���̺갡 �������̶�� �˷��ش�.
        if (isStartWave)
            newTower.OnStartWave();
    }

    // �ϳ��� ���̺갡 ���۵Ǿ���, ���� ����.
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
