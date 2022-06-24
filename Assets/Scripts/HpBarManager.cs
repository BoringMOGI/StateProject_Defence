using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : Singleton<HpBarManager>
{
    public HpBar prefab;
    public int poolCount;

    private Stack<HpBar> stack;

    private void Start()
    {
        stack = new Stack<HpBar>();
        for (int i = 0; i < poolCount; i++)
            CreateHpBar();
    }

    private void CreateHpBar()
    {
        // Instantiate시에 월드에서 생성되게 되면 월드 기준으로 스케일이 1:1 매칭된다.
        // 따라서 애초에 만들때부터 Canvas밑에서 생성한다. (=>그래서 캔버스의 스케일을 따라간다)
        HpBar newHpBar = Instantiate(prefab, transform);
        newHpBar.gameObject.SetActive(false);
        stack.Push(newHpBar);
    }

    public HpBar GetHpBar()
    {
        if (stack.Count <= 0)
            CreateHpBar();

        HpBar hpbar = stack.Pop();
        hpbar.gameObject.SetActive(true);
        return hpbar;
    }

    public void OnReturn(HpBar hpBar)
    {
        hpBar.gameObject.SetActive(false);
        stack.Push(hpBar);
    }

}
