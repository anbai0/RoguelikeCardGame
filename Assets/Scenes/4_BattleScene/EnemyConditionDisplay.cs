using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Dictionary<string, int> saveCondition;
    WaitForSeconds waitFor1milliSec = new WaitForSeconds(0.1f);

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
                sortIcon.Sort(); //���O���Ƀ\�[�g
            }
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
                sortIcon.Sort();
            }
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
                sortIcon.Sort();
            }
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
                sortIcon.Sort();
            }
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
                sortIcon.Sort();
            }
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
                sortIcon.Sort();
            }
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
                sortIcon.Sort();
            }
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
                sortIcon.Sort();
            }
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
}