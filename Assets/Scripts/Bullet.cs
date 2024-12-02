using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // 두 개의 스프라이트 필드 추가
    public Sprite bullet0Sprite;  // Bullet0 스프라이트
    public Sprite bullet1Sprite;  // Bullet1 스프라이트

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // SetType 함수로 스프라이트 설정
    public void SetType(int type)
    {
        if (type == 0)
        {
            spriteRenderer.sprite = bullet0Sprite;  // Bullet0 스프라이트 설정
        }
        else if (type == 1)
        {
            spriteRenderer.sprite = bullet1Sprite;  // Bullet1 스프라이트 설정
        }
    }

    // 플레이어와 충돌 시 사망 처리
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌 시 처리 (예: 플레이어 태그가 "Player"일 때)
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 사망 처리
            Destroy(collision.gameObject);  // 플레이어를 사망시킴
            Destroy(gameObject);  // 총알도 삭제
        }
    }
    
}
