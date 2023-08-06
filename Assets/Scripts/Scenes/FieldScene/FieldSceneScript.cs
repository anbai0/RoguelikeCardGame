using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SceneêÿÇËë÷Ç¶
            sceneController.sceneChange("BattleScene");
        }
    }
}
