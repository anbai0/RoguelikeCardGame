using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Transform cardParent;
    public float zoomSize = 1.1f;
    public float zoomPos = 0.3f;
    Vector3 cardPos;
    bool isDragging;
    private void Start()
    {
        cardPos = transform.position;
        isDragging = false;
    }
    public void OnBeginDrag(PointerEventData eventData) // �h���b�O���n�߂�Ƃ��ɍs������
    {
        transform.localScale = Vector3.one;
        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycasts���I�t�ɂ���
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData) // �h���b�O�������ɋN��������
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // �J�[�h�𗣂����Ƃ��ɍs������
    {
        transform.position = cardPos;
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycasts���I���ɂ���
        GameObject.Find("CardPlace").GetComponent<SortDeck>().Sort();//���O���Ƀ\�[�g������
        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)//�}�E�X�̃|�C���^�[�����������Ƃ��̏���
    {
        if (!Input.GetMouseButton(0) && !isDragging)
        {
            cardPos = transform.position;
            transform.position += Vector3.up * zoomPos;
            transform.localScale = Vector3.one * zoomSize;
        }
    }

    public void OnPointerExit(PointerEventData eventData)//�}�E�X�̃|�C���^�[�����ꂽ�Ƃ��̏���
    {
        if (!Input.GetMouseButton(0) && !isDragging)
        {
            transform.position = cardPos;
            transform.localScale = Vector3.one;
        }
    }
}
