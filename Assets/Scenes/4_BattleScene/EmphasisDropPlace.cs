using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmphasisDropPlace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayGameObject()
    {
        gameObject.SetActive(true);
    }

    public void HiddenGameObject()
    {
        gameObject.SetActive(false);
    }
}
