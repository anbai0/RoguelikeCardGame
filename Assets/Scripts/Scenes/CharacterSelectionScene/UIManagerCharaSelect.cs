using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerCharaSelect : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
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

    private float duration = 0.25f;      //�F���ς��܂ł̕b��

    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float warriorScale;

    private float wizardElapsedTime = 0f;
    private float wizardLate;
    private float wizardScale;

    private int[] warriorRelic = new int[] { 10, 4 };
    private int[] wizardRelic = new int[] { 5, 9 };

    // �Q�Ƃ���UI
    [SerializeField] private GameObject warrior;
    [SerializeField] private GameObject wizard;
    [SerializeField] private GameObject button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite redButton;
    [SerializeField] private RelicController relicPrefab;
    [SerializeField] private Transform warriorRelicPlace;
    [SerializeField] private Transform wizardRelicPlace;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] private CharacterSceneManager sceneManager;
    private GameManager gm;
    //[Header("�\����؂�ւ���UI")]
    //[Header("�N���b�N��ɎQ�Ƃ���UI")]

    void Start()
    {
        // GameManager�擾(�ϐ����ȗ�)
        gm = GameManager.Instance;

        // �����̐F
        originalColor = new Color32(60, 60, 60, 255);

        // �n�C���C�g���̐F
        targetColor = new Color32(255, 255, 255, 255);

        //�ύX�O��Scale
        originalScale = warrior.transform.localScale.x;

        RelicShow();
        UIEventsReload();
    }


    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    /// <summary>
    /// UI�C�x���g���폜���܂��B
    /// UIEventsReload���\�b�h���ŌĂ΂�܂��B
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
        // ��m�I�����ɁA�����b�N�̐������؂�ւ�
        if (UIObject.CompareTag("Relics") && selectWarrior)
        {
            UIObject.transform.GetChild(6).gameObject.SetActive(true);
        }
        // ���@�g���I�����ɁA�����b�N�̐������؂�ւ�
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
        // ��m�I�����ɁA�����b�N�̐������؂�ւ�
        if (UIObject.CompareTag("Relics") && selectWarrior)
        {
            UIObject.transform.GetChild(6).gameObject.SetActive(false);
        }
        // ���@�g���I�����ɁA�����b�N�̐������؂�ւ�
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
        //�I�΂ꂽ�L�������n�C���C�g
        highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());

    }

    void highLight(Image warriorImage, Image wizardImage)
    {

        if (selectWarrior)  //��m���n�C���C�g
        {
            warriorElapsedTime += Time.deltaTime;
            warriorLate = Mathf.Clamp01(warriorElapsedTime / duration);
            warriorImage.color = Color32.Lerp(originalColor, targetColor, warriorLate);
            warriorScale = Mathf.Lerp(originalScale, targetScale, warriorLate);
            warrior.transform.localScale = new Vector3(warriorScale, warriorScale, warriorScale);

            //���@�g�������[���C�g
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
            wizard.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
        if (selectWizard)   //���@�g�����n�C���C�g
        {
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime / duration);
            wizardImage.color = Color32.Lerp(originalColor, targetColor, wizardLate);
            wizardScale = Mathf.Lerp(originalScale, targetScale, wizardLate);
            wizard.transform.localScale = new Vector3(wizardScale, wizardScale, wizardScale);


            //��m�����[���C�g
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
            warrior.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }

        if (!selectWarrior && !selectWizard)    // �ǂ�����I������Ă��Ȃ��ꍇ���[���C�g
        {
            //��m�����[���C�g
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
            warrior.transform.localScale = new Vector3(originalScale, originalScale, originalScale);

            //���@�g�������[���C�g
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
            wizard.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
    }


    /// <summary>
    /// ���������b�N�̕\�������܂��B
    /// </summary>
    void RelicShow()
    {
        for (int relicID = 0; relicID < warriorRelic.Length; relicID++)
        {
            RelicController relic = Instantiate(relicPrefab, warriorRelicPlace);
            relic.transform.localScale = Vector3.one * 1f;                     // ��������Prefab�̑傫������
            relic.Init(warriorRelic[relicID]);                                 // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������

            relic.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[warriorRelic[relicID]]._relicName.ToString();        // �����b�N�̖��O��ύX
            relic.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[warriorRelic[relicID]]._relicEffect.ToString();      // �����b�N�����ύX
        }


        for (int relicID = 0; relicID < wizardRelic.Length; relicID++)
        {
            RelicController relic = Instantiate(relicPrefab, wizardRelicPlace);
            relic.transform.localScale = Vector3.one * 1f;                     // ��������Prefab�̑傫������
            relic.Init(wizardRelic[relicID]);                                  // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������

            relic.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[wizardRelic[relicID]]._relicName.ToString();        // �����b�N�̖��O��ύX
            relic.transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[wizardRelic[relicID]]._relicEffect.ToString();      // �����b�N�����ύX
        }
    }
}

