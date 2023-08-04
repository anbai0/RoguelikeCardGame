using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//����͐퓬���ɃL�����N�^�[���N�����s���̊�{�ƂȂ�X�N���v�g�ł��B
//����Player��Enemy��BattleAction�ł͂��̃X�N���v�g���p�����܂��B
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
    ConditionStatus condition; //��Ԉُ�X�e�[�^�X
    InflictCondition inflictCondition; //��Ԉُ�̌���
    RelicStatus relics; //�������Ă��郌���b�N
    public int GetSetHP { get => HP; set => HP = value; }
    public int GetSetCurrentHP { get => currentHP; set => currentHP = value; }
    public int GetSetAP { get => AP; set => AP = value; }
    public int GetSetConstAP { get => constAP; set => constAP = value; }
    public int GetSetCurrentAP { get => currentAP; set => currentAP = value; }
    public int GetSetChargeAP { get => chargeAP; set => chargeAP = value; }
    public int GetSetGP { get => GP; set => GP = value; }
    public ConditionStatus GetSetCondition { get => condition; set => condition = value; }
    public InflictCondition GetSetInflictCondition { get => inflictCondition; set => inflictCondition = value; }
    public RelicStatus GetSetRelicStatus { get => relics; set => relics = value; }
    // Start is called before the first frame update
    void Start()
    {
        chargeAP = 0;
    }
    public void SetUpAP() //���E���h�J�n����AP�v�Z
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition.curse, condition.invalidBadStatus);
        AP = curse.nextRoundAP;
        currentAP = AP;
        condition.invalidBadStatus = curse.invalidBadStatus;
    }
    public void TurnEnd() //�s���s�\�ɂ���
    {
        currentAP = 0;
    }
    public void UpdateText(Text HPText, Text APText, Text GPText, Slider HPSlider) //�e�L�X�g�̍X�V
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
        //GP�̕\��
        GPText.text = GP.ToString();
    }
    public void ChargeAP() //���E���h�I������AP����
    {
        chargeAP++;
    }
    public bool CheckHP() //���S���ǂ������肷��
    {
        return currentHP <= 0 ? true : false;
    }
    public void SaveRoundAP() //���E���h�J�n����AP��ۑ����Ă���
    {
        roundStartAP = AP;
    }
    public bool IsCurse() //������ԂɂȂ��Ă��邩�m�F����
    {
        //�^�[���I�����̍ő�AP
        int currentMaxAP = inflictCondition.Curse(constAP, chargeAP, condition.curse, condition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false; //���E���h�J�n�����ő�AP�������Ă����ꍇtrue��Ԃ�
    }

    //�ȉ��A��Ԉُ�̌��ʏ���
    public int UpStrength(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, condition.upStrength);
        return attackPower;
    }
    public void AutoHealing()
    {
        currentHP = inflictCondition.AutoHealing(currentHP, condition.autoHealing);
    }
    public void Impatience()
    {
        var impatience = inflictCondition.Impatience(currentAP, condition.impatience, condition.invalidBadStatus);
        currentAP = impatience.currentAP;
        condition.invalidBadStatus = impatience.invalidBadStatus;
    }
    public int Weakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, condition.weakness, condition.invalidBadStatus);
        attackPower = weakness.attackPower;
        condition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    public void Burn()
    {
        var burn = inflictCondition.Burn(currentHP, condition.burn, condition.invalidBadStatus);
        currentHP = burn.currentHP;
        condition.invalidBadStatus = burn.invalidBadStatus;
    }
    public void Poison(int moveCount)
    {
        var poison = inflictCondition.Poison(currentHP, condition.poison, condition.invalidBadStatus, moveCount);
        currentHP = poison.currentHP;
        condition.invalidBadStatus = poison.invalidBadStatus;
    }

    //�ȉ��A�J�[�h��Z�̌���
    public void TakeDamage(int damage) //�_���[�W���󂯂��Ƃ��̏���
    {
        int deductedDamage = 0;
        if (GP > 0) //�K�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - GP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        currentHP -= deductedDamage;
    }
    public void HealingHP(int healingHPPower) //HP�̉�
    {
        currentHP += healingHPPower;
    }
    public void HealingAP(int healingAPPower) //AP�̉�
    {
        currentAP += healingAPPower;
    }
    public void AddGP(int addGP) //GP�̒ǉ�
    {
        GP += addGP;
    }
    public void ReleaseBadStatus() //�o�b�h�X�e�[�^�X����������
    {
        condition.curse = 0;
        condition.impatience = 0;
        condition.weakness = 0;
        condition.burn = 0;
        condition.poison = 0;
    }
    public void ReleaseBuffStatus() //�o�t�X�e�[�^�X����������
    {
        condition.upStrength = 0;
        condition.autoHealing = 0;
        condition.invalidBadStatus = 0;
    }
    public int CheckBadStatus() //�o�b�h�X�e�[�^�X�����邩�`�F�b�N����
    {
        var charaC = condition;
        int badStatus = charaC.curse + charaC.impatience + charaC.weakness + charaC.burn + charaC.poison;
        return badStatus;
    }
    public int CheckBuffStatus() //�o�t�X�e�[�^�X�����邩�`�F�b�N����
    {
        var charaC = condition;
        int buffStatus = charaC.upStrength + charaC.autoHealing + charaC.invalidBadStatus;
        return buffStatus;
    }
    public void AddConditionStatus(string status, int count) //��Ԉُ��ǉ�����
    {
        if (status == "UpStrength")
        {
            condition.upStrength += count;
        }
        else if (status == "AutoHealing")
        {
            condition.autoHealing += count;
        }
        else if (status == "InvalidBadStatus")
        {
            condition.invalidBadStatus += count;
        }
        else if (status == "Curse")
        {
            condition.curse += count;
        }
        else if (status == "Impatience")
        {
            condition.impatience += count;
        }
        else if (status == "Weakness")
        {
            condition.weakness += count;
        }
        else if (status == "Burn")
        {
            condition.burn += count;
        }
        else if (status == "Poison")
        {
            condition.poison += count;
        }
    }
}