using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void LoadCharaSelectScene()
    {
        // �^�C�g���V�[�����A�����[�h���A�L�����I���V�[�������[�h
        sceneFader.SceneChange("CharacterSelectionScene", "TitleScene");
    }
}
