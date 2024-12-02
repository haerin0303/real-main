using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환용
using UnityEngine.UI; // UI 텍스트용

public class Stage1Script : MonoBehaviour
{
    public GameObject bulletPrefab; // 생성할 총알 프리팹
    public Transform spawnPoint; // 총알 생성 위치
    public float moveInterval = 0.1f; // 총알 생성 간격
    public float stageTime = 40f; // 스테이지 제한 시간
    public Text timerText; // UI 텍스트로 남은 시간 표시

    private Vector3 horizontalStartPosition; // 수평 이동 시작 위치
    private List<GameObject> currentBullets = new List<GameObject>(); // 현재 생성된 총알들
    private bool isStageOver = false; // 스테이지 종료 여부 체크

    void Start()
    {
        horizontalStartPosition = spawnPoint.position;

        StartCoroutine(GenerateHorizontalLine()); // 첫 번째 라인 생성
        StartCoroutine(StageTimer()); // 타이머 시작
    }
    IEnumerator StageTimer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < stageTime)
        {
            elapsedTime += Time.deltaTime;

            // UI 텍스트에 남은 시간 표시 (필요 시 유지)
            if (timerText != null)
            {
                timerText.text = "Time Left: " + (stageTime - elapsedTime).ToString("F2");
            }

            // 콘솔에 남은 시간 표시
            Debug.Log("Time Left: " + (stageTime - elapsedTime).ToString("F2"));

            yield return null;
        }

        EndStage(); // 타이머가 끝난 후 스테이지 종료 처리
    }

    void EndStage()
    {
        if (isStageOver) return;

        isStageOver = true;
        Debug.Log("Stage 1 Complete! Loading Next Stage...");
        LoadNextStage(); // 다음 스테이지로 이동
    }

    void LoadNextStage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
        // 다음 씬으로 이동
        SceneManager.LoadScene("Stage2");
    }

    IEnumerator GenerateHorizontalLine()
    {
        currentBullets.Clear();

        for (int i = 0; i < 18; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = horizontalStartPosition + Vector3.right * i;
            bullet.GetComponent<Bullet>().SetType(i % 2);

            currentBullets.Add(bullet);
            yield return new WaitForSeconds(moveInterval);
        }

        foreach (GameObject bullet in currentBullets)
        {
            // 각 총알에 랜덤 지연 시간 추가
            float randomDelay = Random.Range(0.0f, 1.0f);
            StartCoroutine(DropBulletDown(bullet, randomDelay));
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(GenerateVerticalLine());
    }

    IEnumerator DropBulletDown(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null) rb = bullet.AddComponent<Rigidbody2D>();

        rb.gravityScale = 1f;
        rb.velocity = new Vector2(0f, -5f);

        while (true)
        {
            if (bullet.transform.position.y < -10f)
            {
                Destroy(bullet);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator GenerateVerticalLine()
    {
        currentBullets.Clear();

        float verticalSpacing = 0.8f;
        int totalBullets = 13;
        Vector3 rightEdgeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        rightEdgeWorld.z = 0f;

        for (int i = 0; i < totalBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = new Vector3(
                rightEdgeWorld.x - 1f,
                rightEdgeWorld.y - (i * verticalSpacing),
                0f
            );

            bullet.GetComponent<Bullet>().SetType(i % 2);

            currentBullets.Add(bullet);
            yield return new WaitForSeconds(moveInterval);
        }

        foreach (GameObject bullet in currentBullets)
        {
            StartCoroutine(MoveBulletLeft(bullet));
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(GenerateBottomLine());
    }

    IEnumerator GenerateBottomLine()
    {
        currentBullets.Clear();

        Vector3 bottomPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        bottomPosition.y = -4.5f;
        bottomPosition.z = 0f;

        for (int i = 0; i < 18; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = new Vector3(
                bottomPosition.x + (i * 1.0f),
                bottomPosition.y,
                0f
            );

            bullet.GetComponent<Bullet>().SetType(i % 2);

            currentBullets.Add(bullet);
            yield return new WaitForSeconds(moveInterval);
        }

        foreach (GameObject bullet in currentBullets)
        {
            StartCoroutine(MoveBulletUp(bullet));
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(GenerateVerticalLeftLine());
    }

    IEnumerator MoveBulletUp(GameObject bullet)
    {
        float randomSpeed = Random.Range(3f, 7f);
        Vector3 targetPosition = new Vector3(bullet.transform.position.x, 10f, bullet.transform.position.z);

        while (bullet.transform.position.y < 10f)
        {
            bullet.transform.position = Vector3.MoveTowards(
                bullet.transform.position,
                targetPosition,
                randomSpeed * Time.deltaTime
            );

            yield return null;
        }

        Destroy(bullet);
    }

    IEnumerator GenerateVerticalLeftLine()
    {
        currentBullets.Clear();

        float verticalSpacing = 0.8f;
        int totalBullets = 13;

        Vector3 leftEdgeWorld = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        leftEdgeWorld.z = 0f;

        for (int i = 0; i < totalBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = new Vector3(
                leftEdgeWorld.x + 1f,
                leftEdgeWorld.y - (i * verticalSpacing),
                0f
            );

            bullet.GetComponent<Bullet>().SetType(i % 2);

            currentBullets.Add(bullet);
            yield return new WaitForSeconds(moveInterval);
        }

        foreach (GameObject bullet in currentBullets)
        {
            StartCoroutine(MoveBulletRight(bullet));
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(GenerateHorizontalLine());
    }

    IEnumerator MoveBulletLeft(GameObject bullet)
    {
        float randomSpeed = Random.Range(3f, 7f);

        while (bullet.transform.position.x > -10f)
        {
            bullet.transform.position += Vector3.left * randomSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(bullet);
    }

    IEnumerator MoveBulletRight(GameObject bullet)
    {
        float randomSpeed = Random.Range(3f, 7f);

        while (bullet.transform.position.x < 10f)
        {
            bullet.transform.position += Vector3.right * randomSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(bullet);
    }
}
