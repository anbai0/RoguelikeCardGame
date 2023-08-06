using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Dictionary<int, int> hasPlayerRelics = new Dictionary<int, int>();
    int playerMoney;//�v���C���[�̏�����
    CardEffectList cardEffectList;//�J�[�h�̌��ʃX�N���v�g
    RelicEffectList relicEffect; //�����b�N�̌���
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
        cardEffectList = GetComponent<CardEffectList>();
        GetSetCondition = new ConditionStatus();
        GetSetInflictCondition = GetComponent<InflictCondition>();
        //hasPlayerRelics = GameManager.Instance.hasRelics;
        //Debug�p
        for(int RelicID = 1; RelicID <= 12; RelicID++)
        {
            hasPlayerRelics[RelicID] = 0;
        }
    }
    /// <summary>
    /// �퓬�J�n���ɔ������郌���b�N����
    /// </summary>
    /// <param name="enemyBattleAction">�G�l�~�[�̃X�e�[�^�X</param>
    /// <param name="enemyName">�G�l�~�[�̖��O</param>
    /// <returns>�ύX���������G�l�~�[�̃X�e�[�^�X</returns>
    public EnemyBattleAction StartRelicEffect(EnemyBattleAction enemyBattleAction, string enemyName)
    {
        relicEffect = GetComponent<RelicEffectList>();
        var es = enemyBattleAction;
        var relicEffectID2 = relicEffect.RelicID2(hasPlayerRelics[2], GetSetCondition.upStrength, es.GetSetCondition.upStrength);
        GetSetCondition.upStrength = relicEffectID2.playerUpStrength;
        es.GetSetCondition.upStrength = relicEffectID2.enemyUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(hasPlayerRelics[4], GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetConstAP, GetSetChargeAP).constAP;
        es.GetSetCondition.burn = relicEffect.RelicID6(hasPlayerRelics[6], es.GetSetCondition.burn);
        GetSetHP = relicEffect.RelicID7(hasPlayerRelics[7], GetSetHP);
        GetSetGP = relicEffect.RelicID8(hasPlayerRelics[8], GetSetGP);
        GetSetCondition.upStrength = relicEffect.RelicID12(hasPlayerRelics[12], enemyName, GetSetCondition.upStrength);
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
        GetSetCondition = relicEffect.RelicID11(hasPlayerRelics[11], GetSetCondition);
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
