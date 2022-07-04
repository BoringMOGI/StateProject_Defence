using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerSpawner : Singleton<TowerSpawner>
{
    public LayerMask towerGroundMask;   // ��ġ �� ���̾�.
    public TowerGroup[] towerGroups;    // Ÿ�Ժ� Ÿ�� �׷� �迭.

    private bool isSetMode;             // Ÿ�� ��ġ ���ΰ�?

    private void Start()
    {
        TowerButtonManager.Instance.Setup(towerGroups);

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

    private Tower GetPrefab(Tower.TYPE type)
    {
        // ������ �׷쿡�� type�� �ش��ϴ� �׷��� �˻��Ѵ�.
        // Linq ������.
        var find = from g in towerGroups        // ��𼭺���?
                   where g.type == type         // ���� ��������?
                   select g.firstTower;         // ���� ���� ���� �����ΰ�?

        // �˻� ��� Tower���� ������ ������ ������ �ϳ��� ����ֱ� ������ First�� �޾ƿ�.
        return find.First();
    }
    private Tower GetNextPrefab(Tower tower)
    {
        var find = from g in towerGroups            // ��𼭺���?
                   where g.type == tower.towerType  // ���� ��������?
                   select g;                        // ���� ���� ���� �����ΰ�?

        TowerGroup group = find.First();

        // Towers�迭���� �Ű����� tower�� index�� ���ΰ�?
        int currentIndex = System.Array.IndexOf(group.towers, tower);
        currentIndex++;
        Debug.Log(currentIndex);
        
        // ���� �ִ� ������ �Ѿ�ٸ� ������ �� ����.
        if(group.towers.Length <= group.MaxLevel)
        {
            return null;
        }
        else
        {
            return group.towers[currentIndex];
        }
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

    public void OnUpgradeTower(Tower tower)
    {
        Tower prefab = GetNextPrefab(tower);    // �Ű����� tower�� ���� ���� Ÿ���� �˻��Ѵ�.
        Debug.Log(prefab.name);
    }
    public void OnSellTower(Tower tower)
    {
        int sellPrice = tower.sellPrice;    // �ǸŰ���.
        tower.OnDeselect();                 // ��������.
        Destroy(tower.gameObject);          // ����.
        GameManager.Instance.OnAddGold(sellPrice);  // �� ���.
    }
}
