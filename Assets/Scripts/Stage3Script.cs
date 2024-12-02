using System.Collections;
using UnityEngine;

public class Stage3Manager : MonoBehaviour
{
    public GameObject[] images; // 이미지 배열
    public GameObject[] blackBars; // BlackBar 배열
    public GameObject player; // 플레이어 오브젝트
    public GameObject gameOverPanel; // 게임 오버 패널
    private bool isGameOver = false;

    public GameObject jsPrefab; // JS 프리팹
    public float spawnInterval = 1.5f; // JS 생성 간격
    public float spawnXMin = -9f; // X 좌표 최소값
    public float spawnXMax = 9f;  // X 좌표 최대값
    public float spawnY = 4.32f; // JS 생성 Y 좌표
    public float fallSpeed = 2f; // JS가 떨어지는 속도
    public int spawnCount = 3; // 한 번에 생성할 JS 개수

    // Start is called before the first frame update
    void Start()
    {
        ShowRandomImageAndBlackBar();
        InvokeRepeating(nameof(SpawnJS), 0f, spawnInterval); // JS를 일정 간격으로 생성
    }

    // 이미지와 검정색 영역을 랜덤으로 보여주는 함수
    private void ShowRandomImageAndBlackBar()
    {
        StartCoroutine(ShowImageAndBlackBarCoroutine());
    }

    // 코루틴을 이용해 이미지와 blackBar를 순차적으로 처리
    private IEnumerator ShowImageAndBlackBarCoroutine()
    {
        while (!isGameOver) // 게임 오버 상태일 때까지 반복
        {
            // 랜덤 인덱스 생성
            int randomIndex = Random.Range(0, images.Length);

            // 해당 인덱스의 이미지와 blackBar 선택
            GameObject selectedImage = images[randomIndex];
            GameObject selectedBlackBar = blackBars[randomIndex];

            // 이미지 보이게 하기
            selectedImage.SetActive(true);

            // 이미지를 화면 중앙에 위치시키기 (가정: RectTransform 사용)
            RectTransform imageRectTransform = selectedImage.GetComponent<RectTransform>();
            imageRectTransform.anchoredPosition = Vector2.zero; // 중앙에 배치

            // 이미지가 2초 동안 보이게 하기
            yield return new WaitForSeconds(2f);

            // 이미지가 끝나면 해당 blackBar를 보이게 하기
            selectedImage.SetActive(false);
            selectedBlackBar.SetActive(true);

            // blackBar가 2초 동안 보이게 하기
            yield return new WaitForSeconds(2f);

            // blackBar가 끝난 후 해당 blackBar 비활성화
            selectedBlackBar.SetActive(false);
        }

        // 게임 오버 체크
        if (isGameOver)
        {
            ShowGameOverPanel();
        }
    }

    // JS 생성 및 떨어지는 로직 추가
    private void SpawnJS()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float randomX = Random.Range(spawnXMin, spawnXMax); // 랜덤 X 좌표
            Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f); // 생성 위치

            // JS 프리팹 생성
            GameObject jsInstance = Instantiate(jsPrefab, spawnPosition, Quaternion.identity);

            // JS 이동 스크립트 추가
            jsInstance.AddComponent<Stage3JS>().fallSpeed = fallSpeed;
        }
    }

    // 게임 오버 패널을 표시하는 메서드
    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0; // 게임 멈추기
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned!");
        }
    }

    // 검정색 영역과 충돌했을 때 게임 오버 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlackBar") && !isGameOver)
        {
            isGameOver = true;
        }
    }
}
