using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonSO : ScriptableObject
{
    public Vector2 minBounds; // 이동 가능한 최소 x, y 값
    public Vector2 maxBounds; // 이동 가능한 최대 x, y 값
    public float minCameraBounds;  // 카메라 최소 이동 값
    public float maxCameraBounds;  // 카메라 최대 이동 값
}
