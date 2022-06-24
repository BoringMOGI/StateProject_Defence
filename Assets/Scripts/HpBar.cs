using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �������̽� : ��� �� ������ ���� �Լ��� �����ؾ��Ѵ�.
// ��, �ش� �Լ��� ������ �����Ѵ�.
public interface IHpBar
{
    Transform GetPivot();
    float GetHp();
    float GetMaxHp();
}

public class HpBar : MonoBehaviour
{
    public Image hpImage;
    IHpBar target;
    Camera cam;

    public void Setup(IHpBar target)
    {
        if(cam == null)
            cam = Camera.main;

        this.target = target;
        transform.position = target.GetPivot().position;
    }

    private void Update()
    {
        // Ÿ���� �Ҿ�����ų� Ÿ���� hp�� 0���϶��
        // �׾��ٰ� �Ǵ��� Ǯ �Ŵ����� �ǵ��ư���.
        if(target == null || target.GetHp() <= 0)
        {
            HpBarManager.Instance.OnReturn(this);
            return;
        }

        // Ÿ���� Destroy�Ǹ� Missing ������ �߱� ������ �׿� ���� ����ó��.
        try
        {
            // ���� ������Ʈ�� ������ǥ(world space), ü�¹ٴ� ��ũ����ǥ(screen space)
            // ���� ī�޶� ���� ���� ��ǥ�� ��ũ����ǥ�� ��ȯ�ؾ��Ѵ�.
            transform.position = target.GetPivot().position;
            hpImage.fillAmount = target.GetHp() / target.GetMaxHp();
        }
        catch
        {
            HpBarManager.Instance.OnReturn(this);
        }
    }

}