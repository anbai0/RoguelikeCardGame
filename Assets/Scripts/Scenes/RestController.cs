using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 休憩に関する処理をするスクリプトです。
/// </summary>
public class RestController : MonoBehaviour
{
    public GameManager gm {private get; set; }

    private const int restPrice = 70;          // 休憩の値段
    [Header("参照するUI")]
    [SerializeField] GameObject restButton;
    [SerializeField] TextMeshProUGUI restText;
    [Header ("ShopSceneの時だけ使う")]
    [SerializeField] TextMeshProUGUI restPriceText;

    private void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;
    }


    /// <summary>
    /// 休憩できるか判定するのとお金が足りない場合、
    /// テキストの色を変えたりボタンをグレーアウトにします
    /// </summary>
    /// <param name="sceneType">呼び出す側のシーン名</param>
    /// <returns>休憩できるできる場合true</returns>
    public bool CheckRest(string sceneType)
    {
        if (sceneType == "ShopScene")                              // ShopSceneから呼ばれたときだけ値段チェックする
        {
            if (gm.playerData._playerMoney < restPrice)          // お金が足りない場合
            {
                restPriceText.color = Color.red;              // 値段を赤く表示
            }

            // 現在HPがMaxの場合またはお金が足りない場合
            if (gm.playerData._playerHP == gm.playerData._playerCurrentHP || gm.playerData._playerMoney < restPrice)
            {
                restButton.GetComponent<Image>().color = Color.gray;  // 休憩ボタンをグレーアウト
                return false;
            }
        }

        if (sceneType == "BonfireScene")                                   // BonfireSceneから呼ばれたときだけ値段チェックする
        {
            // 現在HPがMaxの場合またはお金が足りない場合
            if (gm.playerData._playerHP == gm.playerData._playerCurrentHP)
            {
                restButton.GetComponent<Image>().color = Color.gray;  // 休憩ボタンをグレーアウト
                return false;
            }
        }

        return true;

    }

    /// <summary>
    /// 休憩画面でテキストを表示させるメソッドです
    /// </summary>
    /// <param name="sceneType">呼び出す側のシーン名</param>
    public void ChengeRestText(string sceneType)
    {
        if (sceneType == "ShopScene")
            restText.text = $"{restPrice}Gを消費して\n体力を{gm.playerData._playerHP - gm.playerData._playerCurrentHP}回復しますか？";

        if (sceneType == "BonfireScene")
            restText.text = $"焚火を使用して\n体力を{gm.playerData._playerHP - gm.playerData._playerCurrentHP}回復しますか？";
    }

    /// <summary>
    /// 休憩の処理
    /// 休憩に必要な金額を支払い、
    /// 体力を全回復させます。
    /// </summary>
    /// <param name="sceneType">呼び出す側のシーン名</param>
    public void Rest(string sceneType)
    {
        if (sceneType == "ShopScene") { gm.playerData._playerMoney -= restPrice; }         // ショップシーンから呼ばれたときだけお金を払う
        
        gm.playerData._playerCurrentHP = gm.playerData._playerHP;
    }
}
