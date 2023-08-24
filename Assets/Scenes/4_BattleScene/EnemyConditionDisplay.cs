using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//オリジナルのシェーダーを使用
//マテリアルの張り替えが必要なのでプレイヤーと分けていまず
public class EnemyConditionDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject coditionIcon;
    //保存しておく状態異常のアイコン
    GameObject upStrengthIcon = null;
    GameObject autoHealingIcon = null;
    GameObject invalidBadStatusIcon = null;
    GameObject curseIcon = null;
    GameObject impatienceIcon = null;
    GameObject weaknessIcon = null;
    GameObject burnIcon = null;
    GameObject poisonIcon = null;
    //アイコンの生成を判定するフラグ
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
    /// saveConditionに格納する状態異常の名前と所持数を初期化しておく処理
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
    /// 状態異常の有無に合わせてアイコンの表示・非表示を行う処理
    /// </summary>
    /// <param name="condition">キャラクターの状態異常のステータス</param>
    public void ViewIcon(Dictionary<string, int> condition)
    {
        StartCoroutine(CheckConditionIcon(condition));

    }
    /// <summary>
    /// 実行できる処理があれば左側のアイコンから実行していく処理
    /// </summary>
    /// <param name="_condition">現在の状態異常</param>
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
    /// 筋力増強アイコンの表示・非表示
    /// </summary>
    /// <param name="upStrength">筋力増強の数</param>
    void ViewUpStrength(int upStrength)
    {
        if (upStrength > 0)
        {
            if (!isDisplayUpStrength)
            {
                isDisplayUpStrength = true; //表示中にする
                upStrengthIcon = Instantiate(coditionIcon, this.transform); //アイコンを生成
                upStrengthIcon.name = "Icon1"; //ソートしやすいように名前を変更
                var image = upStrengthIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon1"); //オリジナルのマテリアルを設定
                ConditionDataManager conditiondata = new ConditionDataManager("UpStrength"); //筋力増強のデータを作成
                image.sprite = conditiondata._conditionImage; //状態異常に応じたスプライトを設定
                ViewConditionEffect(upStrengthIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort(); //名前順にソート
            }
            AudioManager.Instance.PlaySE("バフ");
            upStrengthIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            upStrengthIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = upStrengthIcon.transform.Find("ConditionCountText").GetComponent<Text>(); 
            if (upStrength >= 2) //2以上であれば
                iconText.text = upStrength.ToString(); //個数の表示
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
                var image = autoHealingIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon2");
                ConditionDataManager conditiondata = new ConditionDataManager("AutoHealing"); //自動回復のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(autoHealingIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("バフ");
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
    /// 状態異常耐性アイコンの表示・非表示
    /// </summary>
    /// <param name="invalidBadStatus">状態異常耐性の数</param>
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
                ConditionDataManager conditiondata = new ConditionDataManager("InvalidBadStatus"); //状態異常無効のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(invalidBadStatusIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("バフ");
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
                var image = curseIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon4");
                ConditionDataManager conditiondata = new ConditionDataManager("Curse"); //呪縛のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(curseIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("デバフ");
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
                var image = impatienceIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon5");
                ConditionDataManager conditiondata = new ConditionDataManager("Impatience"); //焦燥のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(impatienceIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("デバフ");
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
                var image = weaknessIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon6");
                ConditionDataManager conditiondata = new ConditionDataManager("Weakness"); //衰弱のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(weaknessIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("デバフ");
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
                var image = burnIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon7");
                ConditionDataManager conditiondata = new ConditionDataManager("Burn"); //火傷のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(burnIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("デバフ");
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
                var image = poisonIcon.GetComponent<Image>();
                image.material = Resources.Load<Material>("IconMaterials/EnemyIcon8");
                ConditionDataManager conditiondata = new ConditionDataManager("Poison"); //邪毒のデータを作成
                image.sprite = conditiondata._conditionImage;
                ViewConditionEffect(poisonIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort();
            }
            AudioManager.Instance.PlaySE("デバフ");
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
    /// 状態異常の名前と効果をアイコンのConditionEffectBGの内のテキストに挿入
    /// </summary>
    /// <param name="conditionIcon">テキストを入れるアイコン</param>
    /// <param name="conditionName">状態異常の名前</param>
    /// <param name="conditionEffect">状態異常の効果</param>
    void ViewConditionEffect(GameObject conditionIcon, string conditionName, string conditionEffect)
    {
        Transform conditionEffectBG = conditionIcon.transform.GetChild(2); //EnemyConditionEffectBGを取得
        TextMeshProUGUI conditonNameText = conditionEffectBG.GetChild(0).GetComponent<TextMeshProUGUI>(); //ConditionNameのTMProUGUIを取得
        conditonNameText.text = conditionName;
        TextMeshProUGUI conditonEffectText = conditionEffectBG.GetChild(1).GetComponent<TextMeshProUGUI>(); //ConditionEffectのTMProUGUIを取得
        conditonEffectText.text = conditionEffect;
        conditionIcon.gameObject.tag = "EnemyCondition"; //エネミーの状態異常アイコンはタグをEnemyConditionに付け替える
        conditionEffectBG.gameObject.SetActive(false); //最初は表示をしないようにする
        uiManagerBattle.UIEventsReload();
    }
}
