using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public UnityEvent onClick = null;
    [SerializeField] public UnityEvent onEnter = null;
    [SerializeField] public UnityEvent onExit = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnUIClicked();
        onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnUIEntered();
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUIExited();
        onExit?.Invoke();
    }

    private void OnUIClicked()
    {
        // �{�^���N���b�N���̋��ʏ����iSE���Đ�����Ȃǁj
    }

    private void OnUIEntered()
    {
        // �|�C���^��UI�ɓ��������̋��ʏ���
    }

    private void OnUIExited()
    {
        // �|�C���^��UI����o�����̋��ʏ���
    }
}
