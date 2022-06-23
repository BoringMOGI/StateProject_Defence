using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TowerSpawner towerSpawner;
    public EnemySpawner enemySpawner;
    public GameInfoUI gameInfoUI;

    public int wave;        // �� ���̺�����?
    public int gold;        // ������.
    public int hp;          // ü��.
    public bool isWaving;   // ���̺긦 �������ΰ�?

    private void Start()
    {
        UpdateInfoUI();
    }

    public void OnStartWave()
    {
        // ���̺긦 ���� ���϶��� �ߺ����� �������� �ʴ´�.
        if (isWaving)
            return;

        towerSpawner.OnStartWave();     // Ÿ���鿡�� ���̺� ������ ����.
        enemySpawner.Spawn();           // �� �����⿡�� ���� ��� ����.
        isWaving = true;

        Debug.Log("OnStartWave");
    }
    public void OnEndWave()
    {
        towerSpawner.OnEndWave();       // Ÿ���鿡�� ���̺� ���Ḧ ����.
        isWaving = false;
        wave += 1;                      // ���̺갡 ������ ������ ���� ���̺�� ��ȯ.

        UpdateInfoUI();                 // UI������Ʈ.
        Debug.Log("OnEndWave");
    }

    // ���� �� �����ؼ� ü���� ��´�.
    public void OnDamageHp()
    {
        hp -= 1;
        if(hp <= 0)
        {
            // ���� ����.
            GameOver();            
        }

        UpdateInfoUI();         // UI ����.
    }
    public void OnAddGold(int amount)
    {
        gold += amount;
        UpdateInfoUI();
    }
    public bool OnUseGold(int amount)
    {
        if(gold >= amount)      // ��尡 �䱸�� �̻��� ���.
        {
            gold -= amount;     // ��� ����.
            UpdateInfoUI();     // UI ����.
            return true;        // ����� ��ȯ.
        }

        return false;
    }

    private void GameOver()
    {
        Debug.Log("Game Over...");
    }
    private void GameClear()
    {

    }

    public void UpdateInfoUI()
    {
        gameInfoUI.UpdateInfo(gold, hp, wave);
    }

}
