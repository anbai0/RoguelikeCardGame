using UnityEngine;

public class BounceImage : MonoBehaviour
{
    public float amplitude = 1f;         // ���˂̐U��
    public float frequency = 1f;         // ���˂̎��g��

    private Vector3 initialPosition;     // �����ʒu

    private void Start()
    {
        // �����ʒu��ۑ�����
        initialPosition = transform.position;
    }

    private void Update()
    {

        // ���݂̎��Ԃɉ����ĉ摜�𒵂˂�����
        float newY = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
