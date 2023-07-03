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
        (Card1, Card2, Card3) = lottery.ShopLottery();     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        
        Debug.Log("�J�[�h1:" + Card1 + "\n�J�[�h2:" + Card2 + "\n�J�[�h3:" + Card3);
    }

    void Update()
    {


        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShowCards();
        }
        

    }
}
