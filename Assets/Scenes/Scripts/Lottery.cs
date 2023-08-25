using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�⃌���b�N�̒��I���s���X�N���v�g�ł��B
/// </summary>
public class Lottery : MonoBehaviour
{
    private GameManager gm;

    private const int MaxNumCards = 20;       // �S�J�[�h�̖���
    private const int MaxNumRelics = 11;      // �S�����b�N�̐�

    [Header("�������牺�̓f�o�b�O�p�ɕ\�������Ă܂�")]
    //�e���A���e�B�̃��X�g
    [SerializeField] List<int> cardRarity1List;
    [SerializeField] List<int> cardRarity2List;
    [SerializeField] List<int> cardRarity3List;
    [SerializeField] List<int> relicRarity1List;
    [SerializeField] List<int> relicRarity2List;
    [SerializeField] public List<int> shopCards = new List<int>();       // �V���b�v�ɒǉ����ꂽ�J�[�h�B�f�[�^�����Z�b�g����Ƃ��ɃN���A���܂�
    [SerializeField] List<int> currentCardsLotteryID = new List<int>(); // ���ݒ��I���Ă���J�[�h��ID
    [SerializeField] List<int> currentRelicsLotteryID = new List<int>(); // ���ݒ��I���Ă��郌���b�N��ID

    public static Lottery Instance;
    void Start()
    {
        // �V���O���g���C���X�^���X���Z�b�g�A�b�v
        if (Instance == null)
        {
            Instance = this;
        }

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

        // �����J�[�h�ƃV���b�v�ɏo�Ă���J�[�h�ƍ��񒊑I���ꂽ�J�[�h���i�[
        List<int> currentLotteryCards = new List<int>(gm.playerData._deckList);
        currentLotteryCards.AddRange(shopCards);
        currentLotteryCards.AddRange(currentCardsLotteryID);

        // �����ς݃J�[�h������ΑΉ����関�����̃J�[�h�����O
        for (int num = 0; num < currentLotteryCards.Count; num++)
        {
            if (currentLotteryCards[num] >= 101)
            {
                currentLotteryCards[num] -= 100;
            }
        }

        int cardLottery = -1;

        //���I����
        int maxAttempts = 100;  // �ő厎�s�񐔂�ݒ�
        int attempts = 0;

        while (cardLottery == -1 || (currentLotteryCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            cardLottery = -1;
            Debug.Log("�J�[�h�𒊑I�ł��܂���ł����B");
        }

        currentLotteryCards.Clear();    // ���̒��I���ɏ����J�[�h���قȂ��Ă��邱�Ƃ��l�����AList���N���A
        return cardLottery;
    }


    /// <summary>
    /// CardLottery���\�b�h���g����(selectRarity�̗v�f)�񕪒��I���s���܂��B
    /// �w�肵�����A���e�B���Ȃ��������ɑ���̃��A���e�B�ōĒ��I�����܂��B
    /// shop�ŌĂԏꍇ�͑�������true���w�肵�Ă��������B
    /// </summary>
    /// <param name="selectRarity">���I���������A���e�B1~3��List�Ŏw��</param>
    /// <returns>���I�����J�[�hID</returns>
    public List<int> SelectCardByRarity(List<int> selectRarity, bool fromShopController = false)
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

            currentCardsLotteryID.Add(selectedCard);    // ����̒��I�ŏo���J�[�hID���i�[
            lotteryResult.Add(selectedCard);            // �߂�l�ł��钊�I�̌��ʂ��i�[

            // ShopController����Ă΂ꂽ��
            if (fromShopController)
            {
                shopCards.Add(selectedCard);
            }
        }

        currentCardsLotteryID.Clear();      // ���I���I������̂�List���N���A
        fromShopController = false;

        return lotteryResult;
    }



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

        // ���I����
        int maxAttempts = 100;  // �ő厎�s�񐔂�ݒ�
        int attempts = 0;

        while (relicLottery == -1 || (currentRelicsLotteryID.Contains(SelectedRarityList[relicLottery])) && attempts < maxAttempts)
        {
            relicLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            relicLottery = -1;
            Debug.Log("�����b�N�𒊑I�ł��܂���ł����B");
        }

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

            currentRelicsLotteryID.Add(selectedRelic);
            lotteryResult.Add(selectedRelic);
        }

        currentRelicsLotteryID.Clear();      // ���I���I������̂�List���N���A
        return lotteryResult;
    }

}
