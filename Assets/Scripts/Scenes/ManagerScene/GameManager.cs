using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //プレイヤー
    public PlayerDataManager playerData;
    public List<CardDataManager> cardDataList { private set; get; } = new List<CardDataManager>();
    public List<RelicDataManager> relicDataList { private set; get; } = new List<RelicDataManager>();
    public Dictionary<int, int> hasRelics { private set; get; } = new Dictionary<int, int>();     // 所持しているレリックを格納    
    public int maxCards { get; private set; } = 20;
    public int maxRelics { get; private set; } = 12;
    private const int defaultDeckSize = 3;
    private const int ariadnesThreadID = 1;     // アリドネの糸のレリックのID(デッキの上限を増やすレリック)

    public event Action Ondiscard;      // カードの破棄を実行した場合に発火するイベント

    bool isAlreadyRead = false; // ReadPlayerで読み込んだかを判定する

    [SerializeField] UIManager uiManager;
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    //シングルトン
    public static GameManager Instance;
    private void Awake()
    {
        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }

        InitializeItemData();

        // 各シーンでデバッグするときにコメントを解除してください
        // 一度も読み込んでいなければ
        if (!isAlreadyRead) ReadPlayer("Warrior");
        
    }

    
    /// <summary>
    /// アイテムデータの初期化を行います。
    /// </summary>
    private void InitializeItemData()
    {
        cardDataList.Add(new CardDataManager(1));       // ID順に管理したいため最初の要素だけ代入
        for (int cardID = 1; cardID <= maxCards; cardID++)
        {
            cardDataList.Add(new CardDataManager(cardID));
        }

        relicDataList.Add(new RelicDataManager(1));     // ID順に管理したいため最初の要素だけ代入
        for (int relicID = 1; relicID <= maxRelics; relicID++)
        {
            hasRelics.Add(relicID,0);

            relicDataList.Add(new RelicDataManager(relicID));
        }
    }

    /// <summary>
    /// 選択されたプレイヤーをインスタンス化し、レリックを取得します。
    /// </summary>
    /// <param name="playerJob"></param>
    public void ReadPlayer(string playerJob)
    {
        isAlreadyRead = true;
        if (playerJob == "Warrior")
        {
            playerData = new PlayerDataManager("Warrior");
            hasRelics[10] += 1;      // 黄金の果実
            hasRelics[4] += 2;       // 神秘のピアス
            ShowRelics();
        }
        if (playerJob == "Wizard")
        {
            playerData = new PlayerDataManager("Wizard");
            hasRelics[5] += 1;     // 千里眼鏡
            hasRelics[9] += 2;     // 富豪の金貨袋
            ShowRelics();
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

        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            //辞書内に指定したRelicIDのキーが存在するかどうかとレリックを１つ以上所持しているか
            if (hasRelics.ContainsKey(RelicID) && hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                relic.transform.localScale = Vector3.one * 0.9f;                   // 生成したPrefabの大きさ調整
                relic.Init(RelicID);                                               // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = hasRelics[RelicID].ToString();      // Prefabの子オブジェクトである所持数を表示するテキストを変更

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = relicDataList[RelicID]._relicName.ToString();        // レリックの名前を変更
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = relicDataList[RelicID]._relicEffect.ToString();      // レリック説明変更

            }
        }

        uiManager.UIEventsReload();
    }


    /// <summary>
    /// 所持カードがデッキ上限に達しているかを判定し、
    /// <para>上限に達している場合、カード破棄画面に遷移します。</para>
    /// <para></para>
    /// </summary>
    /// <returns>
    /// デッキ上限に達している場合、true
    /// <para>デッキ上限に達していない場合、false</para>
    /// </returns>
    public bool CheckDeckFull()
    {
        int maxDeckSize = defaultDeckSize + hasRelics[ariadnesThreadID];
        if (playerData._deckList.Count >= maxDeckSize)
        {
            uiManager.ToggleDiscardScreen(true);        // カード破棄画面     

            return true;
        }
        return false;
    }



    public void TriggerDiscardDelegate(bool isDiscard)
    {
        Ondiscard?.Invoke();
    }

    /// <summary>
    /// ゲームデータのリセットをします。
    /// <para>現状リザルトシーンからタイトルシーンに戻るときのみにしか使えないため後で書き換えます。</para>
    /// </summary>
    public void ResetGameData()
    {
        playerData = null;
        isAlreadyRead = false;
        Instance = null;

        // 所持レリック初期化
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics[RelicID] = 0; // キーと値を設定
        }

        ShowRelics();
    }
}
