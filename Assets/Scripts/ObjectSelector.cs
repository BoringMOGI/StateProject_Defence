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
    private ISelect current;        // ���� ���� ���� ������Ʈ.

    private void Start()
    {
        cam = Camera.main;      // ���� ī�޶�.
    }
    private void Update()
    {
        // ������ ������Ʈ�� �����ϸ� �̺�Ʈ�� �߻����� �ʴ´�.
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);  // ���콺 ��ġ -> ���� ������.
            RaycastHit hit;
            ISelect target = null;
            if(Physics.Raycast(pos, Vector3.forward, out hit))
                target = hit.collider.GetComponent<ISelect>();

            // ������ ���� ������Ʈ�� �����ϸ� �������� �ʴ´�.
            if (current != null && current == target)
                return;

            current?.OnDeselect();      // ���� ���� ��Ȱ��ȭ.
            target?.OnSelect();         // ���ο� ���� Ȱ��ȭ.

            current = target;
        }
    }
}
