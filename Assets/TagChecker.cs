using System.Collections.Generic;
using UnityEngine;

public class TagChecker : MonoBehaviour
{
    public string targetTag; // �m�F�������^�O
    public List<GameObject> objectsWithTag; // �^�O���t���Ă���I�u�W�F�N�g��ێ����郊�X�g

    private void Awake()
    {
        objectsWithTag = new List<GameObject>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            GameObject[] objectsArray = GameObject.FindGameObjectsWithTag(targetTag);

            objectsWithTag.AddRange(objectsArray);
        }
    }
}