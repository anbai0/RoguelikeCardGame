using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{

    PlayerDataManager playerDataManager;
    CardDataManager cardData;       // �J�[�h�̃f�[�^������

    [SerializeField]
    List<int> CardRarity1List;
    [SerializeField]
    List<int> CardRarity2List;
    [SerializeField]
    List<int> CardRarity3List;

    List<int> ShopCards;
    List<int> DeckAndShopCards; //���݂̏����J�[�h�{�V���b�v�ɒǉ����ꂽ�J�[�h���i�[����




    int MaxNumCards = 20;
    
    void Start()
    {
        for (int i = 1; i <= MaxNumCards; i++)
        {
            cardData = new CardDataManager(i);
            //�e�J�[�h�̃��A���e�B�ɕ�����
            if (cardData._cardRarity == 1)
            {
                CardRarity1List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 2)
            {
                CardRarity2List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 3)
            {
                CardRarity3List.Add(cardData._cardID);
            }
        }

        CardLottery(1);
    }

    /// <summary>
    /// �J�[�h�̒��I���\�b�h
    /// </summary>
    /// <param name="rarity">���I���������A���e�B</param>
    /// <returns>�w�肵�����A���e�B�̃J�[�hID (int)</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList = null;

        //�����Ŏw�肳�ꂽ���A���e�B��List��SelectedRarityList�֑��
        switch (rarity)
        {
            case 1:
                SelectedRarityList = CardRarity1List;
                break;
            case 2:
                SelectedRarityList = CardRarity2List;
                break;
            case 3:
                SelectedRarityList = CardRarity3List;
                break;
            default:
                Debug.Log("�w�肳�ꂽ���A���e�B������܂���B");
                return -1;
        }

        //�����J�[�h�ƃV���b�v�ɏo�Ă���J�[�h��DeckAndShopCards�֒ǉ�
        DeckAndShopCards.AddRange(playerDataManager._deckList);
        DeckAndShopCards.AddRange(ShopCards);

        int cardLottery;

        //���I����
        while (true)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);   // �w�肳�ꂽ���A���e�B��List�̂��烉���_���ɗv�f��I��
            bool hasCards = false;

            for (int i = 0; i < DeckAndShopCards.Count; i++)  // �����J�[�h�ƃV���b�v�ɏo�Ă���J�[�h�̖�������
            {
                if (SelectedRarityList[cardLottery] == DeckAndShopCards[i])  // ���I���ꂽ�J�[�h���������Ă�����Ē��I
                {
                    hasCards = true;
                    break;
                }
            }
            
            if (!hasCards) // ���I���ꂽ�J�[�h���������Ă��Ȃ������璊�I�I��
                break;
        }

        DeckAndShopCards = null;
        return cardLottery;
    }


    public (int,int,int) ShopLottery()     // ���A�x2���P���A���A�x1���Q���A�񕜂��P���i�m��j
    {
        
        int cardLottery1 = CardLottery(2);
        ShopCards.Add(cardLottery1);     // �V���b�v�ɒǉ�����J�[�h��List�Ɋi�[����

        int cardLottery2 = CardLottery(1);
        ShopCards.Add(cardLottery2);

        int cardLottery3 = CardLottery(1);
        ShopCards.Add(cardLottery3);
        
        return (cardLottery1, cardLottery2, cardLottery3);


    }

    void zako()     // ���A�x1���Q���A���A�x2���P�� 
    {
        
    }

    void kyoutakara()   // ���A�x1���P���A���A�x2���Q��
    {

    }

    void boss()     // ���A�x2���Q���A���A�x3���P��
    {

    }


}
