using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManager : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [Header("Canvasをアタッチ")]
    [SerializeField] private GameObject parent;
    private UIController[] UIs;
    bool isRemoved = true;

    [Header("参照するUI")]
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject confirmationPanel;
    [Space (10)]
    [SerializeField] GameObject overlayOptionButton;
    [SerializeField] GameObject titleOptionButton;
    [SerializeField] GameObject closeOptionButton;
    [SerializeField] GameObject titleBackButton;
    [SerializeField] GameObject closeConfirmButton;
    [SerializeField] GameObject confirmTitleBackButton;
    [Space (10)]
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    [SerializeField] AudioManager audioManager;
    [SerializeField] GameManager gameManager;

    bool isTitleScreen = false; // タイトル画面にいるときにtrueにする
    int maxRelics = 12;

    void Start()
    {
        UIEventsReload();
        //ChangeUI("OverlayOnly");
        ShowRelics();
    }

    /// <summary>
    /// UIの表示、再表示を行います。
    /// </summary>
    void UIEventsReload()
    {
        if (!isRemoved)             // イベントの初期化
            RemoveListeners();

        UIs = parent.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));

        }

        isRemoved = false;
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


    /// <summary>
    /// 左クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UILeftClick(GameObject UIObject)
    {
        Debug.Log(UIObject.name);

        // オプション画面表示
        if (UIObject == (overlayOptionButton || titleOptionButton))
        {
            optionScreen.SetActive(true);
            UIEventsReload();
        }

        // オプション画面非表示
        if (UIObject == closeOptionButton)
        {
            optionScreen.SetActive(false);
        }

        // タイトルへ戻るの確認画面表示
        if (UIObject == titleBackButton)
        {
            confirmationPanel.SetActive(true);
            UIEventsReload();
        }

        // タイトルへ戻るの確認画面非表示
        if (UIObject == closeConfirmButton)
        {
            confirmationPanel.SetActive(false);
        }

        if (UIObject == confirmTitleBackButton)
        {
            // タイトルへ戻る処理

        }

        if (UIObject == overlayOptionButton)
        {

            audioManager.PlaySE("剣");
            audioManager.PlayBGM("BGMの素材の名前");
        }
    }


    /// <summary>
    /// カーソルが触れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが触れたObject</param>
    void UIEnter(GameObject UIObject)
    {
        //Debug.Log("Entered UI: " + UIObject);
    }


    /// <summary>
    /// カーソルが離れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが離れたObject</param>
    void UIExit(GameObject UIObject)
    {
        //Debug.Log("Exited UI: " + UIObject);
    }
    

    /// <summary>
    /// UIを切り替えるメソッドです。
    /// 引数は、タイトル画面であれば、"Title"、
    /// キャラ選択画面であれば"Chara"、
    /// Overlayだけ表示したい場合は、"OverlayOnly"にしてください。
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUI(string type)
    {
        if (type == "Title")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(true);
            titleBackButton.SetActive(false);
        }

        if (type == "Chara")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }

        if (type == "OverlayOnly")
        {
            overlay.SetActive(true);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }

        UIEventsReload();
    }

    private void ShowRelics()
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            if (gameManager.hasRelics.ContainsKey(RelicID) && gameManager.hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                relic.Init(RelicID);                                               // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

                //relicObject.transform.GetChild(1).text = hasRelics[RelicID].ToString();      // Prefabの子オブジェクトである所持数を表示するテキストを変更
            }
        }
    }
}