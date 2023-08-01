using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// このスクリプトはコピーして使います。
// コピーした先で使わないイベントやメソッドは削除してください

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerBase : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [Header ("Canvasをアタッチ")]
    [SerializeField] private GameObject parent;
    private UIController[] UIs;
    bool isRemoved = true;



    void Start()
    {
        UIEventsReload();
    }

    /// <summary>
    /// UIの表示、再表示を行います。
    /// </summary>
    void UIEventsReload()
    {
        if (!isRemoved)             // イベントの初期化
            RemoveListeners();

        UIs = parent.GetComponentsInChildren<UIController>();       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
            UI.onBeginDrag.AddListener(() => UIBeginDrag(UI.gameObject));
            UI.onDrag.AddListener(() => UIDrag(UI.gameObject));
            UI.onDrop.AddListener(() => UIDrop(UI.gameObject));

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
            UI.onRightClick.RemoveAllListeners();
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
            UI.onDrop.RemoveAllListeners();
        }
    }



    /// <summary>
    /// 左クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UILeftClick(GameObject UIObject)
    {
        Debug.Log("LeftClicked UI: " + UIObject);
    }


    /// <summary>
    /// 右クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UIRightClick(GameObject UIObject)
    {
        Debug.Log("RightClicked UI: " + UIObject);
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
    /// UIをドラッグし始めた時に処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">ドラッグしたObject</param>
    void UIBeginDrag(GameObject UIObject)
    {

    }


    /// <summary>
    /// ドラッグしているUIに対して処理をするメソッドです。
    /// </summary>
    /// <param name="UIObject">ドラッグしているObject</param>
    void UIDrag(GameObject UIObject)
    {

    }


    /// <summary>
    /// UIをドラッグアンドドロップしたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">ドラッグアンドドロップしたObject</param>
    void UIDrop(GameObject UIObject)
    {
        Debug.Log("DragAndDrop UI: " + UIObject);
    }
}