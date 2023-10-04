using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrop : MonoBehaviour,IDropHandler
{
    [SerializeField] EmphasisDropPlace emphasisDropPlace;
    public void OnDrop(PointerEventData eventData)
    {
        //下記の一文は、魔女の霊薬を使用するとUIManagerBattleのOnDropにたとりつく前に処理が終わってしまう為、問題が起きないようにこちらにも実装しました。
        emphasisDropPlace.HiddenGameObject();

        if (eventData.button == PointerEventData.InputButton.Right) return;
        if (eventData.button == PointerEventData.InputButton.Middle) return;
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); // ドラッグしてきた情報からCardControllerを取得
        if (card != null && BattleGameManager.Instance.isPlayerTurn) // もしカードがあり、プレイヤーのターンの場合
        {
            if (card.cardDataManager._cardState == 0)//カードが使用可能であれば
            {
                Debug.Log("カードの効果発動");
                BattleGameManager.Instance.PlayerMove(card);
            }
        }
    }
}
