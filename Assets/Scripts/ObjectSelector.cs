using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelect
{
    void OnSelect();
    void OnDeselect();
}

public class ObjectSelector : MonoBehaviour
{
    private Camera cam;
    private ISelect current;        // 현재 선택 중인 오브젝트.

    private void Start()
    {
        cam = Camera.main;      // 메인 카메라.
    }
    private void Update()
    {
        // 동일한 오브젝트를 선택하면 이벤트가 발생하지 않는다.
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);  // 마우스 위치 -> 월드 포지션.
            RaycastHit hit;
            ISelect target = null;
            if(Physics.Raycast(pos, Vector3.forward, out hit))
                target = hit.collider.GetComponent<ISelect>();

            // 이전과 이후 오브젝트가 동일하면 실행하지 않는다.
            if (current != null && current == target)
                return;

            current?.OnDeselect();      // 이전 선택 비활성화.
            target?.OnSelect();         // 새로운 선택 활성화.

            current = target;
        }
    }
}
