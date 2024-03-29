using System.Collections;
using UnityEngine;

/// <summary>
/// BattleReward画面のUIManagerです。
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerBattleReward : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    private bool isSelected = false;
    private GameObject lastSelectedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.25f;    // カードを元のスケールに戻すときに使います
    private Vector3 relicScaleReset = Vector3.one * 2.5f;    // レリックを元のスケールに戻すときに使います
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    public bool isDisplayRelics = false; //レリックの報酬を表示するか判定

    [Header("参照するコンポーネント")]
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private BattleRewardManager battleRewardManager;

    [Header("クリック後に参照するUI")]
    [SerializeField] private GameObject battleRewardUI;
    [SerializeField] private GameObject cardRewardPlace;
    [SerializeField] private GameObject relicRewardPlace;
    [SerializeField] private GameObject applyGetItem;
    [SerializeField] private GameObject closeGetItem;

    GameManager gm;

    void Start()
    {
        UIEventsReload();
        gm = GameManager.Instance;
    }

    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isEventsReset)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                                // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        isEventsReset = false;
    }

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


    void UILeftClick(GameObject UIObject)
    {

        #region BattleRewardUI内での処理

        // アイテムをクリックしたら
        if (UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isSelected = true;
            isClick = true;
            lastSelectedItem = UIObject;
            AudioManager.Instance.PlaySE("選択音1");

            // 最後にクリックしたアイテムの選択状態を解除する
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
                lastSelectedItem.transform.GetChild(8).gameObject.SetActive(false);       // RelicEffectBG(BattleReward)を非表示
            }

            if (lastSelectedItem.CompareTag("Cards"))
            {
                if (gm.CheckDeckFull(lastSelectedItem)) //デッキの上限に達している場合
                {
                    //破棄画面を呼び出し
                    gm.OnCardDiscard += ReGetReward;
                    // 入手ボタン切り替え
                    applyGetItem.SetActive(false);
                    closeGetItem.SetActive(true);
                    isClick = false; //applyGetItemをもう一度クリック出来るようにする
                    isSelected = false; //もし破棄画面から戻るを押された際にもう一度選択出来るようにする
                    return;
                }
                else
                {
                    var cardID = lastSelectedItem.GetComponent<CardController>().cardDataManager._cardID;        //デッキリストにカードを追加する
                    gm.AddCard(cardID);
                }
            }
            if (lastSelectedItem.CompareTag("Relics"))
            {
                var relicID = lastSelectedItem.GetComponent<RelicController>().relicDataManager._relicID;      //レリックリストにレリックを追加する
                gm.hasRelics[relicID]++;
                gm.ShowRelics();
                gm.CheckGetRelicID7();
            }

            lastSelectedItem = null;
            isSelected = false;

            //レリック取得画面に移動
            if (isDisplayRelics)
            {
                battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //報酬画面を閉じる
                isClick = false; //applyGetItemをもう一度クリック出来るようにする
                StartCoroutine(ShowRelicReward());
                isDisplayRelics = false;
            }
            else
            {
                battleRewardManager.UnLoadBattleScene();      // フィールドに戻る
            }
        }

        // "入手しない"を押したら
        if (UIObject == closeGetItem)
        {
            AudioManager.Instance.PlaySE("選択音1");

            //レリック取得画面に移動
            if (isDisplayRelics)
            {
                battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //報酬画面を閉じる
                isClick = false; //applyGetItemをもう一度クリック出来るようにする
                StartCoroutine(ShowRelicReward());
                isDisplayRelics = false;
            }
            else
            {
                battleRewardManager.UnLoadBattleScene();      // フィールドに戻る
            }
        }

        #endregion
    }

    IEnumerator ShowRelicReward()
    {
        // 入手ボタン切り替え
        applyGetItem.SetActive(false);
        closeGetItem.SetActive(true);
        //報酬画面切り替え
        relicRewardPlace.SetActive(true);
        cardRewardPlace.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        //画面のポップアップ
        battleRewardUI.GetComponent<DisplayAnimation>().StartPopUPAnimation();
    }

    /// <summary>
    /// デッキ破棄画面が呼ばれた際にもう一度報酬画面を表示
    /// </summary>
    void ReGetReward()
    {
        if (lastSelectedItem.CompareTag("Cards"))
        {
            var cardID = lastSelectedItem.GetComponent<CardController>().cardDataManager._cardID;        //デッキリストにカードを追加する
            gm.AddCard(cardID);
        }

        if (lastSelectedItem.CompareTag("Relics"))
        {
            var relicID = lastSelectedItem.GetComponent<RelicController>().relicDataManager._relicID;      //レリックリストにレリックを追加する
            gm.hasRelics[relicID]++;
            gm.ShowRelics();
            gm.CheckGetRelicID7();
        }

        lastSelectedItem = null;
        isSelected = false;

        //レリックの報酬も必要なら
        if (isDisplayRelics)
        {
            battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //報酬画面を閉じる
            isClick = false; //applyGetItemをもう一度クリック出来るようにする
            StartCoroutine(ShowRelicReward());
            isDisplayRelics = false;
        }
        else
        {
            battleRewardManager.UnLoadBattleScene();      // フィールドに戻る
        }
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                if (UIObject.GetComponent<CardController>().cardDataManager._cardState == -1)             //報酬用のカードだったら
                {
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.GetChild(0).gameObject.SetActive(true);              // アイテムの見た目を選択状態にする
                }
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // アイテムの見た目を選択状態にする
                UIObject.transform.GetChild(8).gameObject.SetActive(true);                  // RelicEffectBG(BattleReward)を表示
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                if (UIObject.GetComponent<CardController>().cardDataManager._cardState == -1)           //報酬用のカードだったら
                {
                    UIObject.transform.localScale = cardScaleReset;
                    UIObject.transform.GetChild(0).gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する
                }

            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // アイテムの見た目の選択状態を解除する
                UIObject.transform.GetChild(8).gameObject.SetActive(false);                 // RelicEffectBG(BattleReward)を非表示
            }
        }
    }
}
