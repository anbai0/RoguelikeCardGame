using UnityEngine;
using SelfMadeNamespace;

/// <summary>
/// BonfireSceneのUIManagerです。
/// ToDo: レリックがEnterされたときの処理はまだ書き終わってないので後でやります。
/// </summary>
public class UIManagerBonfire : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    private bool isSelected = false;
    private GameObject lastSelectedCards;

    private Vector3 scaleReset = Vector3.one * 0.25f;     // 元のスケールに戻すときに使います
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    [Header("参照するコンポーネント")]
    [SerializeField] private RestController restController;
    [SerializeField] private BonfireManager bonfireManager;

    [Header("表示を切り替えるUI")]
    [SerializeField] private GameObject BonfireUI;
    [SerializeField] private GameObject restUI;

    [Header("クリック後に参照するUI")]
    [SerializeField] private GameObject enhanceButton;
    [SerializeField] private GameObject applyEnhance;
    [SerializeField] private GameObject closeEnhance;
    [SerializeField] private GameObject restButton;
    [SerializeField] private GameObject takeRestButton;
    [SerializeField] private GameObject noRestButton;


    void Start()
    {
        restController.CheckRest("BonfireScene");
        UIEventsReload();
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

        #region BonfireUI内での処理

        // "強化"を押したら
        if (UIObject == enhanceButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            BonfireUI.SetActive(false);     // 強化画面に行く
        }

        // "休憩しない"を押したら
        if (UIObject.CompareTag("ExitButton") && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("選択音1");
            bonfireManager.UnLoadBonfireScene();          // フィールドに戻る
            PlayerController.isPlayerActive = true;       // プレイヤーを動けるようにする
        }

        // "休憩"を押したら
        if (UIObject == restButton)
        {
            if(restController.CheckRest("BonfireScene"))            //休憩できる場合
            {
                restUI.SetActive(true);                             // 休憩UIを表示

                restController.ChengeRestText("BonfireScene");      // 休憩textを更新
                AudioManager.Instance.PlaySE("選択音1");
            }
        }
        #endregion

        #region EnhanceUI内での処理

        // カードをクリックしたら
        if (UIObject.CompareTag("Cards"))
        {
            isSelected = true;

            // 強化ボタン切り替え
            applyEnhance.SetActive(true);
            closeEnhance.SetActive(false);

            // カード選択状態の切り替え
            if (lastSelectedCards != null && lastSelectedCards != UIObject)              // 二回目のクリックかつクリックしたオブジェクトが違う場合   
            {
                lastSelectedCards.transform.localScale = scaleReset;
                lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);
            }

            lastSelectedCards = UIObject;

        }

        // カードをクリックした後、背景をクリックするとカードのクリック状態を解く
        if (isSelected && UIObject.CompareTag("BackGround"))
        {
            lastSelectedCards.transform.localScale = scaleReset;
            lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // アイテムの見た目の選択状態を解除する
            lastSelectedCards = null;
            isSelected = false;

            // 強化ボタン切り替え
            applyEnhance.SetActive(false);
            closeEnhance.SetActive(true);
            
        }

        // "強化する"を押したら
        if (UIObject == applyEnhance && isSelected && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("選択音1");

            bonfireManager.CardEnhance(lastSelectedCards);   // カード強化

            PutOutCampfire();       // 焚火の火を消す

            bonfireManager.UnLoadBonfireScene();      // 強化したらフィールドに戻る
            PlayerController.isPlayerActive = true;       // プレイヤーを動けるようにする
        }

        // "強化しない"を押したら
        if (UIObject == closeEnhance)
        {
            AudioManager.Instance.PlaySE("選択音1");
            BonfireUI.SetActive(true);          // 焚火画面に戻る
            
        }
        #endregion

        #region RestUI内での処理

        // "休憩する"を押したら
        if (UIObject == takeRestButton && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("回復");

            restController.Rest("BonfireScene");                // 回復する
            restUI.SetActive(false);

            PutOutCampfire();       // 焚火の火を消す

            bonfireManager.UnLoadBonfireScene();          // 休憩したらフィールドに戻る
            PlayerController.isPlayerActive = true;       // プレイヤーを動けるようにする
        }
        // "休憩しない"を押したら
        if (UIObject == noRestButton)
        {
            AudioManager.Instance.PlaySE("選択音1");
            restUI.SetActive(false);        // 焚火画面に戻る
        }

        #endregion
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
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = scaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // アイテムの見た目の選択状態を解除する
            }
        }
    }


    /// <summary>
    /// 焚火の火と当たり判定を消します。
    /// </summary>
    void PutOutCampfire()
    {
        PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();

        // 焚火の火を消す
        //ParticleSystem particle = playerController.bonfirePrefab.GetComponent<ParticleSystem>();
        //particle.Stop();

        // 焚火の当たり判定を消す。
        BoxCollider boxCol = playerController.bonfire.GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }
}