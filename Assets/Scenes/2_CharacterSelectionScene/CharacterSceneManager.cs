using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{

    void Update()
    {
        //�^�C�g����ʂ֑J��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �L�����I���V�[�����A�����[�h���A�^�C�g���V�[�������[�h
            SceneFader.Instance.SceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void LoadFieldScene()
    {
        // �L�����I���V�[�����A�����[�h���A�t�B�[���h�V�[�������[�h
        SceneFader.Instance.SceneChange("FieldScene", "CharacterSelectionScene",true);
    }
}
