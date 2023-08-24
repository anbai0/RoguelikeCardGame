using UnityEngine;

/// <summary>
/// カードデータの生成と表示を行うスクリプト
/// </summary>
public class CardController : MonoBehaviour
{
    public CardViewManager cardViewManager;// カードの見た目の処理
    public CardDataManager cardDataManager;// カードのデータを処理

    private void Awake()
    {
        cardViewManager = GetComponent<CardViewManager>();
    }

    public void Init(int cardID)// カードを生成した時に呼ばれる関数
    {
        cardDataManager = new CardDataManager(cardID);// カードデータを生成
        cardViewManager.ViewCard(cardDataManager);// カードデータの表示
    }
}
