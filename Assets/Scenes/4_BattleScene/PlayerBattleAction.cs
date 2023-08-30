using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �v���C���[�̍s�����܂Ƃ߂��X�N���v�g
/// </summary>
public class PlayerBattleAction : CharacterBattleAction
{
    [SerializeField, Header("�v���C���[HP�e�L�X�g")] Text playerHPText;
    [SerializeField, Header("�v���C���[AP�e�L�X�g")] Text playerAPText;
    [SerializeField, Header("�v���C���[GP�e�L�X�g")] Text playerGPText;
    [SerializeField, Header("�_���[�W�\���I�u�W�F�N�g")] GameObject damageUI;
    [SerializeField, Header("�񕜕\���I�u�W�F�N�g")] GameObject healingUI;
    [SerializeField, Header("�K�[�h�\���I�u�W�F�N�g")] GameObject gardUI;
    [SerializeField, Header("�_���[�W�Ɖ񕜕\���̏o���ꏊ")] GameObject damageOrHealingPos;
    [SerializeField, Header("��Ԉُ�̃A�C�R���\���X�N���v�g")] PlayerConditionDisplay playerConditionDisplay;
    [SerializeField, Header("��ʗh��̃X�N���v�g")] ShakeBattleField shakeBattleField;
    
    [SerializeField] RelicEffectList relicEffect;
    [SerializeField] CardEffectList cardEffectList;
    
    public Dictionary<int, int> hasPlayerRelics = new Dictionary<int, int>(); //�v���C���[�̏������Ă��郌���b�N
    public Dictionary<string, int> playerCondition = new Dictionary<string, int>(); //�v���C���[�ɕt�^����Ă����Ԉُ�
    
    int playerMoney; //�v���C���[�̏�����
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
        cardEffectList.ActiveCardEffect(card);
    }

    /// <summary>
    /// �e�X�e�[�^�X���Z�b�g���鏈��
    /// </summary>
    /// <param name="playerData">�E��ɉ������v���C���[�f�[�^</param>
    public void SetStatus(PlayerDataManager playerData)
    {
        GetSetHP = playerData._playerHP;
        GetSetCurrentHP = playerData._playerCurrentHP;
        GetSetAP = playerData._playerAP;
        GetSetConstAP = playerData._playerAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = playerData._playerGP;
        playerMoney = playerData._playerMoney;
        InitializedCondition();
        GetSetCondition = playerCondition;
        GetSetInflictCondition = GetComponent<InflictCondition>();
        hasPlayerRelics = GameManager.Instance.hasRelics;
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
        if (damage <= 0) return; //�_���[�W��0�ȉ��������ꍇ���̏������񂳂Ȃ�

        int deductedDamage = 0;
        if (GetSetGP > 0) //�K�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - GetSetGP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GetSetGP -= damage;
            ViewGard();
        }
        else
        {
            deductedDamage = damage;
        }
        GetSetCurrentHP -= deductedDamage;

        if (deductedDamage > 0)
        {
            AudioManager.Instance.PlaySE("�U��1");
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
    /// �K�[�h�������Ƃ�`����e�L�X�g��\�����鏈��
    /// </summary>
    void ViewGard()
    {
        GameObject gardObj = Instantiate(gardUI, damageOrHealingPos.transform);
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
            AudioManager.Instance.PlaySE("��");
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
        TakeDamage(playerCondition["Burn"]);
    }

    /// <summary>
    /// �דł̌Ăяo��
    /// </summary>
    /// <param name="moveCount">�s����</param>
    public void Poison(int moveCount)
    {
        var poison = GetSetInflictCondition.Poison(playerCondition["Poison"], moveCount);
        TakeDamage(poison);
    }

    /// <summary>
    /// �퓬�J�n���ɔ������郌���b�N����
    /// </summary>
    /// <param name="enemyBattleAction">�G�l�~�[�̃X�e�[�^�X</param>
    /// <param name="enemyType">�G�l�~�[�̎��</param>
    /// <returns>�ύX���������G�l�~�[�̃X�e�[�^�X</returns>
    public EnemyBattleAction StartRelicEffect(EnemyBattleAction enemyBattleAction, string enemyType)
    {
        var es = enemyBattleAction;
        relicEffect.RelicID2(hasPlayerRelics[2]);
        GetSetConstAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(hasPlayerRelics[4], GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetConstAP, GetSetChargeAP).constAP;
        es.AddConditionStatus("Burn", relicEffect.RelicID6(hasPlayerRelics[6]));
        GetSetHP = relicEffect.RelicID7(hasPlayerRelics[7], GetSetHP);
        GetSetGP = relicEffect.RelicID8(hasPlayerRelics[8], GetSetGP);
        AddConditionStatus("UpStrength", relicEffect.RelicID12(hasPlayerRelics[12], enemyType));
        return es;
    }

    /// <summary>
    /// ���E���h���ɔ������郌���b�N����
    /// </summary>
    public void OnRoundRelicEffect()
    {
        
    }

    /// <summary>
    /// ���E���h�I�����Ɉ�x�����������郌���b�N����
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).chargeAP;
    }

    /// <summary>
    /// ���E���h�I�����ɔ������郌���b�N����
    /// </summary>
    public void EndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetAP, GetSetChargeAP).chargeAP;
        playerCondition = relicEffect.RelicID11(hasPlayerRelics[11], playerCondition);
    }

    /// <summary>
    /// �퓬�I�����ɔ������郌���b�N����
    /// </summary>
    /// <returns>�퓬�I����̃S�[���h�l�����ɑ����Ⴆ�鐔</returns>
    public int EndGameRelicEffect()
    {
        HealingHP(relicEffect.RelicID10(hasPlayerRelics[10]));
        return relicEffect.RelicID9(hasPlayerRelics[9]);
    }
}
