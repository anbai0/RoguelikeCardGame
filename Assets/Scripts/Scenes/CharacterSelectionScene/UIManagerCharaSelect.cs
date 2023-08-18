using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerCharaSelect : MonoBehaviour
{
    // UIManagerに最初から定義してある変数
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;
    private bool isClick = false;

    private bool isSelected = false;

    private bool selectWarrior = false;
    private bool selectWizard = false;

    private Color32 originalColor;
    private Color32 targetColor;
    private float originalScale;
    [SerializeField] float targetScale = 0.9f;

    private float duration = 0.25f;      //色が変わるまでの秒数

    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float warriorScale;

    private float wizardElapsedTime = 0f;
    private float wizardLate;
    private float wizardScale;

    private int[] warriorRelic = new int[] { 10, 4 };
    private int[] wizardRelic = new int[] { 5, 9 };

    // 参照するUI
    [SerializeField] private GameObject warrior;
    [SerializeField] private GameObject wizard;
    [SerializeField] private GameObject button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite redButton;
    [SerializeField] private RelicController relicPrefab;
    [SerializeField] private Transform warriorRelicPlace;
    [SerializeField] private Transform wizardRelicPlace;

    [Header("参照するコンポーネント")]
    [SerializeField] private CharacterSceneManager sceneManager;
    private GameManager gm;
    //[Header("表示を切り替えるUI")]
    //[Header("クリック後に参照するUI")]

    void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;

        // 初期の色
        originalColor = new Color32(60, 60, 60, 255);

        // ハイライト時の色
        targetColor = new Color32(255, 255, 255, 255);

        //変更前のScale
        originalScale = warrior.transform.localScale.x;

        RelicShow();
        UIEventsReload();
    }


    #region UIイベントリスナー関係の処理
    /// <summary>
    /// <para> UIイベントリスナーの登録、再登録を行います。</para>
    /// <para>イベントの登録を行った後に、新しく生成したPrefabに対して処理を行いたい場合は、再度このメソッドを呼んでください。</para>
    /// </summary>
    public void UIEventsReload()
    {
        UIs = canvas.GetComponentsInChildren<UIController>(true);       // 指定した親の子オブジェクトのUIControllerコンポーネントをすべて取得
        foreach (UIController UI in UIs)                            // UIs配列内の各要素がUIController型の変数UIに順番に代入され処理される
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UIがクリックされたら、クリックされたUIを関数に渡す
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
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
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
        }
    }
    #endregion

    void UILeftClick(GameObject UIObject)
    {
        if (UIObject == warrior)
        {
            isSelected = true;
            selectWarrior = true;
            selectWizard = false;
            warriorRelicPlace.gameObject.SetActive(true);
            wizardRelicPlace.gameObject.SetActive(false);
            image.sprite = redButton;
        }
        if (UIObject == wizard)
        {
            isSelected = true;
            selectWarrior = false;
            selectWizard = true;
            warriorRelicPlace.gameObject.SetActive(false);
            wizardRelicPlace.gameObject.SetActive(true);
            image.sprite = redButton;
        }


        if (UIObject == button && isSelected && !isClick)
        {
            isClick = true;

            if (selectWarrior)
                gm.ReadPlayer("Warrior");
            if (selectWizard)
                gm.ReadPlayer("Wizard");

            sceneManager.LoadFieldScene();
        }
    }


    void UIEnter(GameObject UIObject)
    {
        // 戦士選択時に、レリックの説明文切り替え
        if (UIObject.CompareTag("Relics") && selectWarrior)
        {
            UIObject.transform.GetChild(6).gameObject.SetActive(true);
        }
        // 魔法使い選択時に、レリックの説明文切り替え
        if (UIObject.CompareTag("Relics") && selectWizard)
        {
            UIObject.transform.GetChild(7).gameObject.SetActive(true);
        }


        if (isSelected) return;

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
        // 戦士選択時に、レリックの説明文切り替え
        if (UIObject.CompareTag("Relics") && selectWarrior)
        {
            UIObject.transform.GetChild(6).gameObject.SetActive(false);
        }
        // 魔法使い選択時に、レリックの説明文切り替え
        if (UIObject.CompareTag("Relics") && selectWizard)
        {
            UIObject.transform.GetChild(7).gameObject.SetActive(false);
        }


        if (isSelected) return;

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

    void highLight(Image warriorImage, Image wizardImage)
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

        if (!selectWarrior && !selectWizard)    // どちらも選択されていない場合ローライト
        {
            //戦士をローライト
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
            warrior.transform.localScale = new Vector3(originalScale, originalScale, originalScale);

            //魔法使いをローライト
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
            wizard.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
    }


    /// <summary>
    /// 初期レリックの表示をします。
    /// </summary>
    void RelicShow()
    {
        for (int relicID = 0; relicID < warriorRelic.Length; relicID++)
        {
            RelicController relic = Instantiate(relicPrefab, warriorRelicPlace);
            relic.transform.localScale = Vector3.one * 1f;                     // 生成したPrefabの大きさ調整
            relic.Init(warriorRelic[relicID]);                                 // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

            relic.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[warriorRelic[relicID]]._relicName.ToString();        // レリックの名前を変更
            relic.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[warriorRelic[relicID]]._relicEffect.ToString();      // レリック説明変更
        }


        for (int relicID = 0; relicID < wizardRelic.Length; relicID++)
        {
            RelicController relic = Instantiate(relicPrefab, wizardRelicPlace);
            relic.transform.localScale = Vector3.one * 1f;                     // 生成したPrefabの大きさ調整
            relic.Init(wizardRelic[relicID]);                                  // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

            relic.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[wizardRelic[relicID]]._relicName.ToString();        // レリックの名前を変更
            relic.transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[wizardRelic[relicID]]._relicEffect.ToString();      // レリック説明変更
        }
    }
}

