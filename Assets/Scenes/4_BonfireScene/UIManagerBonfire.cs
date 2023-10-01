using UnityEngine;
using SelfMadeNamespace;
using DG.Tweening;

/// <summary>
/// BonfireScene��UIManager�ł��B
/// ToDo: �����b�N��Enter���ꂽ�Ƃ��̏����͂܂������I����ĂȂ��̂Ō�ł��܂��B
/// </summary>
public class UIManagerBonfire : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    private bool isSelected = false;
    private GameObject lastSelectedCards;

    private Vector3 scaleReset = Vector3.one * 0.25f;     // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] private RestController restController;
    [SerializeField] private BonfireManager bonfireManager;

    [Header("�\����؂�ւ���UI")]
    [SerializeField] private GameObject BonfireUI;
    [SerializeField] private GameObject restUI;
    [SerializeField] private Transform enhancedCardHolder;
    [SerializeField] private RectTransform triangularArrow;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] private GameObject closeBonfire;
    [SerializeField] private GameObject enhanceButton;
    [SerializeField] private GameObject applyEnhance;
    [SerializeField] private GameObject closeEnhance;
    [SerializeField] private GameObject restButton;
    [SerializeField] private GameObject takeRestButton;
    [SerializeField] private GameObject noRestButton;

    Tween arrowTween;

    void Start()
    {
        restController.CheckRest("BonfireScene");
        arrowTween = triangularArrow.DOAnchorPosX(-10f, 0.35f).SetLoops(-1, LoopType.Yoyo);
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

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                                // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
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
        }
    }
    #endregion


    void UILeftClick(GameObject UIObject)
    {

        #region BonfireUI���ł̏���

        // "����"����������
        if (UIObject == enhanceButton)
        {
            AudioManager.Instance.PlaySE("�I����1");
            BonfireUI.SetActive(false);     // ������ʂɍs��
        }

        // "�x�e���Ȃ�"����������
        if (UIObject == closeBonfire && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("�I����1");
            bonfireManager.UnLoadBonfireScene();          // �t�B�[���h�ɖ߂�
        }

        // "�x�e"����������
        if (UIObject == restButton)
        {
            if(restController.CheckRest("BonfireScene"))            //�x�e�ł���ꍇ
            {
                restUI.SetActive(true);                             // �x�eUI��\��

                AudioManager.Instance.PlaySE("�I����1");
            }
        }
        #endregion

        #region EnhanceUI���ł̏���

        // �J�[�h���N���b�N������
        if (UIObject.CompareTag("Cards"))
        {
            isSelected = true;
            AudioManager.Instance.PlaySE("�I����1");

            // �����{�^���؂�ւ�
            applyEnhance.SetActive(true);
            closeEnhance.SetActive(false);

            // ������̃J�[�h��\��
            enhancedCardHolder.gameObject.SetActive(true);
            bonfireManager.DisplayEnhancedCard(UIObject);

            // �J�[�h�I����Ԃ̐؂�ւ�
            if (lastSelectedCards != null && lastSelectedCards != UIObject)              // ���ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                lastSelectedCards.transform.localScale = scaleReset;
                lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);
            }

            lastSelectedCards = UIObject;

        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isSelected && UIObject.CompareTag("BackGround"))
        {
            lastSelectedCards.transform.localScale = scaleReset;
            lastSelectedCards.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            lastSelectedCards = null;
            isSelected = false;
            enhancedCardHolder.gameObject.SetActive(false);     // ������̃J�[�h���\����

            // �����{�^���؂�ւ�
            applyEnhance.SetActive(false);
            closeEnhance.SetActive(true);
            
        }

        // "��������"����������
        if (UIObject == applyEnhance && isSelected && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("�I����1");

            bonfireManager.CardEnhance(lastSelectedCards);   // �J�[�h����

            PutOutCampfire();       // ���΂̉΂�����

            bonfireManager.UnLoadBonfireScene();      // ����������t�B�[���h�ɖ߂�
        }

        // "�������Ȃ�"����������
        if (UIObject == closeEnhance)
        {
            AudioManager.Instance.PlaySE("�I����1");
            enhancedCardHolder.gameObject.SetActive(false);     // ������̃J�[�h���\����
            BonfireUI.SetActive(true);          // ���Ή�ʂɖ߂�
            
        }
        #endregion

        #region RestUI���ł̏���

        // "�x�e����"����������
        if (UIObject == takeRestButton && !isClick)
        {
            isClick = true;
            AudioManager.Instance.PlaySE("��");

            restController.Rest("BonfireScene");                // �񕜂���
            restUI.SetActive(false);

            PutOutCampfire();       // ���΂̉΂�����

            bonfireManager.UnLoadBonfireScene();          // �x�e������t�B�[���h�ɖ߂�
        }
        // "�x�e���Ȃ�"����������
        if (UIObject == noRestButton)
        {
            AudioManager.Instance.PlaySE("�I����1");
            restUI.SetActive(false);        // ���Ή�ʂɖ߂�
        }

        #endregion
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                AudioManager.Instance.PlaySE("OnCursor");
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = scaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������
            }
        }
    }


    /// <summary>
    /// ���΂̉΂Ɠ����蔻��������܂��B
    /// </summary>
    void PutOutCampfire()
    {
        // ���΂̉΂�����
        ParticleSystem particle = PlayerController.Instance.bonfire.GetComponentInChildren<ParticleSystem>();
        particle.Stop();
        // ���΂̃|�C���g���C�g������
        particle.transform.GetChild(0).gameObject.SetActive(false);

        // ���΂̓����蔻��������B
        PlayerController.Instance.bonfire.GetComponent<BoxCollider>().enabled = false;

        // �}�b�v�A�C�R���̏���
        PlayerController.Instance.UpdateBonfireIcon();
    }
}