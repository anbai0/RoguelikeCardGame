using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
