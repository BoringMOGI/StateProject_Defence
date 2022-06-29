using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCannon : Singleton<TestCannon>
{
    public LineRenderer line;

    private void Start()
    {
        line.positionCount = 2;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
    }

    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);
        Vector2 point = transform.position + (transform.right * 10f);

        if (hit.collider != null)
        {
            point = hit.collider.transform.position;
            point.y = transform.position.y;
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, point);
    }

}
