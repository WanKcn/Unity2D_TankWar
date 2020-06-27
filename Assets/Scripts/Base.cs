using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private SpriteRenderer sr;

    // 控制渲染 如果被子弹碰到，消失，游戏结束
    public Sprite BrokenSprite;

    // 爆炸效果的引用
    public GameObject explosionPrefab;
    // base死时的音效
    public AudioClip dieAudio;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    public void Die()
    {
        // 一调用死亡方法，切换游戏死亡图片
        sr.sprite = BrokenSprite;
        // 图片切换后产生一个爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        PlayerManager.Instance.isDefeat = true;
        // 当前位置播放
        AudioSource.PlayClipAtPoint(dieAudio,transform.position);
    }
}