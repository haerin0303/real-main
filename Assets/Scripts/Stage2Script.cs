using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 텍스트 사용을 위한 네임스페이스 추가

public class Stage2Script : MonoBehaviour
{
    // 총알 관리 변수
    private List<GameObject> activeBullets = new List<GameObject>();

    // 기본 총알 프리팹
    public GameObject bulletPrefab;

    // 특수 총알 프리팹 배열
    public GameObject[] specialBullets;

    // 총알 생성 간격 및 속도
    public float basicBulletInterval = 1f;
    public float basicBulletFallSpeedMin = 1f;
    public float basicBulletFallSpeedMax = 20f;
    public float specialBulletInterval = 1f;
    public float specialBulletFallSpeedMin = 2f;
    public float specialBulletFallSpeedMax = 10f;

    // 화면 경계
    public float screenLeftX = -10f;
    public float screenRightX = 10f;
    public float screenTopY = 10f;
    public float screenBottomY = -10f;

    // 총알 배치 관련 변수
    public float horizontalSpacing = 2f;
    public int maxBulletsPerRow = 6;

    // 총알 매니저
    private ForScript triangleBulletManager;
    private WhileScript cShapeBulletManager;
    private IfScript continuousBulletManager;

    // 총알 생성 상태
    private bool isSpawningBasicBullets = false;
    private bool isSpawningSpecialBullets = false;
    private bool isSpecialBulletFunctionRunning = false; // 특수 기능 실행 중 여부

    private bool isStageOver = false; // 스테이지 종료 여부 체크
    public Text timerText; // UI 텍스트로 남은 시간 표시

    // 스테이지 시간 설정 (예: 60초)
    public float stageTime = 60f;

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
        // 씬을 빌드 설정에 추가해야만 로드할 수 있습니다.
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    void Start()
    {
        // 기본 총알 및 특수 총알 생성 시작 여부 설정
        isSpawningBasicBullets = true;
        isSpawningSpecialBullets = true;

        if (isSpawningBasicBullets)
        {
            StartCoroutine(SpawnBasicBullets());
        }

        // 필요에 따라 총알 생성 루프 시작
        if (isSpawningSpecialBullets)
        {
            StartCoroutine(SpawnSpecialBullets());
        }

        // 타이머 시작
        StartCoroutine(StageTimer());
    }

    // 총알 충돌 처리
    public void HandleBulletCollision(string bulletType)
    {
        Debug.Log("Bullet collision detected: " + bulletType); // 로그 추가

        // 총알 삭제 및 특수 기능 시작
        if (!isSpecialBulletFunctionRunning)  // 특수 기능이 실행 중이 아니면 실행
        {
            StartCoroutine(DeleteBulletsAndStartFunction(bulletType));
        }
    }

    private IEnumerator DeleteBulletsAndStartFunction(string bulletType)
    {
        // 특수 기능 실행 중이라면 추가 총알 생성 막기
        isSpecialBulletFunctionRunning = true;

        // 모든 총알 삭제
        foreach (GameObject bullet in activeBullets)
        {
            if (bullet != null)
            {
                Destroy(bullet);
            }
        }

        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 특수 총알에 맞은 총알 타입에 따라 특수 기능 시작
        switch (bulletType)
        {
            case "for":
                StartTriangleBulletPattern();
                break;
            case "while":
                StartCShapeBulletPattern();
                break;
            case "break":
                // break 시작 시 기본 총알과 특수 총알 생성을 멈춤
                StopSpawningBasicBullets();
                StopSpawningSpecialBullets();
                StartContinuousBulletPattern();
                break;
            default:
                Debug.Log("Unknown bullet type: " + bulletType);
                break;
        }

        // 특수 기능 실행 후 5초 대기
        yield return new WaitForSeconds(5f);

        // 특수 총알 다시 생성 시작
        isSpecialBulletFunctionRunning = false;

        // break 총알 패턴이 끝나면 기본 총알과 특수 총알 생성 재개
        StartCoroutine(ResumeBulletSpawningAfterDelay(2f)); // 2초 후 재개
    }

    // break 총알 패턴 종료 후, 기본 총알과 특수 총알 생성을 재개
    private IEnumerator ResumeBulletSpawningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 기본 총알 및 특수 총알 생성 다시 시작
        isSpawningBasicBullets = true;
        isSpawningSpecialBullets = true;

