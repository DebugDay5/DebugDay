using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class DungeonChanger : MonoBehaviour
{
    public DungeonSO dungeonSO;
    public PlayerController playerController;
    public CameraPos cameraController;

    public GameObject[] enemys;  // �� �����Ҵ� ����

    public void ChangeDungeon()
    {
        DungeonManager.Instance.SetCurrentMap(dungeonSO);  // �� ������ ������Ʈ
        playerController.UpdateBounds();  // �÷��̾� �̵� ���� ������Ʈ
        cameraController.CameraUpdateBounds();  // ī�޶� �̵� ���� ������Ʈ
    }

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<CameraPos>();

        if (enemys == null ||  enemys.Length == 0 )  // ���� �Ҵ��� ���� ��� �� ������Ʈ �ڵ� Ž��
        {
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.target = enemys;
        }


    }
    private void Start()
    {
        ChangeDungeon();
    }

}
