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
        // ボタンクリック時の共通処理（SEを再生するなど）
    }

    private void OnUIEntered()
    {
        // ポインタがUIに入った時の共通処理
    }

    private void OnUIExited()
    {
        // ポインタがUIから出た時の共通処理
    }
}
