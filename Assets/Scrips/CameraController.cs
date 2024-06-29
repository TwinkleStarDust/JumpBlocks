using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform mainMenuPosition;
    public Transform settingsMenuPosition;

    private void Start()
    {
        // ȷ������������˵�λ��
        transform.position = mainMenuPosition.position;
        transform.rotation = mainMenuPosition.rotation;
    }

    public void MoveToSettings()
    {
        // �ƶ�����ת����������ý���
        transform.DOMove(settingsMenuPosition.position, 1f);
        transform.DORotateQuaternion(settingsMenuPosition.rotation, 1f);
    }

    public void MoveToMainMenu()
    {
        // �ƶ�����ת����������˵�����
        transform.DOMove(mainMenuPosition.position, 1f);
        transform.DORotateQuaternion(mainMenuPosition.rotation, 1f);
    }
}
