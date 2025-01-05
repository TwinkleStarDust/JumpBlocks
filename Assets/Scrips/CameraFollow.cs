using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private Transform target;

    private void Start()
    {
        // 初始化时先找到原始玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void LateUpdate()
    {
        // 如果目标丢失，尝试重新查找玩家
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            return;
        }

        // 计算目标位置
        Vector3 desiredPosition = target.position + offset;

        // 平滑移动到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    // 提供一个方法让其他脚本可以更新跟随目标
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
