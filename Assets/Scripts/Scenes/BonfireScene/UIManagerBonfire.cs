using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BonfireScene��UIManager�ł��B
/// ToDo: �����b�N��Enter���ꂽ�Ƃ��̏����͂܂������I����ĂȂ��̂Ō�ł��܂��B
/// </summary>
public class UIManagerBonfire : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;
    GameObject lastClickedCards;

    Vector3 scaleReset = Vector3.one * 0.25f;    // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    [Header("�Q�Ƃ���X�N���v�g")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private RestController restController;
    [SerializeField] private BonfireManager bonfireManager;

    [Header("�\����؂�ւ���UI")]
    [SerializeField] GameObject BonfireUI;
    [SerializeField] GameObject restUI;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] GameObject enhanceButton;
    [SerializeField] GameObject applyEnhance;
    [SerializeField] GameObject closeEnhance;
    [SerializeField] GameObject restButton;
    [SerializeField] GameObject takeRestButton;
    [SerializeField] GameObject noRestButton;

    bool isRemoved = true;

    void Start()
    {
        restController.CheckRest("BonfireScene");
        UIEventReload();
    }

    public void UIEventReload()
    {
        if(!isRemoved)
            RemoveListeners();

        UIs = Canvas.GetComponentsInChildren<UIController>();       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        isRemoved = false;
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

    void UILeftClick(GameObject UIObject)
    {

        #region BonfireUI���ł̏���

        // "����"����������
        if (UIObject == enhanceButton)
        {
            BonfireUI.SetActive(false);     // ������ʂɍs��
        }

        // "�x�e���Ȃ�"����������
        if (UIObject == UIObject.CompareTag("ExitButton"))
        {
            sceneController.sceneChange("FieldScene");          // �t�B�[���h�ɖ߂�
        }

        // "�x�e"����������
        if (UIObject == restButton)
        {
            if(restController.CheckRest("BonfireScene"))            //�x�e�ł���ꍇ
            {
                restUI.SetActive(true);                             // �x�eUI��\��

                restController.ChengeRestText("BonfireScene");      // �x�etext���X�V
                UIEventReload();
            }
        }
        #endregion

        #region EnhanceUI���ł̏���

        // �J�[�h���N���b�N������
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // �����{�^���؂�ւ�
            applyEnhance.SetActive(true);
            closeEnhance.SetActive(false);
            UIEventReload();

            // �J�[�h�I����Ԃ̐؂�ւ�
            if (lastClickedCards != null && lastClickedCards != UIObject)              // ���ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                lastClickedCards.transform.localScale = scaleReset;
                UIObject.transform.localScale += scaleBoost;
            }

            lastClickedCards = UIObject;

        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            lastClickedCards.transform.localScale = scaleReset;
            lastClickedCards = null;
            isClick = false;

            // �����{�^���؂�ւ�
            applyEnhance.SetActive(false);
            closeEnhance.SetActive(true);
            UIEventReload();
        }

        // "��������"����������
        if (UIObject == applyEnhance && isClick)
        {
            bonfireManager.CardEnhance(lastClickedCards);   // �{�^����O�ɃN���b�N����Card��������

            sceneController.sceneChange("FieldScene");      // ����������t�B�[���h�ɖ߂�
        }

        // "�������Ȃ�"����������
        if (UIObject == closeEnhance)
        {
            BonfireUI.SetActive(true);          // ���Ή�ʂɖ߂�
            UIEventReload();
        }
        #endregion

        #region RestUI���ł̏���

        // "�x�e����"����������
        if (UIObject == takeRestButton)
        {
            restController.Rest("BonfireScene");                // �񕜂���
            restUI.SetActive(false);
            sceneController.sceneChange("FieldScene");          // �t�B�[���h�ɖ߂�
        }
        // "�x�e���Ȃ�"����������
        if (UIObject == noRestButton)
        {
            restUI.SetActive(false);        // ���Ή�ʂɖ߂�
        }

        #endregion
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = scaleReset;
            }
        }
    }
}
