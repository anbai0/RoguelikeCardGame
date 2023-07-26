using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireManager : MonoBehaviour
{
    PlayerDataManager playerData;

    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    List<int> deckNumberList;                    //�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    Vector3 CardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��


    void Start()
    {
        //playerData = GameManager.Instance.playerData;
        playerData = new PlayerDataManager("Wizard");
        InitDeck();
    }

    private void InitDeck() //�f�b�L����
    {
        deckNumberList = playerData._deckList;
        int distribute = DistributionOfCards(deckNumberList.Count);
        if (distribute <= 0) //�f�b�L�̖�����0���Ȃ琶�����Ȃ�
            return;
        for (int init = 1; init <= deckNumberList.Count; init++)// �f�b�L�̖�����
        {
            if (init <= distribute) //���߂�ꂽ����upperCardPlace�ɐ�������
            {
                CardController card = Instantiate(cardPrefab, upperCardPlace);//�J�[�h�𐶐�����
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init-1).ToString();//���������J�[�h�ɖ��O��t����
                card.Init(deckNumberList[init - 1]);//�f�b�L�f�[�^�̕\��
            }
            else //�c���lowerCardPlace�ɐ�������
            {
                CardController card = Instantiate(cardPrefab, lowerCardPlace);//�J�[�h�𐶐�����
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init-1).ToString();//���������J�[�h�ɖ��O��t����
                card.Init(deckNumberList[init - 1]);//�f�b�L�f�[�^�̕\��
            }
        }
    }

    /// <summary>
    /// �f�b�L�̃J�[�h�����ɂ���ď㉺��CardPlace�ɐU�蕪���鐔�����߂�
    /// </summary>
    /// <param name="deckCount">�f�b�L�̖���</param>
    /// <returns>���CardPlace�ɐ�������J�[�h�̖���</returns>
    int DistributionOfCards(int deckCount) 
    {
        int distribute = 0;
        if (0 <= deckCount && deckCount <= 5)//�f�b�L�̐���0�ȏ�5���ȉ��������� 
        {
            distribute = deckCount;//�f�b�L�̖���������
        }
        else if (deckCount > 5)//�f�b�L�̐���6���ȏゾ������
        {
            if (deckCount % 2 == 0)//�f�b�L�̖�����������������
            {
                int value = deckCount / 2;
                distribute = value;//�f�b�L�̔����̖����𐶐�
            }
            else //�f�b�L�̖��������������
            {
                int value = (deckCount - 1) / 2;
                distribute = value + 1;//�f�b�L�̔���+1�̖����𐶐�
            }
        }
        else //�f�b�L�̐���0��������������
        {
            distribute = 0;//�������Ȃ�
        }
        return distribute;
    }

    /// <summary>
    /// �J�[�h�̋��������郁�\�b�h�ł�
    /// </summary>
    /// <param name="selectCard">�I�����ꂽCard</param>
    public void CardEnhance(GameObject selectCard)
    {
        Debug.Log("�J�[�h�����I");
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //�I�����ꂽ�J�[�h��ID���擾
        for (int count = 0; count < deckNumberList.Count; count++)
        {
            if (deckNumberList[count] == id) //�f�b�L����ID�̃J�[�h��T�� 
            {
                if(deckNumberList[count] <= 20 && id != 3) //ID�̃J�[�h���������Ŗ����̗��łȂ����
                {
                    deckNumberList[count] += 100; //���̃J�[�h�̋����ł�ID�ɕύX����
                }
            }
            Debug.Log("���݂�Player�̃f�b�L���X�g�́F" + deckNumberList[count]);
        }
    }
}
