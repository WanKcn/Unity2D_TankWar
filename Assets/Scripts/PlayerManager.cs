using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // 玩家生命值
    public int playerHP = 3;
    public int score = 0;
    public bool isDead;

    public bool isDefeat; // 游戏失败

    public GameObject born;

    public Text playerHpText;
    public Text playerScoreText;
    public GameObject isDefeatUI;

    // 单例
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 如果失败，播放失败图片
        if (isDefeat)
        {
            isDefeatUI.SetActive(true);
            Invoke("ReturnMenu", 3);
            return; // 所有方法不再执行
        }

        // 如果死亡 玩家复活
        if (isDead)
        {
            Recover();
        }

        // 实时更新
        playerScoreText.text = score.ToString();
        playerHpText.text = playerHP.ToString();
    }

    // 复活的方法
    private void Recover()
    {
        if (playerHP <= 0)
        {
            // 游戏失败，返回主界面
            isDefeat = true;
            // 延时3秒回到主页
            Invoke("ReturnMenu", 3);
        }
        else
        {
            playerHP--;
            // 并且在出生地born一次
            GameObject go = Instantiate(born, new Vector3(-2, -8, 0), Quaternion.identity);
            go.GetComponent<Born>().createPlayer = true;
            isDead = false;
        }
    }

    private void ReturnMenu()
    {
        SceneManager.LoadScene("Main");
    }
}