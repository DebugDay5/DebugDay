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
        DungeonManager.Instance.SetCurrentMap(dungeonSO);  // 맵 데이터 업데이트

        playerController.UpdateBounds();  // 플레이어 이동 범위 업데이트
    }


    private void Awake()
    {
        playerController = GameObject.Find("TestPlayer").GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {
        ChangeDungeon();
    }

}
