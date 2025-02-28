using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonSO : ScriptableObject
{
    public Vector2 minPlayerBounds; // �÷��̾��� �̵� ������ �ּ� x, y ��
    public Vector2 maxPlayerBounds; // �÷��̾��� �̵� ������ �ִ� x, y ��
    public float minCameraBounds;  // ī�޶� �ּ� �̵� ��
    public float maxCameraBounds;  // ī�޶� �ִ� �̵� ��
}
