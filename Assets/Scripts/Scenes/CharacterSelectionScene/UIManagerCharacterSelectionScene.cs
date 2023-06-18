using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManagerCharacterSelectionScene : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private UIController[] UIs;

    public GameObject warrior;
    public GameObject wizard;

    bool selectWarrior = false;
    bool selectWizard = false;

    bool isSelect = false;

    private Color32 originalColor;
    private Color32 targetColor;

    private float duration = 0.25f;      //�F���ς��܂ł̕b��
    private float warriorElapsedTime = 0f;
    private float warriorLate;
    private float wizardElapsedTime = 0f;
    private float wizardLate;

    public GameObject button;
    public Image image;
    public Sprite redButton;
    public CharacterSceneManager sceneManager;

    void Start()
    {
        UIs = parent.GetComponentsInChildren<UIController>();       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            //UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onClick.AddListener(() => UIClick(UI.gameObject));         //UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        // �����̐F
        originalColor = new Color32(60, 60, 60, 255);

        // �n�C���C�g���̐F
        targetColor = new Color32(255, 255, 255, 255);
    }

    void UIClick(GameObject UIObject)
    {
        if (UIObject == warrior)
        {
            isSelect = true;
            selectWarrior = true;
            selectWizard = false;
        }
        if (UIObject == wizard)
        {
            isSelect = true;
            selectWarrior = false;
            selectWizard = true;
        }
        if (UIObject == button && isSelect)
        {
            image.sprite = redButton;
            sceneManager.FieldScene();
        }
    }

    void UIEnter(GameObject UIObject)
    {
        if (isSelect) return;

        if (UIObject == warrior)
        {
            selectWarrior = true;
        }
        if (UIObject == wizard)
        {
            selectWizard = true;
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (isSelect) return;

        if (UIObject == warrior)
        {
            selectWarrior = false;
        }
        if (UIObject == wizard)
        {
            selectWizard = false;
        }
    }


    private void Update()
    {
        //�I�΂ꂽ�L�������n�C���C�g
        highLight(warrior.GetComponent<Image>(), wizard.GetComponent<Image>());

    }

    public void highLight(Image warriorImage, Image wizardImage)
    {

        if (selectWarrior)  //��m���n�C���C�g
        {
            warriorElapsedTime += Time.deltaTime;
            warriorLate = Mathf.Clamp01(warriorElapsedTime / duration);
            warriorImage.color = Color32.Lerp(originalColor, targetColor, warriorLate);

            //���@�g�������[���C�g
            wizardElapsedTime = 0;
            wizardImage.color = originalColor;
        }
        if (selectWizard)   //���@�g�����n�C���C�g
        {
            wizardElapsedTime += Time.deltaTime;
            wizardLate = Mathf.Clamp01(wizardElapsedTime / duration);
            wizardImage.color = Color32.Lerp(originalColor, targetColor, wizardLate);

            //��m�����[���C�g
            warriorElapsedTime = 0;
            warriorImage.color = originalColor;
        }
    }
}

