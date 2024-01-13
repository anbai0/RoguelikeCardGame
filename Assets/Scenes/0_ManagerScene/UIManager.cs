using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManager : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;

    private GameManager gm;

    [Header("参照するコンポーネント")]
    [SerializeField] AudioSetting audioSetting;
    [Header("参照するUI")]
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] GameObject cardDiscardScreen;
    [SerializeField] GameObject DeckConfirmation;
    [Header("クリック後に参照するUI")]
    [SerializeField] GameObject overlayOptionButton;
    [SerializeField] GameObject titleOptionButton;
    [SerializeField] GameObject closeOptionButton;
    [SerializeField] GameObject titleBackButton;
    [SerializeField] GameObject closeConfirmButton;
    [SerializeField] GameObject confirmTitleBackButton;
    [SerializeField] GameObject discardReturnButton;
    [SerializeField] GameObject discardButton;
    [SerializeField] GameObject DeckConfirmationButton;
    [SerializeField] GameObject DeckReturnButton;
    [Space(10)]
    [SerializeField] TextMeshProUGUI myMoneyText;   //所持金を表示するテキスト
    [Space(10)]
    [SerializeField] GameObject confimDeskBackPanel;
    [SerializeField] GameObject desktopBackButton;
    [SerializeField] GameObject closeDesktopBackButton;
    [SerializeField] GameObject confimDeskbackButton;

    private bool isShowingCardDiscard = false;  // カード破棄画面を表示しているときだけtrue
    private bool isShowingDeckConfirmation = false; // デッキ確認画面を表示しているときだけtrue

    private bool isSelected = false;
    private GameObject lastSelectedCards;

    [Header("カード表示関係")]
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform deckCardPlace;
    [SerializeField] Transform discardCardPlace;
    [SerializeField] Transform discardHolder;       //破棄するカードを表示するのに使う親オブジェクト
    [SerializeField] Transform getCardHolder;       //取得するカードを表示するのに使う親オブジェクト
    private CardController discardCard;
    private CardController getCard;
    //[SerializeField] Transform upperCardPlace;
    //[SerializeField] Transform lowerCardPlace;
    //private Vector3 upperCardPos = new Vector3(0, 176, 0);   // upperCardのデフォルトの位置
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います
    private Vector3 scaleReset = Vector3.one * 0.25f;     // 生成するカードのスケール
    private List<int> deckNumberList;                     // プレイヤーのもつデッキナンバーのリスト
    
    [Header ("カード図鑑")]
    [SerializeField] GameObject cardPictureBookButton;
    [SerializeField] GameObject cardPictureBookReturunButton;
    [SerializeField] GameObject cardPictureBook;
    [SerializeField] Transform cardPictureBookPlace;

    void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;

        // 破棄するカードを表示するPrefabを生成
        discardCard = Instantiate(cardPrefab, discardHolder);
        discardCard.gameObject.GetComponent<UIController>().enabled = false;        // UIEventを拾ってほしくないためfalseに
        discardCard.transform.SetParent(discardHolder);

        // 取得するカードを表示するPrefabを作成
        getCard = Instantiate(cardPrefab, getCardHolder);
        getCard.gameObject.GetComponent<UIController>().enabled = false;        // UIEventを拾ってほしくないためfalseに
        getCard.transform.SetParent(getCardHolder);

        UIEventsReload();
    }

    void Update()
    {
        RefreshMoneyText();
    }

    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isEventsReset)             // イベントの初期化
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));

        }

        isEventsReset = false;
    }

    /// <summary>
    /// UIイベントを削除します。
    /// UIEventsReloadメソッド内で呼ばれます。
    /// </summary>
    private void RemoveListeners()
    {
        foreach (UIController UI in UIs)
        {
            UI.onLeftClick.RemoveAllListeners();
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
        }
    }
    #endregion

    /// <summary>
    /// 左クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UILeftClick(GameObject UIObject)
    {
        #region Overlayの処理

        // オプション画面表示
        if (UIObject == overlayOptionButton || UIObject == titleOptionButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            if (PlayerController.Instance != null)
                PlayerController.Instance.isSetting = true;
            optionScreen.SetActive(true);
        }

        // オプション画面非表示
        if (UIObject == closeOptionButton)
        {
            audioSetting.SaveAudioSetting();                // 音量設定のデータをセーブ
            AudioManager.Instance.UpdateBGMVolume();        // 今のBGMの音量を変更 
            AudioManager.Instance.PlaySE("選択音1");
            if (PlayerController.Instance != null)
                PlayerController.Instance.isSetting = false;
            optionScreen.SetActive(false);
        }

        // タイトルへ戻るの確認画面表示
        if (UIObject == titleBackButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            confirmationPanel.SetActive(true);
        }

        // タイトルへ戻るの確認画面非表示
        if (UIObject == closeConfirmButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            confirmationPanel.SetActive(false);
        }

        // 確認画面でタイトルへ戻るボタンを押したら
        if (UIObject == confirmTitleBackButton)
        {
            // bgm停止(IEFadeOutBGMVolmeコルーチンでは止まってくれなかったのでStopを使っています。)
            AudioManager.Instance.bgmAudioSource.Stop();    
            AudioManager.Instance.PlaySE("選択音2");

            // カード図鑑画面非表示
            cardPictureBook.SetActive(false);
            cardPictureBookButton.SetActive(true);
            // デッキ確認画面非表示
            isShowingDeckConfirmation = false;
            DeckConfirmation.SetActive(false);

            // タイトルへ戻る処理
            gm.UnloadAllScene();
            confirmationPanel.SetActive(false);
            optionScreen.SetActive(false);
        }

        // デスクトップへ戻る画面でバツボタンを押したら
        if (UIObject == closeDesktopBackButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            confimDeskBackPanel.SetActive(false);
        }

        // デスクトップへ戻るボタンを押したら
        if (UIObject == desktopBackButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            confimDeskBackPanel.SetActive(true);
        }

        // 確認画面でデスクトップへ戻るボタンを押したら
        if (UIObject == confimDeskbackButton)
        {
            audioSetting.SaveAudioSetting();                // 音量設定のデータをセーブ
            AudioManager.Instance.UpdateBGMVolume();        // 今のBGMの音量を変更 
            AudioManager.Instance.PlaySE("選択音1");
            // カード図鑑画面非表示
            cardPictureBook.SetActive(false);
            cardPictureBookButton.SetActive(true);
            // デッキ確認画面非表示
            isShowingDeckConfirmation = false;
            DeckConfirmation.SetActive(false);

            Application.Quit();     // ゲームを終了させる
        }

        #endregion

        #region カード確認画面の処理
        // デッキ確認画面を表示
        if (UIObject == DeckConfirmationButton && !isShowingDeckConfirmation)
        {
            AudioManager.Instance.PlaySE("選択音1");
            PlayerController.Instance.isConfimDeck = true;
            isShowingDeckConfirmation = true;

            // 前回表示したカードをDestroy
            foreach (Transform child in deckCardPlace.transform)
            {
                Destroy(child.gameObject);
            }
            
            DeckConfirmation.SetActive(true);
            InitDeck();
            UIEventsReload();
        }
        // 戻るボタンの処理
        if (UIObject == DeckReturnButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            PlayerController.Instance.isConfimDeck = false;
            isShowingDeckConfirmation = false;
            DeckConfirmation.SetActive(false);
        }
        #endregion

        #region カード破棄画面の処理

        // カード破棄画面が表示されているとき
        if (isShowingCardDiscard)
        {
            // カードをクリックしたら
            if (UIObject.CompareTag("Cards"))
            {
                isSelected = true;
                AudioManager.Instance.PlaySE("選択音1");

                // ボタン切り替え
                discardButton.SetActive(true);

                // 破棄するカードを表示
                discardHolder.gameObject.SetActive(true);
                DisplayDiscardCard(UIObject);

                // カード選択状態の切り替え
                if (lastSelectedCards != null && lastSelectedCards != UIObject)              // 二回目のクリックかつクリックしたオブジェクトが違う場合   
                {
                    lastSelectedCards.transform.localScale = scaleReset;
                    lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.GetChild(0).gameObject.SetActive(true);

                }

                lastSelectedCards = UIObject;

            }

            // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
            if (isSelected && UIObject.CompareTag("BackGround"))
            {
                lastSelectedCards.transform.localScale = scaleReset;
                lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
                lastSelectedCards = null;
                isSelected = false;
                discardHolder.gameObject.SetActive(false);     // 破棄するカードを非表示に

                // 破棄ボタン表示切り替え
                discardButton.SetActive(false);
            }

            // カード破棄画面を非表示に
            if (UIObject == discardReturnButton)
            {
                AudioManager.Instance.PlaySE("選択音1");
                lastSelectedCards = null;
                isSelected = false;
                discardHolder.gameObject.SetActive(false);     // 破棄するカードを非表示に
                ToggleDiscardScreen(false);
                gm.TriggerDiscardAction(false);
            }

            // カードを選んだ後、破棄ボタンを押すと、そのカードを破棄
            if (lastSelectedCards != null && UIObject == discardButton)
            {
                AudioManager.Instance.PlaySE("選択音1");
                discardHolder.gameObject.SetActive(false);     // 破棄するカードを非表示に
                int selectedCardID = lastSelectedCards.GetComponent<CardController>().cardDataManager._cardID; // 選択されたカードのIDを取得

                for (int cardIndex = 0; cardIndex < gm.playerData._deckList.Count; cardIndex++) {

                    if (gm.playerData._deckList[cardIndex] == selectedCardID)
                    {
                        // カードを破棄
                        gm.playerData._deckList.RemoveAt(cardIndex);
                        break;
                    }
                }
                lastSelectedCards.transform.localScale = scaleReset;
                lastSelectedCards = null;
                isSelected = false;
                ToggleDiscardScreen(false);
                gm.TriggerDiscardAction(true);
            }
        
        }

        #endregion

        #region カード図鑑の処理
        if(UIObject == cardPictureBookButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            // 前に表示したカードをDestroy
            foreach (Transform card in cardPictureBookPlace.transform)
            {
                Destroy(card.gameObject);
            }
            cardPictureBookButton.SetActive(false);
            cardPictureBook.SetActive(true);
            ShowCardPictureBook();
        }
        if(UIObject == cardPictureBookReturunButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            cardPictureBook.SetActive(false);
            cardPictureBookButton.SetActive(true);
        }
        #endregion
    }


    /// <summary>
    /// カーソルが触れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが触れたObject</param>
    void UIEnter(GameObject UIObject)
    {
        if (UIObject.CompareTag("Relics"))
        {
            AudioManager.Instance.PlaySE("OnCursor");
            UIObject.transform.GetChild(5).gameObject.SetActive(true);
        }


        // カード破棄画面が表示されているとき
        if ((isShowingCardDiscard || isShowingDeckConfirmation) && !isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                AudioManager.Instance.PlaySE("OnCursor");
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // アイテムの見た目を選択状態にする
            }
        }
    }


    /// <summary>
    /// カーソルが離れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが離れたObject</param>
    void UIExit(GameObject UIObject)
    {
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(false);
        }


        // カード破棄画面が表示されているとき
        if ((isShowingCardDiscard || isShowingDeckConfirmation) && !isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = scaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する
            }
        }
    }
    

    /// <summary>
    /// UIを切り替えるメソッドです。
    /// 引数は、タイトル画面であれば、"Title"、
    /// キャラ選択画面であれば"Chara"、
    /// Overlayだけ表示したい場合は、"OverlayOnly"にしてください。
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUI(string type)
    {
        if (type == "Title")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(true);
            titleBackButton.SetActive(false);
        }

        if (type == "None")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(false);
        }

        if (type == "OverlayOnly")
        {
            overlay.SetActive(true);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }
    }


    /// <summary>
    /// 所持金のテキストを更新するメソッドです。
    /// </summary>
    void RefreshMoneyText()
    {
        if (gm.playerData != null)
            myMoneyText.text = gm.playerData._playerMoney.ToString();
    }


    /// <summary>
    /// カード破棄画面を表示または非表示にするメソッド
    /// </summary>
    /// <param name="show">表示する場合はtrue、非表示にする場合はfalse</param>
    public void ToggleDiscardScreen(bool show, GameObject _getCard = null)
    {
        isShowingCardDiscard = true;
        DeckConfirmationButton.SetActive(false);    // デッキ確認アイコン非表示
        if (show)
        {
            // ボタンの状態リセット
            discardReturnButton.SetActive(true);
            discardButton.SetActive(false);

            // 前回表示したカードをDestroy
            foreach (Transform child in discardCardPlace.transform)
            {
                Destroy(child.gameObject);
            }

            // カード破棄画面を表示
            cardDiscardScreen.SetActive(true);

            // 取得するカードを表示
            getCard.Init(_getCard.GetComponent<CardController>().cardDataManager._cardID);        //デッキデータの表示

            DiscardCardDeck();
            UIEventsReload();
        }
        else
        {
            isShowingCardDiscard = false;
            DeckConfirmationButton.SetActive(true);       // デッキ確認アイコンを表示
            cardDiscardScreen.SetActive(false);
        }
    }

    /// <summary>
    /// デッキのカードをすべて表示します。
    /// </summary>
    private void InitDeck()
    {
        deckNumberList = GameManager.Instance.playerData._deckList;

        for (int init = 0; init < deckNumberList.Count; init++)         // 選択出来るデッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, deckCardPlace);   //カードを生成する
            card.transform.localScale = scaleReset;
            card.name = "Deck" + (init).ToString();                     //生成したカードに名前を付ける
            card.Init(deckNumberList[init]);                            //デッキデータの表示
        }
    }

    /// <summary>
    /// 破棄画面用のInitDeckです。
    /// 魔女の霊薬を除外してデッキのカードをすべて表示します。
    /// </summary>
    private void DiscardCardDeck()
    {
        List<int> discardDeck = gm.playerData._deckList.ToList();   // デッキチェック用にデッキをコピー

        // 魔女の霊薬を除外
        discardDeck.Remove(GameManager.healCardID);

        for (int init = 0; init < discardDeck.Count; init++)         // 選択出来るデッキの枚数分
        {
            //Debug.Log(discardDeck.Count);
            //Debug.Log(discardDeck[init]);
            CardController card = Instantiate(cardPrefab, discardCardPlace);   //カードを生成する
            card.transform.localScale = scaleReset;
            card.name = "Deck" + (init).ToString();                     //生成したカードに名前を付ける
            card.Init(discardDeck[init]);                            //デッキデータの表示
        }
    }

    /// <summary>
    /// 破棄するカードを表示するメソッドです
    /// </summary>
    /// <param name="selectCard">選択されたCard</param>
    public void DisplayDiscardCard(GameObject selectCard)
    {
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //選択されたカードのIDを取得
        discardCard.Init(id);                            //デッキデータの表示
    }

    /// <summary>
    /// 図鑑のカードを表示させるメソッドです。
    /// </summary>
    public void ShowCardPictureBook()
    {
        //List<int> showCardList = GameManager.Instance.gameSettings.collectedCardHistory;
        for(int cardNum = 1;  cardNum <=20; cardNum++)
        {
            CardController card = Instantiate(cardPrefab, cardPictureBookPlace);
            card.transform.localScale = scaleReset;
            card.name = "Deck" + (cardNum).ToString();                     //生成したカードに名前を付ける
            card.Init(cardNum);
            if(!GameManager.Instance.gameSettings.collectedCardHistory[cardNum])
            {
                card.transform.GetChild(6).transform.gameObject.SetActive(true);
            }
        }

        
    }

}