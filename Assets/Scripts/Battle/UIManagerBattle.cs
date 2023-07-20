using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// BattleScene��UIManager�ł��B
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManagerBattle : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    bool isEventsReset = true;
    bool isDragging;    // �h���b�O��Ԃ��𔻒肵�܂�

    void Start()
    {
        UIEventsReload();
    }

    /// <summary>
    /// UI�̕\���A�ĕ\�����s���܂��B
    /// </summary>
    public void UIEventsReload()
    {
        if(!isEventsReset)
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

        isEventsReset = false;
    }

    private void RemoveListeners()
    {
        foreach (UIController UI in UIs)
        {
            UI.onLeftClick.RemoveAllListeners();
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
        }
    }

    /// <summary>
    /// �h���b�O���n�߂��Ƃ��ɏ������郁�\�b�h�ł�
    /// </summary>
    void UIBeginDrag(GameObject UIObject)
    {
        if (UIObject.tag == "Cards")
        {
            UIObject.GetComponent<CardMovement>().CardBeginDrag(UIObject);
        }
    }
    
    /// <summary>
    /// �h���b�O���ɏ������郁�\�b�h�ł�
    /// </summary>
    void UIDrag(GameObject UIObject)
    {
        
    }

    /// <summary>
    /// �h���b�O�A���h�h���b�v�����Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�h���b�O�A���h�h���b�v����Object</param>
    void UIDrop(GameObject UIObject)
    {
        if (UIObject.tag == "Cards")
        {
            UIObject.GetComponent<CardMovement>().CardDorp(UIObject);
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
        if (!Input.GetMouseButton(0) && !isDragging)
        {
            UIObject.GetComponent<CardMovement>().CardEnter(UIObject);
        }
    }


    /// <summary>
    /// �J�[�\�������ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�������ꂽObject</param>
    void UIExit(GameObject UIObject)
    {

        if (!Input.GetMouseButton(0) && !isDragging)
        {
            UIObject.GetComponent<CardMovement>().CardExit(UIObject);
        }
    }


}



