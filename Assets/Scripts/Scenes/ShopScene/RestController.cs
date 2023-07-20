using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 休憩に関する処理をするスクリプトです。
/// </summary>
public class RestController : MonoBehaviour
{
    PlayerDataManager playerData;

    const int restPrice = 70;                       // 休憩の値段
    [Header("参照するUI")]
    [SerializeField] GameObject restButton;
    [SerializeField] Text restText;
    [SerializeField] Text restPriceText;

    private void Start()
    {
        playerData = GameManager.Instance.playerData;
    }

    /// <summary>
    /// 休憩できるか判定するのとお金が足りない場合、
    /// テキストの色を変えたりボタンをグレーアウトにします
    /// </summary>
    /// <returns>休憩できるできる場合true</returns>
    public bool CheckRest()
    {
        if (playerData._playerMoney < restPrice)          // お金が足りない場合
        {
            restPriceText.color = Color.red;              // 値段を赤く表示
        }

        // 現在HPがMaxの場合またはお金が足りない場合
        if (playerData._playerHP == playerData._playerCurrentHP || playerData._playerMoney < restPrice)
        {
            restButton.GetComponent<Image>().color = Color.gray;  // 休憩ボタンをグレーアウト
            return false;
        }

        return true;

    }

    /// <summary>
    /// 休憩画面でテキストを表示させるメソッドです
    /// </summary>
    public void ChengeRestText()
    {
        restText.text = $"{restPrice}Gを消費して\n体力を{playerData._playerHP - playerData._playerCurrentHP}回復しますか？";
    }

    /// <summary>
    /// 休憩の処理
    /// 休憩に必要な金額を支払い、
    /// 体力を全回復させます。
    /// </summary>
    public void Rest()
    {
        playerData._playerMoney -= restPrice;
        playerData._playerCurrentHP = playerData._playerHP;
    }
}
