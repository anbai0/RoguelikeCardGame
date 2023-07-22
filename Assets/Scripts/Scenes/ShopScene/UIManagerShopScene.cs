using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ShopScene��UIManager�ł��B
/// ToDo: �����b�N��Enter���ꂽ�Ƃ��̏����͂܂������I����ĂȂ��̂Ō�ł��܂��B
/// </summary>
public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;
    GameObject lastClickedItem;

    Vector3 cardScaleReset = Vector3.one * 0.37f;    // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    Vector3 relicScaleReset = Vector3.one * 2.5f;
    Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    [Header("�Q�Ƃ���X�N���v�g")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private RestController restController;

    [Header("�\����؂�ւ���UI")]
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject restUI;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] GameObject buyButton;
    [SerializeField] GameObject closeShopping;
    [SerializeField] GameObject restButton;
    [SerializeField] GameObject takeRestButton;
    [SerializeField] GameObject noRestButton;

    bool isRemoved = true;

    void Start()
    {
        restController.CheckRest("ShopScene");
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
            sceneController.sceneChange("FieldScene");
        }
        // "�x�e"����������
        if (UIObject == restButton)
        {
            if(restController.CheckRest("ShopScene"))      //�x�e�ł���ꍇ
            {
                restUI.SetActive(true);

                restController.ChengeRestText("ShopScene");
                UIEventReload();
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
