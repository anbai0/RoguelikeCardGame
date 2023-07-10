using System.Collections;
using System.Collections.Generic;
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
        myMoneyText.text = playerData._money.ToString();
    }
}
