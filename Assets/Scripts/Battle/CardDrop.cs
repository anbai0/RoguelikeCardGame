using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)//カードがこのオブジェクトの範囲に落ちてきたときに行う処理
    {
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); // ドラッグしてきた情報からCardControllerを取得
        if (card != null) // もしカードがあれば
        {
            if (card.cardDataManager._cardState == 0)//カードが使用可能であれば
            {
                //カードの効果を発動
                BattleGameManager.Instance.PlayerTurn(card);
            }
        }
    }
}
