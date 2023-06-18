using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        //ƒ^ƒCƒgƒ‹‰æ–Ê‚Ö‘JˆÚ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SceneØ‚è‘Ö‚¦
            sceneController.sceneChange("TitleScene");
        }
    }

    public void FieldScene()
    {
        sceneController.sceneChange("FieldScene");
    }
}
