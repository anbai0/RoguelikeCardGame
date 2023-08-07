using DG.Tweening;
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
    private GameObject lastClickedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 relicScaleReset = Vector3.one * 2.5f;
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    [Header("�Q�Ƃ���X�N���v�g")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private ShopController shopController;
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
            shopController.PriceTextCheck();
            shopController.HasHealPotion();
        }
        // "�X���o��"����������
        if (UIObject == UIObject.CompareTag("ExitButton"))
        {
            sceneController.SceneChange("FieldScene");
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
        if (UIObject == UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isClick = true;

            // �A�C�e���I����Ԃ̐؂�ւ�
            if (lastClickedItem != null && lastClickedItem != UIObject)    // 2��ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
                if (lastClickedItem == lastClickedItem.CompareTag("Cards"))
                {
                    lastClickedItem.transform.localScale = cardScaleReset;
                    lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                }
                    
                if (lastClickedItem == lastClickedItem.CompareTag("Relics"))
                {
                    lastClickedItem.transform.localScale = relicScaleReset;
                    lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);
                    lastClickedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
                }

                // 2��ڂɑI�������A�C�e����I����Ԃɂ���
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);

                // 2��ڂɑI�������A�C�e���������b�N�������ꍇ�A�����b�N�̐�����\��
                if (UIObject == UIObject.CompareTag("Relics"))
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);

            }
            else if (UIObject == lastClickedItem)      // �����A�C�e����2��N���b�N������(�A�C�e���w��)
            {
                // �I�������A�C�e���𔃂�
                if (UIObject == UIObject.CompareTag("Cards"))
                    shopController.BuyItem(UIObject, "Card");

                if (UIObject == UIObject.CompareTag("Relics"))
                    shopController.BuyItem(UIObject, "Relic");

                shopController.PriceTextCheck();            // �l�i�e�L�X�g�X�V

                lastClickedItem = null;                     // �I����ԃ��Z�b�g
                isClick = false;
            }

            lastClickedItem = UIObject;

        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastClickedItem == lastClickedItem.CompareTag("Cards"))
            {
                lastClickedItem.transform.localScale = cardScaleReset;
                lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastClickedItem == lastClickedItem.CompareTag("Relics"))
            {
                lastClickedItem.transform.localScale = relicScaleReset;
                lastClickedItem.transform.GetChild(0).gameObject.SetActive(false);
            }

            lastClickedItem = null;         // �I����ԃ��Z�b�g
            isClick = false;
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
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
            }

            if (UIObject == UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // �A�C�e���̌����ڂ�I����Ԃɂ���
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);        // �����b�N�̐�����\��
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isClick)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (UIObject == UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
            }
        }
    }
}
