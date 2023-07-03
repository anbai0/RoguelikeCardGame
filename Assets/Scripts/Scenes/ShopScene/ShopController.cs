using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardController cardController;

    public Lottery lottery;

    bool isHeelPotion = false;

    int Card1, Card2, Card3;

    private void Start()
    {


    }


    public void ShowCards()
    {
        (Card1, Card2, Card3) = lottery.ShopLottery();     // メモ: タプルと言って複数の戻り値を受け取れる
        
        Debug.Log("カード1:" + Card1 + "\nカード2:" + Card2 + "\nカード3:" + Card3);
    }

    void Update()
    {


        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShowCards();
        }
        

    }
}
