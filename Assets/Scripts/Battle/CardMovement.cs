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
    public float zoomPos = -77.5f;

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

        Debug.Log("Card�̐e:   " + transform.parent);
        transform.localScale = defaultSize;                     // size��߂�
        cardParent = transform.parent;                          // �J�[�h�̐e���擾
        transform.SetParent(cardParent.parent, false);          // �J�[�h�̐e���甲����
        GetComponent<CanvasGroup>().blocksRaycasts = false;     // blocksRaycasts���I�t�ɂ���
    }

    public void CardDrag(GameObject Card)
    {

    }

    public void CardDorp(GameObject Card)
    {
        transform.position = cardPos;                                  // �J�[�h�����̈ʒu�ɖ߂�
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;             // blocksRaycasts���I���ɂ���
        GameObject.Find("CardPlace").GetComponent<SortDeck>().Sort();       // ���O���Ƀ\�[�g������

    }

    public void CardEnter(GameObject Card)
    {
        cardPos = transform.position;
        GetComponent<RectTransform>().anchoredPosition = Vector3.up * zoomPos;


        transform.localScale = defaultSize * 1.5f;
    }
    public void CardExit(GameObject Card)
    {
        transform.position = cardPos;
        transform.localScale = defaultSize;
    }
}
