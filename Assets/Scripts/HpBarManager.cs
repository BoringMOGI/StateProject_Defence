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
        // Instantiate�ÿ� ���忡�� �����ǰ� �Ǹ� ���� �������� �������� 1:1 ��Ī�ȴ�.
        // ���� ���ʿ� ���鶧���� Canvas�ؿ��� �����Ѵ�. (=>�׷��� ĵ������ �������� ���󰣴�)
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
