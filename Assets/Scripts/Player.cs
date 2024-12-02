using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 7f;  // 이동 속도

    public GameObject gameoverPanel;  // Game Over Panel

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트를 가져옴
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    // 충돌 처리: Bullet이나 BlackBar와 충돌하면 게임 오버
    private void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Bullet"))
        {
            Debug.Log("Game Over: Hit by Bullet!");
            ShowGameOverPanel();
        }
        else if (o.CompareTag("BlackBar"))
        {
            Debug.Log("Game Over: Hit by Blackasdasdar!");
            ShowGameOverPanel();
        }
        else if (o.CompareTag("Bullet_Stage2"))
        {
            Debug.Log("Game Over: Hit by Blackasdasdar!");
            ShowGameOverPanel();
        }
    }

    private void ShowGameOverPanel()
    {
        if (gameoverPanel != null)
        {
            gameoverPanel.SetActive(true);  // Game Over 패널 활성화
            Time.timeScale = 0;            // 게임 멈추기
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned!");
        }   
    }
}
