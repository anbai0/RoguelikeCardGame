using System.Collections.Generic;
using UnityEngine;

public class TagChecker : MonoBehaviour
{
    public string targetTag; // 確認したいタグ
    public List<GameObject> objectsWithTag; // タグが付いているオブジェクトを保持するリスト

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