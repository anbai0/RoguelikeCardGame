using System.Collections;
using UnityEngine;
using SelfMadeNamespace;

public class UIManagerTreasureBox : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;
    private bool isClick = false;

    private bool isSelected = false;
    private GameObject lastSelectedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // カードを元のスケールに戻すときに使います
    private Vector3 relicScaleReset = Vector3.one * 2.5f;    // レリックを元のスケールに戻すときに使います
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    private bool isDisplayRelics = true; //レリックの報酬を表示するか判定

    [Header("参照するコンポーネント")]
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private TreasureBoxManager TBManager;

    [Header("クリック後に参照するUI")]
    [SerializeField] private GameObject treasureBoxUI;
    [SerializeField] private GameObject treasureCardPlace;
    [SerializeField] private GameObject treasureRelicPlace;
    [SerializeField] private GameObject applyGetTreasure;
    [SerializeField] private GameObject closeGetTreasure;


    void Start()
    {
        UIEventsReload();
    }


    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isRemoved)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                                // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        isRemoved = false;
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

            // 入手ボタン切り替え
            applyGetTreasure.SetActive(true);
            closeGetTreasure.SetActive(false);
            //UIEventsReload();

            // カード選択状態の切り替え
            // アイテム選択状態の切り替え
            if (lastSelectedItem != null && lastSelectedItem != UIObject)    // 2回目のクリックかつクリックしたオブジェクトが違う場合   
            {
                // 最後にクリックしたアイテムの選択状態を解除する
                if (lastSelectedItem.CompareTag("Cards"))
                {
                    lastSelectedItem.transform.localScale = cardScaleReset;
                    lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
                }

                if (lastSelectedItem.CompareTag("Relics"))
                {
                    lastSelectedItem.transform.localScale = relicScaleReset;
                    lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                    lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
                }

                // 2回目に選択したアイテムがカードだった場合、カードを選択状態にする
                if (UIObject.CompareTag("Cards"))
                {
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.Find("CardSelectImage").gameObject.SetActive(true);
                }

                // 2回目に選択したアイテムがレリックだった場合、レリックを選択状態にして説明を表示
                if (UIObject.CompareTag("Relics"))
                {
                    UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(true);
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);
                }

            }

            lastSelectedItem = UIObject;

        }

        // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
        if (isSelected && UIObject.CompareTag("BackGround"))
        {
            // 最後にクリックしたアイテムの選択状態を解除する
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
            }
            lastSelectedItem = null;
            isSelected = false;

            // 入手ボタン切り替え
            applyGetTreasure.SetActive(false);
            closeGetTreasure.SetActive(true);
            //UIEventsReload();
        }

        // "入手する"を押したら
        if (UIObject == applyGetTreasure && isSelected && !isClick)
        {
            isClick = true;
            //if (UIObject.CompareTag("Cards"))
            //{
            //    var cardID = UIObject.GetComponent<CardController>().cardDataManager._cardID;        //デッキリストにカードを追加する
            //    GameManager.Instance.playerData._deckList.Add(cardID);
            //}
            //if (UIObject.CompareTag("Relics"))
            //{
            //    var relicID = UIObject.GetComponent<RelicController>().relicDataManager._relicID;      //レリックリストにレリックを追加する
            //    GameManager.Instance.hasRelics[relicID] += 1;
            //}

            // 最後にクリックしたアイテムの選択状態を解除する
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
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
                //treasureChestUI.SetActive(false);
                Debug.Log("フィールドシーンへ移行");
                TBManager.UnLoadTreasureBoxScene(); // フィールドに戻る
                PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
                PlayerController.isPlayerActive = true; // プレイヤーを動けるようにする
                playerController.treasureBox.SetActive(false); //宝箱を消す
            }
        }

        // "入手しない"を押したら
        if (UIObject == closeGetTreasure)
        {
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
                PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
                PlayerController.isPlayerActive = true; // プレイヤーを動けるようにする
                playerController.treasureBox.SetActive(false); //宝箱を消す
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

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.Find("CardSelectImage").gameObject.SetActive(true);              // アイテムの見た目を選択状態にする
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(true);                  // アイテムの見た目を選択状態にする
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);                     // レリックの説明を表示
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScaleReset;
                UIObject.transform.Find("CardSelectImage").gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する

            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(false);                 // アイテムの見た目の選択状態を解除する
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);                    // レリックの説明を非表示
            }
        }
    }
}
