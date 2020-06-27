using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapCreator : MonoBehaviour
{
    // 用来装饰初始化地图所需物体的数组
    // 0.base 1 wall 2.barrier 3.born 4.river 5.grass 6 airWall
    public GameObject[] item;

    // 装已经存物体的位置
    private List<Vector3> itemPositionList = new List<Vector3>();

    private void Awake()
    {
       InitMap(); 
    }

    private void CreatItem(GameObject creatGO, Vector3 cPosition, Quaternion cRotation)
    {
        // 游戏物体，位置，旋转角度
        GameObject itemGo = Instantiate(creatGO, cPosition, cRotation);
        // 父物体设置成装游戏物体的容器 set成当前的游戏物体 保证不会散落出来
        itemGo.transform.SetParent(gameObject.transform);

        // 已产生的位置存放进列表
        itemPositionList.Add(cPosition);
    }

    // 产生随机位置方法
    private Vector3 CreatRandomPosition()
    {
        // 不生成x=-10,10的两列，y=8，y = -8这两行的位置
        while (true)
        {
            // 声音v3接收随机生成的物体
            Vector3 creatPosition = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            // 在列表中有位置不返回，再次循环，无位置返回
            if (!itemPositionList.Contains(creatPosition))
                return creatPosition;
        }
    }

    // 判断位是否存在列表中
    private bool IsPosition(Vector3 cP)
    {
        for (int i = 0; i < itemPositionList.Count; i++)
        {
            if (cP == itemPositionList[i])
            {
                return true;
            }
        }

        return false;
    }

    // 产生敌人的方法
    private void CreateEnemy()
    {
        // 随机三个位置
        int num = Random.Range(0, 3);
        Vector3 EnemyPos;
        if (num == 0)
        {
            EnemyPos = new Vector3(-10, 8, 0);
        }
        else if (num == 1)
        {
            EnemyPos = new Vector3(0, 8, 0);
        }
        else
        {
            EnemyPos = new Vector3(10, 8, 0);
        }

        CreatItem(item[3], EnemyPos, Quaternion.identity);
    }
    
    // 地图实例化的方法
    private void InitMap()
    {
        // 实例化基地
        CreatItem(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        // 用墙围基地
        CreatItem(item[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreatItem(item[1], new Vector3(1, -8, 0), Quaternion.identity);
        CreatItem(item[1], new Vector3(-1, -7, 0), Quaternion.identity);
        CreatItem(item[1], new Vector3(0, -7, 0), Quaternion.identity);
        CreatItem(item[1], new Vector3(1, -7, 0), Quaternion.identity);
        // 格挡砖
        CreatItem(item[2], new Vector3(10, -4, 0), Quaternion.identity);
        CreatItem(item[2], new Vector3(-10, -4, 0), Quaternion.identity);
        CreatItem(item[2], new Vector3(0, -5, 0), Quaternion.identity);

        // 实例化外围墙 最上，最下，最左，最右
        for (int i = -11; i < 12; i++)
        {
            CreatItem(item[6], new Vector3(i, 9, 0), Quaternion.identity);
            CreatItem(item[6], new Vector3(i, -9, 0), Quaternion.identity);
        }

        for (int i = -8; i < 9; i++)
        {
            CreatItem(item[6], new Vector3(-11, i, 0), Quaternion.identity);
            CreatItem(item[6], new Vector3(11, i, 0), Quaternion.identity);
        }

        // 实例化地图 墙 障碍 河流 草 不能写在同一个for 保证均匀
        for (int i = 0; i < 60; i++)
        {
            CreatItem(item[1], CreatRandomPosition(), Quaternion.identity);
        }

        for (int i = 0; i < 17; i++)
        {
            CreatItem(item[2], CreatRandomPosition(), Quaternion.identity);
        }

        for (int i = 0; i < 20; i++)
        {
            CreatItem(item[4], CreatRandomPosition(), Quaternion.identity);
        }

        for (int i = 0; i < 20; i++)
        {
            CreatItem(item[5], CreatRandomPosition(), Quaternion.identity);
        }

        // 实例化玩家和敌人
        // 产生出生的特效
        GameObject go = Instantiate(item[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<Born>().createPlayer = true;
        // 产生敌人 随机顶部三个位置 游戏开始先产生三个敌人
        CreatItem(item[3], new Vector3(-8, 8, 0), Quaternion.identity);
        CreatItem(item[3], new Vector3(8, 8, 0), Quaternion.identity);
        CreatItem(item[3], new Vector3(0, 8, 0), Quaternion.identity);

        // 每隔一断时间产生敌人 每隔一段时间重复调用
        InvokeRepeating("CreateEnemy", 4, 5);
    }
}