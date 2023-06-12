using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PointerManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    CharacterSelection cs;

    public bool isSelect = false;



    private void Start()
    {
        cs = FindObjectOfType<CharacterSelection>();
    }


    //�J�[�\�����摜�ɐG�ꂽ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isSelect)    return;

        //��m�ɐG�ꂽ���m���n�C���C�g
        if (eventData.pointerEnter == cs.warrior)
        {
            cs.selectWarrior = true;
        }
        //���@�g���ɐG�ꂽ�疂�@�g�����n�C���C�g
        if (eventData.pointerEnter == cs.wizard)
        {
            cs.selectWizard = true;
        }

    }

    //�J�[�\�����摜���痣�ꂽ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelect)   return;

        //��m���痣�ꂽ���m�����[���C�g
        if (eventData.pointerEnter == cs.warrior)
        {
            cs.selectWarrior = false;
        }
        //���@�g�����痣�ꂽ�疂�@�g�������[���C�g
        if (eventData.pointerEnter == cs.wizard)
        {
            cs.selectWizard = false;
        }

    }

    //�摜���N���b�N������
    public void OnPointerClick(PointerEventData eventData)
    {
        //��m���N���b�N�������m���n�C���C�g
        if (eventData.pointerClick == cs.warrior)
        {
            isSelect = true;
            cs.selectWarrior = true;
            cs.selectWizard = false;

        }
        //���@�g�����N���b�N�����疂�@�g�����n�C���C�g
        if (eventData.pointerClick == cs.wizard)
        {
            isSelect = true;
            cs.selectWizard = true;
            cs.selectWarrior = false;
        }

    }
}