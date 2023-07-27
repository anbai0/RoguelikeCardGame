using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    PlayerDataManager playerData;

    [SerializeField]
    Text myMoneyText;   //所持金を表示するテキスト



    void Start()
    {
        playerData = GameManager.Instance.playerData;
    }

    void Update()
    {
        RefreshMoneyText();
    }

    /// <summary>
    /// 所持金のテキストを更新するメソッドです。
    /// </summary>
    void RefreshMoneyText()
    {
        myMoneyText.text = playerData._playerMoney.ToString();
    }
}
