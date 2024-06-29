using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform mainMenuPosition;
    public Transform settingsMenuPosition;

    private void Start()
    {
        // 确保摄像机在主菜单位置
        transform.position = mainMenuPosition.position;
        transform.rotation = mainMenuPosition.rotation;
    }

    public void MoveToSettings()
    {
        // 移动和旋转摄像机到设置界面
        transform.DOMove(settingsMenuPosition.position, 1f);
        transform.DORotateQuaternion(settingsMenuPosition.rotation, 1f);
    }

    public void MoveToMainMenu()
    {
        // 移动和旋转摄像机到主菜单界面
        transform.DOMove(mainMenuPosition.position, 1f);
        transform.DORotateQuaternion(mainMenuPosition.rotation, 1f);
    }
}
