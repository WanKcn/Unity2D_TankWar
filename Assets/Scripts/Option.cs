using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 场景管理
public class Option : MonoBehaviour
{
    // 选择的选项 为0 空格加载场景 为1 时指针向下移动到pos2
    private int choose = 1;
    public Transform posOne;
    public Transform posTwo;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 监听ws 根据键盘按下的位置选择值
        if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            choose = 1;
            transform.position = posOne.position;
        }
        else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            choose = 2;
            transform.position = posTwo.position;
        }

        if (choose == 1 && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            SceneManager.LoadScene("Game");
        }
    }
}