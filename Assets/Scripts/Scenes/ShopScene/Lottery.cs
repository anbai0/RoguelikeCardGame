using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{

    PlayerDataManager playerDataManager;
    public CardViewManager cardViewManager;// �J�[�h�̌����ڂ̏���
    public CardDataManager cardData;// �J�[�h�̃f�[�^������
    [SerializeField]
    //CardDataManager []cardDataManager;

    ShopController shopController;

    List<CardDataManager> CardRarity1;
    List<CardDataManager> CardRarity2;
    List<CardDataManager> CardRarity3;

    void Start()
    {
        for(int i = 1; i <= 20; i++)
        {
            cardData = new CardDataManager(i);
            //�e�J�[�h�̃��A���e�B�ɕ�����
            if (cardData._cardRarity == 1)
            {
                CardRarity1.Add(cardData._cardID);
                Debug.Log(CardRarity1[i]);
            }
            if (cardData._cardRarity == 2)
            {
                CardRarity2.Add(cardData);
            }
            if (cardData._cardRarity == 3)
            {
                CardRarity3.Add(cardData);
            }
        }
        
    }


    void Update()
    {
        
    }

    void zako()     //���A�x�P���Q���A�Q���P�� 
    {
        
    }

    void kyoutakara()   //���A�x1���P���A�Q���Q��
    {

    }

    void boss()     //���A�x�Q���Q���A�R���P��
    {

    }

    void shop()     //���A�x2���P���A���A�x�P���Q���A�񕜂��P���i�m��j
    {
        int shopLottery2 = Random.Range(0, CardRarity2.Count);
        int rare2 = 0;
        for(int i = 0; i < playerDataManager._deckList.Count; i++)  //�����Ă���J�[�h�̖�������
        {
            if (CardRarity2[shopLottery2]._cardID == playerDataManager._deckList[i])�@�@//���I���ꂽID�������Ă���J�[�h�ɂ��邩�H
            {
                continue;
            }
            rare2 = CardRarity2[shopLottery2]._cardID;
        }
        shopController.ShowCards(rare2, 1, 1);
    }
}
