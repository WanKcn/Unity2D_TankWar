using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3; // 移动速度
    private Vector3 bulletEulerAngles;
    private float timeVal; // 子弹发射计时器
    private float turnTimeVal; // 控制转向的计时器 默认为4 一产生就转向

    private float v = -1; // 出生往下走
    private float h;

    // 引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // 坦克精灵组 上右下左
    public GameObject bulletPrefab;
    public GameObject explosionPrefab; // 爆炸效果

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 子弹攻击的时间间隔，敌人每隔一段时间射出子弹
        if (timeVal >= 2)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    // 固定物理帧 每一秒匀称的情况下
    private void FixedUpdate()
    {
        Move();
    }

    // 坦克移动方法
    private void Move()
    {
        // 敌人一直往Base走 往Base几率更大
        if (turnTimeVal >= 4)
        {
            int num = Random.Range(0, 8);
            if (num > 4)
            {
                v = -1;
                h = 0;
            }
            else if (num == 0)
            {
                v = 1;
                h = 0;
            }
            else if (num > 0 && num <= 2)
            {
                h = -1;
                v = 0;
            }
            else if (num > 2 && num <= 4)
            {
                h = -1;
                v = 0;
            }

            // 归0， 否则一直旋转
            turnTimeVal = 0;
        }
        else
        {
            turnTimeVal += Time.fixedDeltaTime;
        }

        transform.Translate(Vector3.up * (v * moveSpeed * Time.fixedDeltaTime),
            Space.World);
        if (v < 0)
        {
            sr.sprite = tankSprite[2];
            bulletEulerAngles = new Vector3(0, 0, 180); // -180和正180 都是向下
        }
        else if (v > 0)
        {
            sr.sprite = tankSprite[0];
            bulletEulerAngles = new Vector3(0, 0, 0);
        }

        if (v != 0)
        {
            return;
        }

        transform.Translate(Vector3.right * (h * moveSpeed * Time.fixedDeltaTime),
            Space.World);
        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
    }

    // 坦克的攻击方法 自动射击
    private void Attack()
    {
        // 子弹产生的的角度等于当前坦克角度加子弹旋转角度
        Instantiate(bulletPrefab, transform.position,
            Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
        timeVal = 0;
    }

    // 坦克死亡方法
    private void Died()
    {
        // 产生爆炸特效并且玩家得分加1
        PlayerManager.Instance.score++;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        // 死亡
        Destroy(gameObject);
    }

    // 转换方向 如果两个坦克有触碰，顺势转换方向
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            v *= -1;
            h *= -1;
        }
    }
}