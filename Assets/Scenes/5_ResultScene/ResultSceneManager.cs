using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ResultSceneManager : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] UIManagerResult uiManager;

    [Header("カード表示関係")]
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform deckPlace;
    [SerializeField] GameObject scrollView;     // デッキを表示するUIの親オブジェクト
    private List<int> deckNumberList;                    //プレイヤーのもつデッキナンバーのリスト

    // レリック
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    private Vector3 cardScale = Vector3.one * 0.25f;     // 生成するカードのスケール


    void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;
        AudioManager.Instance.PlayBGM("Result");
        InitDeck();
        ShowRelics();
        uiManager.UIEventsReload();
    }

    private void InitDeck() //デッキ生成
    {
        deckNumberList = GameManager.Instance.playerData._deckList;

        for (int init = 0; init < deckNumberList.Count; init++)         // 選択出来るデッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, deckPlace);   //カードを生成する
            card.transform.localScale = cardScale;
            card.name = "Deck" + (init).ToString();                     //生成したカードに名前を付ける
            card.Init(deckNumberList[init]);                            //デッキデータの表示
        }
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
        gm.UnloadAllScene();     // GameManagerのデータをリセット
    }
}
