using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ResultSceneManager : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] SceneFader sceneFader;
    [SerializeField] UIManagerResult uiManager;
    
    // カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    private List<int> deckNumberList;                    //プレイヤーのもつデッキナンバーのリスト

    // レリック
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    private Vector3 CardScale = Vector3.one * 0.25f;     // 生成するカードのスケール


    void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;

        InitDeck();
        ShowRelics();
        uiManager.UIEventsReload();
    }

    public void InitDeck() //デッキ生成
    {
        deckNumberList = gm.playerData._deckList;
        int distribute = DistributionOfCards(deckNumberList.Count);
        if (distribute <= 0) //デッキの枚数が0枚なら生成しない
            return;
        for (int init = 1; init <= deckNumberList.Count; init++)// デッキの枚数分
        {
            if (init <= distribute) //決められた数をupperCardPlaceに生成する
            {
                CardController card = Instantiate(cardPrefab, upperCardPlace);//カードを生成する
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init - 1).ToString();//生成したカードに名前を付ける
                card.Init(deckNumberList[init - 1]);//デッキデータの表示
            }
            else //残りはlowerCardPlaceに生成する
            {
                CardController card = Instantiate(cardPrefab, lowerCardPlace);//カードを生成する
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init - 1).ToString();//生成したカードに名前を付ける
                card.Init(deckNumberList[init - 1]);//デッキデータの表示
            }
        }
    }

    /// <summary>
    /// デッキのカード枚数によって上下のCardPlaceに振り分ける数を決める
    /// </summary>
    /// <param name="deckCount">デッキの枚数</param>
    /// <returns>上のCardPlaceに生成するカードの枚数</returns>
    int DistributionOfCards(int deckCount)
    {
        int distribute = 0;
        if (0 <= deckCount && deckCount <= 5)//デッキの数が0以上5枚以下だったら 
        {
            distribute = deckCount;//デッキの枚数分生成
        }
        else if (deckCount > 5)//デッキの数が6枚以上だったら
        {
            if (deckCount % 2 == 0)//デッキの枚数が偶数だったら
            {
                int value = deckCount / 2;
                distribute = value;//デッキの半分の枚数を生成
            }
            else //デッキの枚数が奇数だったら
            {
                int value = (deckCount - 1) / 2;
                distribute = value + 1;//デッキの半分+1の枚数を生成
            }
        }
        else //デッキの数が0枚未満だったら
        {
            distribute = 0;//生成しない
        }
        return distribute;
    }


    public void ShowRelics()
    {
        // relicPlaceの子オブジェクトをすべてDestroy
        Transform[] children = relicPlace.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        for (int RelicID = 1; RelicID <= gm.maxRelics; RelicID++)
        {
            if (gm.hasRelics.ContainsKey(RelicID) && gm.hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                //relic.transform.localScale = Vector3.one * 0.9f;                   // 生成したPrefabの大きさ調整
                relic.Init(RelicID);                                               // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = gm.hasRelics[RelicID].ToString();      // Prefabの子オブジェクトである所持数を表示するテキストを変更

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicName.ToString();        // レリックの名前を変更
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicEffect.ToString();      // レリック説明変更
            }
        }

        uiManager.UIEventsReload();
    }


    public void SceneUnLoad()
    {
        gm.ResetGameData();     // GameManagerのデータをリセット

        gm = null;      // 参照解除

        // Resultシーンをアンロードし、タイトルシーンをロード
        sceneFader.SceneChange("TitleScene", "ResultScene");
    }
}
