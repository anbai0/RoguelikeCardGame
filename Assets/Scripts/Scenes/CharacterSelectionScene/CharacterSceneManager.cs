using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        //�^�C�g����ʂ֑J��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Scene�؂�ւ�
            sceneController.sceneChange("TitleScene");
        }
    }

    public void FieldScene()
    {
        sceneController.sceneChange("FieldScene");
    }
}
