using UnityEngine;

public class CameraMouseShake : MonoBehaviour
{
    public float mouseInfluence = 0.5f; // ���Ӱ���ϵ��
    public float lerpSpeed = 2.0f; // ��ֵ�ٶ�
    public float maxDistance = 30.0f; // �����������ƶ�����
    public bool enableShake = true; // �Ƿ����ûζ�Ч��
    public float maxRotationAngle = 30f; // �����ת�Ƕ�

    private Vector3 initialPosition; // �������ʼλ��
    private Vector3 initialRotation; // �������ʼ��ת�Ƕ�
    private Vector3 currentRotation; // ��ǰ�ۻ�����ת�Ƕ�

    private void Start()
    {
        // ��¼�������ʼλ�ú���ת�Ƕ�
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        currentRotation = initialRotation;
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
        // ��ȡ�������
        float mouseX = Input.GetAxis("Mouse X") * mouseInfluence;
        float mouseY = Input.GetAxis("Mouse Y") * mouseInfluence;

        // �����ۻ�����ת�Ƕ�
        currentRotation.x = Mathf.Clamp(currentRotation.x - mouseY,
            initialRotation.x - maxRotationAngle,
            initialRotation.x + maxRotationAngle);

        currentRotation.y = Mathf.Clamp(currentRotation.y + mouseX,
            initialRotation.y - maxRotationAngle,
            initialRotation.y + maxRotationAngle);

        // �������������λ��
        Vector3 mouseOffset = new Vector3(mouseX, mouseY, 0f);
        Vector3 targetPosition = transform.position + mouseOffset;

        // �������������λ���������뷶Χ��
        Vector3 direction = targetPosition - initialPosition;
        direction.z = 0; // ȷ���ƶ����������������ƽ����
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
            targetPosition = initialPosition + direction;
        }

        // ƽ���ƶ������λ�ú���ת
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(currentRotation),
            Time.deltaTime * lerpSpeed);
    }

    // �������λ�ú���ת
    public void ResetCamera()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
        currentRotation = initialRotation;
    }

    // �����������ʼλ�ú���ת�Ƕ�
    public void SetInitialPosition(Vector3 position, Vector3 rotation)
    {
        initialPosition = position;
        initialRotation = rotation;
        currentRotation = rotation;
        ResetCamera();
    }
}
