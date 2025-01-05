using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject disappearingGroundPrefab;
    public GameObject goalPrefab;
    public GameObject player;

    public int spawnAmount;
    public Vector2 step = new Vector2(4.25f, 7.5f);

    [Header("消失方块设置")]
    public float baseDisappearTime = 3f;
    public float timePerHeight = 0.5f;
    public float fadeOutDuration = 1f;

    [Header("多路径生成设置")]
    public int pathCount = 1;        // 生成的路径数量
    public float pathSpacing = 8f;   // 路径之间的间距
    public float mergeHeight = 0.7f; // 路径开始合并的高度比例(0-1)

    [Header("跳一跳关卡设置")]
    public float minPlatformDistance = 5f;  // 最小平台距离
    public float maxPlatformDistance = 10f; // 最大平台距离
    public float platformSizeMin = 2f;      // 最小平台大小
    public float platformSizeMax = 4f;      // 最大平台大小
    public GameObject jumpitPlayerPrefab;   // 跳一跳玩家预制体
    public GameObject powerIndicatorPrefab; // 力度指示器预制体

    private Vector2 playerInitialPosition;
    private int currentLevel;
    private bool useDisappearingGround;

    void Start()
    {
        playerInitialPosition = player.transform.position;
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        currentLevel = int.Parse(sceneName.Split('-')[1]);
        useDisappearingGround = currentLevel >= 4 && currentLevel <= 6;

        // 1-6关使用普通生成逻辑
        if (currentLevel <= 6)
        {
            if (useDisappearingGround)
            {
                SetupLevelParameters();
            }
            SpawnNewWave();
        }
        else
        {
            // 7-9关使用跳一跳逻辑
            SetupJumpitPlayer();
        }
    }

    private void SetupLevelParameters()
    {
        switch (currentLevel)
        {
            case 4: // 第四关：基础消失方块
                if (baseDisappearTime == 3f) baseDisappearTime = 1.5f;
                if (timePerHeight == 0.5f) timePerHeight = 0.2f;
                if (fadeOutDuration == 1f) fadeOutDuration = 0.5f;
                if (spawnAmount == 0) spawnAmount = 8;
                pathCount = 1;
                break;

            case 5: // 第五关：加快消失速度
                if (baseDisappearTime == 3f) baseDisappearTime = 1.2f;
                if (timePerHeight == 0.5f) timePerHeight = 0.15f;
                if (fadeOutDuration == 1f) fadeOutDuration = 0.4f;
                if (spawnAmount == 0) spawnAmount = 10;
                pathCount = 1;
                break;

            case 6: // 第六关：极限速度
                if (baseDisappearTime == 3f) baseDisappearTime = 1.0f;
                if (timePerHeight == 0.5f) timePerHeight = 0.1f;
                if (fadeOutDuration == 1f) fadeOutDuration = 0.3f;
                if (spawnAmount == 0) spawnAmount = 12;
                pathCount = 1;
                break;

        }
    }

    void SpawnNewWave()
    {
        List<List<Vector2>> allPaths = new List<List<Vector2>>();
        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        // 生成起始方块
        Vector2 firstPos = Vector2.zero;
        SpawnGround(firstPos, 0);
        occupiedPositions.Add(firstPos);

        // 计算多路径的宽度
        float pathWidth = (pathCount - 1) * step.x;
        float startX = -pathWidth * 0.5f;

        // 生成每条主路径
        for (int p = 0; p < pathCount; p++)
        {
            List<Vector2> mainPath = new List<Vector2>();
            Vector2 spawnPos = firstPos;
            mainPath.Add(spawnPos);

            // 如果不是第一条路径，需要生成起始点
            if (p > 0)
            {
                // 为每条路径创建一个偏移的起始点，确保路径分开
                spawnPos = firstPos + new Vector2(startX + p * step.x, step.y);
                if (!occupiedPositions.Contains(spawnPos))
                {
                    SpawnGround(spawnPos, 1);
                    occupiedPositions.Add(spawnPos);
                    mainPath.Add(spawnPos);
                }
            }

            // 生成主路径方块
            for (int i = mainPath.Count; i < spawnAmount - 1; i++)
            {
                float progress = (float)i / spawnAmount;
                Vector2 nextPos;

                if (progress > mergeHeight)
                {
                    // 在合并高度之后，路径开始向中心靠拢
                    float targetX = 0;
                    float currentX = spawnPos.x;

                    // 根据当前位置决定下一个方块的位置
                    if (Mathf.Abs(currentX) <= step.x)
                    {
                        // 如果已经接近中心，直接向中心移动
                        nextPos = spawnPos + new Vector2(currentX > 0 ? -step.x : step.x, step.y);
                    }
                    else
                    {
                        // 否则继续向中心靠拢
                        float moveX = Mathf.Sign(targetX - currentX) * step.x;
                        nextPos = spawnPos + new Vector2(moveX, step.y);
                    }
                }
                else
                {
                    // 在合并高度之前，路径保持相对独立
                    float pathX = startX + p * step.x;
                    float currentX = spawnPos.x;

                    // 随机决定下一个方块的方向
                    int randomDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    if (Mathf.Abs(currentX - pathX) > step.x)
                    {
                        // 如果偏离太远，强制向路径中心回归
                        randomDir = (int)Mathf.Sign(pathX - currentX);
                    }

                    nextPos = spawnPos + new Vector2(step.x * randomDir, step.y);

                    // 随机生成分支路径
                    if (Random.Range(0f, 1f) > 0.85f)
                    {
                        Vector2 branchPos = spawnPos + new Vector2(step.x * -randomDir, step.y);
                        if (!occupiedPositions.Contains(branchPos) &&
                            Mathf.Abs(branchPos.x - pathX) <= step.x * 1.5f)
                        {
                            SpawnGround(branchPos, i);
                            occupiedPositions.Add(branchPos);
                        }
                    }
                }

                // 生成下一个方块
                if (!occupiedPositions.Contains(nextPos))
                {
                    SpawnGround(nextPos, i);
                    occupiedPositions.Add(nextPos);
                    spawnPos = nextPos;
                    mainPath.Add(spawnPos);
                }
                else
                {
                    // 如果位置被占用，尝试反方向生成
                    nextPos = spawnPos + new Vector2(-step.x * Mathf.Sign(nextPos.x - spawnPos.x), step.y);
                    if (!occupiedPositions.Contains(nextPos))
                    {
                        SpawnGround(nextPos, i);
                        occupiedPositions.Add(nextPos);
                        spawnPos = nextPos;
                        mainPath.Add(spawnPos);
                    }
                }
            }
            allPaths.Add(mainPath);
        }

        // 在路径之间随机生成连接方块
        if (pathCount > 1)
        {
            for (int i = 2; i < spawnAmount - 2; i++)
            {
                float progress = (float)i / spawnAmount;
                if (progress < mergeHeight && Random.Range(0f, 1f) > 0.8f)
                {
                    for (int p = 0; p < pathCount - 1; p++)
                    {
                        if (i < allPaths[p].Count && i < allPaths[p + 1].Count)
                        {
                            Vector2 startPoint = allPaths[p][i];
                            Vector2 endPoint = allPaths[p + 1][i];

                            // 生成连接方块
                            if (Vector2.Distance(startPoint, endPoint) <= step.x * 2)
                            {
                                Vector2 connectionPoint = Vector2.Lerp(startPoint, endPoint, 0.5f);
                                if (!occupiedPositions.Contains(connectionPoint))
                                {
                                    SpawnGround(connectionPoint, i);
                                    occupiedPositions.Add(connectionPoint);
                                }
                            }
                        }
                    }
                }
            }
        }

        // 找到最高的路径终点
        float maxY = 0;
        Vector2 lastPos = Vector2.zero;
        foreach (var path in allPaths)
        {
            if (path.Count > 0)
            {
                Vector2 pathEnd = path[path.Count - 1];
                if (pathEnd.y > maxY)
                {
                    maxY = pathEnd.y;
                    lastPos = pathEnd;
                }
            }
        }

        // 在最高点生成最后一个平台
        Vector2 finalPos = lastPos + new Vector2(lastPos.x > 0 ? -step.x : step.x, step.y);
        if (!occupiedPositions.Contains(finalPos))
        {
            SpawnGround(finalPos, spawnAmount - 1);
            occupiedPositions.Add(finalPos);
        }

        // 生成终点方块，确保与最后一个平台相连
        Vector2 goalPos = finalPos + new Vector2(finalPos.x > 0 ? -step.x : step.x, step.y);
        if (!occupiedPositions.Contains(goalPos))
        {
            GameObject goal = Instantiate(goalPrefab, goalPos - Vector2.up * 2f, Quaternion.identity);
            goal.transform.SetParent(transform);
            goal.transform.DOMove(goal.transform.position + Vector3.up * 2f, 0.5f).SetDelay(spawnAmount * 0.1f);
        }
    }

    private void SpawnGround(Vector2 position, int index)
    {
        // 检查是否已经有玩家在该位置
        JumpitPlayerController existingPlayer = FindObjectOfType<JumpitPlayerController>();
        if (existingPlayer != null && Vector2.Distance(existingPlayer.transform.position, position) < 0.1f)
        {
            // 如果该位置有玩家，就不生成方块
            return;
        }

        // 计算动画延迟
        float animationDelay = 0.1f;
        if (currentLevel >= 7)
        {
            float heightProgress = position.y / (spawnAmount * step.y);
            animationDelay = heightProgress * 0.5f;
        }
        else
        {
            animationDelay = index * 0.1f;
        }

        GameObject ground = Instantiate(
            useDisappearingGround ? disappearingGroundPrefab : groundPrefab,
            position,
            Quaternion.identity
        );
        ground.transform.SetParent(transform);

        // 设置初始缩放为0
        ground.transform.localScale = Vector3.zero;

        // 使用缩放动画
        ground.transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack)
            .SetDelay(animationDelay);

        if (useDisappearingGround)
        {
            DisappearingGround disappearingGround = ground.GetComponent<DisappearingGround>();
            if (disappearingGround != null)
            {
                disappearingGround.Initialize(baseDisappearTime, timePerHeight, fadeOutDuration, animationDelay + 0.3f);
            }
        }
    }

    public void ResetPlayerPosition()
    {
        // 获取当前玩家
        JumpitPlayerController jumpitPlayer = FindObjectOfType<JumpitPlayerController>();
        if (jumpitPlayer != null)
        {
            // 确保玩家位置正确
            jumpitPlayer.SetStartPosition(new Vector3(0, 3.5f, 0));
            // 重置玩家的所有状态
            jumpitPlayer.ResetGame();
            return;
        }

        // 如果是普通玩家，使用原始重置逻辑
        if (player != null)
        {
            player.transform.position = playerInitialPosition;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        // 清除所有生成的方块
        foreach (Transform child in transform)
        {
            if (child.gameObject != player && child.GetComponent<JumpitPlayerController>() == null)
            {
                Destroy(child.gameObject);
            }
        }

        // 重新生成关卡（仅限1-6关）
        if (currentLevel <= 6)
        {
            SpawnNewWave();
        }
    }

    private void SetupJumpitPlayer()
    {
        // 禁用原版玩家
        if (player != null)
        {
            player.SetActive(false);
        }

        // 设置出生位置
        Vector3 spawnPosition = new Vector3(0, 3.5f, 0);

        // 检查是否已存在JumpitPlayer
        JumpitPlayerController existingPlayer = FindObjectOfType<JumpitPlayerController>();
        if (existingPlayer != null)
        {
            // 如果存在，直接重置
            existingPlayer.SetStartPosition(spawnPosition);
            existingPlayer.ResetGame();
            return;
        }

        // 如果不存在，创建新的玩家
        GameObject jumpitPlayer = Instantiate(jumpitPlayerPrefab, spawnPosition, Quaternion.identity);
        jumpitPlayer.transform.SetParent(transform);

        // 设置玩家控制器
        JumpitPlayerController controller = jumpitPlayer.GetComponent<JumpitPlayerController>();
        if (controller != null)
        {
            controller.SetStartPosition(spawnPosition);
        }

        // 创建力度指示器
        if (powerIndicatorPrefab != null)
        {
            GameObject powerIndicator = Instantiate(powerIndicatorPrefab, jumpitPlayer.transform);
            powerIndicator.transform.localPosition = new Vector3(0, 1, 0);
            if (controller != null)
            {
                controller.powerIndicator = powerIndicator;
            }
        }
    }
}
