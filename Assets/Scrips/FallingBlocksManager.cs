using System.Collections;
using UnityEngine;

public class FallingBlocksManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // 方块预制体数组
    public float spawnInterval = 0.5f; // 生成间隔
    public float spawnHeight = 500f; // 生成高度
    public float minX = 600f; // 生成区域最小X坐标
    public float maxX = 900f; // 生成区域最大X坐标
    public float fallSpeed = 1f; // 下落速度
    public float rotateSpeed = 180f; // 旋转速度
    public Vector3 blockScale = new Vector3(2f, 2f, 2f); // 方块的缩放比例
    public float destroyBelowY = -100f; // 销毁方块的Y坐标阈值

    private Camera mainCamera;
    private Transform blocksContainer; // 用于存放所有生成的方块
    private bool isSpawning = true;

    private void Awake()
    {
        // 创建一个容器来存放所有方块
        blocksContainer = new GameObject("BlocksContainer").transform;
        blocksContainer.SetParent(transform);
    }

    private void Start()
    {
        if (blockPrefabs == null || blockPrefabs.Length == 0)
        {
            Debug.LogError("没有设置方块预制体！");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("找不到主相机！");
            return;
        }

        StartCoroutine(SpawnBlocks());
        StartCoroutine(CleanupBlocks());
    }

    private void OnEnable()
    {
        isSpawning = true;
    }

    private void OnDisable()
    {
        isSpawning = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnBlocks()
    {
        while (isSpawning)
        {
            SpawnBlock();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBlock()
    {
        if (blockPrefabs == null || blockPrefabs.Length == 0) return;

        // 随机选择一个方块预制体
        GameObject blockPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        // 随机生成X坐标
        float randomX = Random.Range(minX, maxX);

        // 生成方块
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);
        GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        // 设置父物体
        newBlock.transform.SetParent(blocksContainer);

        // 调整方块的缩放比例
        newBlock.transform.localScale = blockScale;

        // 添加下落和旋转行为
        StartCoroutine(FallAndRotateBlock(newBlock.transform));
    }

    private IEnumerator FallAndRotateBlock(Transform block)
    {
        while (block != null && block.position.y > destroyBelowY)
        {
            // 下落
            block.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // 旋转
            block.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

            yield return null;
        }

        // 如果方块还存在，销毁它
        if (block != null)
        {
            Destroy(block.gameObject);
        }
    }

    private IEnumerator CleanupBlocks()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // 每5秒检查一次

            // 获取所有方块
            Transform[] blocks = blocksContainer.GetComponentsInChildren<Transform>();

            // 检查并销毁低于阈值的方块
            foreach (Transform block in blocks)
            {
                if (block != blocksContainer && block.position.y < destroyBelowY)
                {
                    Destroy(block.gameObject);
                }
            }
        }
    }

    // 清理所有方块的方法
    public void ClearAllBlocks()
    {
        foreach (Transform child in blocksContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
