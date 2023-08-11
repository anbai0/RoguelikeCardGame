using UnityEngine;
using UnityEngine.UI;

// ���̃X�N���v�g�̓R�s�[���Ďg���܂��B
// �R�s�[������Ŏg��Ȃ��C�x���g�⃁�\�b�h�͍폜���Ă�������

/// <summary>
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManagerBase : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;
    private bool isClick = false;

    //[Header("�Q�Ƃ���X�N���v�g")]
    //[Header("�\����؂�ւ���UI")]
    //[Header("�N���b�N��ɎQ�Ƃ���UI")]



    void Start()
    {
        UIEventsReload();
    }

    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    void UIEventsReload()
    {
        if (!isRemoved)             // �C�x���g�̏�����
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                                // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
            UI.onBeginDrag.AddListener(() => UIBeginDrag(UI.gameObject));
            UI.onDrag.AddListener(() => UIDrag(UI.gameObject));
            UI.onDrop.AddListener(() => UIDrop(UI.gameObject));

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
            UI.onRightClick.RemoveAllListeners();
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
            UI.onDrop.RemoveAllListeners();
        }
    }
    #endregion


    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        Debug.Log("LeftClicked UI: " + UIObject);
    }


    /// <summary>
    /// �E�N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UIRightClick(GameObject UIObject)
    {
        
    }


    /// <summary>
    /// �J�[�\�����G�ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�����G�ꂽObject</param>
    void UIEnter(GameObject UIObject)
    {
        
    }


    /// <summary>
    /// �J�[�\�������ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�������ꂽObject</param>
    void UIExit(GameObject UIObject)
    {

    }


    /// <summary>
    /// UI���h���b�O���n�߂����ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�h���b�O����Object</param>
    void UIBeginDrag(GameObject UIObject)
    {

    }


    /// <summary>
    /// �h���b�O���Ă���UI�ɑ΂��ď��������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�h���b�O���Ă���Object</param>
    void UIDrag(GameObject UIObject)
    {

    }


    /// <summary>
    /// UI���h���b�O�A���h�h���b�v�����Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�h���b�O�A���h�h���b�v����Object</param>
    void UIDrop(GameObject UIObject)
    {

    }
}