using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用


public class TitleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    public GameObject text;
    public GameObject panel;
    public GameObject character;
    public GameObject Button;
    bool isCharaSelect = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCharaSelect)
        {
            text.SetActive(false);
            panel.SetActive(true);
            character.SetActive(true);
            Button.SetActive(true);
            isCharaSelect = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isCharaSelect)
        {
            text.SetActive(true);
            panel.SetActive(false);
            character.SetActive(false);
            Button.SetActive(false);
            isCharaSelect = false;
        }
    }

    public void OnClick()
    {
        //Scene切り替え
        sceneController.sceneChange("FieldScene");
    }
}
