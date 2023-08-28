using UnityEngine;

/// <summary>
/// BattleScene��UIManager�ł��B
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManagerBattle : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;

    private bool isDragging = false;    // �h���b�O��Ԃ��𔻒肵�܂�

    void Start()
    {
        UIEventsReload();
    }

    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        if(!isEventsReset)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            //UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
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
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
            UI.onBeginDrag.RemoveAllListeners();
            UI.onDrag.RemoveAllListeners();
            UI.onDrop.RemoveAllListeners();
        }
    }
    #endregion

    /// <summary>
    /// �J�[�\�����G�ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�����G�ꂽObject</param>
    void UIEnter(GameObject UIObject)
    {

        if (!Input.GetMouseButton(0) && !isDragging)
        {
            if (UIObject.CompareTag("Condition"))
            {
                UIObject.transform.GetChild(1).gameObject.SetActive(true); //PlayerConditionEffectBG��\������
            }
            else if (UIObject.CompareTag("EnemyCondition"))
            {
                UIObject.transform.GetChild(2).gameObject.SetActive(true); //EnemyConditionEffectBG��\������
            }
            else
            {
                UIObject.GetComponent<CardMovement>().CardEnter(UIObject);
            }
            
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
            if (UIObject.CompareTag("Condition"))
            {
                UIObject.transform.GetChild(1).gameObject.SetActive(false); //PlayerConditionEffectBG���\���ɂ���
            }
            else if(UIObject.CompareTag("EnemyCondition"))
            {
                UIObject.transform.GetChild(2).gameObject.SetActive(false); //EnemyConditionEffectBG���\���ɂ���
            }
            else
            {
                UIObject.GetComponent<CardMovement>().CardExit(UIObject);
            }
        }
    }


    /// <summary>
    /// �h���b�O���n�߂��Ƃ��ɏ������郁�\�b�h�ł�
    /// </summary>
    void UIBeginDrag(GameObject UIObject)
    {
        Debug.Log("Begin");
        if (UIObject.CompareTag("Cards"))
        {
            UIObject.GetComponent<CardMovement>().CardBeginDrag(UIObject);
        }
    }

    /// <summary>
    /// �h���b�O���ɏ������郁�\�b�h�ł�
    /// </summary>
    void UIDrag(GameObject UIObject)
    {
        Debug.Log("Drag");
    }

    /// <summary>
    /// �h���b�O�A���h�h���b�v�����Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�h���b�O�A���h�h���b�v����Object</param>
    void UIDrop(GameObject UIObject)
    {
        Debug.Log("Drop");
        Debug.Log(UIObject.name);
        if (UIObject.CompareTag("Cards"))
        {
            UIObject.GetComponent<CardMovement>().CardDorp(UIObject);
        }
    }
}



