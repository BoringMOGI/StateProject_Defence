using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TowerSpawner towerSpawner;
    public EnemySpawner enemySpawner;
    public GameInfoUI gameInfoUI;

    public int wave;        // 몇 웨이브인지?
    public int gold;        // 소지금.
    public int hp;          // 체력.
    public bool isWaving;   // 웨이브를 진행중인가?

    private void Start()
    {
        UpdateInfoUI();
    }

    public void OnStartWave()
    {
        // 웨이브를 진행 중일때는 중복으로 시작하지 않는다.
        if (isWaving)
            return;

        towerSpawner.OnStartWave();     // 타워들에게 웨이브 시작을 전달.
        enemySpawner.Spawn();           // 적 생성기에게 스폰 명령 전달.
        isWaving = true;

        Debug.Log("OnStartWave");
    }
    public void OnEndWave()
    {
        towerSpawner.OnEndWave();       // 타워들에게 웨이브 종료를 전달.
        isWaving = false;
        wave += 1;                      // 웨이브가 끝났기 때문에 다음 웨이브로 변환.

        UpdateInfoUI();                 // UI업데이트.
        Debug.Log("OnEndWave");
    }

    // 적이 골에 도착해서 체력을 깍는다.
    public void OnDamageHp()
    {
        hp -= 1;
        if(hp <= 0)
        {
            // 게임 종료.
            GameOver();            
        }

        UpdateInfoUI();         // UI 갱신.
    }
    public void OnAddGold(int amount)
    {
        gold += amount;
        UpdateInfoUI();
    }
    public bool OnUseGold(int amount)
    {
        if(gold >= amount)      // 골드가 요구량 이상이 경우.
        {
            gold -= amount;     // 골드 감소.
            UpdateInfoUI();     // UI 갱신.
            return true;        // 결과값 반환.
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
