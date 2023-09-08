using System.Collections.Generic;
using UnityEngine;


public class BonfireManager : MonoBehaviour
{

    [SerializeField] GameObject cardEmptyText;

    // �J�[�h�\��
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform cardPlace;
    [SerializeField] Transform enhancedCardHolder;
    [SerializeField] CardController enhancedCard;
    private List<int> deckNumberList;                    //�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    private Vector3 CardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��

    void Start()
    {
        enhancedCard = Instantiate(cardPrefab, enhancedCardHolder);   // ������̃J�[�h��\������Prefab�𐶐�
        enhancedCard.gameObject.GetComponent<UIController>().enabled = false;
        enhancedCard.transform.SetParent(enhancedCardHolder);
        InitDeck();
    }

    private void InitDeck() //�f�b�L����
    {
        deckNumberList = GameManager.Instance.playerData._deckList;
        var selectableList = ExcludeDeckCards(deckNumberList);
        if (selectableList.Count <= 0) //�f�b�L�̖�����0���Ȃ琶�����Ȃ�
        {
            cardEmptyText.SetActive(true); //�����ł���J�[�h���Ȃ����Ƃ�Text�œ`����
            return;
        }

        for (int init = 0; init < selectableList.Count; init++)// �I���o����f�b�L�̖�����
        {
            CardController card = Instantiate(cardPrefab, cardPlace);   //�J�[�h�𐶐�����
            card.transform.localScale = CardScale;
            card.name = "Deck" + (init).ToString();                     //���������J�[�h�ɖ��O��t����
            card.Init(selectableList[init]);                            //�f�b�L�f�[�^�̕\��
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

    /// <summary>
    /// ������̃J�[�h��\�����郁�\�b�h�ł�
    /// </summary>
    /// <param name="selectCard">�I�����ꂽCard</param>
    public void DisplayEnhancedCard(GameObject selectCard)
    {
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //�I�����ꂽ�J�[�h��ID���擾
        enhancedCard.Init(id+100);                            //�f�b�L�f�[�^�̕\��
        
    }

    public void UnLoadBonfireScene()
    {
        // ���΃V�[�����A�����[�h
        SceneFader.Instance.SceneChange(unLoadSceneName: "BonfireScene", allowPlayerMove: true);
    }
}
