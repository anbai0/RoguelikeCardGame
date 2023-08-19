using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardMovement : MonoBehaviour
{
    [SerializeField]
    UIManagerBattle uIManagerBattle;

    public Transform cardParent;
    public Vector3 defaultScale = Vector3.one * 0.22f;
    public float anchorPosY = -77.5f;

    Vector3 cardPos;
    bool isInitialize = false;

    //�J�[�h�̕\���Ȃǂ̖��������ł�����@(������)
    PickCard pickCardScript;
    GameObject pickCard = null;
    CardDataManager cardData;
    TextMeshProUGUI effectText;

    private void Start()
    {
        cardPos = transform.position;
        uIManagerBattle = FindObjectOfType<UIManagerBattle>();

        pickCardScript = GetComponent<PickCard>();
        cardData = GetComponent<CardController>().cardDataManager;
        effectText = transform.Find("CardInfo/CardEffect").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!isInitialize)
        {
            uIManagerBattle.UIEventsReload();
            isInitialize = true;
        }
    }

    public void CardBeginDrag(GameObject Card)
    {
        //transform.localScale = defaultScale;                    // size��߂�
        cardParent = transform.parent;                          // �J�[�h�̐e���擾
        transform.SetParent(cardParent.parent, false);          // �J�[�h�̐e���甲����

        pickCard.transform.Find("CardInfo").gameObject.SetActive(false);
        pickCard.GetComponent<CardState>().isActive = false;
    }

    public void CardDrag(GameObject Card)
    {

    }

    public void CardDorp(GameObject Card)
    {
        transform.SetParent(cardParent, false);
        GameObject.Find("CardPlace").GetComponent<SortName>().Sort();       // ���O���Ƀ\�[�g������


        GameObject.Find("PickCardPlace").GetComponent<SortName>().Sort();
    }

    public void CardEnter(GameObject Card)
    {
        //cardPos = transform.position;
        //transform.localScale = defaultScale * 1.5f;

        //// �X�P�[����傫������ƃJ�[�h�̈ꕔ�������Ȃ��Ȃ�̂ł��炵�Ă��܂�
        //Vector3 anchorePos = GetComponent<RectTransform>().anchoredPosition;
        //anchorePos = new Vector3(anchorePos.x, anchorPosY, anchorePos.z);
        //GetComponent<RectTransform>().anchoredPosition = anchorePos;

        pickCard = pickCardScript.ChoosePickCard(this.gameObject);
        pickCard = pickCardScript.SetPickCardStatus(this.gameObject, pickCard);
    }
    public void CardExit(GameObject Card)
    {
        //transform.position = cardPos;
        //transform.localScale = defaultScale;

        pickCard.transform.Find("CardInfo").gameObject.SetActive(false);
        pickCard.GetComponent<CardState>().isActive = false;
    }
}
