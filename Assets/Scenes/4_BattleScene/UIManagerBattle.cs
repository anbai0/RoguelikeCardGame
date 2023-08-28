using UnityEngine;

/// <summary>
/// BattleSceneのUIManagerです。
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerBattle : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;

    private bool isDragging = false;    // ドラッグ状態かを判定します

    void Start()
    {
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

        UIs = canvas.GetComponentsInChildren<UIController>(true);       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
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
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
            UI.onDrop.RemoveAllListeners();
        }
    }
    #endregion

    /// <summary>
    /// カーソルが触れたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">カーソルが触れたObject</param>
    void UIEnter(GameObject UIObject)
    {

        if (!Input.GetMouseButton(0) && !isDragging)
        {
            if (UIObject.CompareTag("Condition"))
            {
                UIObject.transform.GetChild(1).gameObject.SetActive(true); //PlayerConditionEffectBGを表示する
            }
            else if (UIObject.CompareTag("EnemyCondition"))
            {
                UIObject.transform.GetChild(2).gameObject.SetActive(true); //EnemyConditionEffectBGを表示する
            }
            else
            {
                UIObject.GetComponent<CardMovement>().CardEnter(UIObject);
            }
            
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
            if (UIObject.CompareTag("Condition"))
            {
                UIObject.transform.GetChild(1).gameObject.SetActive(false); //PlayerConditionEffectBGを非表示にする
            }
            else if(UIObject.CompareTag("EnemyCondition"))
            {
                UIObject.transform.GetChild(2).gameObject.SetActive(false); //EnemyConditionEffectBGを非表示にする
            }
            else
            {
                UIObject.GetComponent<CardMovement>().CardExit(UIObject);
            }
        }
    }


    /// <summary>
    /// ドラッグし始めたときに処理するメソッドです
    /// </summary>
    void UIBeginDrag(GameObject UIObject)
    {
        Debug.Log("Begin");
        if (UIObject.CompareTag("Cards"))
        {
            UIObject.GetComponent<CardMovement>().CardBeginDrag(UIObject);
        }
    }

    /// <summary>
    /// ドラッグ中に処理するメソッドです
    /// </summary>
    void UIDrag(GameObject UIObject)
    {
        Debug.Log("Drag");
    }

    /// <summary>
    /// ドラッグアンドドロップしたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">ドラッグアンドドロップしたObject</param>
    void UIDrop(GameObject UIObject)
    {
        Debug.Log("Drop");
        Debug.Log(UIObject.name);
        if (UIObject.CompareTag("Cards"))
        {
            UIObject.GetComponent<CardMovement>().CardDorp(UIObject);
        }
    }
}



