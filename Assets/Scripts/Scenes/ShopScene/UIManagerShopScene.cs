using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField]
    private ShopController shopController;

    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    bool isClick = false;

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

            // ��ڂɃN���b�N�����J�[�h��傫�����āA���̑O�ɃN���b�N�����J�[�h������������
            if (lastClickedCards != null)              
            {
                lastClickedCards.transform.localScale -= Vector3.one * 0.1f;
                UIObject.transform.localScale += Vector3.one * 0.1f;
            }

            lastClickedCards = UIObject;

        }

        // �����J�[�h���N���b�N������(�N���b�N�����I�u�W�F�N�g���Ō�ɃN���b�N�����J�[�h��������)
        if (UIObject == lastClickedCards)
        {
            shopController.BuyCards(UIObject);
        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isClick && UIObject == UIObject.CompareTag("BackGround"))
        {
            lastClickedCards.transform.localScale -= Vector3.one * 0.1f;
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
                UIObject.transform.localScale -= Vector3.one * 0.1f;
            }
        }
    }
}
