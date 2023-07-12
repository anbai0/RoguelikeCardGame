using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


/// <summary>
/// UIのクリックの判定を行います。
/// 判定を行いたいUI(Image、Objectなど多分何でも行けます)に
/// このスクリプトをアタッチして下さい
/// </summary>
public class UIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public UnityEvent onLeftClick = null;
    [SerializeField] public UnityEvent onRightClick = null;
    [SerializeField] public UnityEvent onEnter = null;
    [SerializeField] public UnityEvent onExit = null;
    [SerializeField] public UnityEvent onDrop = null;

    [SerializeField] private bool isDraggable = true;   // trueにするとUIをドラッグアンドドロップできるようになる

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    Vector2 initialPosition;     // ドラッグ前の位置を格納



    #region ドラッグアンドドロップの処理
    private void Awake()
    {
        if (!isDraggable) return;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 0.3f;                       // ドラッグ中半透明にする

        // ドラッグ中のオブジェクトが他のオブジェクトに対してユーザーの入力を透過するためにfalseに
        canvasGroup.blocksRaycasts = false;
        initialPosition = rectTransform.anchoredPosition;    // ドラッグ前の位置を記録
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 1f;                         // 透明度を戻す

        // ドラッグ操作の終了時に、ドロップされたオブジェクトが再びユーザーの入力を受け付けるようにするためtrueに
        canvasGroup.blocksRaycasts = true;

        // ドロップした先に何かがあれば
        if (eventData.pointerEnter != null)
        {

            onDrop?.Invoke();
            rectTransform.anchoredPosition = initialPosition;    // ドラッグ前の位置に戻す
        }
        else // 何もない場合                                   
        {
            rectTransform.anchoredPosition = initialPosition;    // ドラッグ前の位置に戻す
        }
    }
    #endregion



    public void OnPointerClick(PointerEventData eventData)
    {
        // 右クリック
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick?.Invoke();
        }

        // 左クリック
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick?.Invoke();
        }

    }

    // カーソルが触れたとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke();
    }

    // カーソルが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke();
    }



    //private void OnUIClicked()
    //{
    //    // ボタンクリック時の共通処理（SEを再生するなど）
    //}

    //private void OnUIEntered()
    //{
    //    // ポインタがUIに入った時の共通処理
    //}

    //private void OnUIExited()
    //{
    //    // ポインタがUIから出た時の共通処理
    //}
}
