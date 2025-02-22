using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera_KHY : MonoBehaviour
{
    public Transform target;   // ������ �÷��̾�
    public float minBounds;  // ī�޶� �̵� �ּ� ����
    public float maxBounds;  // ī�޶� �̵� �ִ� ����
    private float offsetY;

    private void Start()
    {
        if (target == null)
            return;

        offsetY = transform.position.y - target.position.y;
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos.y = target.position.y + offsetY;

        pos.y = Mathf.Clamp(pos.y, minBounds, maxBounds);  // ī�޶� ��ġ ����

        transform.position = pos;
    }

}
