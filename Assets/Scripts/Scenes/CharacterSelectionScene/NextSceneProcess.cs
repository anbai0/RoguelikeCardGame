using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NextSceneProcess : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    private Image image;
    public Sprite redButton;

    public PointerManager warrior_pm;
    public PointerManager wizard_pm;

    private bool selection = false;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (warrior_pm.isSelect || wizard_pm.isSelect)
        {
            selection = true;
            image.sprite = redButton;
        }

    }

    public void CharaSelect()
    {
        if (!selection) return;
            sceneController.sceneChange("FieldScene");
    }
}
