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
    public float destroyBelowY = -100f; // ���ٷ����Y������ֵ

    private Camera mainCamera;
    private Transform blocksContainer; // ���ڴ���������ɵķ���
    private bool isSpawning = true;

    private void Awake()
    {
        // ����һ��������������з���
        blocksContainer = new GameObject("BlocksContainer").transform;
        blocksContainer.SetParent(transform);
    }

    private void Start()
    {
        if (blockPrefabs == null || blockPrefabs.Length == 0)
        {
            Debug.LogError("û�����÷���Ԥ���壡");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("�Ҳ����������");
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

        // ���ѡ��һ������Ԥ����
        GameObject blockPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        // �������X����
        float randomX = Random.Range(minX, maxX);

        // ���ɷ���
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);
        GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        // ���ø�����
        newBlock.transform.SetParent(blocksContainer);

        // ������������ű���
        newBlock.transform.localScale = blockScale;

        // ����������ת��Ϊ
        StartCoroutine(FallAndRotateBlock(newBlock.transform));
    }

    private IEnumerator FallAndRotateBlock(Transform block)
    {
        while (block != null && block.position.y > destroyBelowY)
        {
            // ����
            block.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // ��ת
            block.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

            yield return null;
        }

        // ������黹���ڣ�������
        if (block != null)
        {
            Destroy(block.gameObject);
        }
    }

    private IEnumerator CleanupBlocks()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // ÿ5����һ��

            // ��ȡ���з���
            Transform[] blocks = blocksContainer.GetComponentsInChildren<Transform>();

            // ��鲢���ٵ�����ֵ�ķ���
            foreach (Transform block in blocks)
            {
                if (block != blocksContainer && block.position.y < destroyBelowY)
                {
                    Destroy(block.gameObject);
                }
            }
        }
    }

    // �������з���ķ���
    public void ClearAllBlocks()
    {
        foreach (Transform child in blocksContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
