using UnityEngine;
using UnityEngine.UI;

public class UIManagerCharacterSelectionScene : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    public GameObject warrior;
    public GameObject wizard;

    bool selectWarrior = false;
    bool selectWizard = false;

    bool isSelect = false;

    private Color32 originalColor;
    private Color32 targetColor;
    private float originalScale;
    public float targetScale = 0.9f;

    private float duration = 0.25f;      //色が変わるまでの秒数
    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float warriorScale;

    private float wizardElapsedTime = 0f;
    private float wizardLate;
    private float wizardScale;

    public GameObject button;
    public Image image;
    public Sprite redButton;
    public CharacterSceneManager sceneManager;


    void Start()
    {
        UIEventReload();

        // 初期の色
        originalColor = new Color32(60, 60, 60, 255);

        // ハイライト時の色
        targetColor = new Color32(255, 255, 255, 255);

        //変更前のScale
        originalScale = warrior.transform.localScale.x;
    }

    public void UIEventReload()
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
        if (UIObject == warrior)
        {
            isSelect = true;
            selectWarrior = true;
            selectWizard = false;
            image.sprite = redButton;
        }
        if (UIObject == wizard)
        {
            isSelect = true;
            selectWarrior = false;
            selectWizard = true;
            image.sprite = redButton;
        }
        if (UIObject == button && isSelect)
        {
            if(UIObject == warrior)
                GameManager.Instance.ReadPlayer("Warrior");
            if (UIObject == wizard)
                GameManager.Instance.ReadPlayer("Wizard");
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
            warriorScale = Mathf.Lerp(originalScale, targetScale, warriorLate);
            warrior.transform.localScale = new Vector3(warriorScale, warriorScale, warriorScale);

            //魔法使いをローライト
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
            wizard.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
        if (selectWizard)   //魔法使いをハイライト
        {
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime / duration);
            wizardImage.color = Color32.Lerp(originalColor, targetColor, wizardLate);
            wizardScale = Mathf.Lerp(originalScale, targetScale, wizardLate);
            wizard.transform.localScale = new Vector3(wizardScale, wizardScale, wizardScale);


            //戦士をローライト
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
            warrior.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
    }
}

