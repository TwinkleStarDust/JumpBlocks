using System.Collections;   
using UnityEngine;

public class FallingBlocksManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // ����Ԥ��������
    public float spawnInterval = 0.5f; // ���ɼ��
    public float spawnHeight = 500f; // ���ɸ߶�
    public float minX = 600f; // ����������СX����
    public float maxX = 900f; // �����������X����
    public float fallSpeed = 1f; // �����ٶ�
    public float rotateSpeed = 180f; // ��ת�ٶ�
    public Vector3 blockScale = new Vector3(2f, 2f, 2f); // ��������ű���

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
        // ���ѡ��һ������Ԥ����
        GameObject blockPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        // �������X����
        float randomX = Random.Range(minX, maxX);

        // ���ɷ���
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);
        GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        // ������������ű���
        newBlock.transform.localScale = blockScale;
    }
}
