using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField]
    private ShopController shopController;

    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;

    Vector3 defaultScale = Vector3.one * 0.37f;

    GameObject lastClickedCards;

    void Start()
    {
        UIEventReload();
    }

    public void UIEventReload()
    {
        UIs = Canvas.GetComponentsInChildren<UIController>();       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            //UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onRightClick.AddListener(() => UIRightClick(UI.gameObject));
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    void UILeftClick(GameObject UIObject)
    {
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            isClick = true;

            // �J�[�h�I����Ԃ̐؂�ւ�
            if (lastClickedCards != null && lastClickedCards != UIObject)              
            {
                lastClickedCards.transform.localScale = defaultScale;
                UIObject.transform.localScale += Vector3.one * 0.1f;
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
            lastClickedCards.transform.localScale = defaultScale;
            lastClickedCards = null;
            isClick = false;
        }
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
                UIObject.transform.localScale += Vector3.one * 0.1f;
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
                UIObject.transform.localScale = defaultScale;
            }
        }
    }
}
