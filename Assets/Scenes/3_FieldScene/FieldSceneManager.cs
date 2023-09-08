using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FieldSceneManager : MonoBehaviour
{

    private void Start()
    {
        // BGMを流します
        if (Random.Range(0, 2) == 0)
        {
            AudioManager.Instance.PlayBGM("Field1");
        }
        else
        {
            AudioManager.Instance.PlayBGM("Field2");
        }
    }

    public void LoadBattleScene()
    {
        // バトルシーンをロード
        SceneFader.Instance.SceneChange("BattleScene");
    }

    public void LoadBonfireScene()
    {
        // 焚火シーンをロード
        SceneFader.Instance.SceneChange("BonfireScene");
    }

    public void LoadTreasureBoxScene()
    {
        // 宝箱シーンをロード
        SceneFader.Instance.SceneChange("TreasureBoxScene");
    }

    public void LoadShopScene()
    {
        // ショップシーンをロード
        SceneFader.Instance.SceneChange("ShopScene");
    }

    public void ActivateShopScene()
    {
        // ショップシーンのオブジェクトを表示
        SceneFader.Instance.ToggleSceneWithFade("ShopScene", true);
    }
}
