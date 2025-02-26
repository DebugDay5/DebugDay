using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class DungeonChanger : MonoBehaviour
{
    public DungeonSO dungeonSO;

    public PlayerController playerController;
    public TestCamera_KHY cameraController;

    public void ChangeDungeon()
    {
        DungeonManager.Instance.SetCurrentMap(dungeonSO);  // �� ������ ������Ʈ

        playerController.UpdateBounds();  // �÷��̾� �̵� ���� ������Ʈ
        cameraController.CameraUpdateBounds();  // ī�޶� �̵� ���� ������Ʈ
    }

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<TestCamera_KHY>();
    }
    private void Start()
    {
        ChangeDungeon();
    }

}
