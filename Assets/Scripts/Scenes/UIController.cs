using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


/// <summary>
/// UI�̃N���b�N�̔�����s���܂��B
/// ������s������UI(Image�AObject�ȂǑ������ł��s���܂�)��
/// ���̃X�N���v�g���A�^�b�`���ĉ�����
/// </summary>
public class UIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public UnityEvent onLeftClick = null;
    [SerializeField] public UnityEvent onRightClick = null;
    [SerializeField] public UnityEvent onEnter = null;
    [SerializeField] public UnityEvent onExit = null;
    [SerializeField] public UnityEvent onDrop = null;

    [SerializeField] private bool isDraggable = true;   // true�ɂ����UI���h���b�O�A���h�h���b�v�ł���悤�ɂȂ�

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    Vector2 initialPosition;     // �h���b�O�O�̈ʒu���i�[



    #region �h���b�O�A���h�h���b�v�̏���
    private void Awake()
    {
        if (!isDraggable) return;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 0.3f;                       // �h���b�O���������ɂ���

        // �h���b�O���̃I�u�W�F�N�g�����̃I�u�W�F�N�g�ɑ΂��ă��[�U�[�̓��͂𓧉߂��邽�߂�false��
        canvasGroup.blocksRaycasts = false;
        initialPosition = rectTransform.anchoredPosition;    // �h���b�O�O�̈ʒu���L�^
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 1f;                         // �����x��߂�

        // �h���b�O����̏I�����ɁA�h���b�v���ꂽ�I�u�W�F�N�g���Ăу��[�U�[�̓��͂��󂯕t����悤�ɂ��邽��true��
        canvasGroup.blocksRaycasts = true;

        // �h���b�v������ɉ����������
        if (eventData.pointerEnter != null)
        {

            onDrop?.Invoke();
            rectTransform.anchoredPosition = initialPosition;    // �h���b�O�O�̈ʒu�ɖ߂�
        }
        else // �����Ȃ��ꍇ                                   
        {
            rectTransform.anchoredPosition = initialPosition;    // �h���b�O�O�̈ʒu�ɖ߂�
        }
    }
    #endregion



    public void OnPointerClick(PointerEventData eventData)
    {
        // �E�N���b�N
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick?.Invoke();
        }

        // ���N���b�N
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick?.Invoke();
        }

    }

    // �J�[�\�����G�ꂽ�Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke();
    }

    // �J�[�\�������ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke();
    }



    //private void OnUIClicked()
    //{
    //    // �{�^���N���b�N���̋��ʏ����iSE���Đ�����Ȃǁj
    //}

    //private void OnUIEntered()
    //{
    //    // �|�C���^��UI�ɓ��������̋��ʏ���
    //}

    //private void OnUIExited()
    //{
    //    // �|�C���^��UI����o�����̋��ʏ���
    //}
}
