using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBattleAction : CharacterBattleAction
{
    [Header("�v���C���[���e�L�X�g")]
    [SerializeField] Text playerNameText;
    [Header("�v���C���[HP�e�L�X�g")]
    [SerializeField] Text playerHPText;
    [Header("�v���C���[AP�e�L�X�g")]
    [SerializeField] Text playerAPText;
    [Header("�v���C���[GP�e�L�X�g")]
    [SerializeField] Text playerGPText;
    [Header("�_���[�W�\���I�u�W�F�N�g")]
    [SerializeField]
    GameObject damageUI;
    [Header("�񕜕\���I�u�W�F�N�g")]
    [SerializeField]
    GameObject healingUI;
    [Header("�_���[�W�Ɖ񕜕\���̏o���ꏊ")]
    [SerializeField]
    GameObject damageOrHealingPos;
    [Header("��Ԉُ�̃A�C�R���\���X�N���v�g")]
    [SerializeField]
    PlayerConditionDisplay playerConditionDisplay;
    [Header("��ʗh��̃X�N���v�g")]
    [SerializeField]
    ShakeBattleField shakeBattleField;

    public Dictionary<int, int> hasPlayerRelics = new Dictionary<int, int>(); //�v���C���[�̏������Ă��郌���b�N
    int maxRelics = 12;
    RelicEffectList relicEffect; //�����b�N�̌���

    public Dictionary<string, int> playerCondition = new Dictionary<string, int>(); //�v���C���[�ɕt�^����Ă����Ԉُ�
    
    CardEffectList cardEffectList;//�J�[�h�̌��ʃX�N���v�g

    int playerMoney;//�v���C���[�̏�����
    public int GetSetPlayerMoney { get => playerMoney; set => playerMoney = value; }

    private void Update()
    {
        UpdateText(playerHPText, playerAPText, playerGPText, null);
    }

    /// <summary>
    /// �v���C���[���g�p�����J�[�h���ʂ̏���
    /// </summary>
    /// <param name="card">�g�p�����J�[�h</param>
    public void Move(CardController card)
    {
        GetSetCurrentAP -= card.cardDataManager._cardCost;
        Debug.Log("���݂�PlayerCurrentAP��" + GetSetCurrentAP);
        cardEffectList.ActiveCardEffect(card);
    }

    /// <summary>
    /// �e�X�e�[�^�X���Z�b�g���鏈��
    /// </summary>
    /// <param name="playerData">�E��ɉ������v���C���[�f�[�^</param>
    public void SetStatus(PlayerDataManager playerData)
    {
        playerNameText.text = "���݂̃L����:" + playerData._playerName;
        GetSetHP = playerData._playerHP;
        GetSetCurrentHP = GetSetHP;
        GetSetAP = playerData._playerAP;
        GetSetConstAP = playerData._playerAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = playerData._playerGP;
        playerMoney = playerData._playerMoney;
        InitializedCondition();
        GetSetCondition = playerCondition;
        cardEffectList = GetComponent<CardEffectList>();
        GetSetInflictCondition = GetComponent<InflictCondition>();
        //hasPlayerRelics = GameManager.Instance.hasRelics;
        //Debug�p
        for(int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasPlayerRelics[RelicID] = 0;
        }
    }

    /// <summary>
    /// ��Ԉُ�̖��O�Ə����������������Ă�������
    /// </summary>
    void InitializedCondition()
    {
        playerCondition.Add("UpStrength", 0);
        playerCondition.Add("AutoHealing", 0);
        playerCondition.Add("InvalidBadStatus", 0);
        playerCondition.Add("Curse", 0);
        playerCondition.Add("Impatience", 0);
        playerCondition.Add("Weakness", 0);
        playerCondition.Add("Burn", 0);
        playerCondition.Add("Poison", 0);
    }

    /// <summary>
    /// �_���[�W���󂯂��Ƃ��̏���
    /// </summary>
    /// <param name="damage">�󂯂��_���[�W</param>
    public void TakeDamage(int damage)
    {
        int deductedDamage = 0;
        if (GetSetGP > 0) //�K�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - GetSetGP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GetSetGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        GetSetCurrentHP -= deductedDamage;

        if (deductedDamage > 0)
        {
            ViewDamage(deductedDamage);
            shakeBattleField.Shake(0.25f, 10f);
        }
    }

    /// <summary>
    /// �_���[�W���o��\�����鏈��
    /// </summary>
    /// <param name="damage">�󂯂��_���[�W</param>
    void ViewDamage(int _damage)
    {
        GameObject damageObj = Instantiate(damageUI, damageOrHealingPos.transform);
        damageObj.GetComponent<TextMeshProUGUI>().text = _damage.ToString();
    }

    /// <summary>
    /// HP�񕜂̏���
    /// </summary>
    /// <param name="healingHPPower">HP�̉񕜗�</param>
    public void HealingHP(int healingHPPower)
    {
        GetSetCurrentHP += healingHPPower;
        if (healingHPPower > 0)
        {
            ViewHealing(healingHPPower);
        }
    }

    /// <summary>
    /// �񕜉��o��\�����鏈��
    /// </summary>
    /// <param name="_healingHPPower">HP�̉񕜗�</param>
    void ViewHealing(int _healingHPPower)
    {
        GameObject healingObj = Instantiate(healingUI, damageOrHealingPos.transform);
        healingObj.GetComponent<TextMeshProUGUI>().text = _healingHPPower.ToString();
    }

    /// <summary>
    /// ��Ԉُ�̃A�C�R����\�����鏈��
    /// </summary>
    public void ViewConditionIcon()
    {
        playerConditionDisplay.ViewIcon(playerCondition);
    }

    //�ȉ��ATakeDamage��HealingHP�Ɋ֌W�����Ԉُ�̌Ăяo��
    /// <summary>
    /// �����񕜂̌Ăяo��
    /// </summary>
    public void AutoHealing()
    {
        HealingHP(GetSetInflictCondition.AutoHealing(playerCondition["AutoHealing"]));
    }

    /// <summary>
    /// �Ώ��̌Ăяo��
    /// </summary>
    public void Burn()
    {
        var burn = GetSetInflictCondition.Burn(playerCondition["Burn"], playerCondition["InvalidBadStatus"]);
        TakeDamage(burn.damage);
        playerCondition["InvalidBadStatus"] = burn.invalidBadStatus;
    }

    /// <summary>
    /// �דł̌Ăяo��
    /// </summary>
    /// <param name="moveCount">�s����</param>
    public void Poison(int moveCount)
    {
        var poison = GetSetInflictCondition.Poison(playerCondition["Poison"], playerCondition["InvalidBadStatus"], moveCount);
        TakeDamage(poison.damage);
        playerCondition["InvalidBadStatus"] = poison.invalidBadStatus;
    }

    /// <summary>
    /// �퓬�J�n���ɔ������郌���b�N����
    /// </summary>
    /// <param name="enemyBattleAction">�G�l�~�[�̃X�e�[�^�X</param>
    /// <param name="enemyType">�G�l�~�[�̎��</param>
    /// <returns>�ύX���������G�l�~�[�̃X�e�[�^�X</returns>
    public EnemyBattleAction StartRelicEffect(EnemyBattleAction enemyBattleAction, string enemyType)
    {
        relicEffect = GetComponent<RelicEffectList>();
        var es = enemyBattleAction;
        Debug.Log(playerCondition.ContainsKey("UpStrength"));
        var relicEffectID2 = relicEffect.RelicID2(hasPlayerRelics[2], playerCondition["UpStrength"], es.enemyCondition["UpStrength"]);
        playerCondition["UpStrength"] = relicEffectID2.playerUpStrength;
        es.enemyCondition["UpStrength"] = relicEffectID2.enemyUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(hasPlayerRelics[4], GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetConstAP, GetSetChargeAP).constAP;
        es.enemyCondition["Burn"] = relicEffect.RelicID6(hasPlayerRelics[6], es.enemyCondition["Burn"]);
        GetSetHP = relicEffect.RelicID7(hasPlayerRelics[7], GetSetHP);
        GetSetGP = relicEffect.RelicID8(hasPlayerRelics[8], GetSetGP);
        playerCondition["UpStrength"] = relicEffect.RelicID12(hasPlayerRelics[12], enemyType, playerCondition["UpStrength"]);
        Debug.Log("�X�^�[�g���̃����b�N���Ăяo����܂���: " + GetSetConstAP + " to " + GetSetChargeAP);
        return es;
    }

    /// <summary>
    /// ���E���h�I�����Ɉ�x�����������郌���b�N����
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).chargeAP;
        GetSetChargeAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetAP, GetSetChargeAP).chargeAP;
    }

    /// <summary>
    /// ���E���h�I�����ɔ������郌���b�N����
    /// </summary>
    public void EndRoundRelicEffect()
    {
        playerCondition = relicEffect.RelicID11(hasPlayerRelics[11], playerCondition);
    }

    /// <summary>
    /// �퓬�I�����ɔ������郌���b�N����
    /// </summary>
    /// <returns>�퓬�I����̃S�[���h�l�����ɑ����Ⴆ�鐔</returns>
    public int EndGameRelicEffect()
    {
        int money = 10;
        money = relicEffect.RelicID9(hasPlayerRelics[9], money);
        GetSetCurrentHP = relicEffect.RelicID10(hasPlayerRelics[10], GetSetCurrentHP);
        return money;
    }
}
