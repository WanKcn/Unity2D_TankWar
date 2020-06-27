using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour
{
    // 让特效播放一度半时间消失
    public GameObject playerPrefab;

    // 敌人数组 随机产生
    public GameObject[] enemyPrefabList;

    public bool createPlayer; // 初始产生敌人

    void Start()
    {
        // 延时调用
        Invoke("BornTank", 1);
        // 延时销毁
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void BornTank()
    {
        if (createPlayer)
        {
            // 出生当前位置 无旋转
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            int num = Random.Range(0, 2);
            Instantiate(enemyPrefabList[num], transform.position, Quaternion.identity);
        }
    }
}