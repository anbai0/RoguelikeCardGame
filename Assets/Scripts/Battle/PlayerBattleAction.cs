using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleAction : MonoBehaviour
{
    [Header("�v���C���[���e�L�X�g")]
    [SerializeField] Text playerNameText;
    [Header("�v���C���[HP�e�L�X�g")]
    [SerializeField] Text playerHPText;
    [Header("�v���C���[AP�e�L�X�g")]
    [SerializeField] Text playerAPText;
    [Header("�v���C���[GP�e�L�X�g")]
    [SerializeField] Text playerGPText;
    private int playerHP;//�ő��HP
    private int playerCurrentHP;//���݂�HP
    private int playerConstAP;//�Q�[���J�n���̍ő�AP
    private int roundStartAP;//���E���h�J�n���̍ő�AP
    private int playerAP;//�ő�AP
    private int playerCurrentAP;//���݂�AP
    private int playerChargeAP;//���E���h�o�߂ŏ㏸���Ă���AP�̒l
    private int playerGP;//�K�[�h�|�C���g
    private int playerMoney;//�v���C���[�̏�����
    private ConditionStatus playerCondition;//�v���C���[�̏�Ԉُ�X�e�[�^�X
    private CardEffectList cardEffectList;//�J�[�h�̌��ʃX�N���v�g
    private InflictCondition inflictCondition;//��Ԉُ�̌��ʃX�N���v�g
    public int GetSetPlayerHP { get => playerHP; set => playerHP = value; }
    public int GetSetPlayerCurrentHP { get => playerCurrentHP; set => playerCurrentHP = value; }
    public int GetSetPlayerConstAP { get => playerConstAP; set => playerConstAP = value; }
    public int GetSetPlayerAP { get => playerAP; set => playerAP = value; }
    public int GetSetPlayerCurrentAP { get => playerCurrentAP; set => playerCurrentAP = value; }
    public int GetSetPlayerChargeAP { get => playerChargeAP; set => playerChargeAP = value; }
    public int GetSetPlayerGP { get => playerGP; set => playerGP = value; }
    public int GetSetPlayerMoney { get => playerMoney; set => playerMoney = value; }
    public ConditionStatus GetSetPlayerCondition { get => playerCondition; set => playerCondition = value; }
    private void Start()
    {
        playerChargeAP = 0;
    }
    private void Update()
    {
        UpdateText();
    }
    public void SetUpAP()//���E���h�J�n����AP�v�Z
    {
        var curse = inflictCondition.Curse(playerConstAP, playerChargeAP, playerCondition.curse, playerCondition.invalidBadStatus);
        playerAP = curse.nextRoundAP;
        playerCurrentAP = playerAP;
        playerCondition.invalidBadStatus = curse.invalidBadStatus;
    }
    public void Move(CardController card)//�v���C���[�̌��ʏ���
    {
        playerCurrentAP -= card.cardDataManager._cardCost;
        Debug.Log("���݂�PlayerCurrentAP��" + playerCurrentAP);
        cardEffectList.ActiveCardEffect(card);
    }
    public void TurnEnd()//�v���C���[���s���s�\�ɂ���
    {
        playerCurrentAP = 0;
    }
    public void SetStatus(PlayerDataManager playerData)//�e�X�e�[�^�X������U��
    {
        playerNameText.text = "���݂̃L����:" + playerData._playerName;
        playerHP = playerData._playerHP;
        playerCurrentHP = playerHP;
        playerConstAP = playerData._playerAP;
        playerAP = playerData._playerAP;
        playerCurrentAP = playerAP;
        playerGP = playerData._playerGP;
        playerMoney = playerData._money;
        cardEffectList = GetComponent<CardEffectList>();
        playerCondition = new ConditionStatus();
        inflictCondition = GetComponent<InflictCondition>();
    }

    public void UpdateText() //�e�e�L�X�g�̍X�V
    {
        if (playerCurrentHP > playerHP)
        {
            playerCurrentHP = playerHP;
        }
        if (playerCurrentHP < 0)
        {
            playerCurrentHP = 0;
        }
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        if (playerCurrentAP > playerAP)
        {
            playerCurrentAP = playerAP;
        }
        if (playerCurrentAP < 0)
        {
            playerCurrentAP = 0;
        }
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        if (playerGP < 0)
        {
            playerGP = 0;
        }
        playerGPText.text = playerGP.ToString();
    }
    public void ChargeAP() //���E���h�I������AP����
    {
        playerChargeAP++;
    }
    public bool CheckHP() //�v���C���[������ł��Ȃ����`�F�b�N����
    {
        if (playerCurrentHP <= 0)
        {
            return true;
        }
        return false;
    }
    public void TakeDamage(int damage) //�_���[�W���󂯂��Ƃ��̏���
    {
        int deductedDamage = 0;
        if (playerGP > 0) //�v���C���[�ɃK�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - playerGP;
            if (deductedDamage < 0)
            {
                deductedDamage = 0;
            }
            playerGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        playerCurrentHP -= deductedDamage;
    }
    public void SaveAP() //���E���h�J�n����AP��ۑ����Ă���
    {
        roundStartAP = playerAP;
    }
    public bool IsCurse() //������ԂɂȂ��Ă��邩�m�F����
    {
        //�^�[���I�����̍ő�AP
        int currentMaxAP = inflictCondition.Curse(playerConstAP, playerChargeAP, playerCondition.curse, playerCondition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false;
    }
    //�ȉ��A��Ԉُ�
    public int PlayerUpStrength(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, playerCondition.upStrength);
        return attackPower;
    }
    public void PlayerAutoHealing()
    {
        playerCurrentHP = inflictCondition.AutoHealing(playerCurrentHP, playerCondition.autoHealing);
    }
    public void PlayerImpatience()
    {
        var impatience = inflictCondition.Impatience(playerCurrentAP, playerCondition.impatience, playerCondition.invalidBadStatus);
        playerCurrentAP = impatience.currentAP;
        playerCondition.invalidBadStatus = impatience.invalidBadStatus;
    }
    public int PlayerWeakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, playerCondition.weakness, playerCondition.invalidBadStatus);
        attackPower = weakness.attackPower;
        playerCondition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    public void PlayerBurn()
    {
        var burn = inflictCondition.Burn(playerCurrentHP, playerCondition.burn, playerCondition.invalidBadStatus);
        playerCurrentHP = burn.currentHP;
        playerCondition.invalidBadStatus = burn.invalidBadStatus;
    }
    public void PlayerPoison(int moveCount)
    {
        var poison = inflictCondition.Poison(playerCurrentHP, playerCondition.poison, playerCondition.invalidBadStatus, moveCount);
        playerCurrentHP = poison.currentHP;
        playerCondition.invalidBadStatus = poison.invalidBadStatus;
    }
    //�ȉ��A�J�[�h����
    public void HealingHP(int healingHPPower) //HP�̉�
    {
        playerCurrentHP += healingHPPower;
    }
    public void HealingAP(int healingAPPower) //AP�̉�
    {
        playerCurrentAP += healingAPPower;
    }
    public void AddGP(int addGP) //GP�̒ǉ�
    {
        playerGP += addGP;
    }
    public void ReleaseBadStatus() //�o�b�h�X�e�[�^�X����������
    {
        playerCondition.curse = 0;
        playerCondition.impatience = 0;
        playerCondition.weakness = 0;
        playerCondition.burn = 0;
        playerCondition.poison = 0;
    }
    public int CheckBadStatus() //�o�b�h�X�e�[�^�X�����邩�`�F�b�N����
    {
        var pc = playerCondition;
        int badStatus = pc.curse + pc.impatience + pc.weakness + pc.burn + pc.poison;
        return badStatus;
    }
    public void AddConditionStatus(string status, int count) //��Ԉُ��ǉ�����
    {
        if (status == "UpStrength")
        {
            playerCondition.upStrength += count;
        }
        else if (status == "AutoHealing")
        {
            playerCondition.autoHealing += count;
        }
        else if (status == "InvalidBadStatus")
        {
            playerCondition.invalidBadStatus += count;
        }
        else if (status == "Curse")
        {
            playerCondition.curse += count;
        }
        else if (status == "Impatience")
        {
            playerCondition.impatience += count;
        }
        else if (status == "Weakness")
        {
            playerCondition.weakness += count;
        }
        else if (status == "Burn")
        {
            playerCondition.burn += count;
        }
        else if (status == "Poison")
        {
            playerCondition.poison += count;
        }
    }
    public void TakeMoney(int getMoney)
    {
    }
}
