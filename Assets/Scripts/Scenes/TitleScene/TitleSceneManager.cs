using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;


    public void LoadCharaSelectScene()
    {
        // �^�C�g���V�[�����A�����[�h���A�L�����I���V�[����Additive
        sceneController.SceneChange("CharacterSelectionScene", "TitleScene");
    }
}
