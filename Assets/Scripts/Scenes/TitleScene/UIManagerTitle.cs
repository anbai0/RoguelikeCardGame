using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManagerTitle : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;

    [Header("�Q�Ƃ���X�N���v�g")]
    [SerializeField] TitleSceneManager sceneManager;

    [Header("�\����؂�ւ���UI")]
    // �e�L�X�g�_�ł����邽�߂ɕK�v�Ȃ���
    [SerializeField] TextMeshProUGUI clickToStartText;
    [SerializeField]
    [Range(0.1f, 10.0f)] float duration = 1.0f;  //�e�L�X�g��_�ł�����Ԋu
    private Color32 startColor = new Color32(255, 255, 255, 255);
    private Color32 endColor = new Color32(255, 255, 255, 20);

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
        foreach (UIController UI in UIs)                            // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
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
        }
    }
    #endregion
    

    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        if (UIObject == UIObject.CompareTag("BackGround"))
        {
            sceneManager.LoadCharaSelectScene();
        }
    }


    void Update()
    {
        // �e�L�X�g��_�ł�����
        clickToStartText.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));
    }
}