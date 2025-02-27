using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
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

        CameraUpdateBounds();
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

    public void CameraUpdateBounds()
    {
        if (DungeonManager.Instance != null && DungeonManager.Instance.currentDungeonData != null)
        {
            minBounds = DungeonManager.Instance.currentDungeonData.minCameraBounds;
            maxBounds = DungeonManager.Instance.currentDungeonData.maxCameraBounds;
        }
    }

}
