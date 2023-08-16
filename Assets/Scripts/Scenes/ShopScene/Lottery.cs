using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�⃌���b�N�̒��I���s���X�N���v�g�ł��B
/// Sart�ɂ��鏈�����x���̂�isInitialize�ϐ����g���Ă��������B
/// </summary>
public class Lottery : MonoBehaviour
{
    public GameManager gm;

    public static bool isInitialize = false;        // Start�ɂ��鏈�����x���̂ŏ������I�������true��
    public bool fromShopController = false;         // ShopController����Ă΂ꂽ�ꍇtrue�ɂ��܂�

    private const int MaxNumCards = 20;       // �S�J�[�h�̖���
    private const int MaxNumRelics = 11;      // �S�����b�N�̐�

    [Header("�������牺�̓f�o�b�O�p�ɕ\�������Ă܂�")]
    //�e���A���e�B�̃��X�g
    [SerializeField] List<int> cardRarity1List;
    [SerializeField] List<int> cardRarity2List;
    [SerializeField] List<int> cardRarity3List;
    [SerializeField] List<int> relicRarity1List;
    [SerializeField] List<int> relicRarity2List;
    [SerializeField] List<int> shopCards = new List<int>();       // �V���b�v�ɒǉ����ꂽ�J�[�h
    //[SerializeField] List<int> shopRelics = new List<int>();      // �V���b�v�ɒǉ����ꂽ�����b�N

    void Start()
    {
        // GameManager�擾(�ϐ����ȗ�)
        gm = GameManager.Instance;

        for (int i = 1; i <= MaxNumCards; i++)
        {
            CardDataManager cardData = gm.cardDataList[i];

            // �e�J�[�h�̃��A���e�B�ɕ�����
            if (cardData._cardRarity == 1)
            {
                cardRarity1List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 2)
            {
                cardRarity2List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 3)
            {
                cardRarity3List.Add(cardData._cardID);
            }
        }

        for (int i = 1; i <= MaxNumRelics; i++)
        {
            RelicDataManager relicData = gm.relicDataList[i];

            // �e�����b�N�̃��A���e�B�ɕ�����
            if (relicData._relicRarity == 1)
            {
                relicRarity1List.Add(relicData._relicID);
            }
            if (relicData._relicRarity == 2)
            {
                relicRarity2List.Add(relicData._relicID);
            }
        }
        
        isInitialize = true;
    }

    /// <summary>
    /// �J�[�h�̒��I���\�b�h
    /// </summary>
    /// <param name="rarity">���I���������A���e�B</param>
    /// <returns>�w�肵�����A���e�B��List�̗v�f</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList;

        // �����Ŏw�肳�ꂽ���A���e�B��List��SelectedRarityList�֑��
        switch (rarity)
        {
            case 1:
                SelectedRarityList = cardRarity1List;
                break;
            case 2:
                SelectedRarityList = cardRarity2List;
                break;
            case 3:
                SelectedRarityList = cardRarity3List;
                break;
            default:
                Debug.Log("�w�肳�ꂽ���A���e�B�̃J�[�h������܂���B");
                return -1;
        }

        // �����J�[�h�ƃV���b�v�ɏo�Ă���J�[�h��DeckAndShopCards�֒ǉ�
        List<int> deckAndShopCards = new List<int>(gm.playerData._deckList);

        // �����ς݃J�[�h������ΑΉ����関�����̃J�[�h�����O
        for (int num = 0; num < deckAndShopCards.Count; num++)
        {
            if (deckAndShopCards[num] >= 101)
            {
                deckAndShopCards[num] -= 100;
            }
        }

        deckAndShopCards.AddRange(shopCards);

        int cardLottery = -1;

        //���I����
        int maxAttempts = 100;  // �ő厎�s�񐔂�ݒ�
        int attempts = 0;

        while (cardLottery == -1 || (deckAndShopCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            cardLottery = -1;
            Debug.Log("�J�[�h�𒊑I�ł��܂���ł����B");
        }

        deckAndShopCards = null;
        return cardLottery;
    }


