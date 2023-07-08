using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    //PlayerDataManager playerDataManager;
    CardDataManager cardData;       // �J�[�h�̃f�[�^������
    CardController cardController;

    [SerializeField]
    Lottery lottery;
    [SerializeField]
    UIManagerShopScene uiManager;


    int heelPotionCardID = 3;   //�񕜃J�[�h��ID
    bool isHeelPotion = false;  //�񕜃J�[�h�������Ă��邩�H

    [SerializeField]
    GameObject cardPrefab;
    [SerializeField]
    GameObject Canvas;
    [SerializeField]
    List<GameObject> cardPlace;     // Card�̐����ʒu


    GameObject cardObject;
    int[] ShopCards;



    [SerializeField]
    int tmpID = 0;      //�f�o�b�O�p


    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();

            // �V���b�v�ɕ��ԃJ�[�h�\��
            for(int i = 0; i < ShopCards.Length; i++)
            {
                cardObject = Instantiate(cardPrefab, cardPlace[i].transform.position, cardPlace[i].transform.rotation);       // �J�[�h��Prefab�𐶐�
                cardObject.transform.SetParent(Canvas.transform);                                                             // Canvas�̎q�ɂ���
                cardController = cardObject.GetComponent<CardController>();                                                   // ��������Prefab��CardController���擾
                cardController.Init(ShopCards[i]);                                                                            // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������
                cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                      // �l�D��\��
            }

            // �񕜃J�[�h�͌Œ�Ȃ̂ŕʓr�\��
            cardObject = Instantiate(cardPrefab, cardPlace[3].transform.position, cardPlace[3].transform.rotation);
            cardObject.transform.SetParent(Canvas.transform);
            cardController = cardObject.GetComponent<CardController>();
            cardController.Init(heelPotionCardID);
            cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);

            uiManager.UIEventReload();
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


    // Lottery�X�N���v�g���璊�I�����J�[�hID���󂯎�郁�\�b�h
    void ShopLottery()
    {
        lottery.fromShopController = true;
        //(Card1, Card2, Card3) = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        ShopCards = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });
        Debug.Log("�J�[�h1:" + ShopCards[0] + "\n�J�[�h2:" + ShopCards[1] + "\n�J�[�h3:" + ShopCards[2]);

        Lottery.isInitialize = false;
    }








    private void DebugLottery()
    {
        lottery.fromShopController = true;
        ShopCards = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });
        Debug.Log("�J�[�h1:" + ShopCards[0] + "\n�J�[�h2:" + ShopCards[1] + "\n�J�[�h3:" + ShopCards[2]);

        // �V���b�v�ɕ��ԃJ�[�h�\��
        for (int i = 0; i < ShopCards.Length; i++)
        {
            cardObject = Instantiate(cardPrefab, cardPlace[i].transform.position, cardPlace[i].transform.rotation);       // �J�[�h��Prefab�𐶐�
            cardObject.transform.SetParent(Canvas.transform);                                                                   // Canvas�̎q�ɂ���
            cardController = cardObject.GetComponent<CardController>();                                                         // ��������Prefab��CardController���擾
            cardController.Init(ShopCards[i]);                                                                                  // �擾����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������

        }

        // �񕜃J�[�h�͌Œ�Ȃ̂ŕʓr�\��
        cardObject = Instantiate(cardPrefab, cardPlace[3].transform.position, cardPlace[3].transform.rotation);
        cardObject.transform.SetParent(Canvas.transform);
        cardController = cardObject.GetComponent<CardController>();
        cardController.Init(heelPotionCardID);

        uiManager.UIEventReload();
    }
}
