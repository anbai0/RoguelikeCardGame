using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p


public class CharacterSelectScene : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Scene�؂�ւ�
            sceneController.sceneChange("FieldScene");
        }
    }
}
