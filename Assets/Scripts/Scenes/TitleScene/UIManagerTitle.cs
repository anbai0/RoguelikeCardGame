using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UIの管理を行うスクリプトです。
/// UIController側で起きた判定に対して処理を行います。
/// </summary>
public class UIManagerTitle : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;

    [Header("参照するスクリプト")]
    [SerializeField] TitleSceneManager sceneManager;

    [Header("表示を切り替えるUI")]
    // テキスト点滅させるために必要なもの
    [SerializeField] TextMeshProUGUI clickToStartText;
    [SerializeField]
    [Range(0.1f, 10.0f)] float duration = 1.0f;  //テキストを点滅させる間隔
    private Color32 startColor = new Color32(255, 255, 255, 255);
    private Color32 endColor = new Color32(255, 255, 255, 20);

    //[Header("クリック後に参照するUI")]



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
        if (!isRemoved)             // イベントの初期化
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
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
        }
    }
    #endregion
    

    /// <summary>
    /// 左クリックされたときに処理するメソッドです。
    /// </summary>
    /// <param name="UIObject">クリックされたObject</param>
    void UILeftClick(GameObject UIObject)
    {
        if (UIObject == UIObject.CompareTag("BackGround"))
        {
            sceneManager.LoadCharaSelectScene();
        }
    }


    void Update()
    {
        // テキストを点滅させる
        clickToStartText.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));
    }
}