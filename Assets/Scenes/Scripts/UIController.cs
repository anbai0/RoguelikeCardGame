using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


/// <summary>
/// UI�̃N���b�N�A�h���b�O�A���h�h���b�v�AEnter�AExit�̔�����s���܂��B
/// ������s������UI(Image�AObject�ȂǑ������ł��s���܂�)��
/// ���̃X�N���v�g���A�^�b�`���ĉ������B
/// </summary>
public class UIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("�h���b�O�A���h�h���b�v���������ꍇ��true�ɂ��Ă�������")]
    [SerializeField] 
    private bool isDraggable = false;   // true�ɂ����UI���h���b�O�A���h�h���b�v�ł���悤�ɂȂ�

    [Header("�e�C�x���g�B��{������Ȃ�")]
    public UnityEvent onLeftClick = null;
    public UnityEvent onRightClick = null;
    public UnityEvent onEnter = null;
    public UnityEvent onExit = null;
    public UnityEvent onBeginDrag = null;
    public UnityEvent onDrag = null;
    public UnityEvent onDrop = null;

    // �h���b�O�A���h�h���b�v�̏����Ɏg���܂�
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;



    public void OnPointerClick(PointerEventData eventData)
    {
        // ���N���b�N
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick?.Invoke();
        }

        // �E�N���b�N
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


    #region �h���b�O�A���h�h���b�v�̏���
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

        // �h���b�O���̃I�u�W�F�N�g�����̃I�u�W�F�N�g�ɑ΂��ă��[�U�[�̓��͂𓧉߂��邽�߂�false��
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

        // �h���b�O����̏I�����ɁA�h���b�v���ꂽ�I�u�W�F�N�g���Ăу��[�U�[�̓��͂��󂯕t����悤�ɂ��邽��true��
        canvasGroup.blocksRaycasts = true;

        onDrop?.Invoke();
    }
    #endregion




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
