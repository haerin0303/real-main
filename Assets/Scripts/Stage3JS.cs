using UnityEngine;

public class Stage3JS : MonoBehaviour
{
    public float fallSpeed; // 떨어지는 속도

    private void Update()
    {
        // 아래로 이동
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 화면 밖으로 나가면 제거
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
