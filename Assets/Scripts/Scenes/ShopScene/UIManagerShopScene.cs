using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    [SerializeField] private ShopController shopController;

    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;
    GameObject lastClickedCards;

    Vector3 scaleReset = Vector3.one * 0.37f;    // ���̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    Vector3 scaleBoost = Vector3.one * 0.1f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    // �؂�ւ���UI
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject restUI;
    // �N���b�N��ɎQ�Ƃ���I�u�W�F�N�g
    [SerializeField] GameObject buy;
    [SerializeField] GameObject CloseShopping;
    [SerializeField] GameObject rest;
    [SerializeField] GameObject RestButton;
    [SerializeField] GameObject noRestButton;

    bool isRemoved = true;

    void Start()
    {
        shopController.CheckRest();
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
        if (UIObject == buy)
        {
            shopUI.SetActive(false);
            shopController.PriceTextCheck();
            shopController.HasHealPotion();
        }
        // "�X���o��"����������
        if (UIObject.tag == "ExitButton")
        {
            sceneController.sceneChange("FieldScene");
        }
        // "�x�e"����������
        if (UIObject == rest)
        {
            if(shopController.CheckRest())      //�x�e�ł���ꍇ
            {
                restUI.SetActive(true);
                UIEventReload();
            }
        }
        #endregion

        #region ShoppingUI���ł̏���

        // �J�[�h���N���b�N������
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // �J�[�h�I����Ԃ̐؂�ւ�
            if (lastClickedCards != null && lastClickedCards != UIObject)              // ���ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                lastClickedCards.transform.localScale = scaleReset;
                UIObject.transform.localScale += scaleBoost;
            }
            else if (UIObject == lastClickedCards)      // �����J�[�h��2��N���b�N������(�J�[�h�w��)
            {
                shopController.BuyItem(UIObject, "Card");
                shopController.PriceTextCheck();
            }

            lastClickedCards = UIObject;

        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            lastClickedCards.transform.localScale = scaleReset;
            lastClickedCards = null;
            isClick = false;
        }

        // �������񂱂�
        if (UIObject == UIObject.CompareTag("Relics"))
        {
            shopController.BuyItem(UIObject, "Relic");
            shopController.PriceTextCheck();
        }

        // ���������I����{�^������������
        if (UIObject == CloseShopping)
        {
            shopUI.SetActive(true);
            shopController.CheckRest();
        }
        #endregion

        #region RestUI���ł̏���

        // "�x�e����"����������
        if (UIObject == RestButton)
        {
            shopController.Rest();      // �񕜂���
            shopController.CheckRest();
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
            }
        }

        if (UIObject == UIObject.CompareTag("Relics"))
        {
            UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);
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

        if (UIObject == UIObject.CompareTag("Relics"))
        {
            UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);
        }
    }
}
