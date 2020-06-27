using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3; // 移动速度
    private Vector3 bulletEulerAngles;
    private float timeVal; // 子弹发射计时器
    private bool isDenfended = true; // 被保护状态
    private float denfenTimeVal = 3; // 被保护的计时器

    // 引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // 坦克精灵组 上右下左
    public GameObject bulletPrefab;
    public GameObject explosionPrefab; // 爆炸效果
    public GameObject shieldPrefab; // 受保护的预制体

    // 声音组建
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;

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
        // 是否处于无敌状态
        if (isDenfended)
        {
            shieldPrefab.SetActive(true); // 如果无敌则显示
            // 受保护时间累减
            denfenTimeVal -= Time.deltaTime;
            if (denfenTimeVal <= 0)
            {
                isDenfended = false;
                shieldPrefab.SetActive(false); // 无敌消失
            }
        }
    }

    // 固定物理帧 每一秒匀称的情况下
    private void FixedUpdate()
    {
        // 玩家不能再移动攻击
        if (PlayerManager.Instance.isDefeat)
            return;
        Move();
        // 子弹攻击CD
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.fixedDeltaTime;
        }
    }

    // 坦克移动方法
    private void Move()
    {
        // 通过监听h和v的输入来控制player在x和y轴位移的方向
        float v = Input.GetAxisRaw("Vertical");
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

        if (Mathf.Abs(v) > 0.05f)
        {
            // 坦克前进音效
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

        // 优先级
        if (v != 0)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
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

        if (Mathf.Abs(h) > 0.05f)
        {
            // 坦克前进音效
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
                moveAudio.Play();
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying)
                moveAudio.Play();
        }
    }

    // 坦克的攻击方法
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 子弹产生的的角度等于当前坦克角度加子弹旋转角度
            Instantiate(bulletPrefab, transform.position,
                Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
            timeVal = 0;
        }
    }

    // 坦克死亡方法
    private void Die()
    {
        // 无敌状态不会爆炸
        if (isDenfended)
        {
            return;
        }

        PlayerManager.Instance.isDead = true;
        // 产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        // 死亡
        Destroy(gameObject);
    }
}