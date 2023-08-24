using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//�I���W�i���̃V�F�[�_�[���g�p
//�}�e���A���̒���ւ����K�v�Ȃ̂Ńv���C���[�ƕ����Ă��܂�
public class EnemyConditionDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject coditionIcon;
    //�ۑ����Ă�����Ԉُ�̃A�C�R��
    GameObject upStrengthIcon = null;
    GameObject autoHealingIcon = null;
    GameObject invalidBadStatusIcon = null;
    GameObject curseIcon = null;
    GameObject impatienceIcon = null;
    GameObject weaknessIcon = null;
    GameObject burnIcon = null;
    GameObject poisonIcon = null;
    //�A�C�R���̐����𔻒肷��t���O
    bool isDisplayUpStrength = false;
    bool isDisplayAutoHealing = false;
    bool isDisplayInvalidBadStatus = false;
    bool isDisplayCurse = false;
    bool isDisplayImpatience = false;
    bool isDisplayWeakness = false;
    bool isDisplayBurn = false;
    bool isDisplayPoison = false;

    public int DebugNum = 0;
    [SerializeField]
    SortName sortIcon;
    private Dictionary<string, int> saveCondition = new Dictionary<string, int>();
    WaitForSeconds waitFor1milliSec = new WaitForSeconds(0.1f);

    [SerializeField]
    UIManagerBattle uiManagerBattle;

    private void Awake()
    {
        InitializedSaveCondition();
    }

    /// <summary>
    /// saveCondition�Ɋi�[�����Ԉُ�̖��O�Ə����������������Ă�������
    /// </summary>
    void InitializedSaveCondition()
    {
        saveCondition.Add("UpStrength", 0);
        saveCondition.Add("AutoHealing", 0);
        saveCondition.Add("InvalidBadStatus", 0);
        saveCondition.Add("Curse", 0);
        saveCondition.Add("Impatience", 0);
        saveCondition.Add("Weakness", 0);
        saveCondition.Add("Burn", 0);
        saveCondition.Add("Poison", 0);
    }

    /// <summary>
    /// ��Ԉُ�̗L���ɍ��킹�ăA�C�R���̕\���E��\�����s������
    /// </summary>
    /// <param name="condition">�L�����N�^�[�̏�Ԉُ�̃X�e�[�^�X</param>
    public void ViewIcon(Dictionary<string, int> condition)
    {
        StartCoroutine(CheckConditionIcon(condition));

    }
    /// <summary>
    /// ���s�ł��鏈��������΍����̃A�C�R��������s���Ă�������
    /// </summary>
    /// <param name="_condition">���݂̏�Ԉُ�</param>
    IEnumerator CheckConditionIcon(Dictionary<string, int> _condition)
    {
        if (saveCondition != _condition)
        {
            if (saveCondition != null && saveCondition["UpStrength"] != _condition["UpStrength"])
                ViewUpStrength(_condition["UpStrength"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["AutoHealing"] != _condition["AutoHealing"])
                ViewAutoHealing(_condition["AutoHealing"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["InvalidBadStatus"] != _condition["InvalidBadStatus"])
                ViewInvalidBadStatus(_condition["InvalidBadStatus"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["Curse"] != _condition["Curse"])
                ViewCurse(_condition["Curse"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["Impatience"] != _condition["Impatience"])
                ViewImpatience(_condition["Impatience"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["Weakness"] != _condition["Weakness"])
                ViewWeakness(_condition["Weakness"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["Burn"] != _condition["Burn"])
                ViewBurn(_condition["Burn"]);
            yield return waitFor1milliSec;
            if (saveCondition != null && saveCondition["Poison"] != _condition["Poison"])
                ViewPoison(_condition["Poison"]);
        }
        saveCondition = new Dictionary<string, int>();
        saveCondition["UpStrength"] = _condition["UpStrength"];
        saveCondition["AutoHealing"] = _condition["AutoHealing"];
        saveCondition["InvalidBadStatus"] = _condition["InvalidBadStatus"];
        saveCondition["Curse"] = _condition["Curse"];
        saveCondition["Impatience"] = _condition["Impatience"];
        saveCondition["Weakness"] = _condition["Weakness"];
        saveCondition["Burn"] = _condition["Burn"];
        saveCondition["Poison"] = _condition["Poison"];
        yield break;
    }

    /// <summary>
    /// �ؗ͑����A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="upStrength">�ؗ͑����̐�</param>
    void ViewUpStrength(int upStrength)
    {
        if (upStrength > 0)
        {
            if (!isDisplayUpStrength)
            {
                isDisplayUpStrength = true; //�\�����ɂ���
                upStrengthIcon = Instantiate(coditionIcon, this.transform); //�A�C�R���𐶐�
                upStrengthIcon.name = "Icon1"; //�\�[�g���₷���悤�ɖ��O��ύX
                var image = upStrengthIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon1"); //�I���W�i���̃}�e���A����ݒ�
                ConditionDataManager conditiondata = new ConditionDataManager("UpStrength"); //�ؗ͑����̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage; //��Ԉُ�ɉ������X�v���C�g��ݒ�
                ViewConditionEffect(upStrengthIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort(); //���O���Ƀ\�[�g
            }
            AudioManager.Instance.PlaySE("�o�t");
            upStrengthIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            upStrengthIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = upStrengthIcon.transform.Find("ConditionCountText").GetComponent<Text>(); 
            if (upStrength >= 2) //2�ȏ�ł����
                iconText.text = upStrength.ToString(); //���̕\��
        }
        else
        {
            isDisplayUpStrength = false;
            Destroy(upStrengthIcon);
        }
    }
    /// <summary>
    /// �����񕜃A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="autoHealing">�����񕜂̐�</param>
    void ViewAutoHealing(int autoHealing)
    {
        if (autoHealing > 0)
        {
            if (!isDisplayAutoHealing)
            {
                isDisplayAutoHealing = true;
                autoHealingIcon = Instantiate(coditionIcon, this.transform);
                autoHealingIcon.name = "Icon2";
                var image = autoHealingIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon2");
                ConditionDataManager conditiondata = new ConditionDataManager("AutoHealing"); //�����񕜂̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(autoHealingIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�o�t");
            autoHealingIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            autoHealingIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = autoHealingIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (autoHealing >= 2)
                iconText.text = autoHealing.ToString();
        }
        else
        {
            isDisplayAutoHealing = false;
            Destroy(autoHealingIcon);
        }
    }
    /// <summary>
    /// ��Ԉُ�ϐ��A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="invalidBadStatus">��Ԉُ�ϐ��̐�</param>
    void ViewInvalidBadStatus(int invalidBadStatus)
    {
        if (invalidBadStatus > 0)
        {
            if (!isDisplayInvalidBadStatus)
            {
                isDisplayInvalidBadStatus = true;
                invalidBadStatusIcon = Instantiate(coditionIcon, this.transform);
                invalidBadStatusIcon.name = "Icon3";
                var image = invalidBadStatusIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon3");
                ConditionDataManager conditiondata = new ConditionDataManager("InvalidBadStatus"); //��Ԉُ햳���̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(invalidBadStatusIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�o�t");
            invalidBadStatusIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            invalidBadStatusIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = invalidBadStatusIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (invalidBadStatus >= 2)
                iconText.text = invalidBadStatus.ToString();
        }
        else
        {
            isDisplayInvalidBadStatus = false;
            Destroy(invalidBadStatusIcon);
        }
    }
    /// <summary>
    /// �����A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="curse">�����̐�</param>
    void ViewCurse(int curse)
    {
        if (curse > 0)
        {
            if (!isDisplayCurse)
            {
                isDisplayCurse = true;
                curseIcon = Instantiate(coditionIcon, this.transform);
                curseIcon.name = "Icon4";
                var image = curseIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon4");
                ConditionDataManager conditiondata = new ConditionDataManager("Curse"); //�����̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(curseIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�f�o�t");
            curseIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            curseIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = curseIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (curse >= 2)
                iconText.text = curse.ToString();
        }
        else
        {
            isDisplayCurse = false;
            Destroy(curseIcon);
        }
    }
    /// <summary>
    /// �ő��A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="impatience">�ő��̐�</param>
    void ViewImpatience(int impatience)
    {
        if (impatience > 0)
        {
            if (!isDisplayImpatience)
            {
                isDisplayImpatience = true;
                impatienceIcon = Instantiate(coditionIcon, this.transform);
                impatienceIcon.name = "Icon5";
                var image = impatienceIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon5");
                ConditionDataManager conditiondata = new ConditionDataManager("Impatience"); //�ő��̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(impatienceIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�f�o�t");
            impatienceIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            impatienceIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = impatienceIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (impatience >= 2)
                iconText.text = impatience.ToString();
        }
        else
        {
            isDisplayImpatience = false;
            Destroy(impatienceIcon);
        }
    }
    /// <summary>
    /// ����A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="weakness">����̐�</param>
    void ViewWeakness(int weakness)
    {
        if (weakness > 0)
        {
            if (!isDisplayWeakness)
            {
                isDisplayWeakness = true;
                weaknessIcon = Instantiate(coditionIcon, this.transform);
                weaknessIcon.name = "Icon6";
                var image = weaknessIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon6");
                ConditionDataManager conditiondata = new ConditionDataManager("Weakness"); //����̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(weaknessIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�f�o�t");
            weaknessIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            weaknessIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = weaknessIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (weakness >= 2)
                iconText.text = weakness.ToString();
        }
        else
        {
            isDisplayWeakness = false;
            Destroy(weaknessIcon);
        }
    }
    /// <summary>
    /// �Ώ��A�C�R���̕\���E��\��
    /// </summary>
    /// <param name="burn">�Ώ��̐�</param>
    void ViewBurn(int burn)
    {
        if (burn > 0)
        {
            if (!isDisplayBurn)
            {
                isDisplayBurn = true;
                burnIcon = Instantiate(coditionIcon, this.transform);
                burnIcon.name = "Icon7";
                var image = burnIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon7");
                ConditionDataManager conditiondata = new ConditionDataManager("Burn"); //�Ώ��̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(burnIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�f�o�t");
            burnIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            burnIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = burnIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (burn >= 2)
                iconText.text = burn.ToString();
        }
        else
        {
            isDisplayBurn = false;
            Destroy(burnIcon);
        }
    }
    /// <summary>
    /// �דŃA�C�R���̕\���E��\��
    /// </summary>
    /// <param name="poison">�דł̐�</param>
    void ViewPoison(int poison)
    {
        if (poison > 0)
        {
            if (!isDisplayPoison)
            {
                isDisplayPoison = true;
                poisonIcon = Instantiate(coditionIcon, this.transform);
                poisonIcon.name = "Icon8";
                var image = poisonIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon8");
                ConditionDataManager conditiondata = new ConditionDataManager("Poison"); //�דł̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(poisonIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("�f�o�t");
            poisonIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            poisonIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = poisonIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            if (poison >= 2)
                iconText.text = poison.ToString();
        }
        else
        {
            isDisplayPoison = false;
            Destroy(poisonIcon);
        }
    }

    /// <summary>
    /// ��Ԉُ�̖��O�ƌ��ʂ��A�C�R����ConditionEffectBG�̓��̃e�L�X�g�ɑ}��
    /// </summary>
    /// <param name="conditionIcon">�e�L�X�g������A�C�R��</param>
    /// <param name="conditionName">��Ԉُ�̖��O</param>
    /// <param name="conditionEffect">��Ԉُ�̌���</param>
    void ViewConditionEffect(GameObject conditionIcon, string conditionName, string conditionEffect)
    {
        Transform conditionEffectBG = conditionIcon.transform.GetChild(2); //EnemyConditionEffectBG���擾
        TextMeshProUGUI conditonNameText = conditionEffectBG.GetChild(0).GetComponent<TextMeshProUGUI>(); //ConditionName��TMProUGUI���擾
        conditonNameText.text = conditionName;
        TextMeshProUGUI conditonEffectText = conditionEffectBG.GetChild(1).GetComponent<TextMeshProUGUI>(); //ConditionEffect��TMProUGUI���擾
        conditonEffectText.text = conditionEffect;
        conditionIcon.gameObject.tag = "EnemyCondition"; //�G�l�~�[�̏�Ԉُ�A�C�R���̓^�O��EnemyCondition�ɕt���ւ���
        conditionEffectBG.gameObject.SetActive(false); //�ŏ��͕\�������Ȃ��悤�ɂ���
        uiManagerBattle.UIEventsReload();
    }
}
