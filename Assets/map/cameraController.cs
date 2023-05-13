using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject lightprefab;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-10, 6, 0);
        transform.rotation =  Quaternion.Euler(30, 90, 0);
       
    }

    // Update is called once per frame
    void Update()
    {
        lightprefab.transform.position = Camera.main.transform.position + new Vector3(0, -4, 0);
    }
}
