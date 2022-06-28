using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public Tower.TYPE spawnType;
    public LayerMask towerGroundMask;
    public Tower[] towerPrefabs;

    private List<Tower> towerList;
    private bool isStartWave;           // ���̺갡 �������ΰ�?
    private bool isSetMode;             // Ÿ�� ��ġ ���ΰ�?

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
        if (isSetMode)
            return;

        isSetMode = true;

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
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = mousePos;
            targetPos.z = 0;

            // ���� ���� Ÿ���� �������� ���콺 ���������� ����.
            target.position = targetPos;

            // ���̸� �̿��� Ÿ�� �׶��� ã��.
            TowerGround ground = null;
            RaycastHit hit;
            if(Physics.Raycast(mousePos, Vector3.forward, out hit, float.MaxValue, towerGroundMask))
            {
                ground = hit.collider.GetComponent<TowerGround>();
                target.position = ground.transform.position;                                
            }

            // ���콺 ��ư Ŭ��.
            if(Input.GetMouseButtonDown(0))
            {
                // �׶��尡 �ƴ� �� ���� Ŭ��. (��ġ ��� ���)
                if (ground == null)
                {
                    Destroy(newTower.gameObject);
                    break;
                }
                // �׶��带 Ŭ���ߴµ� ��ġ ���� ������ ���.
                // �׸��� ��ġ�Ϸ��� Ÿ���� 
                else if (ground.IsSetTower())
                { 
                    GameManager.Instance.OnUseGold(newTower.towerPrice);
                    ground.SetTower(newTower);
                    break;
                }
            }

            yield return null;
        }

        // Ÿ�� ��ġ ��� ����.
        isSetMode = false;
    }


    // Ÿ�� ����.
    public int GetTowerPrice(Tower.TYPE type)
    {
        Tower target = GetPrefab(type);

        //return target?.towerPrice ?? 99999;
        return (target != null) ? target.towerPrice : 999999;
    }
}
