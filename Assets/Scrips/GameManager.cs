using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject groundPrefab; // 地面预制体
    public GameObject goalPrefab; // 终点预制体
    public GameObject player; // 玩家对象

    public int spawnAmount; // 生成数量
    public Vector2 step = new Vector2(4.25f, 7.5f); // 步长

    private Vector2 playerInitialPosition; // 玩家初始位置

    void Start()
    {
        playerInitialPosition = player.transform.position; // 记录玩家初始位置
        SpawnNewWave(); // 生成新波次
    }

    // 生成新波次
    void SpawnNewWave()
    {
        List<Vector2> mainPath = new List<Vector2>(); // 主路径
        Vector2 spawnPos = Vector2.zero; // 初始生成位置

        // 生成主路径
        for (int i = 0; i < spawnAmount; i++)
        {
            int randomDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1; // 随机方向
            spawnPos += new Vector2(step.x * randomDir, step.y); // 更新生成位置
            mainPath.Add(spawnPos); // 添加到主路径列表

            GameObject ground = Instantiate(groundPrefab, spawnPos - Vector2.up * 2f, Quaternion.identity); // 实例化地面
            ground.transform.SetParent(transform); // 设置父级
            ground.transform.DOMove(ground.transform.position + Vector3.up * 2f, 0.5f).SetDelay(i * 0.1f); // 动画移动
        }

        // 生成分支
        for (int i = 1; i < spawnAmount - 1; i++)
        {
            int branches = Random.Range(1, 3); // 生成支路
            for (int j = 0; j < branches; j++)
            {
                Vector2 branchPos = mainPath[i]; // 分支位置
                int randomDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1; // 随机方向
                branchPos += new Vector2(step.x * randomDir, step.y); // 更新分支位置

                GameObject ground = Instantiate(groundPrefab, branchPos - Vector2.up * 2f, Quaternion.identity); // 实例化地面
                ground.transform.SetParent(transform); // 设置父级
                ground.transform.DOMove(ground.transform.position + Vector3.up * 2f, 0.5f).SetDelay((i + j + 1) * 0.1f); // 动画移动

                // 确保分支重新连接到主路径
                if (j == branches - 1)
                {
                    branchPos = mainPath[i + 1]; // 分支重新连接位置
                    GameObject reconnectGround = Instantiate(groundPrefab, branchPos - Vector2.up * 2f, Quaternion.identity); // 实例化地面
                    reconnectGround.transform.SetParent(transform); // 设置父级
                    reconnectGround.transform.DOMove(reconnectGround.transform.position + Vector3.up * 2f, 0.5f).SetDelay((i + j + 1) * 0.1f); // 动画移动
                }
            }
        }

        // 在主路径末尾设置终点
        Vector2 finalPos = mainPath[mainPath.Count - 1]; // 最终位置
        Vector2 goalPos = finalPos + new Vector2(step.x, step.y); // 延伸一格后的终点位置
        GameObject goal = Instantiate(goalPrefab, goalPos - Vector2.up * 2f, Quaternion.identity); // 实例化终点
        goal.transform.SetParent(transform); // 设置父级
        goal.transform.DOMove(goal.transform.position + Vector3.up * 2f, 0.5f).SetDelay(spawnAmount * 0.1f); // 动画移动
    }

    // 重置玩家位置的方法
    public void ResetPlayerPosition()
    {
        player.transform.position = playerInitialPosition; // 重置玩家位置
        var playerController = player.GetComponent<PlayerController>(); // 获取玩家控制器组件
        playerController.SetHasDied(false); // 重置玩家的死亡状态
        playerController.ResetGame(); // 重置游戏状态
    }
}
