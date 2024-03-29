using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


/// <summary>
/// UIのクリック、ドラッグアンドドロップ、Enter、Exitの判定を行います。
/// 判定を行いたいUI(Image、Objectなど多分何でも行けます)に
/// このスクリプトをアタッチして下さい。
/// </summary>
public class UIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("ドラッグアンドドロップをしたい場合はtrueにしてください")]
    [SerializeField] 
    private bool isDraggable = false;   // trueにするとUIをドラッグアンドドロップできるようになる

    [Header("各イベント。基本いじらない")]
    public UnityEvent onLeftClick = null;
    public UnityEvent onRightClick = null;
    public UnityEvent onEnter = null;
    public UnityEvent onExit = null;
    public UnityEvent onBeginDrag = null;
    public UnityEvent onDrag = null;
    public UnityEvent onDrop = null;

    // ドラッグアンドドロップの処理に使います
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;



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


    #region ドラッグアンドドロップの処理
    private void Awake()
    {
        if (!isDraggable) return;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;
        if (eventData.button == PointerEventData.InputButton.Middle) return;
        if (!isDraggable) return;
        if (eventData.button == PointerEventData.InputButton.Right) return;

        // ドラッグ中のオブジェクトが他のオブジェクトに対してユーザーの入力を透過するためにfalseに
        canvasGroup.blocksRaycasts = false;

        onBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;
        if (eventData.button == PointerEventData.InputButton.Middle) return;
        if (!isDraggable) return;
        if (eventData.button == PointerEventData.InputButton.Right) return;

        onDrag?.Invoke();

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;
        if (eventData.button == PointerEventData.InputButton.Middle) return;
        if (!isDraggable) return;
        if (eventData.button == PointerEventData.InputButton.Right) return;

        // ドラッグ操作の終了時に、ドロップされたオブジェクトが再びユーザーの入力を受け付けるようにするためtrueに
        canvasGroup.blocksRaycasts = true;

        onDrop?.Invoke();
    }
    #endregion




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
