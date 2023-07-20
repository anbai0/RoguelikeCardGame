using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireManager : MonoBehaviour
{
    PlayerDataManager playerData;

    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    List<int> deckNumberList;                    //�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    Vector3 CardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��


    void Start()
    {
        playerData = GameManager.Instance.playerData;
        InitDeck();
    }

    private void InitDeck() //�f�b�L����
    {
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// �f�b�L�̖�����
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//�J�[�h�𐶐�����
            card.transform.localScale = CardScale;
            card.name = "Deck" + init.ToString();//���������J�[�h�ɖ��O��t����
            card.Init(deckNumberList[init]);//�f�b�L�f�[�^�̕\��
        }
    }

    /// <summary>
    /// �J�[�h�̋��������郁�\�b�h�ł�
    /// </summary>
    /// <param name="selectCard">�I�����ꂽCard</param>
    public void CardEnhance(GameObject selectCard)
    {
        Debug.Log("�J�[�h�����I");

    }
}
