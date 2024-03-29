using UnityEngine;


/// <summary>
/// ShopSceneのUIManagerです。
/// ToDo: レリックがEnterされたときの処理はまだ書き終わってないので後でやります。
/// </summary>
public class UIManagerShop : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    public bool isSelected = false;
    public GameObject lastSelectedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // 元のスケールに戻すときに使います
    private Vector3 relicScaleReset = Vector3.one * 2.5f;
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    [Header("参照するコンポーネント")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] public RestController restController;

    [Header("表示を切り替えるUI")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject restUI;

    [Header("クリック後に参照するUI")]
    [SerializeField] private GameObject closeShop;
    [SerializeField] private GameObject shoppingButton;
    [SerializeField] private GameObject closeShopping;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject restButton;
    [SerializeField] private GameObject takeRestButton;
    [SerializeField] private GameObject noRestButton;


    void Start()
    {
        restController.CheckRest("ShopScene");      // 休憩のテキスト更新
        UIEventsReload();
    }

    private void Update()
    {
        // ショップシーンがアクティブになってるとき
        if (shopUI.activeSelf)
        {
            restController.CheckRest("ShopScene");      // 休憩のテキスト更新
        }
    }

    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        if(!isEventsReset)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
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

        #region ShopUI内での処理

        // "購入"を押したらshopping画面に遷移
        if (UIObject == shoppingButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            shopUI.SetActive(false);
            shopManager.PriceTextCheck();
            shopManager.HasHealPotion();
        }
        // "店を出る"を押したら
        if (UIObject == closeShop && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("選択音1");
            shopManager.ExitShop();     // ShopSceneを非表示

            isClick = false;
        }
        // "休憩"を押したら
        if (UIObject == restButton)
        {
            if(restController.CheckRest("ShopScene"))      //休憩できる場合
            {
                restUI.SetActive(true);

                AudioManager.Instance.PlaySE("選択音1");
            }
        }
        #endregion

        #region ShoppingUI内での処理
        // アイテムをクリックしたら
        if (UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isSelected = true;
            AudioManager.Instance.PlaySE("選択音1");

            // アイテム選択状態の切り替え
            if (lastSelectedItem != null && lastSelectedItem != UIObject)    // 2回目のクリックかつクリックしたオブジェクトが違う場合   
            {
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
                    lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
                }


                // 2回目に選択したアイテムを選択状態にする
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);

                // 2回目に選択したアイテムがレリックだった場合、レリックの説明を表示
                if (UIObject.CompareTag("Relics"))
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);


                lastSelectedItem = UIObject;    // 最後にクリックしたアイテムを更新
            }


            // 一度も選択されてなかった場合
            if (lastSelectedItem == null)
            {
                lastSelectedItem = UIObject;    // 最後にクリックしたアイテムを更新
            }

            // 何かが選択されている場合
            if (lastSelectedItem != null)
            {
                if (lastSelectedItem.CompareTag("Cards"))
                {
                    if (shopManager.IsItemBuyable(lastSelectedItem, "Cards"))
                    {
                        buyButton.SetActive(true);
                    }
                    else
                    {
                        buyButton.SetActive(false);
                    }
                }

                if (lastSelectedItem.CompareTag("Relics"))
                {
                    if (shopManager.IsItemBuyable(lastSelectedItem, "Relics"))
                    {
                        buyButton.SetActive(true);
                    }
                    else
                    {
                        buyButton.SetActive(false);
                    }
                } 
            } 
        }

        // アイテム購入
        if (UIObject == buyButton && lastSelectedItem != null)
        {
            // 選択したアイテムを買う
            if (lastSelectedItem.CompareTag("Cards"))
                shopManager.BuyItem(lastSelectedItem, "Card");

            if (lastSelectedItem.CompareTag("Relics"))
                shopManager.BuyItem(lastSelectedItem, "Relic");

            ResetItemSelection();
        }

        // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
        if (isSelected && UIObject.CompareTag("BackGround"))
        {
            ResetItemSelection();
        }

        // "買い物を終える"を押したら
        if (UIObject == closeShopping)
        {
            AudioManager.Instance.PlaySE("選択音1");
            ResetItemSelection();
            shopUI.SetActive(true);
            restController.CheckRest("ShopScene");
        }


        #endregion

        #region RestUI内での処理

        // "休憩する"を押したら
        if (UIObject == takeRestButton)
        {
            AudioManager.Instance.PlaySE("回復");
            restController.Rest("ShopScene");      // 回復する
            restController.CheckRest("ShopScene");
            restUI.SetActive(false);
        }
        // "休憩しない"を押したら
        if (UIObject == noRestButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            restUI.SetActive(false);
        }

        #endregion
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                AudioManager.Instance.PlaySE("OnCursor");
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // アイテムの見た目を選択状態にする
            }

            if (UIObject.CompareTag("Relics"))
            {
                AudioManager.Instance.PlaySE("OnCursor");
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // アイテムの見た目を選択状態にする
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);        // レリックの説明を表示
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
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // アイテムの見た目の選択状態を解除する
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
            }
        }
    }

    /// <summary>
    /// アイテムの選択状態をすべて解除するメソッドです。
    /// </summary>
    void ResetItemSelection()
    {
        if (lastSelectedItem == null) return; 

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
            lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
        }

        lastSelectedItem = null;         // 選択状態リセット
        isSelected = false;
        buyButton.SetActive(false);
    }
}
