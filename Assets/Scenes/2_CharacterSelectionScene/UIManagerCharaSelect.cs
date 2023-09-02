using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManagerCharaSelect : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    private bool isCharaSelected = false;

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

    [Header("�J�[�h���I")]
    [SerializeField] GameObject lotteryScreen;
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform cardPlace;
    [SerializeField] GameObject cardDecisionButton;
    //List<int> lotteryCards = new List<int>(); // ���I�����J�[�h���i�[����
    List<int> lotteryCards = null;
    private Vector3 scaleReset = Vector3.one * 0.25f;     // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�
    private bool isCardSelected = false;
    private GameObject lastSelectedCards;

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
        if (!isEventsReset)             // �C�x���g�̏�����
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        isEventsReset = false;
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
            isCharaSelected = true;
            AudioManager.Instance.PlaySE("�I����1");
            selectWarrior = true;
            selectWizard = false;
            warriorRelicPlace.gameObject.SetActive(true);
            wizardRelicPlace.gameObject.SetActive(false);
            image.sprite = redButton;
        }
        if (UIObject == wizard)
        {
            isCharaSelected = true;
            AudioManager.Instance.PlaySE("�I����1");
            selectWarrior = false;
            selectWizard = true;
            warriorRelicPlace.gameObject.SetActive(false);
            wizardRelicPlace.gameObject.SetActive(true);
            image.sprite = redButton;
        }


        if (UIObject == button && isCharaSelected && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("�I����1");

            if (selectWarrior)
                gm.ReadPlayer("Warrior");
            if (selectWizard)
                gm.ReadPlayer("Wizard");

            ShowLottery();
            UIEventsReload();
            lotteryScreen.SetActive(true);
        }


        // �J�[�h���N���b�N������
        if (UIObject.CompareTag("Cards"))
        {
            isCardSelected = true;
            AudioManager.Instance.PlaySE("�I����1");

            // �J�[�h�I����Ԃ̐؂�ւ�
            if (lastSelectedCards != null && lastSelectedCards != UIObject)              // ���ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                lastSelectedCards.transform.localScale = scaleReset;
                lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);
            }

            lastSelectedCards = UIObject;

            cardDecisionButton.SetActive(true);    // ����{�^����\��
        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isCardSelected && UIObject.CompareTag("BackGround"))
        {
            lastSelectedCards.transform.localScale = scaleReset;
            lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            lastSelectedCards = null;
            isCardSelected = false;

            cardDecisionButton.SetActive(false);    // ����{�^�����\��
        }

        // ����{�^������������
        if (UIObject == cardDecisionButton)
        {         
            AudioManager.Instance.PlaySE("�I����2");
            // �Ō�ɃN���b�N�����J�[�h���擾
            GameManager.Instance.AddCard(lastSelectedCards.GetComponent<CardController>().cardDataManager._cardID);
            // �t�B�[���h�V�[���֑J��
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

        // �J�[�h�̑I������
        if (!isCardSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
            }
        }


        if (isCharaSelected) return;

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

        // �J�[�h�I������
        if (!isCardSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = scaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������
            }
        }

        if (isCharaSelected) return;

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

    private void ShowLottery()
    {
        lotteryCards = Lottery.Instance.SelectCardByRarity(new List<int> { 2, 1, 1 });

        for (int init = 0; init < lotteryCards.Count; init++)
        {
            CardController card = Instantiate(cardPrefab, cardPlace);   //�J�[�h�𐶐�����
            card.transform.localScale = scaleReset;
            card.name = "Deck" + (init).ToString();                     //���������J�[�h�ɖ��O��t����
            card.Init(lotteryCards[init]);                            //�f�b�L�f�[�^�̕\��
        }
    }
}

