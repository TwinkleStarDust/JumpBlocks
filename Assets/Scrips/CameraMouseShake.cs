using UnityEngine;

public class CameraMouseShake : MonoBehaviour
{
    public float mouseInfluence = 0.5f; // 鼠标影响的系数
    public float lerpSpeed = 2.0f; // 插值速度
    public float maxDistance = 30.0f; // 摄像机的最大移动距离
    public bool enableShake = true; // 是否启用晃动效果

    private Vector3 initialPosition; // 摄像机初始位置
    private Vector3 initialRotation; // 摄像机初始旋转角度

    private void Start()
    {
        // 记录摄像机初始位置和旋转角度
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (enableShake)
        {
            ApplyMouseInfluence();
        }
    }

    private void ApplyMouseInfluence()
    {
        // 获取鼠标输入并计算偏移量
        float mouseX = Input.GetAxis("Mouse X") * mouseInfluence;
        float mouseY = Input.GetAxis("Mouse Y") * mouseInfluence;
        Vector3 mouseOffset = new Vector3(mouseX, mouseY, 0f);

        // 计算摄像机的新位置和旋转
        Vector3 targetPosition = initialPosition + mouseOffset;
        Vector3 targetRotation = initialRotation + new Vector3(-mouseY, mouseX, 0f);

        // 限制摄像机的新位置在最大距离范围内
        Vector3 direction = targetPosition - initialPosition;
        direction.z = 0; // 确保移动仅在摄像机所看的平面内
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
            targetPosition = initialPosition + direction;
        }

        // 使用插值平滑地移动摄像机位置和旋转
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * lerpSpeed);
    }

    // 公共方法，用于设置摄像机初始位置和旋转角度
    public void SetInitialPosition(Vector3 position, Vector3 rotation)
    {
        initialPosition = position;
        initialRotation = rotation;
    }
}
