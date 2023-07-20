using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// BattleSceneのUIManagerです。
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerBattle : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    bool isEventsReset = true;
    bool isDragging;    // ドラッグ状態かを判定します

    void Start()
    {
        UIEventsReload();
    }

    /// <summary>
    /// UIの表示、再表示を行います。
    /// </summary>
    public void UIEventsReload()
    {
        if(!isEventsReset)
            RemoveListeners();

        UIs = parent.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
            UI.onBeginDrag.AddListener(() => UIBeginDrag(UI.gameObject));
            UI.onDrag.AddListener(() => UIDrag(UI.gameObject));
            UI.onDrop.AddListener(() => UIDrop(UI.gameObject));

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
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
        }
    }

    /// <summary>
    /// ドラッグし始めたときに処理するメソッドです
    /// </summary>
    void UIBeginDrag(GameObject UIObject)
    {
        if (UIObject.tag == "Cards")
        {
            UIObject.GetComponent<CardMovement>().CardBeginDrag(UIObject);
        }
    }
    
    /// <summary>
    /// ドラッグ中に処理するメソッドです
    /// </summary>
    void UIDrag(GameObject UIObject)
    {
        
    }

    /// <summary>
    /// ドラッグアンドドロップしたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">ドラッグアンドドロップしたObject</param>
    void UIDrop(GameObject UIObject)
    {
        if (UIObject.tag == "Cards")
        {
            UIObject.GetComponent<CardMovement>().CardDorp(UIObject);
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
    /// カーソルが触れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが触れたObject</param>
    void UIEnter(GameObject UIObject)
    {
        if (!Input.GetMouseButton(0) && !isDragging)
        {
            UIObject.GetComponent<CardMovement>().CardEnter(UIObject);
        }
    }


    /// <summary>
    /// カーソルが離れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが離れたObject</param>
    void UIExit(GameObject UIObject)
    {

        if (!Input.GetMouseButton(0) && !isDragging)
        {
            UIObject.GetComponent<CardMovement>().CardExit(UIObject);
        }
    }


}



