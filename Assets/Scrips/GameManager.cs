using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject groundPrefab; // ����Ԥ����
    public GameObject goalPrefab; // �յ�Ԥ����
    public GameObject player; // ��Ҷ���

    public int spawnAmount; // ��������
    public Vector2 step = new Vector2(4.25f, 7.5f); // ����

    private Vector2 playerInitialPosition; // ��ҳ�ʼλ��

    void Start()
    {
        playerInitialPosition = player.transform.position; // ��¼��ҳ�ʼλ��
        SpawnNewWave(); // �����²���
    }

    // �����²���
    void SpawnNewWave()
    {
        List<Vector2> mainPath = new List<Vector2>(); // ��·��
        Vector2 spawnPos = Vector2.zero; // ��ʼ����λ��

        // ������·��
        for (int i = 0; i < spawnAmount; i++)
        {
            int randomDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1; // �������
            spawnPos += new Vector2(step.x * randomDir, step.y); // ��������λ��
            mainPath.Add(spawnPos); // ��ӵ���·���б�

            GameObject ground = Instantiate(groundPrefab, spawnPos - Vector2.up * 2f, Quaternion.identity); // ʵ��������
            ground.transform.SetParent(transform); // ���ø���
            ground.transform.DOMove(ground.transform.position + Vector3.up * 2f, 0.5f).SetDelay(i * 0.1f); // �����ƶ�
        }

        // ���ɷ�֧
        for (int i = 1; i < spawnAmount - 1; i++)
        {
            int branches = Random.Range(1, 3); // ����֧·
            for (int j = 0; j < branches; j++)
            {
                Vector2 branchPos = mainPath[i]; // ��֧λ��
                int randomDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1; // �������
                branchPos += new Vector2(step.x * randomDir, step.y); // ���·�֧λ��

                GameObject ground = Instantiate(groundPrefab, branchPos - Vector2.up * 2f, Quaternion.identity); // ʵ��������
                ground.transform.SetParent(transform); // ���ø���
                ground.transform.DOMove(ground.transform.position + Vector3.up * 2f, 0.5f).SetDelay((i + j + 1) * 0.1f); // �����ƶ�

                // ȷ����֧�������ӵ���·��
                if (j == branches - 1)
                {
                    branchPos = mainPath[i + 1]; // ��֧��������λ��
                    GameObject reconnectGround = Instantiate(groundPrefab, branchPos - Vector2.up * 2f, Quaternion.identity); // ʵ��������
                    reconnectGround.transform.SetParent(transform); // ���ø���
                    reconnectGround.transform.DOMove(reconnectGround.transform.position + Vector3.up * 2f, 0.5f).SetDelay((i + j + 1) * 0.1f); // �����ƶ�
                }
            }
        }

        // ����·��ĩβ�����յ�
        Vector2 finalPos = mainPath[mainPath.Count - 1]; // ����λ��
        Vector2 goalPos = finalPos + new Vector2(step.x, step.y); // ����һ�����յ�λ��
        GameObject goal = Instantiate(goalPrefab, goalPos - Vector2.up * 2f, Quaternion.identity); // ʵ�����յ�
        goal.transform.SetParent(transform); // ���ø���
        goal.transform.DOMove(goal.transform.position + Vector3.up * 2f, 0.5f).SetDelay(spawnAmount * 0.1f); // �����ƶ�
    }

    // �������λ�õķ���
    public void ResetPlayerPosition()
    {
        player.transform.position = playerInitialPosition; // �������λ��
        var playerController = player.GetComponent<PlayerController>(); // ��ȡ��ҿ��������
        playerController.SetHasDied(false); // ������ҵ�����״̬
        playerController.ResetGame(); // ������Ϸ״̬
    }
}
