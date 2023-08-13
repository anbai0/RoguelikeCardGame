using UnityEngine;
using TMPro;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManager : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;

    [Header("参照するコンポーネント")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameManager gm;
    [Header("参照するUI")]
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] GameObject cardDiscardScreen;
    [Header("クリック後に参照するUI")]
    [SerializeField] GameObject overlayOptionButton;
    [SerializeField] GameObject titleOptionButton;
    [SerializeField] GameObject closeOptionButton;
    [SerializeField] GameObject titleBackButton;
    [SerializeField] GameObject closeConfirmButton;
    [SerializeField] GameObject confirmTitleBackButton;
    [SerializeField] GameObject returnButton;
    [SerializeField] GameObject discardButton;
    [Space(10)]
    [SerializeField] TextMeshProUGUI myMoneyText;   //所持金を表示するテキスト


    private bool isTitleScreen = false; // タイトル画面にいるときにtrueにする
    public static bool isShowingCardDiscard { get; private set; } = false;  // カード破棄画面を表示しているときだけtrue
    //public static bool cancelDiscard { get; private set; } = false;         // カード破棄画面で戻るボタンを押したときにtrue
    //public static bool discardButtonClicked { get; private set; } = false;  // カード破棄画面で破棄ボタンを押したときにtrue

    private bool isSelected = false;
    private GameObject lastSelectedCards;

    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    private int maxRelics = 12;

    [Header("カード表示関係")]
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    private Vector3 cardScale = Vector3.one * 0.25f;     // 生成するカードのスケール
    private List<int> deckNumberList;                    // プレイヤーのもつデッキナンバーのリスト

    void Start()
    {      
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
        if (!isRemoved)             // イベントの初期化
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));

        }

        isRemoved = false;
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
            Debug.Log(UIObject);
            optionScreen.SetActive(true);
        }

        // オプション画面非表示
        if (UIObject == closeOptionButton)
        {
            Debug.Log(UIObject);
            optionScreen.SetActive(false);
        }

        // タイトルへ戻るの確認画面表示
        if (UIObject == titleBackButton)
        {
            confirmationPanel.SetActive(true);
        }

        // タイトルへ戻るの確認画面非表示
        if (UIObject == closeConfirmButton)
        {
            confirmationPanel.SetActive(false);
        }

        if (UIObject == confirmTitleBackButton)
        {
            // タイトルへ戻る処理

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

                // ボタン切り替え
                returnButton.SetActive(false);
                discardButton.SetActive(true);

                // カード選択状態の切り替え
                if (lastSelectedCards != null && lastSelectedCards != UIObject)              // 二回目のクリックかつクリックしたオブジェクトが違う場合   
                {
                    lastSelectedCards.transform.localScale = cardScale;
                    UIObject.transform.localScale += scaleBoost;
                }

                lastSelectedCards = UIObject;

            }

            // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
            if (isSelected && UIObject.CompareTag("BackGround"))
            {
                lastSelectedCards.transform.localScale = cardScale;
                lastSelectedCards = null;
                isSelected = false;

                // 強化ボタン切り替え
                returnButton.SetActive(true);
                discardButton.SetActive(false);

            }

            // カード破棄画面を非表示に
            if(UIObject == returnButton)
            {
                ToggleDiscardScreen(false);
            }

            // カードを選んだ後、破棄ボタンを押すと、そのカードを破棄
            if (UIObject == discardButton && lastSelectedCards != null)
            {
                discardButtonClicked = true;

                int selectedCardID = lastSelectedCards.GetComponent<CardController>().cardDataManager._cardID; // 選択されたカードのIDを取得

                for (int cardIndex = 0; cardIndex < gm.playerData._deckList.Count; cardIndex++) {

                    if (gm.playerData._deckList[cardIndex] == selectedCardID)
                    {
                        // カードを破棄
                        gm.playerData._deckList.RemoveAt(cardIndex);
                    }
                }
                ToggleDiscardScreen(false);
            }
        
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
            UIObject.transform.GetChild(5).gameObject.SetActive(true);
        }


        // カード破棄画面が表示されているとき
        if (isShowingCardDiscard && !isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
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
        if (isShowingCardDiscard && !isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScale;
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
    public void ToggleDiscardScreen(bool show)
    {
        isShowingCardDiscard = true;
        if (show)
        {
            // ボタンの状態リセット
            returnButton.SetActive(true);
            discardButton.SetActive(false);

            // 前回表示したカードをDestroy
            foreach (Transform child in upperCardPlace.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in lowerCardPlace.transform)
            {
                Destroy(child.gameObject);
            }

            cardDiscardScreen.SetActive(true);
            ShowDeck();
            UIEventsReload();
        }
        else
        {
            isShowingCardDiscard = false;
            cardDiscardScreen.SetActive(false);
        }
    }

    /// <summary>
    /// 持っているカードをすべて表示
    /// </summary>
    private void ShowDeck()
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
                card.transform.localScale = cardScale;
                card.name = "Deck" + (init - 1).ToString();//生成したカードに名前を付ける
                card.Init(deckNumberList[init - 1]);//デッキデータの表示
            }
            else //残りはlowerCardPlaceに生成する
            {
                CardController card = Instantiate(cardPrefab, lowerCardPlace);//カードを生成する
                card.transform.localScale = cardScale;
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
}