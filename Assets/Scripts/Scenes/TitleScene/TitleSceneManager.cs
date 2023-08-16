using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void LoadCharaSelectScene()
    {
        // タイトルシーンをアンロードし、キャラ選択シーンをロード
        sceneFader.SceneChange("CharacterSelectionScene", "TitleScene");
    }
}
