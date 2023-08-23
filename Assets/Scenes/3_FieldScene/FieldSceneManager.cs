using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FieldSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

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
        sceneFader.SceneChange("BattleScene");
    }

    public void LoadBonfireScene()
    {
        // 焚火シーンをロード
        sceneFader.SceneChange("BonfireScene");
    }

    public void LoadTreasureBoxScene()
    {
        // 宝箱シーンをロード
        sceneFader.SceneChange("TreasureBoxScene");
    }

    public void LoadShopScene()
    {
        // ショップシーンをロード
        sceneFader.SceneChange("ShopScene");
    }

    public void ActivateShopScene()
    {
        // ショップシーンのオブジェクトを表示
        sceneFader.ToggleSceneWithFade("ShopScene", true);
    }
}
