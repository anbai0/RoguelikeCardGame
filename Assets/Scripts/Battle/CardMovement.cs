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
        Debug.Log("ドラッグはじめ");
        Card.transform.localScale = defaultSize;                     // sizeを戻し
        cardParent = Card.transform.parent;                          // カードの親を取得
        Card.transform.SetParent(cardParent.parent, false);          // カードの親から抜ける
        Card.GetComponent<CanvasGroup>().blocksRaycasts = false;     // blocksRaycastsをオフにする
    }

    public void CardDrag()
    {

    }

    public void CardDorp(GameObject Card)
    {
        Card.transform.position = cardPos;                       // カードを元の位置に戻す
        Card.transform.SetParent(cardParent, false);
        Card.GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
        GameObject.Find("CardPlace").GetComponent<SortDeck>().Sort();//名前順にソートをする

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
