using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{

    public void LoadCharaSelectScene()
    {
        // タイトルシーンをアンロードし、キャラ選択シーンをロード
        SceneFader.Instance.SceneChange("CharacterSelectionScene", "TitleScene");
    }
}
