using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public Tower.TYPE spawnType;
    public LayerMask towerGroundMask;
    public Tower[] towerPrefabs;

    private List<Tower> towerList;
    private bool isSetMode;             // Ÿ�� ��ġ ���ΰ�?

    private void Start()
    {
        towerList = new List<Tower>();

        TowerButtonManager.Instance.Setup(towerPrefabs);

        GameManager.Instance.onStartWave += OnStartWave;
        GameManager.Instance.onEndWave += OnEndWave;
    }

    // �ϳ��� ���̺갡 ���۵Ǿ���, ���� ����.
    private void OnStartWave()
    {

    }
    private void OnEndWave()
    {

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
