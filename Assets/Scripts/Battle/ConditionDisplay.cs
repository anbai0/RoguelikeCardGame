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
    SortName sortIcon;
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
    /// 状態異常の有無に合わせてアイコンの表示・非表示を行う処理
    /// </summary>
    /// <param name="condition">キャラクターの状態異常のステータス</param>
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
    /// 筋力増強アイコンの表示・非表示
    /// </summary>
    /// <param name="upStrength">筋力増強の数</param>
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
    /// 自動回復アイコンの表示・非表示
    /// </summary>
    /// <param name="autoHealing">自動回復の数</param>
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
    /// 状態異常耐性アイコンの表示・非表示
    /// </summary>
    /// <param name="invalidBadStatus">状態異常耐性の数</param>
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
    /// 呪縛アイコンの表示・非表示
    /// </summary>
    /// <param name="curse">呪縛の数</param>
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
    /// 焦燥アイコンの表示・非表示
    /// </summary>
    /// <param name="impatience">焦燥の数</param>
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
    /// 衰弱アイコンの表示・非表示
    /// </summary>
    /// <param name="weakness">衰弱の数</param>
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
    /// 火傷アイコンの表示・非表示
    /// </summary>
    /// <param name="burn">火傷の数</param>
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
    /// 邪毒アイコンの表示・非表示
    /// </summary>
    /// <param name="poison">邪毒の数</param>
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
