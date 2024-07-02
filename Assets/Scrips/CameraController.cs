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

        // 确保摄像机在主菜单位置
        transform.position = mainMenuPosition.position;
        transform.rotation = mainMenuPosition.rotation;

        // 设置初始位置和旋转角度
        if (cameraMouseShake != null)
        {
            cameraMouseShake.SetInitialPosition(transform.position, transform.eulerAngles);
        }
    }

    public void MoveToSettings()
    {
        // 禁用晃动效果
        if (cameraMouseShake != null)
        {
            cameraMouseShake.enableShake = false;
        }

        // 移动和旋转摄像机到设置界面
        transform.DOMove(settingsMenuPosition.position, 1f);
        transform.DORotateQuaternion(settingsMenuPosition.rotation, 1f).OnComplete(() =>
        {
            // 设置新位置和旋转角度，并重新启用晃动效果
            if (cameraMouseShake != null)
            {
                cameraMouseShake.SetInitialPosition(transform.position, transform.eulerAngles);
                cameraMouseShake.enableShake = true;
            }
        });
    }

    public void MoveToMainMenu()
    {
        // 禁用晃动效果
        if (cameraMouseShake != null)
        {
            cameraMouseShake.enableShake = false;
        }

        // 移动和旋转摄像机到主菜单界面
        transform.DOMove(mainMenuPosition.position, 1f);
        transform.DORotateQuaternion(mainMenuPosition.rotation, 1f).OnComplete(() =>
        {
            // 设置新位置和旋转角度，并重新启用晃动效果
            if (cameraMouseShake != null)
            {
                cameraMouseShake.SetInitialPosition(transform.position, transform.eulerAngles);
                cameraMouseShake.enableShake = true;
            }
        });
    }
}
