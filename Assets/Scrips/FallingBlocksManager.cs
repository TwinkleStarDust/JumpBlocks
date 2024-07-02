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

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnBlocks());
    }

    private IEnumerator SpawnBlocks()
    {
        while (true)
        {
            SpawnBlock();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBlock()
    {
        // 随机选择一个方块预制体
        GameObject blockPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        // 随机生成X坐标
        float randomX = Random.Range(minX, maxX);

        // 生成方块
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);
        GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        // 调整方块的缩放比例
        newBlock.transform.localScale = blockScale;
    }
}
