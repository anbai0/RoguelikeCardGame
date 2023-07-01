using UnityEngine;

public class BounceImage : MonoBehaviour
{
    public float amplitude = 1f;         // ’µ‚Ë‚ÌU•
    public float frequency = 1f;         // ’µ‚Ë‚Ìü”g”

    private Vector3 initialPosition;     // ‰ŠúˆÊ’u

    private void Start()
    {
        // ‰ŠúˆÊ’u‚ğ•Û‘¶‚·‚é
        initialPosition = transform.position;
    }

    private void Update()
    {

        // Œ»İ‚ÌŠÔ‚É‰‚¶‚Ä‰æ‘œ‚ğ’µ‚Ë‚³‚¹‚é
        float newY = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
