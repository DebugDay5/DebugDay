using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonSO : ScriptableObject
{
    public Vector2 minBounds; // �̵� ������ �ּ� x, y ��
    public Vector2 maxBounds; // �̵� ������ �ִ� x, y ��
    public float minCameraBounds;  // ī�޶� �ּ� �̵� ��
    public float maxCameraBounds;  // ī�޶� �ִ� �̵� ��
}
