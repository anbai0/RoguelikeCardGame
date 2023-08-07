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
            // �L�����I���V�[�����A�����[�h���A�^�C�g���V�[����Additive
            sceneController.SceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void LoadFieldScene()
    {
        // �L�����I���V�[�����A�����[�h���A�t�B�[���h�V�[����Additive
        sceneController.SceneChange("FieldScene", "CharacterSelectionScene");
    }
}
