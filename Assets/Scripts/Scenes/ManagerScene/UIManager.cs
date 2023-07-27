using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ���̃X�N���v�g�̓R�s�[���Ďg���܂��B
// �R�s�[������Ŏg��Ȃ��C�x���g�⃁�\�b�h�͍폜���Ă�������

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



    void Start()
    {
        UIEventsReload();
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
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
            UI.onDrop.RemoveAllListeners();
        }
    }


    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        Debug.Log("LeftClicked UI: " + UIObject);
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
        Debug.Log("DragAndDrop UI: " + UIObject);
    }
    
}