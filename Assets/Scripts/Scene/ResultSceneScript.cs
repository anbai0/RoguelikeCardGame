using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p


public class ResultSceneScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Scene�؂�ւ�
            SceneManager.LoadScene("FieldScene");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Scene�؂�ւ�
            SceneManager.LoadScene("TitleScene");
        }
    }
}
