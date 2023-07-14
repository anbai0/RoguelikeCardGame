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



    void Start()
    {
        UIEventReload();
        restUI.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void UIEventReload()
    {
        UIs = Canvas.GetComponentsInChildren<UIController>();       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            //UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    void UILeftClick(GameObject UIObject)
    {

        #region ShopUI���ł̏���
        if (UIObject == buy)
        {
            shopUI.SetActive(false);
        }
        if (UIObject == CloseShopping)
        {
            shopUI.SetActive(true);
        }

        if (UIObject.tag == "ExitButton")
        {
            sceneController.sceneChange("FieldScene");
        }

        if (UIObject == rest)
        {
            restUI.SetActive(true);
        }
        if (UIObject == RestButton)
        {
            //�񕜂���
            shopController.Rest();
        }
        if (UIObject == noRestButton)
        {
            restUI.SetActive(false);
        }

        #endregion

        #region ShoppingUI���ł̏���
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // �J�[�h�I����Ԃ̐؂�ւ�
            if (lastClickedCards != null && lastClickedCards != UIObject)              
            {
                lastClickedCards.transform.localScale = scaleReset;
                UIObject.transform.localScale += scaleBoost;
            }
            else if (UIObject == lastClickedCards)      // �����J�[�h��2��N���b�N������(�J�[�h�w��)
            {
                shopController.BuyCards(UIObject);

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
        #endregion
    }

    void UIRightClick(GameObject UIObject)
    {

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
            Animator anim = UIObject.GetComponent<Animator>();
            anim.SetTrigger("RelicJump");
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
