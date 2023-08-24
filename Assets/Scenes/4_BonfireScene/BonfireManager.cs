using System.Collections.Generic;
using UnityEngine;


public class BonfireManager : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;

    [SerializeField] GameObject cardEmptyText;

    // �J�[�h�\��
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    private List<int> deckNumberList;                    //�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    private Vector3 CardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��

    void Start()
    {
        cardEmptyText.SetActive(false);
        InitDeck();
    }

    private void InitDeck() //�f�b�L����
    {
        deckNumberList = GameManager.Instance.playerData._deckList;
        var selectableList = ExcludeDeckCards(deckNumberList);
        int distribute = DistributionOfCards(selectableList.Count);
        if (distribute <= 0) //�f�b�L�̖�����0���Ȃ琶�����Ȃ�
        {
            cardEmptyText.SetActive(true); //�����ł���J�[�h���Ȃ����Ƃ�Text�œ`����
            return;
        }
        for (int init = 1; init <= selectableList.Count; init++)// �I���o����f�b�L�̖�����
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
    /// �����̗��⋭���ς݂̃J�[�h�̓��X�g���珜�O����
    /// </summary>
    /// <param name="deckList">�v���C���[�̏������Ă���f�b�L���X�g</param>
    /// <returns>�����ł��Ȃ����̂��������f�b�L���X�g</returns>
    List<int> ExcludeDeckCards(List<int> deckList)
    {
        List<int> selectableList = new List<int>();
        for (int deckNum = 0; deckNum < deckList.Count; deckNum++)
        {
            if (deckList[deckNum] != 3 && deckList[deckNum] <= 20) //ID3�̖����̗��ł͂Ȃ��AID��20�ȉ��̖������J�[�h�̏ꍇ
            {
                selectableList.Add(deckList[deckNum]);
            }
        }
        return selectableList;
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
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //�I�����ꂽ�J�[�h��ID���擾
        for (int count = 0; count < deckNumberList.Count; count++)
        {
            if (deckNumberList[count] == id) //�f�b�L����ID�̃J�[�h��T�� 
            {
                deckNumberList[count] += 100; //���̃J�[�h�̋����ł�ID�ɕύX����
            }
        }
    }


    public void UnLoadBonfireScene()
    {
        // ���΃V�[�����A�����[�h
        sceneFader.SceneChange(unLoadSceneName: "BonfireScene");
    }
}
