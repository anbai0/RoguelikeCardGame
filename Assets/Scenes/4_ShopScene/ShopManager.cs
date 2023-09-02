using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// ShopScene��̃A�C�e���̐����A�l�i�`�F�b�N�A�w���������Ǘ����܂��B
/// ToDo: HasHealPotion()���\�b�h�̏���������܂�ǂ��Ȃ�����������̂ŏC���������B
/// </summary>
public class ShopManager : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private UIManagerShop uiManager;
    [SerializeField] private SceneFader sceneFader;

    private const int healCardID = 3;                       // �񕜃J�[�h��ID
    private const int deckLimitIncRelicID = 1;              // �f�b�L�̏����1�����₷�����b�N��ID
    private Vector3 scaleReset = Vector3.one * 0.37f;       // �J�[�h�̃f�t�H���g�̑傫��
    private GameObject buyCard;                             // �J�[�h�𔃂��Ƃ��Ɉꎞ�I�Ɋi�[

    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject shoppingUI;

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


    [SerializeField] int tmpID = 0;      // �f�o�b�O�p

    private void Start()
    {
        // GameManager�擾(�ϐ����ȗ�)
        gm = GameManager.Instance;

        ShopLottery();
        shopCardsID.Add(healCardID);                        // �񕜃J�[�h��ǉ�
        shopRelicsID.Insert(0, deckLimitIncRelicID);        // �f�b�L�̏����1�����₷�����b�N��ǉ�
        //Debug.Log("�����b�N1:   " + shopRelicsID[0] + "\n�����b�N2:   " + shopRelicsID[1] + "\n�����b�N3:  " + shopRelicsID[2]);

        // �V���b�v�ɕ��ԃA�C�e����\��
        ShowItem();

        uiManager.UIEventsReload();          // UIEvent�X�V
    }

    void Update()
    {


        //// �f�o�b�O�p
        //if (Input.GetKeyDown(KeyCode.Space))
        //{    
        //    shopCardsID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        //    shopCardsID.Add(healCardID);                        // �񕜃J�[�h��ǉ�
        //    Debug.Log("�J�[�h1:    " + shopCardsID[0] + "\n�J�[�h2:   " + shopCardsID[1] + "\n�J�[�h3:   " + shopCardsID[2]);

        //    shopRelicsID = lottery.SelectRelicByRarity(new List<int> { 2, 1 });
        //    shopRelicsID.Insert(0, deckLimitIncRelicID);        // �f�b�L�̏����1�����₷�����b�N��ǉ�
        //    Debug.Log("�����b�N1:   " + shopRelicsID[0] + "\n�����b�N2:   " + shopRelicsID[1] + "\n�����b�N3:  " + shopRelicsID[2]);

        //    // �V���b�v�ɕ��ԃA�C�e����\��
        //    ShowItem();

        //    uiManager.UIEventsReload();          // UIEvent�X�V      
        //    Lottery.isInitialize = false;
        //}

        //if (Input.GetKeyDown(KeyCode.Space))        // Space���������ƂɎ���CardID�̃J�[�h���\�������
        //{
        //    if (tmpID >= 20)
        //        tmpID = 0;

        //    tmpID++;

        //    cardController.Init(tmpID);
        //    //DebugLottery();
        //}

        //if (Input.GetKeyDown(KeyCode.RightAlt))
        //{
        //    tmpID++;
        //    if(tmpID == 12)
        //    {
        //        tmpID = 0;
        //    }
        //    shopRelicsID[2] = tmpID;

        //    ShowItem();
        //    uiManager.UIEventsReload();
        //}

    }

    /// <summary>
    /// Lottery�X�N���v�g���璊�I�����J�[�hID���󂯎�郁�\�b�h
    /// </summary>
    void ShopLottery()
    {
        //(Card1, Card2, Card3) = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        shopCardsID = Lottery.Instance.SelectCardByRarity(new List<int> { 2, 1, 1 },true);
        shopRelicsID = Lottery.Instance.SelectRelicByRarity(new List<int> { 2, 1 });

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
            CardController cardController = cardObject.GetComponent<CardController>();                                              // ��������Prefab��CardController���擾
            cardController.Init(shopCardsID[cardID]);                                                                               // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������
            cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // �l�D��\��
            shopCards.Add(cardObject);
        }

        // �����b�N�\��
        for (int relicID = 0; relicID < shopRelicsID.Count; relicID++)
        {
            GameObject relicObject = Instantiate(relicPrefab, relicPlace[relicID].transform.position, relicPlace[relicID].transform.rotation);     // �����b�N��Prefab�𐶐�
            relicObject.transform.SetParent(shoppingUI.transform);                                                                      // shoppingUI�̎q�ɂ���
            RelicController relicController = relicObject.GetComponent<RelicController>();                                              // ��������Prefab��RelicController���擾
            relicController.Init(shopRelicsID[relicID]);                                                                                // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������
            relicObject.transform.Find("RelicPriceBG").gameObject.SetActive(true);                                                      // �l�D��\��
            shopRelics.Add(relicObject);
        }
    }

    /// <summary>
    /// �l�i�e�L�X�g���X�V���܂��B
    /// �A�C�e���𔃂��邩�ǂ����𔻒肵�A
    /// �����Ȃ��ꍇ�l�i��Ԃ��\�����܂�
    /// </summary>
    public void PriceTextCheck()
    {
        // �J�[�h�̒l�i�`�F�b�N
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (gm.playerData._playerMoney >= card.cardDataManager._cardPrice)     // �������������Ȃ�
            {
                TextMeshProUGUI textComponent = shopCards[i].transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();       // Price�\���e�L�X�g���擾
                textComponent.color = Color.white;                                                                                    // ���ŕ\��
            }
            else
            {
                TextMeshProUGUI textComponent = shopCards[i].transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();       // Price�\���e�L�X�g���擾
                textComponent.color = Color.red;                                                                                      // �Ԃŕ\��
            }

        }

        // �����b�N�̒l�i�`�F�b�N
        for (int i = 0; i < shopRelics.Count; i++)
        {
            RelicController relic = shopRelics[i].GetComponent<RelicController>();
            if (gm.playerData._playerMoney >= relic.relicDataManager._relicPrice)
            {
                TextMeshProUGUI textComponent = shopRelics[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.color = Color.white;
            }
            else
            {
                TextMeshProUGUI textComponent = shopRelics[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
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
        foreach(int cardsID in gm.playerData._deckList)
        {
            if(cardsID == healCardID)      // �񕜃J�[�h�������Ă���ꍇ
            {
                // �񕜃J�[�h���O���[�A�E�g�ɂ���
                shopCards[healCardNum].transform.GetChild(1).GetComponent<Image>().color = Color.gray;        // �������܂肢���������ł͂Ȃ��̂ŏC��������
                return true;
            }
        }
        // �񕜃J�[�h���n�C���C�g�ɂ���
        shopCards[healCardNum].transform.GetChild(1).GetComponent<Image>().color = Color.white;
        return false;
    }

    /// <summary>
    /// �A�C�e���𔃂������ł�
    /// </summary>
    /// <param name="selectedItem">�N���b�N����UIObject</param>
    /// <param name="itemType">Card�܂���Relic���w��</param>
    public void BuyItem(GameObject selectedItem, string itemType)
    {
        if (itemType == "Card")
        {
            CardController card = selectedItem.GetComponent<CardController>();
            int selectedCardID = card.cardDataManager._cardID; // �I�����ꂽ�J�[�h��ID���擾

            if (gm.playerData._playerMoney >= card.cardDataManager._cardPrice)           // �������������Ȃ�
            {
                if (selectedCardID != healCardID)   // �I�񂾃J�[�h���񕜃J�[�h�ł͂Ȃ������ꍇ
                {
                    // �f�b�L����`�F�b�N
                    if (gm.CheckDeckFull())     // �f�b�L����ɒB���Ă���ꍇ
                    {
                        gm.OnCardDiscard += RetryBuyItem;   // �J�[�h��j��������A������x���\�b�h���ĂԂ��߂Ƀf���Q�[�g�ɒǉ�

                        buyCard = selectedItem;             // �J�[�h�j����ʂɈڂ邽�߈ꎞ�I�Ɋi�[

                        // �A�C�e���̌����ڂ̑I����Ԃ�����
                        selectedItem.transform.localScale = scaleReset;
                        selectedItem.transform.GetChild(0).gameObject.SetActive(false);
                        // �I����ԃ��Z�b�g
                        uiManager.lastSelectedItem = null;
                        uiManager.isSelected = false;
                        return;
                    }


                    AudioManager.Instance.PlaySE("������");
                    gm.playerData._playerMoney -= card.cardDataManager._cardPrice;       // ����������l�i���̂�����������
                    gm.AddCard(selectedCardID);                         // �f�b�L�ɉ�����

                    selectedItem.SetActive(false);
                }
                else if (!HasHealPotion())          // �I�񂾃J�[�h���񕜃J�[�h�ŁA�񕜃J�[�h���������Ă��Ȃ��ꍇ
                {
                    AudioManager.Instance.PlaySE("������");
                    gm.playerData._playerMoney -= card.cardDataManager._cardPrice;       // ����������l�i���̂�����������
                    gm.AddCard(selectedCardID);                         // �f�b�L�ɉ�����

                    selectedItem.transform.localScale = scaleReset;                     // �X�P�[����߂�
                }
            }
            HasHealPotion();        // �񕜃J�[�h�̌����ڂ��X�V
        }


        if (itemType == "Relic")
        {
            RelicController relic = selectedItem.GetComponent<RelicController>();
            int selectedRelicID = relic.relicDataManager._relicID; // �I�����ꂽ�J�[�h��ID���擾     

            if (gm.playerData._playerMoney >= relic.relicDataManager._relicPrice)         // �������������Ȃ�
            {
                AudioManager.Instance.PlaySE("������");
                gm.playerData._playerMoney -= relic.relicDataManager._relicPrice;         // ����������l�i���̂�����������
                gm.hasRelics[selectedRelicID]++;                                          // �����b�N���擾

                selectedItem.SetActive(false);
            }
        }

        gm.ShowRelics();        // �I�[�o�[���C�̃����b�N�\�����X�V
    }


    /// <summary>
    /// �J�[�h�j����ʂŃJ�[�h��j��������Ăяo����郁�\�b�h�ł��B
    /// </summary>
    public void RetryBuyItem()
    {
        BuyItem(buyCard, "Card");
        buyCard = null;                     // �ꎞ�I�Ɋi�[���Ă��������Ȃ̂�null�ɂ��܂��B
    }

    /// <summary>
    /// �X����o�鏈���ł��B�V���b�v�V�[�����\���ɂ��܂��B
    /// </summary>
    public void ExitShop()
    {
        // �t�F�[�h�C���t�F�[�h�A�E�g�����A�V�[�����\����
        sceneFader.ToggleSceneWithFade("ShopScene", false);
    }
}
