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
        GetSetRelicStatus = new RelicStatus();
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
        var pr = GetSetRelicStatus;
        var relicEffectID2 = relicEffect.RelicID2(pr.hasRelicID2, GetSetCondition.upStrength, es.GetSetCondition.upStrength);
        GetSetCondition.upStrength = relicEffectID2.playerUpStrength;
        es.GetSetCondition.upStrength = relicEffectID2.enemyUpStrength;
        GetSetConstAP = relicEffect.RelicID3(pr.hasRelicID3, GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(pr.hasRelicID4, GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(pr.hasRelicID5, GetSetConstAP, GetSetChargeAP).constAP;
        es.GetSetCondition.burn = relicEffect.RelicID6(pr.hasRelicID6, es.GetSetCondition.burn);
        GetSetHP = relicEffect.RelicID7(pr.hasRelicID7, GetSetHP);
        GetSetGP = relicEffect.RelicID8(pr.hasRelicID8, GetSetGP);
        GetSetCondition.upStrength = relicEffect.RelicID12(pr.hasRelicID12, enemyName, GetSetCondition.upStrength);
        Debug.Log("�X�^�[�g���̃����b�N���Ăяo����܂���: " + GetSetConstAP + " to " + GetSetChargeAP);
        return es;
    }
    /// <summary>
    /// ���E���h�I�����Ɉ�x�����������郌���b�N����
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        var pr = GetSetRelicStatus;
        GetSetChargeAP = relicEffect.RelicID3(pr.hasRelicID3, GetSetConstAP, GetSetChargeAP).chargeAP;
        GetSetChargeAP = relicEffect.RelicID5(pr.hasRelicID5, GetSetAP, GetSetChargeAP).chargeAP;
    }
    /// <summary>
    /// ���E���h�I�����ɔ������郌���b�N����
    /// </summary>
    public void EndRoundRelicEffect()
    {
        var pr = GetSetRelicStatus;
        GetSetCondition = relicEffect.RelicID11(pr.hasRelicID11, GetSetCondition);
    }
    /// <summary>
    /// �퓬�I�����ɔ������郌���b�N����
    /// </summary>
    /// <returns>�퓬�I����̃S�[���h�l�����ɑ����Ⴆ�鐔</returns>
    public int EndGameRelicEffect()
    {
        var pr = GetSetRelicStatus;
        int money = 10;
        money = relicEffect.RelicID9(pr.hasRelicID9, money);
        GetSetCurrentHP = relicEffect.RelicID10(pr.hasRelicID10, GetSetCurrentHP);
        return money;
    }
    public void TakeMoney(int getMoney)
    {
    }
}
