using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 10;
    public bool isPlayerBullet; // 是否敌人子弹

    public AudioClip hitAudio;

    public void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(hitAudio,transform.position);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * (moveSpeed * Time.deltaTime), Space.World);
    }

    // collision 子弹碰撞到的物体
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Tank":
                if (!isPlayerBullet)
                {
                    // 敌人子弹打到玩家die
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                
                break;
            case "Base":
                collision.SendMessage("Die");
                Destroy(gameObject);
                break;
            case "Enemy":
                if (isPlayerBullet)
                {
                    collision.SendMessage("Died");
                    Destroy(gameObject);
                }
                break;
            case "Wall":
                // 碰到墙销毁
                Destroy(collision.gameObject);
                // 自身也要销毁
                Destroy(gameObject);
                break;
            case "Barrier":
                if (isPlayerBullet)
                {
                    PlayAudio();
                }
                // 碰到障碍 自身销毁
                Destroy(gameObject);
                break;
        }
    }
}