    /// <summary>
    /// CardLottery���\�b�h���g����(selectRarity�̗v�f)�񕪒��I���s���܂�
    /// �w�肵�����A���e�B���Ȃ��������ɑ���̃��A���e�B�ōĒ��I�����܂�
    /// </summary>
    /// <param name="selectRarity">���I���������A���e�B1~3��List�Ŏw��</param>
    /// <returns>���I�����J�[�hID</returns>
    public List<int> SelectCardByRarity(List<int> selectRarity)
    {
        List<int> lotteryResult = new List<int>();

        for (int i = 0; i < selectRarity.Count; i++)
        {
            // �J�[�h�̒��I
            int selectedCard = CardLottery(selectRarity[i]);

            // �w�肵�����A���e�B�ɃJ�[�h���Ȃ��������̍Ē��I
            if (selectedCard == -1)
            {
                switch (selectRarity[i])
                {
                    case 1:
                        selectedCard = CardLottery(2);
                        selectedCard = cardRarity2List[selectedCard];   // ���I�����e���A���e�B��List�̗v�f��CardID�ɕϊ�
                        break;
                    case 2:
                        selectedCard = CardLottery(1);
                        selectedCard = cardRarity1List[selectedCard];
                        break;
                    case 3:
                        selectedCard = CardLottery(2);
                        selectedCard = cardRarity2List[selectedCard];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // ���I�����e���A���e�B��List�̗v�f��CardID�ɕϊ�
                switch (selectRarity[i])
                {
                    case 1:
                        selectedCard = cardRarity1List[selectedCard];
                        break;
                    case 2:
                        selectedCard = cardRarity2List[selectedCard];
                        break;
                    case 3:
                        selectedCard = cardRarity3List[selectedCard];
                        break;
                    default:
                        break;
                }
            }

            lotteryResult.Add(selectedCard);

            // ShopController����Ă΂ꂽ��
            if (fromShopController)
            {
                shopCards.Add(selectedCard);
            }
        }

        fromShopController = false;

        return lotteryResult;
    }


    #region �d�����O��������̃����b�N���I
    /// <summary>
    /// �����b�N�̒��I���\�b�h
    /// �J�[�h���I���\�b�h�Ɠ����悤�ɏd�����Ȃ��悤�ɏ����������܂�����
    /// �g��Ȃ��Ȃ����̂ŃR�����g�A�E�g���܂��B
    /// �����b�N�̏����ōs���l�����肵���ꍇ�ɔ����Ă��̏����͎c���Ă����܂��B
    /// </summary>
    /// <param name="rarity">���I���������A���e�B</param>
    /// <returns>�w�肵�����A���e�B��List�̗v�f</returns>
    //int RelicLottery(int rarity)
    //{
    //    List<int> SelectedRarityList;

    //    // �����Ŏw�肳�ꂽ���A���e�B��List��SelectedRarityList�֑��
    //    switch (rarity)
    //    {
    //        case 1:
    //            SelectedRarityList = relicRarity1List;
    //            break;
    //        case 2:
    //            SelectedRarityList = relicRarity2List;
    //            break;
    //        default:
    //            Debug.Log("�w�肳�ꂽ���A���e�B�̃����b�N������܂���B");
    //            return -1;
    //    }

    //    // ���������b�N�ƃV���b�v�ɏo�Ă���J�[�h��myRelicAndShopRelics�֒ǉ�
    //    List<int> myRelicAndShopRelics = new List<int>(playerData._relicList);
    //    myRelicAndShopRelics.AddRange(shopRelics);

    //    int relicLottery = -1;

    //    // ���I����
    //    int maxAttempts = 100;  // �ő厎�s�񐔂�ݒ�
    //    int attempts = 0;

    //    while (relicLottery == -1 || (myRelicAndShopRelics.Contains(SelectedRarityList[relicLottery])) && attempts < maxAttempts)
    //    {
    //        relicLottery = Random.Range(0, SelectedRarityList.Count);
    //        attempts++;
    //    }

    //    if (attempts >= maxAttempts)
    //    {
    //        relicLottery = -1;
    //        Debug.Log("�����b�N�𒊑I�ł��܂���ł����B");
    //    }

    //    myRelicAndShopRelics = null;
    //    return relicLottery;
    //}




    /// <summary>
    /// RelicLottery���\�b�h���g����(selectRarity�̗v�f)�񕪒��I���s���܂�
    /// �w�肵�����A���e�B���Ȃ��������ɑ���̃��A���e�B�ōĒ��I�����܂�
    /// </summary>
    /// <param name="selectRarity">���I���������A���e�B1~2��List�Ŏw��</param>
    /// <returns>���I���������b�NID</returns>
    //public List<int> SelectRelicByRarity(List<int> selectRarity)
    //{
    //    List<int> lotteryResult = new List<int>();

    //    for (int i = 0; i < selectRarity.Count; i++)
    //    {
    //        // �����b�N�̒��I
    //        int selectedRelic = RelicLottery(selectRarity[i]);

    //        // �w�肵�����A���e�B�Ƀ����b�N���Ȃ��������̍Ē��I
    //        if (selectedRelic == -1)
    //        {
    //            switch (selectRarity[i])
    //            {
    //                case 1:
    //                    selectedRelic = RelicLottery(2);
    //                    selectedRelic = relicRarity2List[selectedRelic];   // ���I�����e���A���e�B��List�̗v�f��RelicID�ɕϊ�
    //                    break;
    //                case 2:
    //                    selectedRelic = RelicLottery(1);
    //                    selectedRelic = relicRarity1List[selectedRelic];
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //        else
    //        {
    //            // ���I�����e���A���e�B��List�̗v�f��RelicID�ɕϊ�
    //            switch (selectRarity[i])
    //            {
    //                case 1:
    //                    selectedRelic = relicRarity1List[selectedRelic];
    //                    break;
    //                case 2:
    //                    selectedRelic = relicRarity2List[selectedRelic];
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }

