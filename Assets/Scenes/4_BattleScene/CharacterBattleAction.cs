using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �퓬���ɃL�����N�^�[���N�����s���̊�{�ƂȂ�X�N���v�g
/// (Player��Enemy��BattleAction�ł͂��̃X�N���v�g���p��)
/// </summary>
public class CharacterBattleAction : MonoBehaviour
{
    //�X�e�[�^�X
    int HP; //�ő�HP
    int currentHP; //���݂�HP
    int AP; //�ő�AP
    int constAP; //�퓬�J�n���̍ő�AP
    int currentAP; //���݂�AP
    int chargeAP; //���E���h���i�ނ��Ƃɑ�������AP�̒l
    int roundStartAP; //���E���h�J�n���̍ő�AP(IsCurse�̔���ɗp����)
    int GP; //�_���[�W��h����K�[�h�|�C���g
    public int GetSetHP { get => HP; set => HP = value; }
    public int GetSetCurrentHP { get => currentHP; set => currentHP = value; }
    public int GetSetAP { get => AP; set => AP = value; }
    public int GetSetConstAP { get => constAP; set => constAP = value; }
    public int GetSetCurrentAP { get => currentAP; set => currentAP = value; }
    public int GetSetChargeAP { get => chargeAP; set => chargeAP = value; }
    public int GetSetGP { get => GP; set => GP = value; }

    Dictionary<string, int> condition = new Dictionary<string, int>(); //�t�^����Ă����Ԉُ�
    InflictCondition inflictCondition; //��Ԉُ�̌���
    public Dictionary<string, int> GetSetCondition { get => condition; set => condition = value; }
    public InflictCondition GetSetInflictCondition { get => inflictCondition; set => inflictCondition = value; }

    bool isGardAppeared; //�K�[�h�̃A�C�R�����\������Ă��邩����

    void Start()
    {
        chargeAP = 0;
        isGardAppeared = false;
    }
    
    /// <summary>
    /// ���E���h�J�n����AP�v�Z
    /// </summary>
    public void SetUpAP() 
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition["Curse"], condition["InvalidBadStatus"]);
        AP = curse.nextRoundAP;
        currentAP = AP;
        condition["InvalidBadStatus"] = curse.invalidBadStatus;
    }

    /// <summary>
    /// ������Ԃ̎���AP���X�V���鏈��
    /// </summary>
    public void CursedUpdateAP()
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition["Curse"], condition["InvalidBadStatus"]);
        AP = curse.nextRoundAP;
        condition["InvalidBadStatus"] = curse.invalidBadStatus;
    }
    
    /// <summary>
    /// AP��0�ɂ��čs���s�\�ɂ���
    /// </summary>
    public void TurnEnd()
    {
        currentAP = 0;
    }
    
    /// <summary>
    /// �e�L�X�g�̍X�V
    /// </summary>
    /// <param name="HPText">HP�e�L�X�g</param>
    /// <param name="APText">AP�e�L�X�g</param>
    /// <param name="GPText">GP�e�L�X�g</param>
    /// <param name="HPSlider">HP�X���C�_�[(�G�l�~�[�̂�)</param>
    public void UpdateText(Text HPText, Text APText, Text GPText, Slider HPSlider)
    {
        //currentHP�̒l��0�ȏ�HP�ȉ�
        currentHP = currentHP > HP ? HP : currentHP;
        currentHP = currentHP < 0 ? 0 : currentHP;
        //HP�̕\��
        HPText.text = currentHP + "/" + HP;
        //�X���C�_�[�������HP�̃X���C�_�[��\��
        if (HPSlider != null)
            HPSlider.value = currentHP / (float)HP;
        //currentAP�̒l��0�ȏ�AP�ȉ�
        currentAP = currentAP > AP ? AP : currentAP;
        currentAP = currentAP < 0 ? 0 : currentAP;
        //AP�̕\��
        APText.text = currentAP + "/" + AP;
        //GP�̒l��0�ȏ�
        GP = GP < 0 ? 0 : GP;
        //GP��������΃K�[�h�A�C�R���͕\�����Ȃ�
        var GPIcon = GPText.transform.parent.gameObject;
        if (GP == 0)
        {
            GPIcon.SetActive(false);
            isGardAppeared = false;
        }
        else
        {
            GPIcon.SetActive(true);
            if (!isGardAppeared)
            {
                isGardAppeared = true;
                GPIconAppearance(GPIcon);
            }
        }
        //GP�̕\��
        GPText.text = GP.ToString();
    }
    
    /// <summary>
    /// �K�[�h�A�C�R�����o��������Ƃ��ɓ������A�j���[�V����
    /// </summary>
    /// <param name="_GPIcon">�e�L�����N�^�[��GP�A�C�R��</param>
    private void GPIconAppearance(GameObject _GPIcon)
    {
        _GPIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
        _GPIcon.GetComponent<IconAnimation>().StartAnimation();
    }
    
    /// <summary>
    /// ���E���h�I������AP����
    /// </summary>
    public void ChargeAP() 
    {
        chargeAP++;
    }
    
    /// <summary>
    /// ���S���ǂ�������
    /// </summary>
    /// <returns></returns>
    public bool CheckHP() 
    {
        return currentHP <= 0 ? true : false;
    }
    
    /// <summary>
    /// ���E���h�J�n����AP��ۑ����Ă���(Curse�̌v�Z�p)
    /// </summary>
    public void SaveRoundAP() 
    {
        roundStartAP = AP;
    }
    
    /// <summary>
    /// ������ԂɂȂ��Ă��邩����
    /// </summary>
    /// <returns>���E���h�J�n�����ő�AP�������Ă����ꍇtrue��Ԃ�</returns>
    public bool IsCurse() 
    {
        //�^�[���I�����̍ő�AP
        int currentMaxAP = inflictCondition.Curse(constAP, chargeAP, condition["Curse"], condition["InvalidBadStatus"]).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false;
    }

    //�ȉ��A��Ԉُ�̌Ăяo��
    
    /// <summary>
    /// �ؗ͑����̌Ăяo��
    /// </summary>
    /// <param name="attackPower">�U����</param>
    /// <returns>���������U����</returns>
    public int UpStrength(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, condition["UpStrength"]);
        return attackPower;
    }
    
    /// <summary>
    /// �ő��̌Ăяo��
    /// </summary>
    public void Impatience()
    {
        var impatience = inflictCondition.Impatience(condition["Impatience"], condition["InvalidBadStatus"]);
        currentAP -= impatience.impatience;
        condition["InvalidBadStatus"] = impatience.invalidBadStatus;
    }
    
    /// <summary>
    /// ����̌Ăяo��
    /// </summary>
    /// <param name="attackPower">�U����</param>
    /// <returns>���������U����</returns>
    public int Weakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, condition["Weakness"], condition["InvalidBadStatus"]);
        attackPower = weakness.attackPower;
        condition["InvalidBadStatus"] = weakness.invalidBadStatus;
        return attackPower;
    }

    //�ȉ��A�J�[�h��Z�̌���
    /// <summary>
    /// AP�̉�
    /// </summary>
    /// <param name="healingAPPower">AP�̉񕜗�</param>
    public void HealingAP(int healingAPPower) //AP�̉�
    {
        currentAP += healingAPPower;
    }
    
    /// <summary>
    /// GP�̒ǉ�
    /// </summary>
    /// <param name="addGP"></param>
    public void AddGP(int addGP) 
    {
        GP += addGP;
    }
    
    /// <summary>
    /// �o�b�h�X�e�[�^�X����������
    /// </summary>
    public void ReleaseBadStatus() 
    {
        condition["Curse"] = 0;
        condition["Impatience"] = 0;
        condition["Weakness"] = 0;
        condition["Burn"] = 0;
        condition["Poison"] = 0;
    }
    
    /// <summary>
    /// �o�t�X�e�[�^�X����������
    /// </summary>
    public void ReleaseBuffStatus() 
    {
        condition["UpStrength"] = 0;
        condition["AutoHealing"] = 0;
        condition["InvalidBadStatus"] = 0;
    }
    
    /// <summary>
    /// �o�b�h�X�e�[�^�X�����邩�`�F�b�N����
    /// </summary>
    /// <returns>�o�b�h�X�e�[�^�X�̐�</returns>
    public int CheckBadStatus() 
    {
        var charaC = condition;
        int badStatus = charaC["Curse"] + charaC["Impatience"] + charaC["Weakness"] + charaC["Burn"] + charaC["Poison"];
        return badStatus;
    }
    
    /// <summary>
    /// �o�t�X�e�[�^�X�����邩�`�F�b�N����
    /// </summary>
    /// <returns>�o�t�X�e�[�^�X�̐�</returns>
    public int CheckBuffStatus() 
    {
        var charaC = condition;
        int buffStatus = charaC["UpStrength"] + charaC["AutoHealing"] + charaC["InvalidBadStatus"];
        return buffStatus;
    }
    
    /// <summary>
    /// ��Ԉُ��ǉ�����
    /// </summary>
    /// <param name="status">��Ԉُ�̖��O</param>
    /// <param name="count">��Ԉُ�̌�</param>
    public void AddConditionStatus(string status, int count) 
    {
        if (status == "UpStrength")
        {
            condition["UpStrength"] += count;
        }
        else if (status == "AutoHealing")
        {
            condition["AutoHealing"] += count;
        }
        else if (status == "InvalidBadStatus")
        {
            condition["InvalidBadStatus"] += count;
        }
        else if (status == "Curse")
        {
            condition["Curse"] += count;
        }
        else if (status == "Impatience")
        {
            condition["Impatience"] += count;
        }
        else if (status == "Weakness")
        {
            condition["Weakness"] += count;
        }
        else if (status == "Burn")
        {
            condition["Burn"] += count;
        }
        else if (status == "Poison")
        {
            condition["Poison"] += count;
        }
    }
}
