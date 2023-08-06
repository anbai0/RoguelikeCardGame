using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;


    public void CharaSelectScene()
    {
        //SceneêÿÇËë÷Ç¶
        sceneController.sceneChange("CharacterSelectionScene", "TitleScene");
    }
}
