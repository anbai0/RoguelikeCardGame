using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsController : MonoBehaviour
{
    [SerializeField] GameObject tipsUIICon;
    [SerializeField] GameObject cloaseTipsButton;
    [SerializeField] GameObject tipsUI;

    /// <summary>
    /// Tipsの表示を切り替えます。
    /// Tips表示アイコンとTips非表示アイコンに使います。
    /// </summary>
    /// <param name="isDisplayed"></param>
    public void ToggleTipsIcon(bool isDisplayed)
    {
        tipsUI.SetActive(isDisplayed);
    }

}
