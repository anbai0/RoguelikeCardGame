using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//�I���W�i���̃V�F�[�_�[���g�p
//�}�e���A���̒���ւ����K�v�Ȃ̂ŃG�l�~�[�ƕ����Ă��܂�
/// <summary>
/// �v���C���[�̏�Ԉُ�A�C�R����\������X�N���v�g
/// </summary>
public class PlayerConditionDisplay : MonoBehaviour
{
    [SerializeField] GameObject coditionIcon;
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

    [SerializeField] SortName sortIcon;
    Dictionary<string, int> previousCondition = new Dictionary<string, int>();
    Queue<KeyValuePair<string, int>> iconQueue = new Queue<KeyValuePair<string, int>>();
    float iconSpawnInterval = 0.1f; // �A�C�R���𐶐�����Ԋu�i�b�j
    float iconSpawnTimer = 0f;

    [SerializeField] UIManagerBattle uiManagerBattle;

    private void Awake()
    {
        InitializedCondition();
    }

    /// <summary>
    /// ��Ԉُ�̖��O�Ə����������������Ă�������
    /// </summary>
    void InitializedCondition()
    {
        previousCondition.Add("UpStrength", 0);
        previousCondition.Add("AutoHealing", 0);
        previousCondition.Add("InvalidBadStatus", 0);
        previousCondition.Add("Curse", 0);
        previousCondition.Add("Impatience", 0);
        previousCondition.Add("Weakness", 0);
        previousCondition.Add("Burn", 0);
        previousCondition.Add("Poison", 0);
    }

    void Update()
    {
        iconSpawnTimer += Time.deltaTime;

        if (iconQueue.Count > 0 && iconSpawnTimer >= iconSpawnInterval)
        {
            // �ǉ����ꂽ��Ԉُ���L���[������o���Đ���
            var conditionPair = iconQueue.Dequeue();
            ViewConditionIcon(conditionPair.Key, conditionPair.Value);
            // �^�C�}�[�����Z�b�g
            iconSpawnTimer = 0f;
        }
    }

    /// <summary>
    /// ��Ԉُ�A�C�R���̍X�V�����鏈��
    /// </summary>
    /// <param name="condition">���݂̏�Ԉُ�</param>
    public void UpdateConditionIcon(Dictionary<string, int> condition)
    {
        //�O��̏�Ԉُ�Ɣ�r����
        foreach (var pair in condition)
        {
            string conditionName = pair.Key;
            int conditionCount = pair.Value;
            if (previousCondition.ContainsKey(conditionName))
            {
                int previousCount = previousCondition[conditionName];
                if (previousCount != conditionCount) //�O��̏�Ԉُ킩��ω����������ꍇ
                {
                    iconQueue.Enqueue(new KeyValuePair<string, int>(conditionName, conditionCount));�@// ��Ԉُ���L���[�ɒǉ�
                }
            }
        }
        // �O��̏�Ԉُ�����݂̏�Ԉُ�ɍX�V
        previousCondition.Clear();
        foreach (var pair in condition)
        {
            previousCondition[pair.Key] = pair.Value;
        }
    }

    /// <summary>
    /// ��Ԉُ�A�C�R���̕\��
    /// </summary>
    /// <param name="conditionName">��Ԉُ�̖��O</param>
    /// <param name="conditionCount">��Ԉُ�̌�</param>
    void ViewConditionIcon(string conditionName, int conditionCount)
    {
        if (conditionName == "UpStrength")
        {
            ViewUpStrength(conditionCount);
        }
        else if (conditionName == "AutoHealing")
        {
            ViewAutoHealing(conditionCount);
        }
        else if (conditionName == "InvalidBadStatus")
        {
            ViewInvalidBadStatus(conditionCount);
        }
        else if (conditionName == "Curse")
        {
            ViewCurse(conditionCount);
        }
        else if (conditionName == "Impatience")
        {
            ViewImpatience(conditionCount);
        }
        else if (conditionName == "Weakness")
        {
            ViewWeakness(conditionCount);
        }
        else if (conditionName == "Burn")
        {
            ViewBurn(conditionCount);
        }
        else if (conditionName == "Poison")
        {
            ViewPoison(conditionCount);
        }
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon1"); //�I���W�i���̃}�e���A����ݒ�
                ConditionDataManager conditiondata = new ConditionDataManager("UpStrength"); //�ؗ͑����̃f�[�^���쐬
                image.sprite = conditiondata._conditionImage; //��Ԉُ�ɉ������X�v���C�g��ݒ�
                ViewConditionEffect(upStrengthIcon, conditiondata._conditionName, conditiondata._conditionEffect);
                sortIcon.Sort(); //���O���Ƀ\�[�g
            }
            AudioManager.Instance.PlaySE("�o�t");
            upStrengthIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
            upStrengthIcon.GetComponent<IconAnimation>().StartAnimation();
            Text iconText = upStrengthIcon.transform.Find("ConditionCountText").GetComponent<Text>(); 
            if (upStrength >= 2) //�Q�ȏ�ł����
            {
                iconText.text = upStrength.ToString(); //���̕\��
            }
            else
            {
                iconText.text = null;
            }
                
        }
        else
        {
            isDisplayUpStrength = false;
            Destroy(upStrengthIcon);
            upStrengthIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon2");
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
            {
                iconText.text = autoHealing.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayAutoHealing = false;
            Destroy(autoHealingIcon);
            autoHealingIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon3");
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
            {
                iconText.text = invalidBadStatus.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayInvalidBadStatus = false;
            Destroy(invalidBadStatusIcon);
            invalidBadStatusIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon4");
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
            {
                iconText.text = curse.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayCurse = false;
            Destroy(curseIcon);
            curseIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon5");
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
            {
                iconText.text = impatience.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayImpatience = false;
            Destroy(impatienceIcon);
            impatienceIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon6");
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
            {
                iconText.text = weakness.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayWeakness = false;
            Destroy(weaknessIcon);
            weaknessIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon7");
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
            {
                iconText.text = burn.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayBurn = false;
            Destroy(burnIcon);
            burnIcon = null;
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
                image.material = Resources.Load<Material>("IconMaterials/PlayerIcon8");
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
            {
                iconText.text = poison.ToString();
            }
            else
            {
                iconText.text = null;
            }
        }
        else
        {
            isDisplayPoison = false;
            Destroy(poisonIcon);
            poisonIcon = null;
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
        Transform conditionEffectBG = conditionIcon.transform.GetChild(1); //PlayerConditionEffectBG���擾
        TextMeshProUGUI conditonNameText = conditionEffectBG.GetChild(0).GetComponent<TextMeshProUGUI>(); //ConditionName��TMProUGUI���擾
        conditonNameText.text = conditionName;
        TextMeshProUGUI conditonEffectText = conditionEffectBG.GetChild(1).GetComponent<TextMeshProUGUI>(); //ConditionEffect��TMProUGUI���擾
        conditonEffectText.text = conditionEffect;
        conditionEffectBG.gameObject.SetActive(false); //�ŏ��͕\�������Ȃ��悤�ɂ���
        uiManagerBattle.UIEventsReload();
    }
}
