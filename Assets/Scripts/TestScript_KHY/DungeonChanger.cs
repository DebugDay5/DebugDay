using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class DungeonChanger : MonoBehaviour
{

    public DungeonSO dungeonSO;

    public TopDownCharacterController playerController;

    public void ChangeDungeon()
    {
        DungeonManager.Instance.SetCurrentMap(dungeonSO);  // �� ������ ������Ʈ

        playerController.UpdateBounds();  // �÷��̾� �̵� ���� ������Ʈ
    }


    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<TopDownCharacterController>();

    }
    private void Start()
    {
        ChangeDungeon();
    }

}
