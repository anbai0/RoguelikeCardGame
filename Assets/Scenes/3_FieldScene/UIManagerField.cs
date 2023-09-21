using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerField : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    //private bool isClick = false;

    [Header("参照するUI")]
    [SerializeField] GameObject miniMap;
    [SerializeField] GameObject closeButtonPrefab;
    [SerializeField] GameObject bgPrefab;
    GameObject enlargedMap;  
    GameObject closeButton;
    GameObject bg;


    void Start()
    {
        UIEventsReload();
    }

    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    void UIEventsReload()
    {
        if (!isEventsReset)             // イベントの初期化
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                                // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
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
        }
    }
    #endregion


    /// <summary>
    /// 左クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UILeftClick(GameObject UIObject)
    {
        // ミニマップをクリックしたら拡大マップ生成
        if (UIObject == miniMap)
        {
            // 背景生成
            bg = Instantiate(bgPrefab,canvas.transform);

            // 拡大マップ生成
            enlargedMap = Instantiate(miniMap, miniMap.transform.position, Quaternion.identity, canvas.transform);
            enlargedMap.transform.localPosition = Vector3.zero;
            enlargedMap.GetComponent<Mask>().enabled = false;
            enlargedMap.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 1000);
            enlargedMap.transform.GetChild(0).transform.localPosition = new Vector3(-400 , 400);
            // 閉じるボタン生成
            closeButton = Instantiate(closeButtonPrefab, closeButtonPrefab.transform.position, Quaternion.identity, enlargedMap.transform);
            closeButton.transform.localPosition = new Vector3(400, 400);
            UIEventsReload();
            // プレイヤーを動けなくする
            PlayerController.Instance.isEvents = true;
        }

        // 拡大マップのcloseButtonを押したら拡大マップをDestroy
        if (UIObject == closeButton)
        {
            Destroy(bg); bg = null;
            Destroy(enlargedMap); enlargedMap = null;
            // プレイヤーを動けるようにする
            PlayerController.Instance.isEvents = false;
        }
    }
}