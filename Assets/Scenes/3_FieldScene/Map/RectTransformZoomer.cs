using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The RectTransform with this script attached is zoomed by scrolling and
/// moved up, down, left, or right by dragging.
/// This script will only work if the Canvas setting is either
/// * ScreenSpace - Overlay
/// * ScreenSpace - Camera
/// It also works even if the "UI Scale Mode" of CanvasScaler is set to "Scale With Screen Size".
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class RectTransformZoomer : MonoBehaviour, IDragHandler
{
    [SerializeField] Canvas uiCanvas;
    [SerializeField] float zoomSpeed = 1f;
    [SerializeField] float minZoomRate = 1f;
    [SerializeField] float maxZoomRate = 10f;

    RectTransform targetContent;
    CanvasScaler canvasScaler;

    float CurrentZoomScale => targetContent.localScale.x;

    bool ShouldScaleDragMove =>
        canvasScaler != null &&
        canvasScaler.IsActive() &&
        canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize;

    void Awake()
    {
        targetContent = GetComponent<RectTransform>();
        canvasScaler = uiCanvas.GetComponent<CanvasScaler>();
    }

    void Update()
    {
        var scroll = Input.mouseScrollDelta.y;
        ScrollToZoomMap(Input.mousePosition, scroll);

        // “®‚©‚¹‚é—Ìˆæ‚ð§ŒÀ‚µ‚Ä‚Ü‚·B
        if (transform.localPosition.x > 450)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.x = 450f;
            transform.localPosition = newPosition;
        }
        if (transform.localPosition.x < -450)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.x = -450f;
            transform.localPosition = newPosition;
        }
        if (transform.localPosition.y > 450)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.y = 450f;
            transform.localPosition = newPosition;
        }
        if (transform.localPosition.y < -450)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.y = -450f;
            transform.localPosition = newPosition;
        }
    }

    /// <summary>
    /// Adjust to keep the mouse's position at the same location on the content even after zooming.
    /// </summary>
    /// <param name="mousePosition">Current mouse position.</param>
    /// <param name="scroll">Mouse scroll delta.</param>
    public void ScrollToZoomMap(Vector2 mousePosition, float scroll)
    {
        GetLocalPointInRectangle(mousePosition, out var beforeZoomLocalPosition);

        var afterZoomScale = CurrentZoomScale + scroll * zoomSpeed;
        afterZoomScale = Mathf.Clamp(afterZoomScale, minZoomRate, maxZoomRate);
        DoZoom(afterZoomScale);

        GetLocalPointInRectangle(mousePosition, out var afterZoomLocalPosition);

        var positionDiff = afterZoomLocalPosition - beforeZoomLocalPosition;
        var scaledPositionDiff = positionDiff * afterZoomScale;
        var newAnchoredPosition = targetContent.anchoredPosition + scaledPositionDiff;

        targetContent.anchoredPosition = newAnchoredPosition;
    }

    /// <summary>
    /// Move the window according to the amount of drag.
    /// Automatically called by IDragHandler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        var dragMoveDelta = eventData.delta;

        if (ShouldScaleDragMove)
        {
            var dragMoveScale = canvasScaler.referenceResolution.x / Screen.width;
            dragMoveDelta *= dragMoveScale;
        }

        targetContent.anchoredPosition += dragMoveDelta;
    }

    void DoZoom(float zoomScale)
    {
        targetContent.localScale = Vector3.one * zoomScale;
    }

    void GetLocalPointInRectangle(Vector2 mousePosition, out Vector2 localPosition)
    {
        var targetCamera = uiCanvas.renderMode switch
        {
            RenderMode.ScreenSpaceCamera => uiCanvas.worldCamera,
            RenderMode.ScreenSpaceOverlay => null,
            _ => throw new System.NotSupportedException(),
        };

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetContent, mousePosition, targetCamera, out localPosition);
    }
}