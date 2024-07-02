using UnityEngine;

public class CameraMouseShake : MonoBehaviour
{
    public float mouseInfluence = 0.5f; // ���Ӱ���ϵ��
    public float lerpSpeed = 2.0f; // ��ֵ�ٶ�
    public float maxDistance = 30.0f; // �����������ƶ�����
    public bool enableShake = true; // �Ƿ����ûζ�Ч��

    private Vector3 initialPosition; // �������ʼλ��
    private Vector3 initialRotation; // �������ʼ��ת�Ƕ�

    private void Start()
    {
        // ��¼�������ʼλ�ú���ת�Ƕ�
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
        // ��ȡ������벢����ƫ����
        float mouseX = Input.GetAxis("Mouse X") * mouseInfluence;
        float mouseY = Input.GetAxis("Mouse Y") * mouseInfluence;
        Vector3 mouseOffset = new Vector3(mouseX, mouseY, 0f);

        // �������������λ�ú���ת
        Vector3 targetPosition = initialPosition + mouseOffset;
        Vector3 targetRotation = initialRotation + new Vector3(-mouseY, mouseX, 0f);

        // �������������λ���������뷶Χ��
        Vector3 direction = targetPosition - initialPosition;
        direction.z = 0; // ȷ���ƶ����������������ƽ����
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
            targetPosition = initialPosition + direction;
        }

        // ʹ�ò�ֵƽ�����ƶ������λ�ú���ת
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * lerpSpeed);
    }

    // �������������������������ʼλ�ú���ת�Ƕ�
    public void SetInitialPosition(Vector3 position, Vector3 rotation)
    {
        initialPosition = position;
        initialRotation = rotation;
    }
}
