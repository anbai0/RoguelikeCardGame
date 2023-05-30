using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p


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
        //�L�����N�^�[��ʂ֑J��
        if ((Input.GetMouseButton(0)) && !isCharaSelect)
        {
            text.SetActive(false);
            panel.SetActive(true);
            character.SetActive(true);
            Button.SetActive(true);
            isCharaSelect = true;
        }
        //�^�C�g����ʂ֑J��
        if (Input.GetKeyDown(KeyCode.Escape) && isCharaSelect)
        {
            text.SetActive(true);
            panel.SetActive(false);
            character.SetActive(false);
            Button.SetActive(false);
            isCharaSelect = false;
        }
    }

    //�{�^�����N���b�N������
    public void OnClick()
    {
        //Scene�؂�ւ�
        sceneController.sceneChange("FieldScene");
    }
}
