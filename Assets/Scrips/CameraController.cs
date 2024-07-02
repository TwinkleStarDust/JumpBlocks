using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform mainMenuPosition;
    public Transform settingsMenuPosition;
    private CameraMouseShake cameraMouseShake;

    private void Start()
    {
        cameraMouseShake = GetComponent<CameraMouseShake>();

        // ȷ������������˵�λ��
        transform.position = mainMenuPosition.position;
        transform.rotation = mainMenuPosition.rotation;

        // ���ó�ʼλ�ú���ת�Ƕ�
        if (cameraMouseShake != null)
        {
            cameraMouseShake.SetInitialPosition(transform.position, transform.eulerAngles);
        }
    }

    public void MoveToSettings()
    {
        // ���ûζ�Ч��
        if (cameraMouseShake != null)
        {
            cameraMouseShake.enableShake = false;
        }

        // �ƶ�����ת����������ý���
        transform.DOMove(settingsMenuPosition.position, 1f);
        transform.DORotateQuaternion(settingsMenuPosition.rotation, 1f).OnComplete(() =>
        {
            // ������λ�ú���ת�Ƕȣ����������ûζ�Ч��
            if (cameraMouseShake != null)
            {
                cameraMouseShake.SetInitialPosition(transform.position, transform.eulerAngles);
                cameraMouseShake.enableShake = true;
            }
        });
    }

    public void MoveToMainMenu()
    {
        // ���ûζ�Ч��
        if (cameraMouseShake != null)
        {
            cameraMouseShake.enableShake = false;
        }

        // �ƶ�����ת����������˵�����
        transform.DOMove(mainMenuPosition.position, 1f);
        transform.DORotateQuaternion(mainMenuPosition.rotation, 1f).OnComplete(() =>
        {
            // ������λ�ú���ת�Ƕȣ����������ûζ�Ч��
            if (cameraMouseShake != null)
            {
                cameraMouseShake.SetInitialPosition(transform.position, transform.eulerAngles);
                cameraMouseShake.enableShake = true;
            }
        });
    }
}
