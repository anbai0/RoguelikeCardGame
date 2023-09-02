using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameSettingsJson gameSettingsJson;
    public GameSettings gameSettings;
    [SerializeField] private AudioSetting audioSetting;

    // プレイヤー
    public PlayerDataManager playerData;
    public List<CardDataManager> cardDataList { private set; get; } = new List<CardDataManager>();
    public List<RelicDataManager> relicDataList { private set; get; } = new List<RelicDataManager>();
    public Dictionary<int, int> hasRelics { private set; get; } = new Dictionary<int, int>();     // 所持しているレリックを格納    
    public int maxCards { get; private set; } = 20;
    public int maxRelics { get; private set; } = 12;
    private const int defaultDeckSize = 4;
    public const int healCardID = 3;           // 魔女の霊薬のID
    private const int ariadnesThreadID = 1;     // アリドネの糸のレリックのID(デッキの上限を増やすレリック)

    private const int id7HPIncreaseAmount = 5;  //心の器のHP増加量

    public Action OnCardDiscard;      // カードの破棄を実行した時に呼び出されるデリゲート

    private bool isAlreadyRead = false; // ReadPlayerで読み込んだかを判定する

    //フィールド

    public int floor = 1; //階層

    [SerializeField] UIManager uiManager;
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;
    [SerializeField] SceneFader sceneFader;

    public static GameManager Instance;     // シングルトン
    protected void Awake()
    {
        gameSettings = gameSettingsJson.loadGameSettingsData();     // ゲーム設定のロード
        

        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }

        audioSetting.InstantiateAudioSetting();
        InitializeItemData();
        
        // 各シーンでデバッグするときにコメントを解除してください
        // 一度も読み込んでいなければ
        //if (!isAlreadyRead) ReadPlayer("Debug");
    }

    private void Update()
    {
        Debug.Log("GameManager:   " + gameSettings.overallVolume);
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
            hasRelics.Add(relicID, 0);

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
        if (playerJob == "Debug")
        {
            playerData = new PlayerDataManager("Debug");
            // 全レリック取得
            hasRelics[1] += 5;
            hasRelics[2] += 1;
            hasRelics[3] += 1;
            hasRelics[4] += 1;
            hasRelics[5] += 1;
            hasRelics[6] += 1;
            hasRelics[7] += 1;
            hasRelics[8] += 1;
            hasRelics[9] += 1;
            hasRelics[10] += 1;
            hasRelics[11] += 1;
            hasRelics[12] += 1;

            ShowRelics();
            return;
        }
        playerData._deckList.Clear(); //デッキリストを空にする
        //開始時に配布されるカードを追加する
        playerData._deckList.Add(1); //スイング
        playerData._deckList.Add(2); //ヒール
        playerData._deckList.Add(4); //ガード

    }


    #region カード関係
    /// <summary>
    /// 渡されたカードIDのカードを取得し、カード図鑑に登録します。
    /// </summary>
    /// <param name="cardID"></param>
    public void AddCard(int cardID)
    {
        playerData._deckList.Add(cardID);
        //gameSettings.collectedCardHistory[cardID] = true;
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
        List<int> checkDeck = playerData._deckList.ToList();   // デッキチェック用にデッキをコピー

        // 魔女の霊薬を除外
        checkDeck.Remove(healCardID);

        // 所持しているカードがデッキのサイズ以上だったら
        if (checkDeck.Count >= maxDeckSize)
        {
            uiManager.ToggleDiscardScreen(true);        // カード破棄画面
            return true;
        }
        return false;
    }


    /// <summary>
    /// カードを破棄した場合、メソッド内のイベントを発火させます。
    /// </summary>
    /// <param name="isDiscard">カードを破棄した場合、true,破棄しなかった場合、false</param>
    public void TriggerDiscardAction(bool isDiscard)
    {
        if (isDiscard)
        {
            OnCardDiscard?.Invoke();
            OnCardDiscard = null;
        }
        else
        {
            OnCardDiscard = null;
            return;
        }
    }
    #endregion


    #region レリック関係
    /// <summary>
    /// Overlayに所持レリックを表示します。
    /// </summary>
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
    /// レリックを入手した際にIDを確認し、ID7(心の器)の時に効果を処理する
    /// </summary>
    /// <param name="relicID">入手したレリックの番号</param>
    public void CheckGetRelicID7(int relicID)
    {
        if (relicID == 7) //レリックがID7(心の器)の場合
        {
            playerData._playerHP += id7HPIncreaseAmount; //心の器の増加量分HPを上昇させる
        }
    }
    #endregion


    #region 全シーンアンロードの処理とデータの初期化
    /// <summary>
    /// ゲームデータのリセットをします。
    /// <para>現状リザルトシーンからタイトルシーンに戻るときのみにしか使えないため後で書き換えます。</para>
    /// </summary>
    private void ResetGameData()
    {
        Lottery.Instance.shopCards.Clear();
        PlayerController.isPlayerActive = true;
        PlayerController.isEvents = false;
        PlayerController.isSetting = false;
        PlayerController.isConfimDeck = false;

        playerData = null;

        // 所持レリック初期化
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics[RelicID] = 0; // キーと値を設定
        }

        ShowRelics();

        floor = 1; //階層を1に戻す
    }

    /// <summary>
    /// UnloadAllScenesメソッドをFadeOutInWrapperメソッドに渡して実行します。
    /// </summary>
    public void UnloadAllScene()
    {
        sceneFader.FadeOutInWrapper(UnloadAllScenes);
    }

    /// <summary>
    /// ManagerScene以外のシーンをアンロードし、メモリの開放を行い、データをリセットして、
    /// タイトルシーンをロードします。
    /// </summary>
    public async Task UnloadAllScenes()
    {
        AsyncOperation asyncOperation;

        // フィールドシーンとマネージャーシーンを除外してアンロード
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "ManagerScene" && scene.name != "FieldScene")
            {
                asyncOperation = SceneManager.UnloadSceneAsync(scene);
                while (!asyncOperation.isDone) await Task.Yield();
            }
        }

        // 参照解除の関係でフィールドシーンを最後にアンロードする。
        Scene fieldScene = SceneManager.GetSceneByName("FieldScene");
        if (fieldScene.isLoaded)
        {
            asyncOperation = SceneManager.UnloadSceneAsync(fieldScene);
            while (!asyncOperation.isDone) await Task.Yield();
        }

        // タイトルシーンロード
        asyncOperation = asyncOperation = SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
        while (!asyncOperation.isDone) await Task.Yield();

        Resources.UnloadUnusedAssets();

        ResetGameData();
    }
    #endregion

}
