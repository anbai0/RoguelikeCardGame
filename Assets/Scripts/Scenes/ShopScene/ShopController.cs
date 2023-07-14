using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerData;
    CardController cardController;

    [SerializeField] Lottery lottery;
    [SerializeField] UIManagerShopScene uiManager;

    // �Q�Ƃ���UI
    [SerializeField] GameObject shoppingUI;
    [SerializeField] GameObject rest;

    const int healCardID = 3;   // �񕜃J�[�h��ID
    const int restPrice = 70;   // �x�e�̒l�i

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] List<GameObject> cardPlace;     // Card�̐����ʒu

    GameObject cardObject;  // ���������J�[�hID���i�[����

    /// <summary>
    /// �V���b�v�ɏo�Ă���J�[�h��ID���i�[���܂�
    /// </summary>
    [SerializeField] List<int> shopCardsID = null;

    /// <summary>
    /// �J�[�h�̒l�i��\�����邽�߂ɕK�v�ȃI�u�W�F�N�g���i�[���܂�
    /// </summary>
    [SerializeField] List<GameObject> shopCards = null;


    Vector3 scaleReset = Vector3.one * 0.37f;     // �J�[�h�̃f�t�H���g�̑傫��
 
    [SerializeField]
    int tmpID = 0;      // �f�o�b�O�p

    private void Start()
    {
        playerData = GameManager.Instance.playerData;

    }

    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();
            shopCardsID.Add(healCardID);       // �񕜃J�[�h��ǉ�

            // �V���b�v�ɕ��ԃJ�[�h�\��
            for (int i = 0; i < shopCardsID.Count; i++)
            {
                CardsShow(i);
            }

            uiManager.UIEventReload();          // UI(�J�[�h)��\��
            Lottery.isInitialize = false;
        }

        PriceCheck();
        



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
        Debug.Log("�J�[�h1:" + shopCardsID[0] + "\n�J�[�h2:" + shopCardsID[1] + "\n�J�[�h3:" + shopCardsID[2]);

    }

    /// <summary>
    /// �J�[�h�̕\�����s���܂��B
    /// </summary>
    /// <param name="cardID">�\���������J�[�h��ID</param>
    void CardsShow(int cardID)
    {
        cardObject = Instantiate(cardPrefab, cardPlace[cardID].transform.position, cardPlace[cardID].transform.rotation);       // �J�[�h��Prefab�𐶐�
        cardObject.transform.SetParent(shoppingUI.transform);                                                                   // Canvas�̎q�ɂ���
        cardController = cardObject.GetComponent<CardController>();                                                             // ��������Prefab��CardController���擾
        cardController.Init(shopCardsID[cardID]);                                                                               // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������
        cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // �l�D��\��
        shopCards.Add(cardObject);
    }

    /// <summary>
    /// �J�[�h�𔃂��邩�ǂ����𔻒肵�A
    /// �ς��Ȃ������ꍇ�l�i��Ԃ��\�����܂�
    /// </summary>
    void PriceCheck()
    {
        //�������`�F�b�N
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (playerData._money >= card.cardDataManager._cardPrice)     // �������������Ȃ�
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
    }

    /// <summary>
    /// �x�e�ł��邩���肵�܂�
    /// </summary>
    /// <returns>�x�e�ł���ł���ꍇtrue</returns>
    public bool CheckRest()
    {
        if (playerData._playerHP == playerData._playerCurrentHP)     // ���݂�HP��MAX�̏ꍇ
        {
            rest.GetComponent<Image>().color = Color.gray;
            return false;
        }
        if (playerData._money < restPrice)          // ����������Ȃ��ꍇ
        {
            rest.GetComponent<Image>().color = Color.gray;
            ///  �v���C�X�e�L�X�g��Ԃ��\��
            return false;
        }
        return true;

    }

    public void Rest()
    {
        if (playerData._playerHP == playerData._playerCurrentHP)
        {
            
        }
    }


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
                return true;
            }
        }
        return false;
    }
    
    public void BuyCards(GameObject selectCard)
    {
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectCard == shopCards[i])         // �N���b�N�����J�[�h�ƃV���b�v�ɕ���ł�J�[�h����v������
            {
                CardController card = shopCards[i].GetComponent<CardController>();

                if (playerData._money >= card.cardDataManager._cardPrice)           // �������������Ȃ�
                {
                    if (shopCardsID[i] != shopCardsID[healCardID])                  // �I�񂾃J�[�h���񕜃J�[�h�ł͂Ȃ������ꍇ
                    {
                        playerData._money -= card.cardDataManager._cardPrice;       // ����������l�i���̂�����������
                        playerData._deckList.Add(shopCardsID[i]);                   // �f�b�L�ɉ�����

                        selectCard.GetComponent<Image>().color = Color.gray;        // �������J�[�h���Â�����
                        selectCard.transform.localScale = scaleReset;               // �X�P�[����߂�

                        selectCard.SetActive(false);

                    } else if (!HasHealPotion())   // �I�񂾃J�[�h���񕜃J�[�h�ŁA�񕜃J�[�h���������Ă��Ȃ��ꍇ
                    {
                        playerData._money -= card.cardDataManager._cardPrice;
                        playerData._deckList.Add(shopCardsID[i]);

                        selectCard.GetComponent<Image>().color = Color.gray;
                        selectCard.transform.localScale = scaleReset;
                    }
                }
                    
            }
        }


    }


}
