using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject coditionIcon;
    GameObject upStrengthIcon = null;
    GameObject autoHealingIcon = null;
    GameObject invalidBadStatusIcon = null;
    GameObject curseIcon = null;
    GameObject impatienceIcon = null;
    GameObject weaknessIcon = null;
    GameObject burnIcon = null;
    GameObject poisonIcon = null;
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
    SortDeck sortIcon;
    enum conditionState
    {
        UPSTRENGTH = 0,
        AUTOHEALING,
        INVALIDBADSTATUS,
        CURSE,
        IMPATIENCE,
        WEAKNESS,
        BURN,
        POISON,
        NUM
    };
    /// <summary>
    /// ��Ԉُ�̗L���ɍ��킹�ăA�C�R���̕\���E��\�����s������
    /// </summary>
    /// <param name="condition">�L�����N�^�[�̏�Ԉُ�̃X�e�[�^�X</param>
    public void ViewIcon(ConditionStatus condition)
    {
        ViewUpStrength(condition.upStrength);
        ViewAutoHealing(condition.autoHealing);
        ViewInvalidBadSttus(condition.invalidBadStatus);
        ViewCurse(condition.curse);
        ViewImpatience(condition.impatience);
        ViewWeakness(condition.weakness);
        ViewBurn(condition.burn);
        ViewPoison(condition.poison);
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
                isDisplayUpStrength = true;
                upStrengthIcon = Instantiate(coditionIcon, this.transform);
                upStrengthIcon.name = "Icon1";
                sortIcon.Sort();
            }
            Text iconText = upStrengthIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            iconText.text = upStrength.ToString();
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
                sortIcon.Sort();
            }
            Text iconText = autoHealingIcon.transform.Find("ConditionCountText").GetComponent<Text>();
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
    void ViewInvalidBadSttus(int invalidBadStatus)
    {
        if (invalidBadStatus > 0)
        {
            if (!isDisplayInvalidBadStatus)
            {
                isDisplayInvalidBadStatus = true;
                invalidBadStatusIcon = Instantiate(coditionIcon, this.transform);
                invalidBadStatusIcon.name = "Icon3";
                sortIcon.Sort();
            }
            Text iconText = invalidBadStatusIcon.transform.Find("ConditionCountText").GetComponent<Text>();
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
                sortIcon.Sort();
            }
            Text iconText = curseIcon.transform.Find("ConditionCountText").GetComponent<Text>();
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
                sortIcon.Sort();
            }
            Text iconText = impatienceIcon.transform.Find("ConditionCountText").GetComponent<Text>();
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
                sortIcon.Sort();
            }
            Text iconText = weaknessIcon.transform.Find("ConditionCountText").GetComponent<Text>();
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
                sortIcon.Sort();
            }
            Text iconText = burnIcon.transform.Find("ConditionCountText").GetComponent<Text>();
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
                sortIcon.Sort();
            }
            Text iconText = poisonIcon.transform.Find("ConditionCountText").GetComponent<Text>();
            iconText.text = poison.ToString();
        }
        else
        {
            isDisplayPoison = false;
            Destroy(poisonIcon);
        }
    }
}
