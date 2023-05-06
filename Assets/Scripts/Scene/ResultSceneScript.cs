using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用


public class ResultSceneScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Scene切り替え
            SceneManager.LoadScene("FieldScene");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Scene切り替え
            SceneManager.LoadScene("TitleScene");
        }
    }
}
