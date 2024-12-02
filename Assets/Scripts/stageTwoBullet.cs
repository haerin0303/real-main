using UnityEngine;

public class stageTwoBullet : MonoBehaviour
{
    public float fallSpeed = 5f; // 떨어지는 속도 설정

    void Update()
    {
        // 아래로 이동
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 화면 아래로 나가면 삭제
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    // 충돌 처리
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by bullet!");
            Destroy(gameObject); // 충돌한 총알 파괴
        }
        else
        {
            Debug.Log($"Bullet collided with: {other.gameObject.name}");
        }
    }
}
