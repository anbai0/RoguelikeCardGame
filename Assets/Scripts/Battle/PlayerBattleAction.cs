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
    public int GetSetPlayerMoney { get => playerMoney; set => playerMoney = value; }
    private void Update()
    {
        UpdateText(playerHPText, playerAPText, playerGPText, null);
    }
    public void Move(CardController card)//�v���C���[�̌��ʏ���
    {
        GetSetCurrentAP -= card.cardDataManager._cardCost;
        Debug.Log("���݂�PlayerCurrentAP��" + GetSetCurrentAP);
        cardEffectList.ActiveCardEffect(card);
    }
    public void SetStatus(PlayerDataManager playerData)//�e�X�e�[�^�X������U��
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
    }
    public void TakeMoney(int getMoney)
    {
    }
}
