using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CharacterSelection : MonoBehaviour
{
    public GameObject warrior;
    public GameObject wizard;

    public bool selectWarrior;
    public bool selectWizard;

    private Color32 originalColor;
    private Color32 targetColor;

    private float duration = 0.25f;      //色が変わるまでの秒数
    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float wizardElapsedTime = 0f;
    private float wizardLate;

    private void Start()
    {
        // 初期の色
        originalColor = new Color32(60, 60, 60, 255);

        // ハイライト時の色
        targetColor = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        if (selectWarrior)
        {
            selectWizard = false;

            highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());
        }
        else if (selectWizard)
        {
            selectWarrior = false;
            highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());
        }

    }
    public void highLight(Image warriorImage,Image wizardImage)
    {

        if (selectWarrior)
        {
            warriorElapsedTime += Time.deltaTime;
            Debug.Log(warriorElapsedTime);
            warriorLate = Mathf.Clamp01(warriorElapsedTime/duration);
            warriorImage.color = Color32.Lerp(originalColor,targetColor,warriorLate);
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
        }
        else if (selectWizard)
        {
            Debug.Log(wizardElapsedTime);
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime/duration);
            wizardImage.color = Color32.Lerp(originalColor,targetColor,wizardLate);
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
        }
    }


    //public void highLight(Image image)
    //{

    //    if (selectWarrior)
    //    {
    //        warriorElapsedTime += Time.deltaTime;
    //        warriorLate = Mathf.Clamp01(warriorElapsedTime / duration);
    //        Debug.Log(warriorElapsedTime);
    //        image.color = Color32.Lerp(originalColor, targetColor, warriorLate);
    //        Debug.Log(image + ":highlight");
    //    }
    //    if (selectWizard)
    //    {
    //        wizardElapsedTime += Time.deltaTime;
    //        wizardLate = Mathf.Clamp01(wizardElapsedTime / duration);
    //        image.color = Color32.Lerp(originalColor, targetColor, wizardLate);
    //        Debug.Log(image + ":highlight");
    //    }
    //}

    //public void lowLight(Image image)
    //{
    //    if (selectWizard)
    //    {
    //        Debug.Log(image + ":lowlight");
    //        if(warriorElapsedTime >= 1)warriorElapsedTime = 0;
    //        warriorLate = 0;
    //        image.color = originalColor;
    //    }
    //    if (selectWarrior)
    //    {
    //        Debug.Log(image + ":lowlight");
    //        if(wizardElapsedTime >= 0)wizardElapsedTime = 0;
    //        image.color = originalColor;
    //    }
    //}



    //public void warriorHighlight(Image image)    //キャラクターををハイライトする
    //{

    //    warriorElapsedTime += Time.deltaTime;
    //    warriorLate = Mathf.Clamp01(warriorElapsedTime / duration);
    //    image.color = Color32.Lerp(originalColor, targetColor, warriorLate);
    //}
    //public void warriorLowlight(Image image)     //キャラクターをローライトする（暗くする）
    //{
    //    warriorElapsedTime = 0;
    //    image.color = originalColor;    //色を戻す
    //}

    //public void wizardHighlight(Image image)    //キャラクターををハイライトする
    //{

    //    wizardElapsedTime += Time.deltaTime;
    //    wizardLate = Mathf.Clamp01(wizardElapsedTime / duration);
    //    image.color = Color32.Lerp(originalColor, targetColor, wizardLate);
    //}
    //public void wizardLowlight(Image image)     //キャラクターをローライトする（暗くする）
    //{
    //    wizardElapsedTime = 0;
    //    image.color = originalColor;    //色を戻す
    //}


}