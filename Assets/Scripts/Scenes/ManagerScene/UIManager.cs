using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManager : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [Header("Canvas���A�^�b�`")]
    [SerializeField] private GameObject parent;
    private UIController[] UIs;
    bool isRemoved = true;

    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject confirmationPanel;
    [Space (10)]
    [SerializeField] GameObject overlayOptionButton;
    [SerializeField] GameObject titleOptionButton;
    [SerializeField] GameObject closeOptionButton;
    [SerializeField] GameObject titleBackButton;
    [SerializeField] GameObject closeConfirmButton;
    [SerializeField] GameObject confirmTitleBackButton;


    bool isTitleScreen = false; // �^�C�g����ʂɂ���Ƃ���true�ɂ���

    void Start()
    {
        UIEventsReload();
        //ChangeUI("OverlayOnly");
    }

    /// <summary>
    /// UI�̕\���A�ĕ\�����s���܂��B
    /// </summary>
    void UIEventsReload()
    {
        if (!isRemoved)             // �C�x���g�̏�����
            RemoveListeners();

        UIs = parent.GetComponentsInChildren<UIController>();       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
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


    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        Debug.Log(UIObject.name);

        // �I�v�V������ʕ\��
        if (UIObject == (overlayOptionButton || titleOptionButton))
        {
            optionScreen.SetActive(true);
            UIEventsReload();
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
            UIEventsReload();
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
    }


    /// <summary>
    /// �J�[�\�����G�ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�����G�ꂽObject</param>
    void UIEnter(GameObject UIObject)
    {
        //Debug.Log("Entered UI: " + UIObject);
    }


    /// <summary>
    /// �J�[�\�������ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�������ꂽObject</param>
    void UIExit(GameObject UIObject)
    {
        //Debug.Log("Exited UI: " + UIObject);
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

        if (type == "Chara")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }

        if (type == "OverlayOnly")
        {
            overlay.SetActive(true);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }

        UIEventsReload();
    }
}