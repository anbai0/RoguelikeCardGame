using UnityEngine;
using UnityEngine.UI;

public class UIManagerBase : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    void Start()
    {
        UIs = parent.GetComponentsInChildren<UIController>();       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
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

    }

    void UIExit(GameObject UIObject)
    {

    }
}
