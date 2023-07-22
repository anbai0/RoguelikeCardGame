using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BonfireSceneのUIManagerです。
/// ToDo: レリックがEnterされたときの処理はまだ書き終わってないので後でやります。
/// </summary>
public class UIManagerBonfire : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;
    GameObject lastClickedCards;

    Vector3 scaleReset = Vector3.one * 0.25f;    // 元のスケールに戻すときに使います
    Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    [Header("参照するスクリプト")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private RestController restController;
    [SerializeField] private BonfireManager bonfireManager;

    [Header("表示を切り替えるUI")]
    [SerializeField] GameObject BonfireUI;
    [SerializeField] GameObject restUI;

    [Header("クリック後に参照するUI")]
    [SerializeField] GameObject enhanceButton;
    [SerializeField] GameObject applyEnhance;
    [SerializeField] GameObject closeEnhance;
    [SerializeField] GameObject restButton;
    [SerializeField] GameObject takeRestButton;
    [SerializeField] GameObject noRestButton;

    bool isRemoved = true;

    void Start()
    {
        restController.CheckRest("BonfireScene");
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

        #region BonfireUI内での処理

        // "強化"を押したら
        if (UIObject == enhanceButton)
        {
            BonfireUI.SetActive(false);     // 強化画面に行く
        }

        // "休憩しない"を押したら
        if (UIObject == UIObject.CompareTag("ExitButton"))
        {
            sceneController.sceneChange("FieldScene");          // フィールドに戻る
        }

        // "休憩"を押したら
        if (UIObject == restButton)
        {
            if(restController.CheckRest("BonfireScene"))            //休憩できる場合
            {
                restUI.SetActive(true);                             // 休憩UIを表示

                restController.ChengeRestText("BonfireScene");      // 休憩textを更新
                UIEventReload();
            }
        }
        #endregion

        #region EnhanceUI内での処理

        // カードをクリックしたら
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // 強化ボタン切り替え
            applyEnhance.SetActive(true);
            closeEnhance.SetActive(false);
            UIEventReload();

            // カード選択状態の切り替え
            if (lastClickedCards != null && lastClickedCards != UIObject)              // 二回目のクリックかつクリックしたオブジェクトが違う場合   
            {
                lastClickedCards.transform.localScale = scaleReset;
                UIObject.transform.localScale += scaleBoost;
            }

            lastClickedCards = UIObject;

        }

        // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            lastClickedCards.transform.localScale = scaleReset;
            lastClickedCards = null;
            isClick = false;

            // 強化ボタン切り替え
            applyEnhance.SetActive(false);
            closeEnhance.SetActive(true);
            UIEventReload();
        }

        // "強化する"を押したら
        if (UIObject == applyEnhance && isClick)
        {
            bonfireManager.CardEnhance(lastClickedCards);   // ボタンを前にクリックしたCardを引数に

            sceneController.sceneChange("FieldScene");      // 強化したらフィールドに戻る
        }

        // "強化しない"を押したら
        if (UIObject == closeEnhance)
        {
            BonfireUI.SetActive(true);          // 焚火画面に戻る
            UIEventReload();
        }
        #endregion

        #region RestUI内での処理

        // "休憩する"を押したら
        if (UIObject == takeRestButton)
        {
            restController.Rest("BonfireScene");                // 回復する
            restUI.SetActive(false);
            sceneController.sceneChange("FieldScene");          // フィールドに戻る
        }
        // "休憩しない"を押したら
        if (UIObject == noRestButton)
        {
            restUI.SetActive(false);        // 焚火画面に戻る
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
