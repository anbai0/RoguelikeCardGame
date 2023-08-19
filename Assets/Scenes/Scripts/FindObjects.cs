using System.Collections.Generic;
using UnityEngine;

public class FindObjects : MonoBehaviour
{

    [Header("タグを設定して1を押すとObjectを確認できます")]
    public string targetTag; // 確認したいタグ
    public List<GameObject> objectsWithTag; // タグが付いているオブジェクトを保持するリスト

    [Header("コンポーネントやスクリプトを'実行中に'アタッチして2を押すとObjectを確認できます。")]
    public MonoBehaviour targetComponent; // 対象のコンポーネントやスクリプト
    public List<GameObject> objectsWithComponent; // コンポーネントがアタッチされているオブジェクトを保持するリスト




    private void Awake()
    {
        objectsWithTag = new List<GameObject>();


        objectsWithComponent = new List<GameObject>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject[] objectsArray = GameObject.FindGameObjectsWithTag(targetTag);

            objectsWithTag.AddRange(objectsArray);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Component[] components = null;
            components = FindObjectsOfType(targetComponent.GetType()) as Component[];

            foreach (Component component in components)
            {
                objectsWithComponent.Add(component.gameObject);
            }
        }

    }
}