using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public Transform target;   // 추적할 플레이어
    public float minBounds;  // 카메라 이동 최소 제한
    public float maxBounds;  // 카메라 이동 최대 제한
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

        pos.y = Mathf.Clamp(pos.y, minBounds, maxBounds);  // 카메라 위치 제한

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
