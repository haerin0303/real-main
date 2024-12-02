using System.Collections;
using UnityEngine;

public class IfScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float screenLeftX = -10f;
    public float screenRightX = 10f;
    public float screenTopY = 10f;
    public float screenBottomY = -10f;
    public float cBulletInterval = 0.05f;
    private int excludedSectionX;

    void Start()
    {
        excludedSectionX = Random.Range(0, 3);
        StartCoroutine(SpawnCShapedBulletsInSections());

        // 2초 뒤에 StopBulletGeneration 호출
        StartCoroutine(StopBulletGenerationAfterDelay(2f));
    }

    // 총알 생성 코루틴 (2초 동안만 총알을 생성)
    public IEnumerator SpawnCShapedBulletsInSections()
    {
        float elapsedTime = 0f; // 경과 시간 추적

        while (elapsedTime < 2f) // 2초 동안만 생성
        {
            if (excludedSectionX != 0)
                SpawnCShapedBulletInRegion(screenLeftX + 2f, -1f); 
            if (excludedSectionX != 1)
                SpawnCShapedBulletInRegion(0f, 0f); 
            if (excludedSectionX != 2)
                SpawnCShapedBulletInRegion(screenRightX - 2f, 1f); 

            elapsedTime += cBulletInterval; // 시간 증가
            yield return new WaitForSeconds(cBulletInterval); // 일정 간격으로 생성
        }

        // 2초 후 더 이상 총알을 생성하지 않음
        StopBulletGeneration();
    }

    // C자 형태로 총알을 생성하는 함수
    void SpawnCShapedBulletInRegion(float xPos, float direction)
    {
        for (int i = 0; i < 10; i++)
        {
            float spawnY = Camera.main.orthographicSize + i * 2f;
            float spawnX = Mathf.Lerp(xPos - 3f, xPos + 3f, (i % 10) / 9f);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(MoveBulletDown(bullet, 30f));
        }
    }

    // 총알이 아래로 내려가는 함수
    IEnumerator MoveBulletDown(GameObject bullet, float fallSpeed)
    {
        while (bullet.transform.position.y > -10f)
        {
            bullet.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(bullet);
    }

    // 총알 생성을 멈추는 함수
    public void StopBulletGeneration()
    {
        Debug.Log("Stopping all continuous bullets!");

        // 총알 생성 중지: 현재 활성화된 총알들을 비활성화하거나 제거
        foreach (var bullet in GetComponentsInChildren<Bullet>())
        {
            // 예시: 총알들을 비활성화하거나 제거
            Destroy(bullet.gameObject);
        }

        // 추가적으로 다른 멈춤 처리 필요 시 구현
    }

    // 2초 후 StopBulletGeneration 호출
    private IEnumerator StopBulletGenerationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopBulletGeneration();
    }
}
