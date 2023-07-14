using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CardEffectList : MonoBehaviour
{
    BattleGameManager bg;
    PlayerBattleAction player;
    EnemyBattleAction enemy;
    int cardID;
    int cardAttackPower;
    int cardHealingPower;
    int cardGuardPoint;

    public void ActiveCardEffect(CardController card)
    {
        bg = BattleGameManager.Instance;
        player = GetComponent<PlayerBattleAction>();
        enemy = GetComponent<EnemyBattleAction>();
        cardID = card.cardDataManager._cardID;
        cardAttackPower = card.cardDataManager._cardAttackPower;
        cardHealingPower = card.cardDataManager._cardHealingPower;
        cardGuardPoint = card.cardDataManager._cardGuardPoint;
        //upStrength = bg.playerCondition.upStrength;
        //weakness = bg.playerCondition.weakness;
        Debug.Log("Card�̃i���o�[��" + cardID);
        //�J�[�h��ID�ɉ����ď������Ăяo��
        switch (cardID)
        {
            case 1:
                CardID1();
                break;
            case 2:
                CardID2();
                break;
            case 3:
                CardID3(card);
                break;
            case 4:
                CardID4();
                break;
            case 5:
                CardID5(card);
                break;
            case 6:
                CardID6();
                break;
            case 7:
                CardID7();
                break;
            case 8:
                CardID8(card);
                break;
            case 9:
                CardID9();
                break;
            case 10:
                CardID10();
                break;
            case 11:
                CardID11();
                break;
            case 12:
                CardID12();
                break;
            case 13:
                CardID13(card);
                break;
            case 14:
                CardID14();
                break;
            case 15:
                CardID15(card);
                break;
            case 16:
                CardID16();
                break;
            case 17:
                CardID17(card);
                break;
            case 18:
                CardID18(card);
                break;
            case 19:
                CardID19(card);
                break;
            case 20:
                CardID20();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    /// <summary>
    /// �Z���F�X�C���O
    /// </summary>
    private void CardID1()
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// �Z���F�q�[��
    /// </summary>
    private void CardID2()
    {
        Debug.Log("CardID2���Ăяo����܂���");
        //HP����
        PlayerHealing(cardHealingPower);
    }
    /// <summary>
    /// �Z���F�����̗��
    /// </summary>
    private void CardID3(CardController card)
    {
        //HP����
        PlayerHealing(cardHealingPower);
        Destroy(card.gameObject);
    }
    /// <summary>
    /// �Z���F�K�[�h
    /// </summary>
    private void CardID4()
    {
        //�K�[�h��ǉ�
        PlayerAddGP(cardGuardPoint);
    }
    /// <summary>
    /// �Z���F�c�o���Ԃ�
    /// </summary>
    private void CardID5(CardController card)
    {
        //���ԍ��ŃG�l�~�[���U��
        StartCoroutine(ID5Attacking(cardAttackPower, 0.8f));
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }

    IEnumerator ID5Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        Debug.Log("���݂�weakness��" + player.GetSetPlayerCondition.weakness);
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        Debug.Log("���݂�weakness��" + player.GetSetPlayerCondition.weakness);
        PlayerAttacking(attackMethod);
        bg.isCoroutine = false;
        bg.TurnCalc();
        //bg.isCardEffect = false;
    }
    /// <summary>
    /// �Z���F�V�[���h�o�b�V��
    /// </summary>
    private void CardID6()
    {
        //�K�[�h��ǉ�
        PlayerAddGP(cardGuardPoint);
        //�G�l�~�[���U��
        PlayerAttacking(player.GetSetPlayerGP);
    }
    /// <summary>
    /// �Z���F�t��
    /// </summary>
    private void CardID7()
    {
        //�o�b�h�X�e�[�^�X���P�ł��������ꍇ
        if (player.CheckBadStatus() > 0)
        {
            //�o�b�h�X�e�[�^�X������
            PlayerReleaseBadStatus();
            //�ؗ͑�����2�t�^
            player.AddConditionStatus("UpStrength", 2);
        }
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// �Z���F���R�V�F�g
    /// </summary>
    private void CardID8(CardController card)
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�J�[�h�̍U���͂��P����
        card.cardDataManager._cardAttackPower++;
        //���������U���͂��J�[�h�ɔ��f
        GameObject cardEffect = card.gameObject.transform.GetChild(2).gameObject;//���ʕ\���̃I�u�W�F�N�g
        Text cardText = cardEffect.GetComponent<Text>();
        string text = cardText.text;
        string pattern = @"\d+";// ���p�����𒊏o���邽�߂̐��K�\���p�^�[��
        MatchCollection matches = Regex.Matches(text, pattern);// ���K�\���p�^�[���Ɉ�v���锼�p����������
        foreach (Match match in matches)// ���p������+1����
        {
            string numberString = match.Value;
            int number;
            if (Int32.TryParse(numberString, out number))
            {
                int addNumber = number + 1;

                // ���̕�������̔��p������u������
                text = text.Replace(numberString, addNumber.ToString());
            }
            else
            {
                Debug.LogWarning("Invalid number format: " + numberString);
            }
        }
        cardText.text = text;
    }
    /// <summary>
    /// �Z���F�򉻂̕�
    /// </summary>
    private void CardID9()
    {
        //�S�Ẵo�b�h�X�e�[�^�X������
        PlayerReleaseBadStatus();
        //��Ԉُ햳����1�t�^
        player.AddConditionStatus("InvalidBadStatus", 1);
    }
    /// <summary>
    /// �Z���F�N���A���F�[��
    /// </summary>
    private void CardID10()
    {
        //�K�[�h��ǉ�
        PlayerAddGP(cardGuardPoint);
        //��Ԉُ햳����3�t�^
        player.AddConditionStatus("InvalidBadStatus", 3);
    }
    /// <summary>
    /// �Z���F�A�N�Z�����[�g
    /// </summary>
    private void CardID11()
    {
        //BattleGameManager�̃A�N�Z�����[�g�@�\��ON
        bg.isAccelerate = true;
    }
    /// <summary>
    /// �Z���F�����
    /// </summary>
    private void CardID12()
    {
        //���ԍ��ŃG�l�~�[���U��
        StartCoroutine(ID12Attacking(cardAttackPower, 0.2f));
    }
    IEnumerator ID12Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        for (int count = 0; count < 3; count++)
        {
            yield return new WaitForSeconds(attackInterval);
            PlayerAttacking(attackMethod);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// �Z���F�t���o�[�X�g
    /// </summary>
    private void CardID13(CardController card)
    {
        //AP��S����
        //���݂�AP����2�{�̃_���[�W���v�Z
        int damage = player.GetSetPlayerCurrentAP * 2;
        player.GetSetPlayerCurrentAP = 0;
        //�G�l�~�[���U��
        PlayerAttacking(damage);
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// �Z���F�n�C�{���P�[�m  
    /// </summary>
    private void CardID14()
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�Ώ���2�t�^
        enemy.AddConditionStatus("Burn", 2);
    }
    /// <summary>
    /// �Z���F�f�r���h���C��
    /// </summary>
    private void CardID15(CardController card)
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //HP����
        PlayerHealing(cardHealingPower);
        //�G�l�~�[�ɐ����1�t�^
        enemy.AddConditionStatus("Weakness", 1);
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// �Z���F�K���e�B�[��
    /// </summary>
    private void CardID16()
    {
        if (enemy.GetSetEnemyCondition.burn > 0)//�G�l�~�[���Ώ���Ԃ������ꍇ
        {
            //�Ώ���3�{�̃_���[�W�ŃG�l�~�[���U��
            PlayerAttacking(enemy.GetSetEnemyCondition.burn * 3);
        }
        else
        {
            //�G�l�~�[���U��
            PlayerAttacking(cardAttackPower);
        }
    }
    /// <summary>
    /// �Z���F��軂̏Ռ�
    /// </summary>
    private void CardID17(CardController card)
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�G�l�~�[���s���s�\�ɂ���
        enemy.TurnEnd();
        //���̐퓬���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// �Z���F�G�N�X�J���o�[
    /// </summary>
    private void CardID18(CardController card)
    {
        PlayerAttacking(cardAttackPower);
        if (bg.roundCount >= 3)//3���E���h�ڈȍ~�̏ꍇ
        {
            //�v���C���[��AP���񕜂���
            StartCoroutine(ID18APHealing(1.0f));
        }
        //���̐퓬���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 2;
    }
    IEnumerator ID18APHealing(float attackInterval)
    {
        bg.isCoroutine = true;
        yield return new WaitForSeconds(attackInterval);
        //�v���C���[��AP��7�񕜂���
        player.HealingAP(7);
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// �Z���F�A���@�������F�[��
    /// </summary>
    private void CardID19(CardController card)
    {
        //�����񕜂�1�t�^
        player.AddConditionStatus("AutoHealing", 1);
        //��Ԉُ햳����2�t�^
        player.AddConditionStatus("InvalidBadStatus", 2);
        //���̐퓬���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// �Z���F�S��
    /// </summary>
    private void CardID20()
    {
        //�Ώ���1�t�^
        enemy.AddConditionStatus("Burn", 1);
    }
    private void PlayerAttacking(int attackMethod)//�G�l�~�[�ւ̍U������ 
    {
        attackMethod = ChangeAttackPower(attackMethod);
        Debug.Log("�v�Z��̍U���͂�" + attackMethod);
        enemy.TakeDamage(attackMethod);
    }
    private int ChangeAttackPower(int attackPower) //��Ԉُ�ɂ��U���͂̑���
    {
        attackPower = player.PlayerUpStrength(attackPower);
        attackPower = player.PlayerWeakness(attackPower);
        return attackPower;
    }
    private void PlayerHealing(int healingPower)//�v���C���[��HP�񕜏���
    {
        player.HealingHP(healingPower);
    }
    private void PlayerAddGP(int addGP)//�v���C���[�ɃK�[�h��ǉ�
    {
        player.AddGP(addGP);
    }
    private void PlayerReleaseBadStatus()//�v���C���[�ɕt�^���ꂽ�f�o�t�̉���
    {
        player.ReleaseBadStatus();
    }
}
