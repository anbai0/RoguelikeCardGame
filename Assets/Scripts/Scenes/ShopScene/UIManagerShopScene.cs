using UnityEngine;
using UnityEngine.UI;

public class UIManagerShopScene : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private UIController[] UIs;

    public float amplitude = 1f;
    public float frequency = 1f;

    void Start()
    {
        ReloadUI();
    }

    public void ReloadUI()
    {
        UIs = Canvas.GetComponentsInChildren<UIController>();       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            //UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onClick.AddListener(() => UIClick(UI.gameObject));         //UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }
    }

    void UIClick(GameObject UIObject)
    {
        
    }

    void UIEnter(GameObject UIObject)
    {
        if(UIObject == UIObject.CompareTag("Cards"))
        {
            UIObject.transform.localScale += Vector3.one * 0.1f;
        }

        if (UIObject == UIObject.CompareTag("Relics"))
        {
            Animator anim = UIObject.GetComponent<Animator>();
            anim.SetTrigger("RelicJump");
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (UIObject == UIObject.CompareTag("Cards"))
        {
            UIObject.transform.localScale -= Vector3.one * 0.1f;
        }


    }
}
