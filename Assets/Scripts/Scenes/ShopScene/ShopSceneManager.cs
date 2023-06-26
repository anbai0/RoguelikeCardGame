using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneManager: MonoBehaviour
{

    [SerializeField]
    private SceneController sceneController;

    


    public void FieldScene()
    {
        sceneController.sceneChange("FieldScene");
    }

    
}
