using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Transform cardParent;
    public float zoomSize = 1.1f;
    public float zoomPos = 0.3f;
    Vector3 cardPos;
    bool isDragging;
    private void Start()
    {
        cardPos = transform.position;
        isDragging = false;
    }
    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        transform.localScale = Vector3.one;
        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        transform.position = cardPos;
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
        GameObject.Find("CardPlace").GetComponent<SortDeck>().Sort();//名前順にソートをする
        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)//マウスのポインターをかざしたときの処理
    {
        if (!Input.GetMouseButton(0) && !isDragging)
        {
            cardPos = transform.position;
            transform.position += Vector3.up * zoomPos;
            transform.localScale = Vector3.one * zoomSize;
        }
    }

    public void OnPointerExit(PointerEventData eventData)//マウスのポインターが離れたときの処理
    {
        if (!Input.GetMouseButton(0) && !isDragging)
        {
            transform.position = cardPos;
            transform.localScale = Vector3.one;
        }
    }
}
