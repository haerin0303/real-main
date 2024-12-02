using System.Collections;
using UnityEngine;

public class ForScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float horizontalSpacing = 2f;
    public float verticalSpacing = 2f;
    public int maxRows = 9;
    public float triangleBulletInterval = 0.1f;

    private bool[] rowSpawned;

    // 일정한 떨어지는 속도
    public float fixedFallSpeed = 5f;

    void Start()
    {
        rowSpawned = new bool[maxRows];
        StartCoroutine(SpawnTriangleBulletPattern());
    }
    public IEnumerator SpawnTriangleBulletPattern()
    {
        // 화면의 가로 크기를 계산 (카메라의 orthoSize와 화면의 비율을 이용)
        float screenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        
        // 화면 중앙 X 좌표 계산 (기존 코드에서의 문제는 여기서 발생)
        float centerX = 0f;  // 가운데를 기준으로 할 때, 플레이어가 아닌 화면의 중앙을 사용

        for (int row = 0; row < maxRows; row++)
        {
            int bulletsInRow = row + 1;  // 첫 번째 행은 1개, 두 번째 행은 2개, 세 번째 행은 3개...

            // 각 행에서 총알을 생성할 X 좌표 계산
            for (int i = 0; i < bulletsInRow; i++)
            {
                // 화면 중앙을 기준으로 배치
                float spawnX = centerX + (i - (bulletsInRow / 2f)) * horizontalSpacing; // 가운데를 기준으로 총알 배치
                float spawnY = Camera.main.orthographicSize + (row * verticalSpacing);  // Y 좌표는 행마다 올라가게
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

                // 총알을 생성하고 아래로 떨어지게 만들기
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                // 일정한 속도로 떨어짐
                StartCoroutine(MoveBulletDown(bullet, fixedFallSpeed));
            }

            yield return new WaitForSeconds(triangleBulletInterval);
        }
    }

    // 총알을 일정 속도로 떨어뜨리기
    IEnumerator MoveBulletDown(GameObject bullet, float fallSpeed)
    {
        while (bullet.transform.position.y > -10f)
        {
            bullet.transform.position += Vector3.down * fallSpeed * Time.deltaTime;  // 일정 속도로 떨어지도록
            yield return null;
        }
        Destroy(bullet);
    }

    public void StopBulletGeneration()
    {
        Debug.Log("Stopping all triangle bullets!");

        // 총알 생성 중지: 현재 활성화된 총알들을 비활성화하거나 제거
        foreach (var bullet in GetComponentsInChildren<Bullet>())
        {
            // 예시: 총알들을 비활성화하거나 제거
            Destroy(bullet.gameObject);
        }

        // 추가적으로 다른 멈춤 처리 필요 시 구현
    }

    
}