    //        lotteryResult.Add(selectedRelic);

    //        // ShopController����Ă΂ꂽ��
    //        if (fromShopController)
    //        {
    //            shopRelics.Add(selectedRelic);
    //        }
    //    }

    //    fromShopController = false;

    //    return lotteryResult;
    //}

    #endregion



    #region �d�����O���������̃����b�N���I
    /// <summary>
    /// �����b�N�̒��I���\�b�h
    /// </summary>
    /// <param name="rarity">���I���������A���e�B</param>
    /// <returns>�w�肵�����A���e�B��List�̗v�f</returns>
    int RelicLottery(int rarity)
    {
        List<int> SelectedRarityList;

        // �����Ŏw�肳�ꂽ���A���e�B��List��SelectedRarityList�֑��
        switch (rarity)
        {
            case 1:
                SelectedRarityList = relicRarity1List;
                break;
            case 2:
                SelectedRarityList = relicRarity2List;
                break;
            default:
                Debug.Log("�w�肳�ꂽ���A���e�B�̃����b�N������܂���B");
                return -1;
        }

        int relicLottery = -1;

        relicLottery = Random.Range(0, SelectedRarityList.Count);

        return relicLottery;
    }



    /// <summary>
    /// RelicLottery���\�b�h���g����(selectRarity�̗v�f)�񕪒��I���s���܂�
    /// </summary>
    /// <param name="selectRarity">���I���������A���e�B1~2��List�Ŏw��</param>
    /// <returns>���I���������b�NID</returns>
    public List<int> SelectRelicByRarity(List<int> selectRarity)
    {
        List<int> lotteryResult = new List<int>();

        for (int i = 0; i < selectRarity.Count; i++)
        {
            // �����b�N�̒��I
            int selectedRelic = RelicLottery(selectRarity[i]);

            // ���I�����e���A���e�B��List�̗v�f��RelicID�ɕϊ�
            switch (selectRarity[i])
            {
                case 1:
                    selectedRelic = relicRarity1List[selectedRelic];
                    break;
                case 2:
                    selectedRelic = relicRarity2List[selectedRelic];
                    break;
                default:
                    break;
            }

            lotteryResult.Add(selectedRelic);
        }

        return lotteryResult;
    }

    #endregion



}
