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

    private float duration = 0.25f;      //�F���ς��܂ł̕b��
    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float wizardElapsedTime = 0f;
    private float wizardLate;

    private void Start()
    {
        warrior_pm = warrior.GetComponent<PointerManager>();
        wizard_pm = wizard.GetComponent<PointerManager>();

        // �����̐F
        originalColor = new Color32(60, 60, 60, 255);

        // �n�C���C�g���̐F
        targetColor = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        //�^�C�g����ʂ֑J��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Scene�؂�ւ�
            sceneController.sceneChange("TitleScene");
        }


        //�摜���N���b�N���ꂽ�Ƃ��Ɋe�L������isSelect��true�ɂ���
        if (warrior_pm.isSelect == true || wizard_pm.isSelect == true)
        {
            warrior_pm.isSelect = true;
            wizard_pm.isSelect = true;
        }

        //�I�΂ꂽ�L�������n�C���C�g
        if (selectWarrior)
            highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());
        if (selectWizard)
            highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());

    }


    public void highLight(Image warriorImage,Image wizardImage)
    {

        if (selectWarrior)  //��m���n�C���C�g
        {
            warriorElapsedTime += Time.deltaTime;
            warriorLate = Mathf.Clamp01(warriorElapsedTime/duration);
            warriorImage.color = Color32.Lerp(originalColor,targetColor,warriorLate);

            //���@�g�������[���C�g
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;      
        }
        if (selectWizard)   //���@�g�����n�C���C�g
        {
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime/duration);
            wizardImage.color = Color32.Lerp(originalColor,targetColor,wizardLate);

            //��m�����[���C�g
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
        }
    }


    //�t�B�[���h��ʂ֑J��
    public void OnClick()
    {
        sceneController.sceneChange("FieldScene");
    }
}