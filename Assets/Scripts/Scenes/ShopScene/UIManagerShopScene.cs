using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    [SerializeField] private ShopController shopController;

    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;
    GameObject lastClickedCards;

    Vector3 scaleReset = Vector3.one * 0.37f;    // 元のスケールに戻すときに使います
    Vector3 scaleBoost = Vector3.one * 0.1f;     // 元のスケールに乗算して使います

    // 切り替えるUI
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject restUI;
    // クリック後に参照するオブジェクト
    [SerializeField] GameObject buy;
    [SerializeField] GameObject CloseShopping;
    [SerializeField] GameObject rest;
    [SerializeField] GameObject RestButton;
    [SerializeField] GameObject noRestButton;

    bool isRemoved = true;

    void Start()
    {
        shopController.CheckRest();
        UIEventReload();
    }

    public void UIEventReload()
    {
        if(!isRemoved)
            RemoveListeners();

        UIs = Canvas.GetComponentsInChildren<UIController>();       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
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

    void UILeftClick(GameObject UIObject)
    {

        #region ShopUI内での処理

        // "購入"を押したら
        if (UIObject == buy)
        {
            shopUI.SetActive(false);
            shopController.PriceTextCheck();
            shopController.HasHealPotion();
        }
        // "店を出る"を押したら
        if (UIObject.tag == "ExitButton")
        {
            sceneController.sceneChange("FieldScene");
        }
        // "休憩"を押したら
        if (UIObject == rest)
        {
            if(shopController.CheckRest())      //休憩できる場合
            {
                restUI.SetActive(true);
                UIEventReload();
            }
        }
        #endregion

        #region ShoppingUI内での処理

        // カードをクリックしたら
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // カード選択状態の切り替え
            if (lastClickedCards != null && lastClickedCards != UIObject)              // 二回目のクリックかつクリックしたオブジェクトが違う場合   
            {
                lastClickedCards.transform.localScale = scaleReset;
                UIObject.transform.localScale += scaleBoost;
            }
            else if (UIObject == lastClickedCards)      // 同じカードを2回クリックしたら(カード購入)
            {
                shopController.BuyItem(UIObject, "Card");
                shopController.PriceTextCheck();
            }

            lastClickedCards = UIObject;

        }

        // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            lastClickedCards.transform.localScale = scaleReset;
            lastClickedCards = null;
            isClick = false;
        }

        // いったんこれ
        if (UIObject == UIObject.CompareTag("Relics"))
        {
            shopController.BuyItem(UIObject, "Relic");
            shopController.PriceTextCheck();
        }

        // 買い物を終えるボタンを押したら
        if (UIObject == CloseShopping)
        {
            shopUI.SetActive(true);
            shopController.CheckRest();
        }
        #endregion

        #region RestUI内での処理

        // "休憩する"を押したら
        if (UIObject == RestButton)
        {
            shopController.Rest();      // 回復する
            shopController.CheckRest();
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
            }
        }

        if (UIObject == UIObject.CompareTag("Relics"))
        {
            UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);
        }

    }

    void UIExit(GameObject UIObject)
    {
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = scaleReset;
            }
        }

        if (UIObject == UIObject.CompareTag("Relics"))
        {
            UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);
        }
    }
}
