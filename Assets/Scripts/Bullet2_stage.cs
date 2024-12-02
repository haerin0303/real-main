using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Stage2 : MonoBehaviour
{
    public Stage2Script stage2Script;

    void Start()
    {
        if (stage2Script == null)
        {
            stage2Script = FindObjectOfType<Stage2Script>(); // 자동 할당
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by bullet!");
            string bulletTag = tag;
            switch (bulletTag)
            {
                case "for":
                    stage2Script.HandleBulletCollision("for");
                    break;
                case "while":
                    stage2Script.HandleBulletCollision("while");
                    break;
                case "break":
                    stage2Script.HandleBulletCollision("break");
                    break;
                default:
                    Debug.Log("Unknown bullet type: " + bulletTag);
                    break;
            }

            Destroy(gameObject); // 총알 파괴
        }
    }
}
