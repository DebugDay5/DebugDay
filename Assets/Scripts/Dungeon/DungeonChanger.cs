using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class DungeonChanger : MonoBehaviour
{
    public DungeonSO dungeonSO;
    public PlayerController playerController;
    public TestCamera_KHY cameraController;

    public GameObject[] enemys;  // 적 수동할당 가능

    public void ChangeDungeon()
    {
        DungeonManager.Instance.SetCurrentMap(dungeonSO);  // 맵 데이터 업데이트
        playerController.UpdateBounds();  // 플레이어 이동 범위 업데이트
        cameraController.CameraUpdateBounds();  // 카메라 이동 범위 업데이트
    }

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<TestCamera_KHY>();

        if (enemys == null ||  enemys.Length == 0 )  // 수동 할당이 없을 경우 적 오브젝트 자동 탐색
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
