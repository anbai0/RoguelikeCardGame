using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening.Core.Easing;

/// <summary>
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManager : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameManager gm;
    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject confirmationPanel;
    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] GameObject overlayOptionButton;
    [SerializeField] GameObject titleOptionButton;
    [SerializeField] GameObject closeOptionButton;
    [SerializeField] GameObject titleBackButton;
    [SerializeField] GameObject closeConfirmButton;
    [SerializeField] GameObject confirmTitleBackButton;
    [Space(10)]
    [SerializeField] TextMeshProUGUI myMoneyText;   //��������\������e�L�X�g


    private bool isTitleScreen = false; // �^�C�g����ʂɂ���Ƃ���true�ɂ���
    private int maxRelics = 12;

    void Start()
    {
        UIEventsReload();
    }

    void Update()
    {
        RefreshMoneyText();
    }

    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isRemoved)             // �C�x���g�̏�����
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            //UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));

        }

        isRemoved = false;
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

    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        // �I�v�V������ʕ\��
        if (UIObject == (overlayOptionButton || titleOptionButton))
        {
            optionScreen.SetActive(true);
        }

        // �I�v�V������ʔ�\��
        if (UIObject == closeOptionButton)
        {
            optionScreen.SetActive(false);
        }

        // �^�C�g���֖߂�̊m�F��ʕ\��
        if (UIObject == titleBackButton)
        {
            confirmationPanel.SetActive(true);
        }

        // �^�C�g���֖߂�̊m�F��ʔ�\��
        if (UIObject == closeConfirmButton)
        {
            confirmationPanel.SetActive(false);
        }

        if (UIObject == confirmTitleBackButton)
        {
            // �^�C�g���֖߂鏈��

        }

        if (UIObject == overlayOptionButton)
        {

            audioManager.PlaySE("��");
            audioManager.PlayBGM("BGM�̑f�ނ̖��O");
        }
    }


    /// <summary>
    /// �J�[�\�����G�ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�����G�ꂽObject</param>
    void UIEnter(GameObject UIObject)
    {
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(true);
        }     
    }


    /// <summary>
    /// �J�[�\�������ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�������ꂽObject</param>
    void UIExit(GameObject UIObject)
    {
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(false);
        }
    }
    

    /// <summary>
    /// UI��؂�ւ��郁�\�b�h�ł��B
    /// �����́A�^�C�g����ʂł���΁A"Title"�A
    /// �L�����I����ʂł����"Chara"�A
    /// Overlay�����\���������ꍇ�́A"OverlayOnly"�ɂ��Ă��������B
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUI(string type)
    {
        if (type == "Title")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(true);
            titleBackButton.SetActive(false);
        }

        if (type == "None")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(false);
        }

        if (type == "OverlayOnly")
        {
            overlay.SetActive(true);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }
    }


    /// <summary>
    /// �������̃e�L�X�g���X�V���郁�\�b�h�ł��B
    /// </summary>
    void RefreshMoneyText()
    {
        if (gm.playerData != null)
            myMoneyText.text = gm.playerData._playerMoney.ToString();
    }
}