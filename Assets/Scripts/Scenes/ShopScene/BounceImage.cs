using UnityEngine;

public class BounceImage : MonoBehaviour
{
    public float amplitude = 1f;         // 跳ねの振幅
    public float frequency = 1f;         // 跳ねの周波数

    private Vector3 initialPosition;     // 初期位置

    private void Start()
    {
        // 初期位置を保存する
        initialPosition = transform.position;
    }

    private void Update()
    {

        // 現在の時間に応じて画像を跳ねさせる
        float newY = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
