using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardDataManager cardData;       // �J�[�h�̃f�[�^������
    CardController cardController;
    public Lottery lottery;

    int Card1, Card2, Card3;    //���I���ꂽ�J�[�h��ID������ϐ�
    int heelPotionCardID = 3;   //�񕜃J�[�h��ID

    bool isHeelPotion = false;  //�񕜃J�[�h�������Ă��邩�H


    [SerializeField]
    Transform cardPlace1, cardPlace2, cardPlace3;
    [SerializeField]
    Object cardPrefab;
    
    private CardController SelectController;    //���������J�[�hPrefab�ɃA�^�b�`����Ă���CardController���i�[����

    [SerializeField]
    int tmpID = 0;

    private void Start()
    {
        Object card1 = Instantiate(cardPrefab, cardPlace1);
        Object card2 = Instantiate(cardPrefab, cardPlace2);
        Object card3 = Instantiate(cardPrefab, cardPlace3);




    }



    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    (Card1, Card2, Card3) = lottery.ShopLottery();     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        //    Debug.Log("�J�[�h1:" + Card1 + "\n�J�[�h2:" + Card2 + "\n�J�[�h3:" + Card3);
        //}


        if (Lottery.isInitialize)
        {
            lottery1();

            Card1.

            CardController card = SelectController.GetComponent<CardController>();//�J�[�h�𐶐�����
            card.Init(Card1);

        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(tmpID >= 21)
                tmpID = 0;
            
            tmpID++;

            CreateCard(tmpID);
        }

    }

    void CreateCard(int ID)
    {
        CardController card = SelectController.GetComponent<CardController>();//�J�[�h�𐶐�����
        card.Init(ID);
    }

    void lottery1()
    {
        (Card1, Card2, Card3) = lottery.ShopLottery();     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        Debug.Log("�J�[�h1:" + Card1 + "\n�J�[�h2:" + Card2 + "\n�J�[�h3:" + Card3);

        Lottery.isInitialize = false;
    }
    
}
