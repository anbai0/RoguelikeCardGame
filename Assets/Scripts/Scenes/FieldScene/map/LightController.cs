using UnityEngine;

public class LightController : MonoBehaviour
{

    void Update()
    {
        transform.position = Camera.main.transform.position + new Vector3(0, -4, 0);
    }
}
