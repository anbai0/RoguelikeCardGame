using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField]
    private ShopController shopController;

    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;

    Vector3 defaultScale = Vector3.one * 0.37f;

    GameObject lastClickedCards;

    void Start()
    {
        UIEventReload();
    }

    public void UIEventReload()
    {
        UIs = Canvas.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    void UILeftClick(GameObject UIObject)
    {
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // カード選択状態の切り替え
            if (lastClickedCards != null && lastClickedCards != UIObject)              
            {
                lastClickedCards.transform.localScale = defaultScale;
                UIObject.transform.localScale += Vector3.one * 0.1f;
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
            lastClickedCards.transform.localScale = defaultScale;
            lastClickedCards = null;
            isClick = false;
        }
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
                UIObject.transform.localScale += Vector3.one * 0.1f;
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
                UIObject.transform.localScale = defaultScale;
            }
        }
    }
}
