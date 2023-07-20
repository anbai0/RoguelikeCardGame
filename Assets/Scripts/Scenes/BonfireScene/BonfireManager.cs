using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireManager : MonoBehaviour
{
    PlayerDataManager playerData;

    //カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    List<int> deckNumberList;                    //プレイヤーのもつデッキナンバーのリスト

    Vector3 CardScale = Vector3.one * 0.25f;     // 生成するカードのスケール


    void Start()
    {
        playerData = GameManager.Instance.playerData;
        InitDeck();
    }

    private void InitDeck() //デッキ生成
    {
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// デッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//カードを生成する
            card.transform.localScale = CardScale;
            card.name = "Deck" + init.ToString();//生成したカードに名前を付ける
            card.Init(deckNumberList[init]);//デッキデータの表示
        }
    }

    /// <summary>
    /// カードの強化をするメソッドです
    /// </summary>
    /// <param name="selectCard">選択されたCard</param>
    public void CardEnhance(GameObject selectCard)
    {
        Debug.Log("カード強化！");

    }
}
