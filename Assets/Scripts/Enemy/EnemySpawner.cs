using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    // Enemy프리팹, 스폰 위치 넣기
    public GameObject meleeEnemyPrefab; // 근접 Enemy
    public GameObject rangeEnemyPrefab; // 원거리 Enemy
    public GameObject bossEnemyPrefab; // 보스 Enemy
    public Transform[] spawnPoints; // 스폰 위치
    public int enemiesPerRound = 5; // Enemy 수
    public int currentRound = 1; // 라운드

    public void StartRound()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(Random.Range(0, 2) == 0 ? meleeEnemyPrefab : rangeEnemyPrefab, spawnPoint.position, Quaternion.identity); // 랜덤한 적 소환
        }

        if (currentRound == 5) // 보스 라운드
        {
            Instantiate(bossEnemyPrefab, spawnPoints[0].position, Quaternion.identity);
        }
    }

    public void IncreaseDifficulty() // 라운드가 상승할 때마다 Enemy의 수가 오름
    {
        currentRound++;
        enemiesPerRound += 2;
    }
}
