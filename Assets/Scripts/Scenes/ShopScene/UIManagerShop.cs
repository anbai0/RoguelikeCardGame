using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ShopSceneのUIManagerです。
/// ToDo: レリックがEnterされたときの処理はまだ書き終わってないので後でやります。
/// </summary>
public class UIManagerShop : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;

    private bool isClick = false;
    private GameObject lastClickedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // 元のスケールに戻すときに使います
    private Vector3 relicScaleReset = Vector3.one * 2.5f;
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    [Header("参照するスクリプト")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private RestController restController;

    [Header("表示を切り替えるUI")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject restUI;

    [Header("クリック後に参照するUI")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject closeShopping;
    [SerializeField] private GameObject restButton;
    [SerializeField] private GameObject takeRestButton;
    [SerializeField] private GameObject noRestButton;


    void Start()
    {
        restController.CheckRest("ShopScene");      // 休憩のテキスト更新
        UIEventsReload();
    }


    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        if(!isRemoved)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
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

        #region ShopUI内での処理

        // "購入"を押したら
        if (UIObject == buyButton)
        {
            shopUI.SetActive(false);
            shopController.PriceTextCheck();
            shopController.HasHealPotion();
        }
        // "店を出る"を押したら
        if (UIObject == UIObject.CompareTag("ExitButton"))
        {
            sceneController.SceneChange("FieldScene");
        }
        // "休憩"を押したら
        if (UIObject == restButton)
        {
            if(restController.CheckRest("ShopScene"))      //休憩できる場合
            {
                restUI.SetActive(true);

                restController.ChengeRestText("ShopScene");
            }
        }
        #endregion

        #region ShoppingUI内での処理

        // アイテムをクリックしたら
        if (UIObject == UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isClick = true;

            // アイテム選択状態の切り替え
            if (lastClickedItem != null && lastClickedItem != UIObject)    // 2回目のクリックかつクリックしたオブジェクトが違う場合   
            {
                // 最後にクリックしたアイテムの選択状態を解除する
                if (lastClickedItem == lastClickedItem.CompareTag("Cards"))
                {
                    lastClickedItem.transform.localScale = cardScaleReset;
                    lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
                }
                    
                if (lastClickedItem == lastClickedItem.CompareTag("Relics"))
                {
                    lastClickedItem.transform.localScale = relicScaleReset;
                    lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);
                    lastClickedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
                }

                // 2回目に選択したアイテムを選択状態にする
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);

                // 2回目に選択したアイテムがレリックだった場合、レリックの説明を表示
                if (UIObject == UIObject.CompareTag("Relics"))
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);

            }
            else if (UIObject == lastClickedItem)      // 同じアイテムを2回クリックしたら(アイテム購入)
            {
                // 選択したアイテムを買う
                if (UIObject == UIObject.CompareTag("Cards"))
                    shopController.BuyItem(UIObject, "Card");

                if (UIObject == UIObject.CompareTag("Relics"))
                    shopController.BuyItem(UIObject, "Relic");

                shopController.PriceTextCheck();            // 値段テキスト更新

                lastClickedItem = null;                     // 選択状態リセット
                isClick = false;
            }

            lastClickedItem = UIObject;

        }

        // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            // 最後にクリックしたアイテムの選択状態を解除する
            if (lastClickedItem == lastClickedItem.CompareTag("Cards"))
            {
                lastClickedItem.transform.localScale = cardScaleReset;
                lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
            }

            if (lastClickedItem == lastClickedItem.CompareTag("Relics"))
            {
                lastClickedItem.transform.localScale = relicScaleReset;
                lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);
            }

            lastClickedItem = null;         // 選択状態リセット
            isClick = false;
        }

        // "買い物を終える"を押したら
        if (UIObject == closeShopping)
        {
            shopUI.SetActive(true);
            restController.CheckRest("ShopScene");
        }
        #endregion

        #region RestUI内での処理

        // "休憩する"を押したら
        if (UIObject == takeRestButton)
        {
            restController.Rest("ShopScene");      // 回復する
            restController.CheckRest("ShopScene");
            restUI.SetActive(false);
        }
        // "休憩しない"を押したら
        if (UIObject == noRestButton)
        {
            restUI.SetActive(false);
        }

        #endregion
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // アイテムの見た目を選択状態にする
            }

            if (UIObject == UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // アイテムの見た目を選択状態にする
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);        // レリックの説明を表示
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する
            }

            if (UIObject == UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // アイテムの見た目の選択状態を解除する
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // レリックの説明を非表示
            }
        }
    }
}
