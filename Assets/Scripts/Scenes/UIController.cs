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
    public UnityEvent onLeftClick = null;
    public UnityEvent onRightClick = null;
    public UnityEvent onEnter = null;
    public UnityEvent onExit = null;
    public UnityEvent onBeginDrag = null;
    public UnityEvent onDrag = null;
    public UnityEvent onDrop = null;
    public UnityEvent onDropEmpty = null;

    [SerializeField] private bool isDraggable = false;   // trueにするとUIをドラッグアンドドロップできるようになる

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup; 
    Vector3 initialPosition;        // ドラッグ前の位置を格納



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

        onBeginDrag?.Invoke();

        initialPosition = transform.position;        // ドラッグ前の位置を記録

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        onDrag?.Invoke();

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 1f;                         // 透明度を戻す

        // ドラッグ操作の終了時に、ドロップされたオブジェクトが再びユーザーの入力を受け付けるようにするためtrueに
        canvasGroup.blocksRaycasts = true;

        onDrop?.Invoke();

    }
    #endregion



    public void OnPointerClick(PointerEventData eventData)
    {
        // 左クリック
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick?.Invoke();
        }

        // 右クリック
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
