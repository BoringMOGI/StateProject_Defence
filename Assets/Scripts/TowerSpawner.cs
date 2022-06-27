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

        TowerButtonManager.Instance.Setup(towerPrefabs);
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

    public void OnSelectTowerType(int index)
    {
        Tower.TYPE type = (Tower.TYPE)index;
        Debug.Log($"Ÿ�� Ÿ�� ���� : {type}");
        spawnType = type;
    }


    private Tower GetPrefab(Tower.TYPE type)
    {
        // ������ �迭���� type�� �ش��ϴ� Ÿ���� �˻��Ѵ�.
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

    // Ÿ�� ��ġ ��û.
    public void OnRequestTower(Tower.TYPE type)
    {
        Tower newTower = Instantiate(GetPrefab(type));
        StartCoroutine(OnSetMode(newTower)); // �������� Ŭ������ ����.
    }
    IEnumerator OnSetMode(Tower newTower)
    {
        Camera cam = Camera.main;
        Transform target = newTower.transform;
        while(true)
        {
            // ���콺 ��ǥ(Screen)�� ���� ��ǥ�� ��ȯ. (��, z���� ī�޶��� z�� ���� ������ 0���� ����)
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;                        
            target.position = pos;  // ���� ���� Ÿ���� �������� ���콺 ���������� ����.

            yield return null;
        }
    }


    // Ÿ�� ����.
    public int GetTowerPrice(Tower.TYPE type)
    {
        Tower target = GetPrefab(type);

        //return target?.towerPrice ?? 99999;
        return (target != null) ? target.towerPrice : 999999;
    }
}
