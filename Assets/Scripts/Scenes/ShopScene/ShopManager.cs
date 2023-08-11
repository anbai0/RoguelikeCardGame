using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SelfMadeNamespace;

/// <summary>
/// ShopScene��̃A�C�e���̐����A�l�i�`�F�b�N�A�w���������Ǘ����܂��B
/// ToDo: HasHealPotion()���\�b�h�̏���������܂�ǂ��Ȃ�����������̂ŏC���������B
/// </summary>
public class ShopManager : MonoBehaviour
{
    GameManager gm;

    [SerializeField] private Lottery lottery;
    [SerializeField] private UIManagerShop uiManager;
    [SerializeField] private ManagerSceneLoader msLoader;
    [SerializeField] private SceneFader sceneFader;

    private const int healCardID = 3;                       // �񕜃J�[�h��ID
    private const int deckLimitIncRelicID = 1;              // �f�b�L�̏����1�����₷�����b�N��ID
    private Vector3 scaleReset = Vector3.one * 0.37f;       // �J�[�h�̃f�t�H���g�̑傫��

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


    [SerializeField]    int tmpID = 0;      // �f�o�b�O�p

    private void Start()
    {
        // GameManager�擾
        gm = msLoader.GetGameManager();
    }

    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();
            shopCardsID.Add(healCardID);                        // �񕜃J�[�h��ǉ�
            shopRelicsID.Insert(0, deckLimitIncRelicID);        // �f�b�L�̏����1�����₷�����b�N��ǉ�
            //Debug.Log("�����b�N1:   " + shopRelicsID[0] + "\n�����b�N2:   " + shopRelicsID[1] + "\n�����b�N3:  " + shopRelicsID[2]);

            // �V���b�v�ɕ��ԃA�C�e����\��
            ShowItem();
            
            uiManager.UIEventsReload();          // UIEvent�X�V      
            Lottery.isInitialize = false;
        }

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
    /// �ς��Ȃ������ꍇ�l�i��Ԃ��\�����܂�
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
            if(cardsID == shopCardsID[healCardID])      // �񕜃J�[�h�������Ă���ꍇ
            {
                // �񕜃J�[�h���O���[�A�E�g�ɂ���
                shopCards[healCardNum].transform.GetChild(1).GetComponent<Image>().color = Color.gray;        // �������܂肢���������ł͂Ȃ��̂ŏC��������
                return true;
            }
        }
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
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectedItem == shopCards[i])         // �N���b�N�����J�[�h�ƃV���b�v�ɕ���ł�J�[�h����v������
            {
                CardController card = shopCards[i].GetComponent<CardController>();

                if (gm.playerData._playerMoney >= card.cardDataManager._cardPrice)           // �������������Ȃ�
                {
                    if (shopCardsID[i] != shopCardsID[healCardID])                        // �I�񂾃J�[�h���񕜃J�[�h�ł͂Ȃ������ꍇ
                    {
                        gm.playerData._playerMoney -= card.cardDataManager._cardPrice;       // ����������l�i���̂�����������
                        gm.playerData._deckList.Add(shopCardsID[i]);                         // �f�b�L�ɉ�����

                        selectedItem.SetActive(false);

                    } else if (!HasHealPotion())   // �I�񂾃J�[�h���񕜃J�[�h�ŁA�񕜃J�[�h���������Ă��Ȃ��ꍇ
                    {
                        gm.playerData._playerMoney -= card.cardDataManager._cardPrice;
                        gm.playerData._deckList.Add(shopCardsID[i]);


                        // �񕜃J�[�h���O���[�A�E�g�ɂ���
                        selectedItem.transform.GetChild(1).GetComponent<Image>().color = Color.gray;        // �������܂肢���������ł͂Ȃ��̂ŏC��������
                        selectedItem.transform.localScale = scaleReset;
                    }
                }               
            }
        }

        if (itemType == "Relic")
        for (int i = 0; i < shopRelics.Count; i++)
        {
            if (selectedItem == shopRelics[i])         // �N���b�N���������b�N�ƃV���b�v�ɕ���ł郌���b�N����v������
            {
                RelicController relic = shopRelics[i].GetComponent<RelicController>();

                if (gm.playerData._playerMoney >= relic.relicDataManager._relicPrice)         // �������������Ȃ�
                {
                    gm.playerData._playerMoney -= relic.relicDataManager._relicPrice;         // ����������l�i���̂�����������
                    gm.hasRelics[shopRelicsID[i]]++;                                          // �����b�N���擾
                    selectedItem.transform.localScale = scaleReset;                           // �X�P�[����߂�

                    selectedItem.SetActive(false);
                }
            }
        }

        gm.ShowRelics();        // �I�[�o�[���C�̃����b�N�\�����X�V
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