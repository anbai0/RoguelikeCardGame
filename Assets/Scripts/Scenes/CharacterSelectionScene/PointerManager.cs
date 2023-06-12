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


    //カーソルが画像に触れたら
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isSelect)    return;

        //戦士に触れたら戦士をハイライト
        if (eventData.pointerEnter == cs.warrior)
        {
            cs.selectWarrior = true;
        }
        //魔法使いに触れたら魔法使いをハイライト
        if (eventData.pointerEnter == cs.wizard)
        {
            cs.selectWizard = true;
        }

    }

    //カーソルが画像から離れたら
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelect)   return;

        //戦士から離れたら戦士をローライト
        if (eventData.pointerEnter == cs.warrior)
        {
            cs.selectWarrior = false;
        }
        //魔法使いから離れたら魔法使いをローライト
        if (eventData.pointerEnter == cs.wizard)
        {
            cs.selectWizard = false;
        }

    }

    //画像をクリックしたら
    public void OnPointerClick(PointerEventData eventData)
    {
        //戦士をクリックしたら戦士をハイライト
        if (eventData.pointerClick == cs.warrior)
        {
            isSelect = true;
            cs.selectWarrior = true;
            cs.selectWizard = false;

        }
        //魔法使いをクリックしたら魔法使いをハイライト
        if (eventData.pointerClick == cs.wizard)
        {
            isSelect = true;
            cs.selectWizard = true;
            cs.selectWarrior = false;
        }

    }
}