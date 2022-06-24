using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인터페이스 : 상속 시 무조건 내부 함수를 구현해야한다.
// 즉, 해당 함수가 있음을 보장한다.
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
        // 타겟을 잃어버리거나 타겟의 hp가 0이하라면
        // 죽었다고 판단해 풀 매니저로 되돌아간다.
        if(target == null || target.GetHp() <= 0)
        {
            HpBarManager.Instance.OnReturn(this);
            return;
        }

        // 타겟이 Destroy되면 Missing 에러가 뜨기 때문에 그에 대한 예외처리.
        try
        {
            // 게임 오브젝트는 월드좌표(world space), 체력바는 스크린좌표(screen space)
            // 따라서 카메라를 통해 월드 좌표를 스크린좌표로 변환해야한다.
            transform.position = target.GetPivot().position;
            hpImage.fillAmount = target.GetHp() / target.GetMaxHp();
        }
        catch
        {
            HpBarManager.Instance.OnReturn(this);
        }
    }

}