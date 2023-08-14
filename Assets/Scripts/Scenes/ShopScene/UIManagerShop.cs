using DG.Tweening;
using SelfMadeNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ShopScene��UIManager�ł��B
/// ToDo: �����b�N��Enter���ꂽ�Ƃ��̏����͂܂������I����ĂȂ��̂Ō�ł��܂��B
/// </summary>
public class UIManagerShop : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;
    private bool isClick = false;

    public bool isSelected = false;
    public GameObject lastSelectedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 relicScaleReset = Vector3.one * 2.5f;
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private RestController restController;

    [Header("�\����؂�ւ���UI")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject restUI;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject closeShopping;
    [SerializeField] private GameObject restButton;
    [SerializeField] private GameObject takeRestButton;
    [SerializeField] private GameObject noRestButton;


    void Start()
    {
        restController.CheckRest("ShopScene");      // �x�e�̃e�L�X�g�X�V
        UIEventsReload();
    }


    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        if(!isRemoved)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
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
    #endregion


    void UILeftClick(GameObject UIObject)
    {

        #region ShopUI���ł̏���

        // "�w��"����������
        if (UIObject == buyButton)
        {
            shopUI.SetActive(false);
            shopManager.PriceTextCheck();
            shopManager.HasHealPotion();
        }
        // "�X���o��"����������
        if (UIObject.CompareTag("ExitButton") && !isClick)
        {
            isClick = true;
            shopManager.ExitShop();     // ShopScene���\��

            // �t�B�[���h�V�[���̃v���C���[�𓮂���悤�ɂ���
            PlayerController.isPlayerActive = true;

            isClick = false;
        }
        // "�x�e"����������
        if (UIObject == restButton)
        {
            if(restController.CheckRest("ShopScene"))      //�x�e�ł���ꍇ
            {
                restUI.SetActive(true);

                restController.ChengeRestText("ShopScene");
            }
        }
        #endregion

        #region ShoppingUI���ł̏���

        // �A�C�e�����N���b�N������
        if (UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isSelected = true;

            // �A�C�e���I����Ԃ̐؂�ւ�
            if (lastSelectedItem != null && lastSelectedItem != UIObject)    // 2��ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
                if (lastSelectedItem.CompareTag("Cards"))
                {
                    lastSelectedItem.transform.localScale = cardScaleReset;
                    lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                }
                    
                if (lastSelectedItem.CompareTag("Relics"))
                {
                    lastSelectedItem.transform.localScale = relicScaleReset;
                    lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
                    lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
                }

                // 2��ڂɑI�������A�C�e����I����Ԃɂ���
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);

                // 2��ڂɑI�������A�C�e���������b�N�������ꍇ�A�����b�N�̐�����\��
                if (UIObject.CompareTag("Relics"))
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);
                
            }
            if (UIObject == lastSelectedItem)      // �����A�C�e����2��N���b�N������(�A�C�e���w��)
            {
                // �I�������A�C�e���𔃂�
                if (UIObject.CompareTag("Cards"))
                    shopManager.BuyItem(UIObject, "Card");

                if (UIObject.CompareTag("Relics"))
                    shopManager.BuyItem(UIObject, "Relic");

                shopManager.PriceTextCheck();            // �l�i�e�L�X�g�X�V

                lastSelectedItem = null;                     // �I����ԃ��Z�b�g
                isSelected = false;
            }
            else
            {
                lastSelectedItem = UIObject;    // �Ō�ɃN���b�N�����A�C�e�����X�V
            }
        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isSelected && UIObject.CompareTag("BackGround"))
        {
            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
            }

            lastSelectedItem = null;         // �I����ԃ��Z�b�g
            isSelected = false;
        }

        // "���������I����"����������
        if (UIObject == closeShopping)
        {
            shopUI.SetActive(true);
            restController.CheckRest("ShopScene");
        }
        #endregion

        #region RestUI���ł̏���

        // "�x�e����"����������
        if (UIObject == takeRestButton)
        {
            restController.Rest("ShopScene");      // �񕜂���
            restController.CheckRest("ShopScene");
            restUI.SetActive(false);
        }
        // "�x�e���Ȃ�"����������
        if (UIObject == noRestButton)
        {
            restUI.SetActive(false);
        }

        #endregion
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // �A�C�e���̌����ڂ�I����Ԃɂ���
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);        // �����b�N�̐�����\��
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
            }
        }
    }
}
