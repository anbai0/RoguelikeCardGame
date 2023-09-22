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

    [Header("�~�j�}�b�v��UI")]   
    [SerializeField] GameObject miniMapBG;
    [SerializeField] GameObject miniMap;
    [Header("�g��}�b�v��UI")]
    [SerializeField] GameObject enlargedMap;
    [SerializeField] Transform mapControl;
    [SerializeField] GameObject closeButton;

    GameObject map;


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
        if (UIObject == miniMapBG)
        {
            // �g��}�b�v��\��
            enlargedMap.SetActive(true);

            // �g��}�b�v�Ƀ}�b�v���𕡐�
            map = Instantiate(miniMap, miniMap.transform.position, Quaternion.identity, mapControl.transform);           
            map.transform.transform.localPosition = new Vector2(-500 , 500);
            map.GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 2000);
            map.transform.localScale = Vector3.one * 1.25f;

            // �g��}�b�v�̈ʒu�����Z�b�g
            mapControl.localPosition = Vector3.zero;

            // �v���C���[�𓮂��Ȃ�����
            PlayerController.Instance.isEvents = true;

            UIEventsReload();
        }

        // �g��}�b�v��closeButton����������g��}�b�v��Destroy
        if (UIObject == closeButton)
        {
            // �g��}�b�v���\���ɂ��A���������}�b�v���폜
            enlargedMap.SetActive(false);
            Destroy(map); map = null;

            // �v���C���[�𓮂���悤�ɂ���
            PlayerController.Instance.isEvents = false;
        }
    }
}