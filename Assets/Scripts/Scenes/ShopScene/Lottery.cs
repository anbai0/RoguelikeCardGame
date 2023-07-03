using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public List<int> ShopCards = new List<int>();
    List<int> DeckAndShopCards; //���݂̏����J�[�h�{�V���b�v�ɒǉ����ꂽ�J�[�h���i�[����

    int MaxNumCards = 20;

    int foreachCount = 0;

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

    }

    /// <summary>
    /// �J�[�h�̒��I���\�b�h
    /// </summary>
    /// <param name="rarity">���I���������A���e�B</param>
    /// <returns>�w�肵�����A���e�B�̃J�[�hID (int)</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList;

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
        var playerData = new PlayerDataManager("Warrior");
        DeckAndShopCards = new List<int>(playerData._deckList);
        DeckAndShopCards.AddRange(ShopCards);

        //foreachCount++;
        //Debug.Log(foreachCount + "���:   " + string.Join(", ", DeckAndShopCards));

        int cardLottery = -1;

        //���I����
        int maxAttempts = 100;  // �ő厎�s�񐔂�ݒ�
        int attempts = 0;


        while ((cardLottery == -1 || DeckAndShopCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }


        if (attempts >= maxAttempts)
        {
            Debug.Log("�J�[�h�𒊑I�ł��܂���ł����B");
            cardLottery = -1;

        }


        DeckAndShopCards = null;
        return cardLottery;
    }



    public (int,int,int) ShopLottery()     // ���A�x2���P���A���A�x1���Q���A�񕜂��P���i�m��j
    {
        //���A���e�B2�̒��I
        int cardLottery1 = CardRarity2List[CardLottery(2)];
        ShopCards.Add(cardLottery1);     // �V���b�v�ɒǉ�����J�[�h��List�Ɋi�[����
        //���A���e�B2�̃J�[�h���Ȃ�������(cardLottery==-1)
        //���A���e�B1�̃J�[�h�𒊑I

        // ���A���e�B1�̒��I
        int cardLottery2 = CardRarity1List[CardLottery(1)];
        ShopCards.Add(cardLottery2);
        //���A���e�B1�̃J�[�h���Ȃ�������(cardLottery==-1)
        //���A���e�B2�̃J�[�h�𒊑I


        // ���A���e�B1�̒��I
        int cardLottery3 = CardRarity1List[CardLottery(1)];
        ShopCards.Add(cardLottery3);
        //���A���e�B1�̃J�[�h���Ȃ�������(cardLottery==-1)
        //���A���e�B2�̃J�[�h�𒊑I









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
