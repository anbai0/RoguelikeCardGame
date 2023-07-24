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
    bool isDisplayUpStrength;
    bool isDisplayAutoHealing;
    bool isDisplayInvalidBadStatus;
    bool isDisplayCurse;
    bool isDisplayImpatience;
    bool isDisplayWeakness;
    bool isDisplayBurn;
    bool isDisplayPoison;
    public int DebugNum = 0;
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
    // Start is called before the first frame update
    void Start()
    {
        isDisplayUpStrength = false;
        isDisplayAutoHealing = false;
        isDisplayInvalidBadStatus = false;
        isDisplayCurse = false;
        isDisplayImpatience = false;
        isDisplayWeakness = false;
        isDisplayBurn = false;
        isDisplayPoison = false;
        sortIcon = GetComponent<SortDeck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewIcon(ConditionStatus playerCondition) 
    {
        int upStrength = playerCondition.upStrength;
        int autoHealing = playerCondition.autoHealing;
        int invalidBadStatus  = playerCondition.invalidBadStatus;
        int curse = playerCondition.curse;
        int impatience = playerCondition.impatience;
        int weakness = playerCondition.weakness;
        int burn = playerCondition.burn;
        int poison = playerCondition.poison;
        ViewUpStrength(upStrength+DebugNum);
        ViewAutoHealing(autoHealing + DebugNum);
        ViewInvalidBadSttus(invalidBadStatus + DebugNum);
        ViewCurse(curse + DebugNum);
        ViewImpatience(impatience + DebugNum);
        ViewWeakness(weakness + DebugNum);
        ViewBurn(burn + DebugNum);
        ViewPoison(poison + DebugNum);
        
    }

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
