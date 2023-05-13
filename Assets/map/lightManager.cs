using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightManager : MonoBehaviour
{
    [SerializeField] GameObject lightObject;
    private float lightStrength;

    void Start()
    {
        lightStrength = lightObject.GetComponent<Light>().intensity;
        lightStrength = 70.0f;
    }

    void Update()
    {
        lightObject.GetComponent<Light>().intensity = lightStrength;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lightStrength++;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lightStrength--;
        }
    }
}