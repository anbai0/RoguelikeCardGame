using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsController : MonoBehaviour
{
    [SerializeField] GameObject tipsUIICon;
    [SerializeField] GameObject cloaseTipsButton;
    [SerializeField] GameObject tipsUI;

    /// <summary>
    /// Tips�̕\����؂�ւ��܂��B
    /// Tips�\���A�C�R����Tips��\���A�C�R���Ɏg���܂��B
    /// </summary>
    /// <param name="isDisplayed"></param>
    public void ToggleTipsIcon(bool isDisplayed)
    {
        tipsUI.SetActive(isDisplayed);
    }

}
