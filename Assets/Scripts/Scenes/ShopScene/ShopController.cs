using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardController cardController;

    Lottery lottery;

    bool isHeelPotion = false;

    private void Start()
    {
        //playerDataManager = GameManager.Instance.playerData;
        //foreach (var deck in playerDataManager._deckList)
        //{
        //    if (deck == 3)
        //    {
        //        isHeelPotion = true;
        //    }

        //}

        ////�v���C���[�̃f�b�L���쐬
        //deckNumberList = playerData._deckList;
        //for (int init = 0; init < deckNumberList.Count; init++)// �f�b�L�̖�����
        //{
        //    CardController card = Instantiate(cardPrefab, CardPlace);//�J�[�h�𐶐�����
        //    card.name = "Deck" + init.ToString();//���������J�[�h�ɖ��O��t����
        //    card.Init(deckNumberList[init]);//�f�b�L�f�[�^�̕\��
        //}

    }
    void Update()
    {


        //playerDataManager._money -= 50;
        //playerDataManager._deckList.Add(1);

    }

    public void ShowCards()
    {
        (int Card1, int Card2, int Card3) = lottery.ShopLottery();     // ����: �^�v���ƌ����ĕ����̖߂�l���󂯎���
        cardController.Init(rare2); 
        
    }
}
