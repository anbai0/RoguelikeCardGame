using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;



    //�{�^�����N���b�N������
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Scene�؂�ւ�
            sceneController.sceneChange("CharacterSelectionScene");
        }
    }
}
