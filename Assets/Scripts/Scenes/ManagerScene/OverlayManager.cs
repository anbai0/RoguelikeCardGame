using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    PlayerDataManager playerData;

    [SerializeField]
    Text myMoneyText;   //��������\������e�L�X�g

    void Start()
    {
        playerData = GameManager.Instance.playerData;
    }

    void Update()
    {
        RefreshMoneyText();
    }

    /// <summary>
    /// �������̃e�L�X�g���X�V���郁�\�b�h�ł��B
    /// </summary>
    void RefreshMoneyText()
    {
        if (playerData != null)
        myMoneyText.text = playerData._playerMoney.ToString();
    }
}
