using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    //PlayerDataManager playerDataManager;
    CardDataManager cardData;       // �J�[�h�̃f�[�^������
    CardController cardController;
    public Lottery lottery;
    [SerializeField]
    UIManagerShopScene uiManager;

    int Card1, Card2, Card3;    //���I���ꂽ�J�[�h��ID������ϐ�
    int heelPotionCardID = 3;   //�񕜃J�[�h��ID

    bool isHeelPotion = false;  //�񕜃J�[�h�������Ă��邩�H


    [SerializeField]
    GameObject Canvas, cardPlace1, cardPlace2, cardPlace3, cardPlace4;  //Canvas��Card�̐����ʒu
    [SerializeField]
    GameObject cardPrefab;

    GameObject card1, card2, card3, card4;
    Sprite cardImage;


    private CardController SelectController;    //���������J�[�hPrefab�ɃA�^�b�`����Ă���CardController���i�[����


    CardController selectCard1, selectCard2, selectCard3, selectCard4;



    [SerializeField]
    int tmpID = 0;

    private void Start()
    {





    }



    void Update()
    {

        if (Lottery.isInitialize)
        {
            lottery1();


            //���̃J�[�h�̐���
            card1 = Instantiate(cardPrefab, cardPlace1.transform.position, cardPlace1.transform.rotation);                  //�J�[�h��Prefab�𐶐�
            card1.transform.SetParent(Canvas.transform);
            selectCard1 = card1.GetComponent<CardController>();                        //��������Prefab��CardController���擾
            selectCard1.Init(Card1);                                                   //����CardController��Init���\�b�h���g���J�[�h�̐����ƕ\��������

            //�^�񒆂̃J�[�h�̐���
            card2 = Instantiate(cardPrefab, cardPlace2.transform.position, cardPlace2.transform.rotation);
            card2.transform.SetParent(Canvas.transform);
            selectCard2 = card2.GetComponent<CardController>();
            selectCard2.Init(Card2);

            //�E�̃J�[�h�̐���
            card3 = Instantiate(cardPrefab, cardPlace3.transform.position, cardPlace3.transform.rotation);
            card3.transform.SetParent(Canvas.transform);
            selectCard3 = card3.GetComponent<CardController>();
            selectCard3.Init(Card3);

            //�񕜃J�[�h�̐���
            card4 = Instantiate(cardPrefab, cardPlace4.transform.position, cardPlace4.transform.rotation);
            card4.transform.SetParent(Canvas.transform);
            selectCard4 = card4.GetComponent<CardController>();
            selectCard4.Init(heelPotionCardID);

            uiManager.ReloadUI();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tmpID >= 20)
                tmpID = 0;

            tmpID++;

            selectCard1.Init(tmpID);
        }

    }



    void lottery1()
    {
        (Card1, Card2, Card3) = lottery.ShopLottery(2,1,1);     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        Debug.Log("�J�[�h1:" + Card1 + "\n�J�[�h2:" + Card2 + "\n�J�[�h3:" + Card3);

        Lottery.isInitialize = false;
    }
    
}
