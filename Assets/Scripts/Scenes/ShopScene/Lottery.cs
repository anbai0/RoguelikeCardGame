using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{
    CardDataManager cardData;       // �J�[�h�̃f�[�^������

    //�e���A���e�B�̃��X�g
    [SerializeField]
    List<int> CardRarity1List;
    [SerializeField]
    List<int> CardRarity2List;
    [SerializeField]
    List<int> CardRarity3List;

    public List<int> ShopCards = new List<int>();   //�V���b�v�ɒǉ����ꂽ�J�[�h
    List<int> DeckAndShopCards;                     //���݂̏����J�[�h�{�V���b�v�ɒǉ����ꂽ�J�[�h���i�[����

    public static bool isInitialize = false;

    int MaxNumCards = 20;       //�J�[�h����

    public bool fromShopController = false;


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

        isInitialize = true;
    }

    /// <summary>
    /// �J�[�h�̒��I���\�b�h
    /// </summary>
    /// <param name="rarity">���I���������A���e�B</param>
    /// <returns>�w�肵�����A���e�B��List�̗v�f (int)</returns>
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

        int cardLottery = -1;

        //���I����
        int maxAttempts = 100;  // �ő厎�s�񐔂�ݒ�
        int attempts = 0;

        while (cardLottery == -1 || (DeckAndShopCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }


        if (attempts >= maxAttempts)
        {
            cardLottery = -1;
            Debug.Log("�J�[�h�𒊑I�ł��܂���ł����B");
        }


        DeckAndShopCards = null;
        return cardLottery;
    }



    public int[] SelectCardByRarity(int[] selectRarity)
    {

        int[] lotteryResult = new int[selectRarity.Length];
        //int result1, result2, result3;

        
        for (int i = 0; i < selectRarity.Length; i++)
        {
            //�J�[�h�̒��I
            lotteryResult[i] = CardLottery(selectRarity[i]);

            //�w�肵�����A���e�B�ɃJ�[�h���Ȃ��������̍Ē��I
            if (lotteryResult[i] == -1)
            {
                Debug.Log("a");
                switch (selectRarity[i])
                {
                    case 1:
                        lotteryResult[i] = CardLottery(2);
                        lotteryResult[i] = CardRarity2List[lotteryResult[i]];   //���I�����e���A���e�B��List�̗v�f��CardID�ɕϊ�
                        break;
                    case 2:
                        lotteryResult[i] = CardLottery(1);
                        lotteryResult[i] = CardRarity1List[lotteryResult[i]];
                        break;
                    case 3:
                        lotteryResult[i] = CardLottery(2);
                        lotteryResult[i] = CardRarity2List[lotteryResult[i]];
                        break;
                    default:
                        break;
                }
            }else
            {
                //���I�����e���A���e�B��List�̗v�f��CardID�ɕϊ�
                switch (selectRarity[i])
                {
                    case 1:
                        lotteryResult[i] = CardRarity1List[lotteryResult[i]];
                        break;
                    case 2:
                        lotteryResult[i] = CardRarity2List[lotteryResult[i]];
                        break;
                    case 3:
                        lotteryResult[i] = CardRarity3List[lotteryResult[i]];
                        break;
                    default:
                        break;
                }
            }

            // ShopController����Ă΂ꂽ��
            if (fromShopController)
            {
                ShopCards.Add(lotteryResult[i]);
            }
        }

        fromShopController = false;

        //result1 = lotteryResult[0];
        //result2 = lotteryResult[1];
        //result3 = lotteryResult[2];


        return (lotteryResult);
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
