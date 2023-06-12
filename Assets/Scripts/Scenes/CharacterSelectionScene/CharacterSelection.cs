using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    public GameObject warrior;
    public GameObject wizard;

    private PointerManager warrior_pm;
    private PointerManager wizard_pm;

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
        warrior_pm = warrior.GetComponent<PointerManager>();
        wizard_pm = wizard.GetComponent<PointerManager>();

        // 初期の色
        originalColor = new Color32(60, 60, 60, 255);

        // ハイライト時の色
        targetColor = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        //タイトル画面へ遷移
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Scene切り替え
            sceneController.sceneChange("TitleScene");
        }


        //画像がクリックされたときに各キャラのisSelectをtrueにする
        if (warrior_pm.isSelect == true || wizard_pm.isSelect == true)
        {
            warrior_pm.isSelect = true;
            wizard_pm.isSelect = true;
        }

        //選ばれたキャラをハイライト
        if (selectWarrior)
            highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());
        if (selectWizard)
            highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());

    }


    public void highLight(Image warriorImage,Image wizardImage)
    {

        if (selectWarrior)  //戦士をハイライト
        {
            warriorElapsedTime += Time.deltaTime;
            warriorLate = Mathf.Clamp01(warriorElapsedTime/duration);
            warriorImage.color = Color32.Lerp(originalColor,targetColor,warriorLate);

            //魔法使いをローライト
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;      
        }
        if (selectWizard)   //魔法使いをハイライト
        {
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime/duration);
            wizardImage.color = Color32.Lerp(originalColor,targetColor,wizardLate);

            //戦士をローライト
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
        }
    }


    //フィールド画面へ遷移
    public void OnClick()
    {
        sceneController.sceneChange("FieldScene");
    }
}