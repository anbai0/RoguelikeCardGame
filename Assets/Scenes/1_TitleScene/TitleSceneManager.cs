using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{

    public void LoadCharaSelectScene()
    {
        // �^�C�g���V�[�����A�����[�h���A�L�����I���V�[�������[�h
        SceneFader.Instance.SceneChange("CharacterSelectionScene", "TitleScene");
    }
}
