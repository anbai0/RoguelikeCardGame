using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour
{
    [SerializeField]
    UIManagerBattle uIManagerBattle;

    public Transform cardParent;
    public Vector3 defaultSize = Vector3.one * 0.22f;
    public float zoomPos = 150f;

    Vector3 cardPos;
    bool isInitialize = false;


    private void Start()
    {
        cardPos = transform.position;
        uIManagerBattle = FindObjectOfType<UIManagerBattle>();
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
        Debug.Log("�h���b�O�͂���");
        Card.transform.localScale = defaultSize;                     // size��߂�
        cardParent = Card.transform.parent;                          // �J�[�h�̐e���擾
        Card.transform.SetParent(cardParent.parent, false);          // �J�[�h�̐e���甲����
        Card.GetComponent<CanvasGroup>().blocksRaycasts = false;     // blocksRaycasts���I�t�ɂ���
    }

    public void CardDrag()
    {

    }

    public void CardDorp(GameObject Card)
    {
        Card.transform.position = cardPos;                       // �J�[�h�����̈ʒu�ɖ߂�
        Card.transform.SetParent(cardParent, false);
        Card.GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycasts���I���ɂ���
        GameObject.Find("CardPlace").GetComponent<SortDeck>().Sort();//���O���Ƀ\�[�g������

    }

    public void CardEnter(GameObject Card)
    {
        ////cardPos = Card.transform.position;
        //Card.transform.position += Vector3.up * zoomPos;
        //Card.transform.localScale = defaultSize * 1.5f;
    }
    public void CardExit(GameObject Card)
    {
        //Card.transform.position = cardPos;
        //Card.transform.localScale = defaultSize;
    }
}
