using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CardEffectList : MonoBehaviour
{
    [SerializeField] Text playerHPText;
    [SerializeField] Text playerAPText;
    [SerializeField] Text playerGPText;
    [SerializeField] Text enemyHPText;
    [SerializeField] Slider enemyHPSlider;
    int cardID;
    int cardAttackPower;
    int cardHealingPower;
    int cardGuardPoint;
    BattleGameManager bg;

    public void ActiveCardEffect(CardController card)
    {
        bg = BattleGameManager.Instance;
        cardID = card.cardDataManager._cardID;
        cardAttackPower = card.cardDataManager._cardAttackPower;
        cardHealingPower = card.cardDataManager._cardHealingPower;
        cardGuardPoint = card.cardDataManager._cardGuardPoint;
        Debug.Log("Card�̃i���o�[��" + cardID);
        //�J�[�h��ID�ɉ����ď������Ăяo��
        switch (cardID)
        {
            case 1: CardID1();
                break;
            case 2: CardID2();
                break;
            case 3: CardID3(card);
                break;
            case 4: CardID4();
                break;
            case 5: CardID5(card);
                break;
            case 6: CardID6();
                break;
            case 7: CardID7();
                break;
            case 8: CardID8(card);
                break;
            case 9: CardID9();
                break;
            case 10: CardID10();
                break;
            case 11: CardID11();
                break;
            case 12: CardID12();
                break;
            case 13: CardID13(card);
                break;
            case 14: CardID14();
                break;
            case 15: CardID15(card);
                break;
            case 16: CardID16();
                break;
            case 17: CardID17(card);
                break;
            case 18: CardID18(card);
                break;
            case 19: CardID19(card);
                break;
            case 20: CardID20();
                break;
            default:Debug.Assert(false);
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
        PlayerHealing();
    }
    /// <summary>
    /// �Z���F�����̗��
    /// </summary>
    private void CardID3(CardController card)
    {
        //HP����
        PlayerHealing();
        Destroy(card.gameObject);
    }
    /// <summary>
    /// �Z���F�K�[�h
    /// </summary>
    private void CardID4()
    {
        //�K�[�h��ǉ�
        PlayerAddGP();
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

    IEnumerator ID5Attacking(int attackMethod,float attackInterval) 
    {
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
    }
    /// <summary>
    /// �Z���F�V�[���h�o�b�V��
    /// </summary>
    private void CardID6()
    {
        //�K�[�h��ǉ�
        PlayerAddGP();
        //�G�l�~�[���U��
        PlayerAttacking(bg.playerGP);
    }
    /// <summary>
    /// �Z���F�t��
    /// </summary>
    private void CardID7()
    {
        //�o�b�h�X�e�[�^�X���P�ł��������ꍇ
        if (bg.playerCondition.curse > 0 
            || bg.playerCondition.impatience > 0 
            || bg.playerCondition.weakness > 0 
            || bg.playerCondition.burn > 0 
            || bg.playerCondition.poison > 0) 
        {
            //�o�b�h�X�e�[�^�X������
            ReleaseBadStatus();
            //�ؗ͑�����2�t�^
            bg.playerCondition.upStrength += 2;
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
        //�o�b�h�X�e�[�^�X������
        ReleaseBadStatus();
        //��Ԉُ햳����1�t�^
        bg.playerCondition.invalidBadStatus += 1;
    }
    /// <summary>
    /// �Z���F�N���A���F�[��
    /// </summary>
    private void CardID10()
    {
        //�K�[�h��ǉ�
        PlayerAddGP();
        //��Ԉُ햳����3�t�^
        bg.playerCondition.invalidBadStatus += 3;
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
        PlayerAttacking(attackMethod);
        for (int count = 0; count < 3; count++)
        {
            yield return new WaitForSeconds(attackInterval);
            PlayerAttacking(attackMethod);
        }
    }
    /// <summary>
    /// �Z���F�t���o�[�X�g
    /// </summary>
    private void CardID13(CardController card)
    {
        //���݂�AP���̃_���[�W���v�Z
        int damage = bg.playerCurrentAP * 2;
        //AP��S����
        bg.playerCurrentAP = 0;
        bg.playerAPText.text = bg.playerCurrentAP + "/" + bg.playerAP;
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
        bg.enemyCondition.burn += 2;
    }
    /// <summary>
    /// �Z���F�f�r���h���C��
    /// </summary>
    private void CardID15(CardController card)
    {
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //HP����
        PlayerHealing();
        //�G�l�~�[�ɐ����1�t�^
        bg.enemyCondition.weakness += 1;
        //���̃��E���h���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// �Z���F�K���e�B�[��
    /// </summary>
    private void CardID16()
    {
        if (bg.enemyCondition.burn > 0)//�G�l�~�[���Ώ���Ԃ������ꍇ
        {
            //�Ώ���3�{�̃_���[�W�ŃG�l�~�[���U��
            PlayerAttacking(bg.enemyCondition.burn * 3);
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
        //�s���s�\�̏��������������M�肽��
        //�G�l�~�[���U��
        PlayerAttacking(cardAttackPower);
        //�G�l�~�[���s���s�\�ɂ���
        bg.enemyCurrentAP = 0;
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
        yield return new WaitForSeconds(attackInterval);
        //�v���C���[��AP��7�񕜂���
        bg.playerCurrentAP += 7;
        if (bg.playerCurrentAP > bg.playerAP)
        {
            bg.playerCurrentAP = bg.playerAP;
        }
        playerAPText.text = bg.playerCurrentAP + "/" + bg.playerAP;
    }
    /// <summary>
    /// �Z���F�A���@�������F�[��
    /// </summary>
    private void CardID19(CardController card)
    {
        //�����񕜂�1�t�^
        bg.playerCondition.autoHealing += 1;
        //��Ԉُ햳����2�t�^
        bg.playerCondition.invalidBadStatus += 2;
        //���̐퓬���J�[�h���g�p�s�ɂ���
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// �Z���F�S��
    /// </summary>
    private void CardID20()
    {
        //�Ώ���1�t�^
        bg.enemyCondition.burn += 1;
    }
    private void PlayerAttacking(int attackMethod)//�G�l�~�[�ւ̍U������ 
    {
        attackMethod += bg.playerCondition.upStrength;//�ؗ͑����̌���
        attackMethod -= bg.playerCondition.weakness;//����̌���
        if (attackMethod < 0) 
        {
            attackMethod = 0;
        }
        int attackPower = attackMethod - bg.enemyGP;
        if (bg.enemyGP > 0) 
        {
            bg.enemyGP -= attackMethod;
            if (bg.enemyGP <= 0) 
            {
                bg.enemyGP = 0;
            }
        }
        if (attackPower <= 0) 
        {
            attackPower = 0;
        }
        Debug.Log("�v�Z��̍U���͂�" + attackPower);
        bg.enemyCurrentHP -= attackPower;
        enemyHPText.text = bg.enemyCurrentHP + "/" + bg.enemyHP;
        enemyHPSlider.value = bg.enemyCurrentHP / (float)bg.enemyHP;
    }
    private void PlayerHealing()//�v���C���[��HP�񕜏���
    {
        bg.playerCurrentHP += cardHealingPower;
    }
    private void PlayerAddGP()//�v���C���[�ɃK�[�h��ǉ�
    {
        bg.playerGP += cardGuardPoint;
    }
    private void ReleaseBadStatus()//�v���C���[�ɕt�^���ꂽ�f�o�t�̉���
    {
        bg.playerCondition.curse = 0;
        bg.playerCondition.impatience = 0;
        bg.playerCondition.weakness = 0;
        bg.playerCondition.burn = 0;
        bg.playerCondition.poison = 0;
    }
}
