using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerData;
    CardController cardController;
    RelicController relicController;

    [SerializeField] Lottery lottery;
    [SerializeField] UIManagerShopScene uiManager;

    const int healCardID = 3;                       // �񕜃J�[�h��ID
    const int deckLimitIncRelicID = 1;              // �f�b�L�̏����1�����₷�����b�N��ID
    const int restPrice = 70;                       // �x�e�̒l�i
    Vector3 scaleReset = Vector3.one * 0.37f;       // �J�[�h�̃f�t�H���g�̑傫��

    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject shoppingUI;
    [SerializeField] GameObject restButton;
    [SerializeField] Text restText;
    [SerializeField] Text restPriceText;

    [Header("�V���b�v�ɕ��ԃA�C�e����Prefab")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject relicPrefab;

    [Header("�ePrefab�̐����ʒu")]
    [SerializeField] List<GameObject> cardPlace;     // Card�̐����ʒu
    [SerializeField] List<GameObject> relicPlace;    // Relic�̐����ʒu


    [Header("�������牺�̓f�o�b�O�p�ɕ\�������Ă܂�")]
    /// <summary>
    /// �V���b�v�ɏo�Ă���J�[�h��ID���i�[���܂�
    /// </summary>
    [SerializeField] List<int> shopCardsID = null;

    /// <summary>
    /// �V���b�v�ɏo�Ă��郌���b�N��ID���i�[���܂�
    /// </summary>
    [SerializeField] List<int> shopRelicsID = null;

    /// <summary>
    /// �J�[�h�̒l�i��\�����邽�߂ɕK�v�ȃI�u�W�F�N�g���i�[���܂�
    /// </summary>
    [SerializeField] List<GameObject> shopCards = null;

    /// <summary>
    /// �����b�N�̒l�i��\�����邽�߂ɕK�v�ȃI�u�W�F�N�g���i�[���܂�
    /// </summary>
    [SerializeField] List<GameObject> shopRelics = null;


    [SerializeField]    int tmpID = 0;      // �f�o�b�O�p

    private void Start()
    {
        playerData = GameManager.Instance.playerData;

    }

    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();
            shopCardsID.Add(healCardID);                        // �񕜃J�[�h��ǉ�
            shopRelicsID.Insert(0, deckLimitIncRelicID);        // �f�b�L�̏����1�����₷�����b�N��ǉ�
            Debug.Log("�����b�N1:   " + shopRelicsID[0] + "\n�����b�N2:   " + shopRelicsID[1] + "\n�����b�N3:  " + shopRelicsID[2]);

            // �V���b�v�ɕ��ԃA�C�e����\��
            ShowItem();
            
            uiManager.UIEventReload();          // UIEvent�X�V      
            Lottery.isInitialize = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShopLottery();
            shopCardsID.Add(healCardID);                        // �񕜃J�[�h��ǉ�
            shopRelicsID.Insert(0, deckLimitIncRelicID);        // �f�b�L�̏����1�����₷�����b�N��ǉ�
            Debug.Log("�����b�N1:   " + shopRelicsID[0] + "\n�����b�N2:   " + shopRelicsID[1] + "\n�����b�N3:  " + shopRelicsID[2]);

            // �V���b�v�ɕ��ԃA�C�e����\��
            ShowItem();

            uiManager.UIEventReload();          // UIEvent�X�V      
            Lottery.isInitialize = false;
        }

        //if (Input.GetKeyDown(KeyCode.Space))        // Space���������ƂɎ���CardID�̃J�[�h���\�������
        //{
        //    if (tmpID >= 20)
        //        tmpID = 0;

        //    tmpID++;

        //    cardController.Init(tmpID);
        //    //DebugLottery();
        //}

    }

    /// <summary>
    /// Lottery�X�N���v�g���璊�I�����J�[�hID���󂯎�郁�\�b�h
    /// </summary>
    void ShopLottery()
    {
        lottery.fromShopController = true;
        //(Card1, Card2, Card3) = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        shopCardsID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        shopRelicsID = lottery.SelectRelicByRarity(new List<int> { 2, 1 });
        Debug.Log("�J�[�h1:    " + shopCardsID[0] + "\n�J�[�h2:   " + shopCardsID[1] + "\n�J�[�h3:   " + shopCardsID[2]);

    }

    /// <summary>
    /// �V���b�v�ɕ��ԃA�C�e���̕\�����s���܂��B
    /// </summary>
    void ShowItem()
    {
        // �J�[�h�\��
        for (int cardID = 0; cardID < shopCardsID.Count; cardID++) 
        {
            GameObject cardObject = Instantiate(cardPrefab, cardPlace[cardID].transform.position, cardPlace[cardID].transform.rotation);       // �J�[�h��Prefab�𐶐�
            cardObject.transform.SetParent(shoppingUI.transform);                                                                   // shoppingUI�̎q�ɂ���
            cardController = cardObject.GetComponent<CardController>();                                                             // ��������Prefab��CardController���擾
            cardController.Init(shopCardsID[cardID]);                                                                               // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������
            cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // �l�D��\��
            shopCards.Add(cardObject);
        }

        // �����b�N�\��
        for (int relicID = 0; relicID < shopRelicsID.Count; relicID++)
        {
            GameObject relicObject = Instantiate(relicPrefab, relicPlace[relicID].transform.position, relicPlace[relicID].transform.rotation);     // �����b�N��Prefab�𐶐�
            relicObject.transform.SetParent(shoppingUI.transform);                                                                      // shoppingUI�̎q�ɂ���
            relicController = relicObject.GetComponent<RelicController>();                                                              // ��������Prefab��RelicController���擾
            relicController.Init(shopRelicsID[relicID]);                                                                                // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������
            relicObject.transform.Find("RelicPriceBG").gameObject.SetActive(true);                                                      // �l�D��\��
            shopRelics.Add(relicObject);
        }
    }

    /// <summary>
    /// �A�C�e���𔃂��邩�ǂ����𔻒肵�A
    /// �ς��Ȃ������ꍇ�l�i��Ԃ��\�����܂�
    /// </summary>
    public void PriceTextCheck()
    {
        // �J�[�h�̒l�i�`�F�b�N
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (playerData._playerMoney >= card.cardDataManager._cardPrice)     // �������������Ȃ�
            {
                Text textComponent = shopCards[i].transform.GetChild(3).GetChild(0).GetComponent<Text>();       // Price�\���e�L�X�g���擾
                textComponent.color = Color.white;                                                              // ���ŕ\��
            }
            else
            {
                Text textComponent = shopCards[i].transform.GetChild(3).GetChild(0).GetComponent<Text>();       // Price�\���e�L�X�g���擾
                textComponent.color = Color.red;                                                                // �Ԃŕ\��
            }

        }

        // �����b�N�̒l�i�`�F�b�N
        for (int i = 0; i < shopRelics.Count; i++)
        {
            RelicController relic = shopRelics[i].GetComponent<RelicController>();
            if (playerData._playerMoney >= relic.relicDataManager._relicPrice)
            {
                TextMeshProUGUI textComponent = shopRelics[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.color = Color.white;
            }
            else
            {
                TextMeshProUGUI textComponent = shopRelics[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.color = Color.red;
            }
        }
    }

    int healCardNum = 3;
    /// <summary>
    /// �����̗��������Ă��邩���肵�܂�
    /// </summary>
    /// <returns>�����Ă���ꍇtrue��Ԃ��܂�</returns>
    public bool HasHealPotion()
    {
        foreach(int cardsID in playerData._deckList)
        {
            if(cardsID == shopCardsID[healCardID])      // �񕜃J�[�h�������Ă���ꍇ
            {
                // �񕜃J�[�h���O���[�A�E�g�ɂ���
                shopCards[healCardNum].GetComponent<Image>().color = Color.gray;        // �������܂肢���������ł͂Ȃ��̂ŏC��������
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// �A�C�e���𔃂������ł�
    /// </summary>
    /// <param name="selectItem">�N���b�N����UIObject</param>
    /// <param name="itemType">Card�܂���Relic���w��</param>
    public void BuyItem(GameObject selectItem, string itemType)
    {
        if (itemType == "Card")
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectItem == shopCards[i])         // �N���b�N�����J�[�h�ƃV���b�v�ɕ���ł�J�[�h����v������
            {
                CardController card = shopCards[i].GetComponent<CardController>();

                if (playerData._playerMoney >= card.cardDataManager._cardPrice)           // �������������Ȃ�
                {
                    if (shopCardsID[i] != shopCardsID[healCardID])                        // �I�񂾃J�[�h���񕜃J�[�h�ł͂Ȃ������ꍇ
                    {
                        playerData._playerMoney -= card.cardDataManager._cardPrice;       // ����������l�i���̂�����������
                        playerData._deckList.Add(shopCardsID[i]);                         // �f�b�L�ɉ�����

                        selectItem.GetComponent<Image>().color = Color.gray;              // �������J�[�h���O���[�A�E�g����
                        selectItem.transform.localScale = scaleReset;                     // �X�P�[����߂�

                        selectItem.SetActive(false);

                    } else if (!HasHealPotion())   // �I�񂾃J�[�h���񕜃J�[�h�ŁA�񕜃J�[�h���������Ă��Ȃ��ꍇ
                    {
                        playerData._playerMoney -= card.cardDataManager._cardPrice;
                        playerData._deckList.Add(shopCardsID[i]);

                        selectItem.GetComponent<Image>().color = Color.gray;
                        selectItem.transform.localScale = scaleReset;
                    }
                }               
            }
        }

        if (itemType == "Relic")
        for (int i = 0; i < shopRelics.Count; i++)
        {
            if (selectItem == shopRelics[i])         // �N���b�N���������b�N�ƃV���b�v�ɕ���ł郌���b�N����v������
            {
                RelicController relic = shopRelics[i].GetComponent<RelicController>();

                if (playerData._playerMoney >= relic.relicDataManager._relicPrice)          // �������������Ȃ�
                {
                    playerData._playerMoney -= relic.relicDataManager._relicPrice;          // ����������l�i���̂�����������
                    playerData._relicList.Add(shopRelicsID[i]);                             // �����b�N���X�g�ɉ�����

                    selectItem.transform.localScale = scaleReset;                           // �X�P�[����߂�

                    selectItem.SetActive(false);
                }
            }
        }
    }



    /// <summary>
    /// �x�e�ł��邩���肵�܂�
    /// </summary>
    /// <returns>�x�e�ł���ł���ꍇtrue</returns>
    public bool CheckRest()
    {
        if (playerData._playerMoney < restPrice)          // ����������Ȃ��ꍇ
        {
            restPriceText.color = Color.red;              // �l�i��Ԃ��\��
        }

        // ����HP��Max�̏ꍇ�܂��͂���������Ȃ��ꍇ
        if (playerData._playerHP == playerData._playerCurrentHP || playerData._playerMoney < restPrice)
        {
            restButton.GetComponent<Image>().color = Color.gray;  // �x�e�{�^�����O���[�A�E�g
            return false;
        }

        return true;

    }

    /// <summary>
    /// �x�e��ʂŃe�L�X�g��\�������郁�\�b�h�ł�
    /// </summary>
    public void ChengeRestText()
    {
        restText.text = $"70G�������\n�̗͂�{playerData._playerHP - playerData._playerCurrentHP}�񕜂��܂����H";
    }

    /// <summary>
    /// �x�e�̏���
    /// �x�e�ɕK�v�ȋ��z���x�����A
    /// �̗͂�S�񕜂����܂��B
    /// </summary>
    public void Rest()
    {
        playerData._playerMoney -= restPrice;
        playerData._playerCurrentHP = playerData._playerHP;
    }
}
