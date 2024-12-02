using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Script : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float moveInterval = 0.1f;
    // public float fallInterval = 0.5f;
    // public float moveSpeedVertical = 1f;
    // public float moveSpeedHorizontal = 2f;
    // public int bulletCount = 10;
    // public float fallDistance = 10f;

   // private bool isGenerating = false;
    private Vector3 horizontalStartPosition;
    private List<GameObject> currentBullets = new List<GameObject>();

    void Start()
    {
        horizontalStartPosition = spawnPoint.position;
        StartCoroutine(GenerateHorizontalLine());
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