using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManagerCharacterSelectionScene : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    public GameObject warrior;
    public GameObject wizard;

    bool selectWarrior = false;
    bool selectWizard = false;

    bool isSelect = false;

    private Color32 originalColor;
    private Color32 targetColor;

    private float duration = 0.25f;      //色が変わるまでの秒数
    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float wizardElapsedTime = 0f;
    private float wizardLate;

    public GameObject button;
    public Image image;
    public Sprite redButton;
    public CharacterSceneManager sceneManager;

    void Start()
    {
        UIs = parent.GetComponentsInChildren<UIController>();       //指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            //UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onClick.AddListener(() => UIClick(UI.gameObject));         //UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        // 初期の色
        originalColor = new Color32(60, 60, 60, 255);

        // ハイライト時の色
        targetColor = new Color32(255, 255, 255, 255);
    }

    void UIClick(GameObject UIObject)
    {
        if (UIObject == warrior)
        {
            isSelect = true;
            selectWarrior = true;
            selectWizard = false;
        }
        if (UIObject == wizard)
        {
            isSelect = true;
            selectWarrior = false;
            selectWizard = true;
        }
        if (UIObject == button && isSelect)
        {
            image.sprite = redButton;
            sceneManager.FieldScene();
        }
    }

    void UIEnter(GameObject UIObject)
    {
        if (isSelect) return;

        if (UIObject == warrior)
        {
            selectWarrior = true;
        }
        if (UIObject == wizard)
        {
            selectWizard = true;
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (isSelect) return;

        if (UIObject == warrior)
        {
            selectWarrior = false;
        }
        if (UIObject == wizard)
        {
            selectWizard = false;
        }
    }


    private void Update()
    {
        //選ばれたキャラをハイライト
        highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());

    }

    public void highLight(Image warriorImage, Image wizardImage)
    {

        if (selectWarrior)  //戦士をハイライト
        {
            warriorElapsedTime += Time.deltaTime;
            warriorLate = Mathf.Clamp01(warriorElapsedTime / duration);
            warriorImage.color = Color32.Lerp(originalColor, targetColor, warriorLate);

            //魔法使いをローライト
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
        }
        if (selectWizard)   //魔法使いをハイライト
        {
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime / duration);
            wizardImage.color = Color32.Lerp(originalColor, targetColor, wizardLate);

            //戦士をローライト
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
        }
    }
}

