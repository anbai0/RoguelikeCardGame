using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerBase : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    void Start()
    {
        UIEventsReload();
    }

    /// <summary>
    /// UIの表示、再表示を行います。
    /// </summary>
    void UIEventsReload()
    {
        UIs = parent.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
            UI.onDrop.AddListener(() => UIDragAndDrop(UI.gameObject));

        }
    }


    /// <summary>
    /// ドラッグアンドドロップしたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">ドラッグアンドドロップしたObject</param>
    void UIDragAndDrop(GameObject UIObject)
    {
        Debug.Log("DragAndDrop UI: " + UIObject);
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


}



