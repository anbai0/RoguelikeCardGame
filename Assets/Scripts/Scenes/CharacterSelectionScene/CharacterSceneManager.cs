using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    void Update()
    {
        //�^�C�g����ʂ֑J��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �L�����I���V�[�����A�����[�h���A�^�C�g���V�[�������[�h
            sceneFader.SceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void LoadFieldScene()
    {
        // �L�����I���V�[�����A�����[�h���A�t�B�[���h�V�[�������[�h
        sceneFader.SceneChange("FieldScene", "CharacterSelectionScene");
    }
}
