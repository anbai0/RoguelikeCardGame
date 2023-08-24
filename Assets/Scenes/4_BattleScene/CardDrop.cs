using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// カードをDropPlaceにドロップした時にプレイヤーの行動を呼び出すスクリプト
/// </summary>
public class CardDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)//カードがこのオブジェクトの範囲に落ちてきたときに行う処理
    {
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); // ドラッグしてきた情報からCardControllerを取得
        if (card != null && BattleGameManager.Instance.isPlayerTurn) // もしカードがあり、プレイヤーのターンの場合
        {
            if (card.cardDataManager._cardState == 0)//カードが使用可能であれば
            {
                //カードの効果を発動
                BattleGameManager.Instance.PlayerMove(card);
            }
        }
    }
}
