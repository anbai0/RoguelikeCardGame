using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneManager: MonoBehaviour
{

    [SerializeField]
    private SceneFader sceneController;

    


    public void FieldScene()
    {
        sceneController.SceneChange("FieldScene");
    }

    
}
