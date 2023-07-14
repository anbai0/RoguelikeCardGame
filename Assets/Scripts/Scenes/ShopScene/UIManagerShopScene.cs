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



    void Start()
    {
        UIEventReload();
        restUI.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void UIEventReload()
    {
        UIs = Canvas.GetComponentsInChildren<UIController>();       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
            //UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    void UILeftClick(GameObject UIObject)
    {

        #region ShopUI内での処理
        if (UIObject == buy)
        {
            shopUI.SetActive(false);
        }
        if (UIObject == CloseShopping)
        {
            shopUI.SetActive(true);
        }

        if (UIObject.tag == "ExitButton")
        {
            sceneController.sceneChange("FieldScene");
        }

        if (UIObject == rest)
        {
            restUI.SetActive(true);
        }
        if (UIObject == RestButton)
        {
            //回復する
            shopController.Rest();
        }
        if (UIObject == noRestButton)
        {
            restUI.SetActive(false);
        }

        #endregion

        #region ShoppingUI内での処理
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // カード選択状態の切り替え
            if (lastClickedCards != null && lastClickedCards != UIObject)              
            {
                lastClickedCards.transform.localScale = scaleReset;
                UIObject.transform.localScale += scaleBoost;
            }
            else if (UIObject == lastClickedCards)      // 同じカードを2回クリックしたら(カード購入)
            {
                shopController.BuyCards(UIObject);

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
        #endregion
    }

    void UIRightClick(GameObject UIObject)
    {

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
            Animator anim = UIObject.GetComponent<Animator>();
            anim.SetTrigger("RelicJump");
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
    }
}
