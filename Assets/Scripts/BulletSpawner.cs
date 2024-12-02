using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject[] bulletPrefabs; // 총알 Prefab 배열
    public float spawnInterval = 0.1f; // 총알 생성 간격 (기본적으로 매우 빠름)
    public float specialSpawnChance = 0.1f; // 특별 총알(1~5번) 생성 확률 (10%)
    public float screenLeftX = -8f; // 화면 좌측 경계
    public float screenRightX = 8f; // 화면 우측 경계
    public float spawnY = 10f; // 총알 생성 Y 위치

    public static event Action<GameObject> OnBulletSpawned; // 총알 생성 이벤트

    private void Start()
    {
        // 일정 간격으로 총알 생성
        InvokeRepeating(nameof(SpawnBullet), 0f, spawnInterval);
    }

    private void SpawnBullet()
    {
        GameObject bulletPrefab;

        // 특별 총알 생성 여부 결정
        if (UnityEngine.Random.value < specialSpawnChance)
        {
            int specialIndex = UnityEngine.Random.Range(1, bulletPrefabs.Length); // 1~5번
            bulletPrefab = bulletPrefabs[specialIndex];
        }
        else
        {
            bulletPrefab = bulletPrefabs[0]; // 기본적으로 0번 총알(`C`)
        }

        // 랜덤 X 좌표에서 생성
        float randomX = UnityEngine.Random.Range(screenLeftX, screenRightX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // 생성된 총알 알리기
        OnBulletSpawned?.Invoke(bullet);
    }
}
