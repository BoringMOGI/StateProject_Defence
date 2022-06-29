using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameInfoUI gameInfoUI;

    public int wave;        // �� ���̺�����?
    public int gold;        // ������.
    public int hp;          // ü��.
    public bool isWaving;   // ���̺긦 �������ΰ�?

    public delegate void FuntionEvent();
    public event FuntionEvent onStartWave;      // ���̺� ���� �̺�Ʈ.
    public event FuntionEvent onEndWave;        // ���̺� ���� �̺�Ʈ.

    private void Start()
    {
        UpdateInfoUI();
    }

    public void OnStartWave()
    {
        // ���̺긦 ���� ���϶��� �ߺ����� �������� �ʴ´�.
        if (isWaving)
            return;

        isWaving = true;
        onStartWave?.Invoke();          // �̺�Ʈ ��� �Լ� ȣ��.

        Debug.Log("OnStartWave");
    }
    public void OnEndWave()
    {
        wave += 1;                      // ���̺갡 ������ ������ ���� ���̺�� ��ȯ.
        isWaving = false;
        onEndWave?.Invoke();            // �̺�Ʈ ��� �Լ� ȣ��.

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
        if(IsEnoughGold(amount))      // ��尡 �䱸�� �̻��� ���.
        {
            gold -= amount;     // ��� ����.
            UpdateInfoUI();     // UI ����.
            return true;        // ����� ��ȯ.
        }

        return false;
    }
    public bool IsEnoughGold(int amount)
    {
        return gold >= amount;
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
