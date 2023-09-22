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

    [Header("ミニマップのUI")]   
    [SerializeField] GameObject miniMapBG;
    [SerializeField] GameObject miniMap;
    [Header("拡大マップのUI")]
    [SerializeField] GameObject enlargedMap;
    [SerializeField] Transform mapControl;
    [SerializeField] GameObject closeButton;

    GameObject map;


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
        if (UIObject == miniMapBG)
        {
            // 拡大マップを表示
            enlargedMap.SetActive(true);

            // 拡大マップにマップ情報を複製
            map = Instantiate(miniMap, miniMap.transform.position, Quaternion.identity, mapControl.transform);           
            map.transform.transform.localPosition = new Vector2(-500 , 500);
            map.GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 2000);
            map.transform.localScale = Vector3.one * 1.25f;

            // 拡大マップの位置をリセット
            mapControl.localPosition = Vector3.zero;

            // プレイヤーを動けなくする
            PlayerController.Instance.isEvents = true;

            UIEventsReload();
        }

        // 拡大マップのcloseButtonを押したら拡大マップをDestroy
        if (UIObject == closeButton)
        {
            // 拡大マップを非表示にし、複製したマップを削除
            enlargedMap.SetActive(false);
            Destroy(map); map = null;

            // プレイヤーを動けるようにする
            PlayerController.Instance.isEvents = false;
        }
    }
}