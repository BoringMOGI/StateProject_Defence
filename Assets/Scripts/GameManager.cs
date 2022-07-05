using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameInfoUI gameInfoUI;

    public int wave;        // 몇 웨이브인지?
    public int gold;        // 소지금.
    public int hp;          // 체력.
    public bool isWaving;   // 웨이브를 진행중인가?

    public delegate void FuntionEvent();
    public event FuntionEvent onStartWave;      // 웨이브 시작 이벤트.
    public event FuntionEvent onEndWave;        // 웨이브 종료 이벤트.

    private void Start()
    {
        UpdateInfoUI();
    }
    private void Update()
    {
        // 옵션창 열기, Pause.
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OptionWindow.Instance.SwitchOption();
        }
    }

    public void OnStartWave()
    {
        // 웨이브를 진행 중일때는 중복으로 시작하지 않는다.
        if (isWaving)
            return;

        isWaving = true;
        onStartWave?.Invoke();          // 이벤트 등록 함수 호출.

        Debug.Log("OnStartWave");
    }
    public void OnEndWave()
    {
        wave += 1;                      // 웨이브가 끝났기 때문에 다음 웨이브로 변환.
        isWaving = false;
        onEndWave?.Invoke();            // 이벤트 등록 함수 호출.

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
        if(IsEnoughGold(amount))      // 골드가 요구량 이상이 경우.
        {
            gold -= amount;     // 골드 감소.
            UpdateInfoUI();     // UI 갱신.
            return true;        // 결과값 반환.
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


public static class CTime
{
    public static bool isPause = false;
    public static float deltaTime => Time.deltaTime * (isPause ? 0.0f : 1.0f);
}