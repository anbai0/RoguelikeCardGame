using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PointerManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    CharacterSelection cs;

    bool isSelect = false;



    private void Start()
    {
        cs = FindObjectOfType<CharacterSelection>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {


        //if (cs)
        //{
        //    //åªç›ÇÃPointerManagerÇ™charcterÇÃPointerManagerÇ∆àÍívÇµÇƒÇ¢ÇÈèÍçátrue,ÇªÇ§Ç≈Ç»Ç¢èÍçáfalseÇë„ì¸
        //    cs.warrior_s.selectImage = this == cs.warrior.GetComponent<PointerManager>();
        //    cs.wizard_s.selectImage = this == cs.wizard.GetComponent<PointerManager>();
        //}

        if (eventData.pointerClick == cs.warrior)
        {
            isSelect = true;
            cs.selectWarrior = true;

        }
        if (eventData.pointerClick == cs.wizard)
        {
            isSelect = true;
            cs.selectWizard = true;
        }

        

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isSelect)
        {
            if (eventData.pointerEnter == cs.warrior)
            {
                cs.selectWarrior = true;
            }
            if (eventData.pointerEnter == cs.wizard)
            {
                cs.selectWizard = true;
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelect)
        {
            if (eventData.pointerEnter == cs.warrior)
            {
                cs.selectWarrior = false;
            }
            if (eventData.pointerEnter == cs.wizard)
            {
                cs.selectWizard = false;
            }
        }

    }
}