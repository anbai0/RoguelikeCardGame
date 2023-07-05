using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    public float amplitude = 1f;
    public float frequency = 1f;

    void Start()
    {
        ReloadUI();
    }

    public void ReloadUI()
    {
        UIs = Canvas.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
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
        if(UIObject == UIObject.CompareTag("Cards"))
        {
            UIObject.transform.localScale += Vector3.one * 0.1f;
        }

        if (UIObject == UIObject.CompareTag("Relics"))
        {
            Animator anim = UIObject.GetComponent<Animator>();
            anim.SetTrigger("RelicJump");
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            UIObject.transform.localScale -= Vector3.one * 0.1f;
        }


    }
}
