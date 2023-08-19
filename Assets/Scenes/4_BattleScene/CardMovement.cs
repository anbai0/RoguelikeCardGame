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

    //カードの表示などの問題を解決できる方法(仮決定)
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
        //transform.localScale = defaultScale;                    // sizeを戻し
        cardParent = transform.parent;                          // カードの親を取得
        transform.SetParent(cardParent.parent, false);          // カードの親から抜ける

        pickCard.transform.Find("CardInfo").gameObject.SetActive(false);
        pickCard.GetComponent<CardState>().isActive = false;
    }

    public void CardDrag(GameObject Card)
    {

    }

    public void CardDorp(GameObject Card)
    {
        transform.SetParent(cardParent, false);
        GameObject.Find("CardPlace").GetComponent<SortName>().Sort();       // 名前順にソートをする


        GameObject.Find("PickCardPlace").GetComponent<SortName>().Sort();
    }

    public void CardEnter(GameObject Card)
    {
        //cardPos = transform.position;
        //transform.localScale = defaultScale * 1.5f;

        //// スケールを大きくするとカードの一部が見えなくなるのでずらしています
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
