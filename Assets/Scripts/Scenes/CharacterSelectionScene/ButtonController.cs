using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEditorInternal.ReorderableList;
using Unity.VisualScripting;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent onClick = null;

    private bool isPushed = false;

    public void ButtonActive(bool active)
    {
        isPushed = !active;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonClick();
        onClick?.Invoke();
    }

    private void OnButtonClick()
    {
        // ClickéûÇÃã§í èàóùÅiSEñ¬ÇÁÇ∑Ç»Ç«Åj

    }
}
