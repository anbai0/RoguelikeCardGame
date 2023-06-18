using UnityEngine;
using UnityEngine.UI;

public class UIManagerBase : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    void Start()
    {
        UIs = parent.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onClick.AddListener(() => UIClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    void UIClick(GameObject UIObject)
    {

    }

    void UIEnter(GameObject UIObject)
    {

    }

    void UIExit(GameObject UIObject)
    {

    }
}
