using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISelect
{
    void OnSelect();
    void OnDeselect();
}

public class ObjectSelector : MonoBehaviour
{
    private Camera cam;
    private ISelect current;        // ���� ���� ���� ������Ʈ.
    private EventSystem eventSystem;


    private void Start()
    {
        cam = Camera.main;                  // ���� ī�޶�.
        eventSystem = EventSystem.current;  // ���� UI �̺�Ʈ ó����.
    }
    private void Update()
    {
        // ������ ������Ʈ�� �����ϸ� �̺�Ʈ�� �߻����� �ʴ´�.
        // ���� UI���� ���콺 �����Ͱ� �ִٸ� �۵����� �ʴ´�.
        if(Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject())
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);  // ���콺 ��ġ -> ���� ������.
            RaycastHit hit;
            ISelect target = null;
            if(Physics.Raycast(pos, Vector3.forward, out hit))
                target = hit.collider.GetComponent<ISelect>();

            // ������ ���� ������Ʈ�� �����ϸ� �������� �ʴ´�.
            if (current != null && current == target)
                return;

            // current�� ���߿� �����Ǹ� ������ ����Ų��.
            try
            {
                current?.OnDeselect();      // ���� ���� ��Ȱ��ȭ.
            }
            catch
            {
                Debug.Log("����ó��");
                current = null;
            }
            
            target?.OnSelect();         // ���ο� ���� Ȱ��ȭ.
            current = target;
        }
    }
}
