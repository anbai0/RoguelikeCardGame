using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour
{
    [SerializeField]
    UIManagerBattle uIManagerBattle;

    public Transform cardParent;
    public Vector3 defaultScale = Vector3.one * 0.22f;
    public float anchorPosY = -77.5f;

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

        Debug.Log("Cardの親:   " + transform.parent);
        transform.localScale = defaultScale;                     // sizeを戻し
        cardParent = transform.parent;                          // カードの親を取得
        transform.SetParent(cardParent.parent, false);          // カードの親から抜ける
        GetComponent<CanvasGroup>().blocksRaycasts = false;     // blocksRaycastsをオフにする
    }

    public void CardDrag(GameObject Card)
    {

    }

    public void CardDorp(GameObject Card)
    {
        transform.position = cardPos;                                  // カードを元の位置に戻す
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;             // blocksRaycastsをオンにする
        GameObject.Find("CardPlace").GetComponent<SortDeck>().Sort();       // 名前順にソートをする

    }

    public void CardEnter(GameObject Card)
    {
        cardPos = transform.position;
        transform.localScale = defaultScale * 1.5f;

        // スケールを大きくするとカードの一部が見えなくなるのでずらしています
        Vector3 anchorePos = GetComponent<RectTransform>().anchoredPosition;
        anchorePos = new Vector3(anchorePos.x, anchorPosY, anchorePos.z);
        GetComponent<RectTransform>().anchoredPosition = anchorePos;
        
    }
    public void CardExit(GameObject Card)
    {
        transform.position = cardPos;
        transform.localScale = defaultScale;
    }
}
