using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManagerField : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    //private bool isClick = false;

    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject miniMap;
    [SerializeField] GameObject closeButtonPrefab;
    [SerializeField] GameObject bgPrefab;
    GameObject enlargedMap;  
    GameObject closeButton;
    GameObject bg;


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
        if (!isEventsReset)             // �C�x���g�̏�����
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                                // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
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
        }
    }
    #endregion


    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        // �~�j�}�b�v���N���b�N������g��}�b�v����
        if (UIObject == miniMap)
        {
            // �w�i����
            bg = Instantiate(bgPrefab,canvas.transform);

            // �g��}�b�v����
            enlargedMap = Instantiate(miniMap, miniMap.transform.position, Quaternion.identity, canvas.transform);
            enlargedMap.transform.localPosition = Vector3.zero;
            enlargedMap.GetComponent<Mask>().enabled = false;
            enlargedMap.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 1000);
            enlargedMap.transform.GetChild(0).transform.localPosition = new Vector3(-400 , 400);
            // ����{�^������
            closeButton = Instantiate(closeButtonPrefab, closeButtonPrefab.transform.position, Quaternion.identity, enlargedMap.transform);
            closeButton.transform.localPosition = new Vector3(400, 400);
            UIEventsReload();
            // �v���C���[�𓮂��Ȃ�����
            PlayerController.Instance.isEvents = true;
        }

        // �g��}�b�v��closeButton����������g��}�b�v��Destroy
        if (UIObject == closeButton)
        {
            Destroy(bg); bg = null;
            Destroy(enlargedMap); enlargedMap = null;
            // �v���C���[�𓮂���悤�ɂ���
            PlayerController.Instance.isEvents = false;
        }
    }
}