using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    string playerInput = "";
    bool isDebug = false;
    [SerializeField] UIManagerCharaSelect uiManager;

    void Update()
    {
        //タイトル画面へ遷移
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // キャラ選択シーンをアンロードし、タイトルシーンをロード
            SceneFader.Instance.SceneChange("TitleScene", "CharacterSelectionScene");
        }


        // 隠しコマンド
        if (Input.inputString.Length > 0 && !uiManager.isClick)
        {
            playerInput += Input.inputString;

            if (playerInput.Length > 5) playerInput = "";

            // "001"が入力されたかどうかをチェック
            if (playerInput.Contains("001") && !isDebug)
            {
                isDebug = true;
                Debug.Log("001が入力されました！");

                GameManager.Instance.ReadPlayer("DebugChan");

                uiManager.ShowLottery();
                uiManager.UIEventsReload();
                uiManager.lotteryScreen.SetActive(true);

                // 入力をリセット
                playerInput = "";
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySE("guard1");         
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioManager.Instance.PlaySE("guard2");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AudioManager.Instance.PlaySE("guard3");
        }
    }

    public void LoadFieldScene()
    {
        // キャラ選択シーンをアンロードし、フィールドシーンをロード
        SceneFader.Instance.SceneChange("FieldScene", "CharacterSelectionScene",true);
    }
}
