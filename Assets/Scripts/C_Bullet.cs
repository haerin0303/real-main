using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 Prefab
    public Transform firePoint;     // 총알이 발사될 위치
    public float bulletSpeed = 10f; // 총알 속도

    void Update()
    {
        // 발사 버튼(예: 마우스 왼쪽 버튼)을 눌렀을 때
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 속도 부여
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed; // 총알이 오른쪽 방향으로 발사됨
        }
    }
}
