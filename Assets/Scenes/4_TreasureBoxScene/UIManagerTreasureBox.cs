using System.Collections;
using UnityEngine;
using SelfMadeNamespace;

public class UIManagerTreasureBox : MonoBehaviour
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

    private bool isDisplayRelics = true; //レリックの報酬を表示するか判定

    [Header("参照するコンポーネント")]
    [SerializeField] private TreasureBoxManager TBManager;

    [Header("クリック後に参照するUI")]
    [SerializeField] private GameObject treasureBoxUI;
    [SerializeField] private GameObject treasureCardPlace;
    [SerializeField] private GameObject treasureRelicPlace;
    [SerializeField] private GameObject applyGetTreasure;
    [SerializeField] private GameObject closeGetTreasure;

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

        #region TreasureBoxUI内での処理

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
                lastSelectedItem.transform.GetChild(8).gameObject.SetActive(false);                    // RelicEffectBG(BattleReward)を非表示
            }

            if (lastSelectedItem.CompareTag("Cards"))
            {
                if (gm.CheckDeckFull(lastSelectedItem)) //デッキの上限に達している場合
                {
                    //破棄画面を呼び出し
                    gm.OnCardDiscard += ReGetTreasure;
                    // 入手ボタン切り替え
                    applyGetTreasure.SetActive(false);
                    closeGetTreasure.SetActive(true);
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

            //レリックの報酬も必要なら
            if (isDisplayRelics)
            {
                isClick = false; //applyGetItemをもう一度クリック出来るようにする
                StartCoroutine(ShowRelicTreasure());
                isDisplayRelics = false;
            }
            else
            {
                Debug.Log("フィールドシーンへ移行");
                TBManager.UnLoadTreasureBoxScene(); // フィールドに戻る
                ExitTreasureBox();
            }

        }

        // "入手しない"を押したら
        if (UIObject == closeGetTreasure)
        {
            AudioManager.Instance.PlaySE("選択音1");
            //レリックの報酬も必要なら
            if (isDisplayRelics)
            {
                isClick = false; //applyGetItemをもう一度クリック出来るようにする
                StartCoroutine(ShowRelicTreasure());
                isDisplayRelics = false;
            }
            else
            {
                Debug.Log("フィールドシーンへ移行");
                TBManager.UnLoadTreasureBoxScene(); // フィールドに戻る
                ExitTreasureBox();
            }
        }

        #endregion
    }

    IEnumerator ShowRelicTreasure()
    {
        // 入手ボタン切り替え
        applyGetTreasure.SetActive(false);
        closeGetTreasure.SetActive(true);
        //報酬画面切り替え
        treasureRelicPlace.SetActive(true);
        treasureCardPlace.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        //画面のポップアップ
        treasureBoxUI.SetActive(true);
    }

    /// <summary>
    ///  デッキ破棄画面が呼ばれた際にもう一度宝箱獲得画面を表示
    /// </summary>
    void ReGetTreasure()
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
            isClick = false; //applyGetItemをもう一度クリック出来るようにする
            StartCoroutine(ShowRelicTreasure());
            isDisplayRelics = false;
        }
        else
        {
            Debug.Log("フィールドシーンへ移行");
            TBManager.UnLoadTreasureBoxScene(); // フィールドに戻る
            ExitTreasureBox();
        }
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // アイテムの見た目を選択状態にする
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // アイテムの見た目を選択状態にする
                UIObject.transform.GetChild(8).gameObject.SetActive(true);                    // RelicEffectBG(BattleReward)を表示
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                AudioManager.Instance.PlaySE("OnCursor");
                UIObject.transform.localScale = cardScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する

            }

            if (UIObject.CompareTag("Relics"))
            {
                AudioManager.Instance.PlaySE("OnCursor");
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // アイテムの見た目の選択状態を解除する
                UIObject.transform.GetChild(8).gameObject.SetActive(false);                    // RelicEffectBG(BattleReward)を非表示
            }
        }
    }

    void ExitTreasureBox()
    {
        Destroy(PlayerController.Instance.treasureBox);         //宝箱を消す
        Destroy(PlayerController.Instance.treasureBoxIcon);     // マップアイコンを削除
        PlayerController.Instance.playerIcon.SetActive(true);   // プレイヤーアイコンを表示
    }
}