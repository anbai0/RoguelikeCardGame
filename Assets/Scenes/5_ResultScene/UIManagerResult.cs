using UnityEngine;
using TMPro;

// ほぼManagerSceneのUIManagerと処理が被っているので後でまとめます

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerResult : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    private GameManager gm;

    private Vector3 scaleReset = Vector3.one * 0.25f;     // 元のスケールに戻すときに使います
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // 元のスケールに乗算して使います

    [Header("参照するコンポーネント")]
    [SerializeField] ResultSceneManager resultSceneManager;

    [Header("表示を切り替えるUI")]
    [SerializeField] TextMeshProUGUI myMoneyText;   //所持金を表示するテキスト
    [SerializeField] GameObject titleBackButton;
    //[Header("クリック後に参照するUI")]



    void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;

        RefreshMoneyText();
        UIEventsReload();
    }


    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isEventsReset)             // イベントの初期化
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

    /// <summary>
    /// UIイベントを削除します。
    /// UIEventsReloadメソッド内で呼ばれます。
    /// </summary>
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


    /// <summary>
    /// 左クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UILeftClick(GameObject UIObject)
    {
        if (UIObject == titleBackButton && !isClick)
        {
            isClick = true;
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());
            AudioManager.Instance.PlaySE("選択音1");

            // タイトルに戻る処理
            resultSceneManager.SceneUnLoad();
        }
    }


    /// <summary>
    /// カーソルが触れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが触れたObject</param>
    void UIEnter(GameObject UIObject)
    {
        // カードを拡大
        if (UIObject.CompareTag("Cards"))
        {
            UIObject.transform.localScale += scaleBoost;
        }

        // レリックの説明を表示
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// カーソルが離れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが離れたObject</param>
    void UIExit(GameObject UIObject)
    {
        // カードを元のサイズに
        if (UIObject.CompareTag("Cards"))
        {
            UIObject.transform.localScale = scaleReset;
        }

        // レリックの説明を非表示
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 所持金のテキストを更新するメソッドです。
    /// </summary>
    void RefreshMoneyText()
    {
        if (gm.playerData != null)
            myMoneyText.text = gm.playerData._playerMoney.ToString();
    }
}