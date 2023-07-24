using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

//���̃X�N���v�g�̓J�[�h�̌��ʂ��܂Ƃ߂����ł��B
//�t�B�[���h��Ƀh���b�v���ꂽ�J�[�h�̏��������Ŏ󂯎����ʂ̏��������܂��B
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
        Debug.Log("Card�̃i���o�[��" + cardID);
        //�J�[�h��ID�ɉ����ď������Ăяo��
        switch (cardID)
        {
            case 1:
                CardID1();
                break;
            case 101:
                CardID1();
                break;
            case 2:
                CardID2();
                break;
            case 102:
                CardID2();
                break;
            case 3:
                CardID3(card);
                break;
            case 4:
                CardID4();
                break;
            case 104:
                CardID4();
                break;
            case 5:
                CardID5(card);
                break;
            case 105:
                CardID105(card);
                break;
            case 6:
                CardID6();
                break;
            case 106:
                CardID6();
                break;
            case 7:
                CardID7();
                break;
            case 107:
                CardID107();
                break;
            case 8:
                CardID8(card);
                break;
            case 108:
                CardID8(card);
                break;
            case 9:
                CardID9();
                break;
            case 109:
                CardID109();
                break;
            case 10:
                CardID10();
                break;
            case 110:
                CardID10();
                break;
            case 11:
                CardID11();
                break;
            case 111:
                CardID111();
                break;
            case 12:
                CardID12();
                break;
            case 112:
                CardID112();
                break;
            case 13:
                CardID13(card);
                break;
            case 113:
                CardID113(card);
                break;
            case 14:
                CardID14();
                break;
            case 114:
                CardID114();
                break;
            case 15:
                CardID15(card);
                break;
            case 115:
                CardID15(card);
                break;
            case 16:
                CardID16();
                break;
            case 116:
                CardID16();
                break;
            case 17:
                CardID17(card);
                break;
            case 117:
                CardID17(card);
                break;
            case 18:
                CardID18(card);
                break;
            case 118:
                CardID18(card);
                break;
            case 19:
                CardID19(card);
                break;
            case 119:
                CardID19(card);
                break;
            case 20:
                CardID20();
                break;
            case 120:
                CardID120();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    /// <summary>
    /// �Z���F�X�C���O,�����X�C���O
    /// ���ʁF3(4)�_���[�W��^����B
    /// </summary>
    private void CardID1()
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// �Z���F�q�[��,�����q�[��
    /// ���ʁF������HP��3(4)�񕜁B
    /// </summary>
    private void CardID2()
    {
        //HP����
        PlayerHealing(cardHealingPower);
    }
    /// <summary>
    /// �Z���F�����̗��
    /// ���ʁF������HP��20�񕜂ł���B�i�������A��x�g���Ƃ��̃J�[�h�͏��ł���j
    /// </summary>
    private void CardID3(CardController card)
    {
        //HP����
        PlayerHealing(cardHealingPower);
        Destroy(card.gameObject);
    }
    /// <summary>
    /// �Z���F�K�[�h,�����K�[�h
    /// ���ʁF3(4)�K�[�h�𓾂�
    /// </summary>
    private void CardID4()
    {
        //�K�[�h��ǉ�
        PlayerAddGP(cardGuardPoint);
    }
    /// <summary>
    /// �Z���F�c�o���Ԃ�
    /// ���ʁF2�_���[�W��2��^����B
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
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// �Z���F�����c�o���Ԃ�
    /// ���ʁF2�_���[�W��3��^����B
    /// </summary>
    private void CardID105(CardController card)
    {
        //���ԍ��ŃG�l�~�[���U��
        StartCoroutine(ID105Attacking(cardAttackPower, 0.8f));
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }
    IEnumerator ID105Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// �Z���F�V�[���h�o�b�V��,�����V�[���h�o�b�V��
    /// ���ʁF�K�[�h��3���āA�����̃K�[�h�̐��Ɠ����_���[�W��^����B
    /// </summary>
    private void CardID6()
    {
        //�K�[�h��ǉ�
        PlayerAddGP(cardGuardPoint);
        //�G�l�~�[���U��
        PlayerAttacking(player.GetSetGP);
    }

    /// <summary>
    /// �Z���F�t��
    /// ���ʁF�����Ƀf�o�t������Ȃ炷�ׂĂ��������ċؗ͑�����2����B5�_���[�W�^����B
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
    /// �Z���F�����t��
    /// ���ʁF�����Ƀf�o�t������Ȃ炷�ׂĂ��������ċؗ͑�����3����B5�_���[�W�^����B
    /// </summary>
    private void CardID107()
    {
        //�o�b�h�X�e�[�^�X���P�ł��������ꍇ
        if (player.CheckBadStatus() > 0)
        {
            //�o�b�h�X�e�[�^�X������
            PlayerReleaseBadStatus();
            //�ؗ͑�����3�t�^
            player.AddConditionStatus("UpStrength", 3);
        }
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// �Z���F���R�V�F�g,�������R�V�F�g
    /// ���ʁF�����1(2)�_���[�W��^����B���̃Q�[�����A���̃R�}���h�̃_���[�W��1������B
    /// </summary>
    private void CardID8(CardController card)
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�J�[�h�̍U���͂��P����
        card.cardDataManager._cardAttackPower++;
        //���������U���͂��J�[�h�ɔ��f
        GameObject cardEffect = card.gameObject.transform.Find("CardInfo/CardEffect").gameObject;//���ʕ\���̃I�u�W�F�N�g
        TextMeshProUGUI cardText = cardEffect.GetComponent<TextMeshProUGUI>();
        string text = cardText.text;
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
            {
                // �ŏ��̔��p�����݂̂�+1���������ɕύX
                text = text.Remove(i, 1).Insert(i, card.cardDataManager._cardAttackPower.ToString());
                break;
            }
        }
        cardText.text = text; //�J�[�h��Text�ɔ��f
    }
    /// <summary>
    /// �Z���F�򉻂̕�
    /// ���ʁF�������󂯂Ă���f�o�t�����ׂĉ�������B��Ԉُ햳����1����B
    /// </summary>
    private void CardID9()
    {
        //�S�Ẵo�b�h�X�e�[�^�X������
        PlayerReleaseBadStatus();
        //��Ԉُ햳����1�t�^
        player.AddConditionStatus("InvalidBadStatus", 1);
    }
    /// <summary>
    /// �Z���F�����򉻂̕�
    /// ���ʁF�������󂯂Ă���f�o�t�����ׂĉ�������B��Ԉُ햳����2����B
    /// </summary>
    private void CardID109()
    {
        //�S�Ẵo�b�h�X�e�[�^�X������
        PlayerReleaseBadStatus();
        //��Ԉُ햳����2�t�^
        player.AddConditionStatus("InvalidBadStatus", 2);
    }
    /// <summary>
    /// �Z���F�N���A���F�[��,�����N���A���F�[��
    /// ���ʁF�K�[�h��5����B��Ԉُ햳����3����B
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
    /// ���ʁF�����1�_���[�W��^����B���̃Q�[�����A���̃R�}���h�̃_���[�W��1������B
    /// </summary>
    private void CardID11()
    {
        //BattleGameManager�̃A�N�Z�����[�g�@�\��ON
        bg.isAccelerate = true;
        bg.accelerateValue = 1;//�J�[�h�̃R�X�g��1������
    }
    /// <summary>
    /// �Z���F�����A�N�Z�����[�g
    /// ���ʁF�����2�_���[�W��^����B���̃Q�[�����A���̃R�}���h�̃_���[�W��1������B
    /// </summary>
    private void CardID111()
    {
        //BattleGameManager�̃A�N�Z�����[�g�@�\��ON
        bg.isAccelerate = true;
        bg.accelerateValue = 2;//�J�[�h�̃R�X�g��2������
    }
    /// <summary>
    /// �Z���F�����
    /// ���ʁF2�_���[�W��4��^����B
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
    /// �Z���F���������
    /// ���ʁF2�_���[�W��5��^����B
    /// </summary>
    private void CardID112()
    {
        //���ԍ��ŃG�l�~�[���U��
        StartCoroutine(ID112Attacking(cardAttackPower, 0.2f));
    }
    IEnumerator ID112Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        for (int count = 0; count < 4; count++)
        {
            yield return new WaitForSeconds(attackInterval);
            PlayerAttacking(attackMethod);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// �Z���F�t���o�[�X�g
    /// ���ʁFAP�����ׂď���A���������2�{�_���[�W��^����B
    /// </summary>
    private void CardID13(CardController card)
    {
        //AP��S����
        //���݂�AP����2�{�̃_���[�W���v�Z
        int damage = player.GetSetCurrentAP * 2;
        player.GetSetCurrentAP = 0;
        //�G�l�~�[���U��
        PlayerAttacking(damage);
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// �Z���F�����t���o�[�X�g
    /// ���ʁFAP�����ׂď���A���������3�{�_���[�W��^����B
    /// </summary>
    private void CardID113(CardController card)
    {
        //AP��S����
        //���݂�AP����3�{�̃_���[�W���v�Z
        int damage = player.GetSetCurrentAP * 3;
        player.GetSetCurrentAP = 0;
        //�G�l�~�[���U��
        PlayerAttacking(damage);
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// �Z���F�n�C�{���P�[�m 
    /// ���ʁF9�_���[�W��^���A����ɉΏ���2�^����B
    /// </summary>
    private void CardID14()
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�Ώ���2�t�^
        enemy.AddConditionStatus("Burn", 2);
    }
    /// <summary>
    /// �Z���F�����n�C�{���P�[�m 
    /// ���ʁF9�_���[�W��^���A����ɉΏ���4�^����B
    /// </summary>
    private void CardID114()
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�Ώ���4�t�^
        enemy.AddConditionStatus("Burn", 4);
    }
    /// <summary>
    /// �Z���F�f�r���h���C��,�����f�r���h���C��
    /// ���ʁF4�_���[�W��^���A�����̗̑͂�4�񕜂���B����ɐ����1�^����B
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
    /// �Z���F�K���e�B�[��,�����K���e�B�[��
    /// ���ʁF5(7)�_���[�W��^����B���肪�Ώ������Ȃ炩����3�{�̃_���[�W��^����B
    /// </summary>
    private void CardID16()
    {
        if (enemy.GetSetCondition.burn > 0)//�G�l�~�[���Ώ���Ԃ������ꍇ
        {
            //�Ώ���3�{�̃_���[�W�ŃG�l�~�[���U��
            PlayerAttacking(enemy.GetSetCondition.burn * 3);
        }
        else
        {
            //�G�l�~�[���U��
            PlayerAttacking(cardAttackPower);
        }
    }
    /// <summary>
    /// �Z���F��軂̏Ռ�,������軂̏Ռ�
    /// ���ʁF5(10)�_���[�W��^���A��������̃��E���h���s���s�\��Ԃɂ���B
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
    /// �Z���F�G�N�X�J���o�[,�����G�N�X�J���o�[
    /// ���ʁF10(12)�_���[�W��^����B���ꂪ3���E���h�ڈȍ~�Ȃ�AP��7�񕜂���B
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
    /// �Z���F�A���@�������F�[��,�����A���@�������F�[��
    /// ���ʁF���R������1����B��Ԉُ햳����2����B
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
    /// ���ʁF����ɉΏ���1�^����
    /// </summary>
    private void CardID20()
    {
        //�Ώ���1�t�^
        enemy.AddConditionStatus("Burn", 1);
    }
    /// <summary>
    /// �Z���F�����S��
    /// ���ʁF����ɉΏ���2�^����
    /// </summary>
    private void CardID120()
    {
        //�Ώ���1�t�^
        enemy.AddConditionStatus("Burn", 2);
    }
    private void PlayerAttacking(int attackMethod)//�G�l�~�[�ւ̍U������ 
    {
        attackMethod = ChangeAttackPower(attackMethod);
        Debug.Log("�v�Z��̍U���͂�" + attackMethod);
        enemy.TakeDamage(attackMethod);
    }
    private int ChangeAttackPower(int attackPower) //��Ԉُ�ɂ��U���͂̑���
    {
        attackPower = player.UpStrength(attackPower);
        attackPower = player.Weakness(attackPower);
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