        StartCoroutine(SpawnBasicBullets());
        StartCoroutine(SpawnSpecialBullets());
    }

    // For 총알 패턴 시작
    private void StartTriangleBulletPattern()
    {
        if (triangleBulletManager == null)
        {
            GameObject triangleBulletManagerObject = new GameObject("TriangleBulletManager");
            triangleBulletManager = triangleBulletManagerObject.AddComponent<ForScript>();
            triangleBulletManager.bulletPrefab = bulletPrefab;
        }

        Debug.Log("For bullet logic executed");
        StartCoroutine(triangleBulletManager.SpawnTriangleBulletPattern());
    }

    // While 총알 패턴 시작
    private void StartCShapeBulletPattern()
    {
        if (cShapeBulletManager == null)
        {
            GameObject cShapeBulletManagerObject = new GameObject("CShapeBulletManager");
            cShapeBulletManager = cShapeBulletManagerObject.AddComponent<WhileScript>();
            cShapeBulletManager.bulletPrefab = bulletPrefab;
        }

        Debug.Log("While bullet logic executed");
        StartCoroutine(cShapeBulletManager.SpawnCShapedBulletsInSections());
    }

    // Break 총알 패턴 시작
    private void StartContinuousBulletPattern()
    {
        if (continuousBulletManager == null)
        {
            GameObject continuousBulletManagerObject = new GameObject("ContinuousBulletManager");
            continuousBulletManager = continuousBulletManagerObject.AddComponent<IfScript>();
            continuousBulletManager.bulletPrefab = bulletPrefab;
        }

        Debug.Log("Break bullet logic executed");
        StartCoroutine(continuousBulletManager.SpawnCShapedBulletsInSections());

        // 2초 뒤에 멈추도록 처리
        StartCoroutine(StopContinuousBulletPatternAfterDelay(2f));
    }

    // 2초 뒤에 계속되는 총알 패턴을 멈추는 함수
    private IEnumerator StopContinuousBulletPatternAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 총알 생성 멈추기
        if (continuousBulletManager != null)
        {
            continuousBulletManager.StopBulletGeneration();
        }

        // 특수 기능을 멈추고, 더 이상 생성하지 않도록 설정
        StopSpawningSpecialBullets();
    }

    // 특별 총알 생성
    IEnumerator SpawnSpecialBullets()
    {
        while (isSpawningSpecialBullets)
        {
            if (isSpecialBulletFunctionRunning) // 특수 기능이 실행 중이면 생성하지 않음
            {
                yield return null; // 대기
                continue;
            }

            int numberOfBullets = Random.Range(1, 6);

            for (int i = 0; i < numberOfBullets; i++)
            {
                int bulletType = Random.Range(0, specialBullets.Length);
                float randomX = Random.Range(screenLeftX, screenRightX);
                float spawnY = Camera.main.orthographicSize;
                Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

                // 위치가 겹치지 않도록 확인
                if (!IsPositionOccupied(spawnPosition))
                {
                    // 총알을 겹치지 않는 위치에 생성
                    GameObject bulletPrefab = specialBullets[bulletType];
                    GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                    bullet.tag = bulletPrefab.tag;
                    activeBullets.Add(bullet);

                    // 총알 떨어지는 속도 설정
                    float randomFallSpeed = Random.Range(specialBulletFallSpeedMin, specialBulletFallSpeedMax);
                    StartCoroutine(MoveBulletDown(bullet, randomFallSpeed));
                }
                else
                {
                    // 겹치는 위치에선 재시도
                    i--;
                }
            }

            yield return new WaitForSeconds(specialBulletInterval);
        }
    }

    // 기본 총알 생성
    IEnumerator SpawnBasicBullets()
    {
        while (isSpawningBasicBullets)
        {
            int numberOfBullets = Random.Range(1, 6);
            for (int i = 0; i < numberOfBullets; i++)
            {
                float randomX = Random.Range(screenLeftX, screenRightX);
                float spawnY = Camera.main.orthographicSize;
                Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

                // 위치가 겹치지 않도록 확인
                if (!IsPositionOccupied(spawnPosition))
                {
                    GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                    activeBullets.Add(bullet);

                    float randomFallSpeed = Random.Range(basicBulletFallSpeedMin, basicBulletFallSpeedMax);
                    StartCoroutine(MoveBulletDown(bullet, randomFallSpeed));
                }
                else
                {
                    i--;
                }
            }

            yield return new WaitForSeconds(basicBulletInterval);
        }
    }

    // 총알이 겹치는지 체크하는 함수
    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject bullet in activeBullets)
        {
            if (bullet != null && Vector3.Distance(bullet.transform.position, position) < horizontalSpacing)
            {
                return true;
            }
        }
        return false;
    }

    // 총알을 떨어뜨리는 함수
    IEnumerator MoveBulletDown(GameObject bullet, float fallSpeed)
    {
        while (bullet != null && bullet.transform.position.y > screenBottomY)
        {
            bullet.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        // 화면 밖으로 나가면 총알 제거
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }

    // 총알 생성을 멈추는 함수
    void StopSpawningBasicBullets()
    {
        isSpawningBasicBullets = false;
    }

    void StopSpawningSpecialBullets()
    {
        isSpawningSpecialBullets = false;
    }
}
