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


    const int heelCardID = 3;   //�񕜃J�[�h��ID
    bool isHeelPotion = false;  //�񕜃J�[�h�������Ă��邩�H

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] List<GameObject> cardPlace;     // Card�̐����ʒu

    GameObject cardObject;  // ���������J�[�hID���i�[����
    List<int> shopCardsID = null;

    // �J�[�h�̒l�i��\�����邽�߂ɕK�v�ȃI�u�W�F�N�g���i�[����
    [SerializeField] List<GameObject> shopCards = null;


    [SerializeField]
    int tmpID = 0;      //�f�o�b�O�p

    private void Start()
    {
        playerData = GameManager.Instance.playerData;

    }

    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();
            shopCardsID.Add(heelCardID);       // �񕜃J�[�h��ǉ�

            // �V���b�v�ɕ��ԃJ�[�h�\��
            for (int i = 0; i < shopCardsID.Count; i++)
            {
                CardsShow(i);
            }

            uiManager.UIEventReload();          // UI(�J�[�h)��\��
        }

        if (Input.GetKeyDown(KeyCode.Space))        // Space���������ƂɎ���CardID�̃J�[�h���\�������
        {
            if (tmpID >= 20)
                tmpID = 0;

            tmpID++;

            cardController.Init(tmpID);
            //DebugLottery();
        }

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

        Lottery.isInitialize = false;
    }

    /// <summary>
    /// �J�[�h�̕\�����s���܂��B
    /// </summary>
    /// <param name="cardID">�\���������J�[�h��ID</param>
    void CardsShow(int cardID)
    {
        cardObject = Instantiate(cardPrefab, cardPlace[cardID].transform.position, cardPlace[cardID].transform.rotation);       // �J�[�h��Prefab�𐶐�
        cardObject.transform.SetParent(Canvas.transform);                                                                       // Canvas�̎q�ɂ���
        cardController = cardObject.GetComponent<CardController>();                                                             // ��������Prefab��CardController���擾
        cardController.Init(shopCardsID[cardID]);                                                                              // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������
        cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // �l�D��\��
        shopCards.Add(cardObject);
    }

    /// <summary>
    /// �J�[�h�𔃂��邩�ǂ����𔻒肵�܂�
    /// </summary>
    void PriceCheck()
    {
        //�������`�F�b�N
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (playerData._money < card.cardDataManager._cardPrice)     //������������Ȃ�������
            {
                Text textComponent = shopCards[i].transform.GetChild(3).GetChild(0).GetComponent<Text>();       // Price�\���e�L�X�g���擾
                textComponent.color = Color.red;                                                                // �Ԃŕ\��
            }

        }
    }

    public void BuyCards(GameObject selectCard)
    {
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectCard == shopCards[i])
            {
                CardController card = shopCards[i].GetComponent<CardController>();
                playerData._deckList.Add(shopCardsID[i]);
            }
        }


    }



    /// <summary>
    /// �J�[�h���I�̃f�o�b�O�p�ł��B
    /// </summary>
    private void DebugLottery()
    {
        lottery.fromShopController = true;
        shopCardsID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        Debug.Log("�J�[�h1:" + shopCardsID[0] + "\n�J�[�h2:" + shopCardsID[1] + "\n�J�[�h3:" + shopCardsID[2]);

        // �V���b�v�ɕ��ԃJ�[�h�\��
        for (int i = 0; i < shopCardsID.Count; i++)
        {
            cardObject = Instantiate(cardPrefab, cardPlace[i].transform.position, cardPlace[i].transform.rotation);       // �J�[�h��Prefab�𐶐�
            cardObject.transform.SetParent(Canvas.transform);                                                                   // Canvas�̎q�ɂ���
            cardController = cardObject.GetComponent<CardController>();                                                         // ��������Prefab��CardController���擾
            cardController.Init(shopCardsID[i]);                                                                                  // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������

        }

        // �񕜃J�[�h�͌Œ�Ȃ̂ŕʓr�\��
        cardObject = Instantiate(cardPrefab, cardPlace[3].transform.position, cardPlace[3].transform.rotation);
        cardObject.transform.SetParent(Canvas.transform);
        cardController = cardObject.GetComponent<CardController>();
        cardController.Init(heelCardID);

        uiManager.UIEventReload();
    }
}
