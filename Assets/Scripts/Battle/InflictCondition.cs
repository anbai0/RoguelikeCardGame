using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictCondition : MonoBehaviour
{
    /// <summary>
    /// �o�t:�ؗ͑���
    /// ����:�^����_���[�W��(1�~���̃o�t�̐�)�㏸����
    /// </summary>
    /// <param name="attackPower">�U����</param>
    /// <param name="upStrength">�ؗ͑���</param>
    /// <returns>�ؗ͑��������Z���ꂽ�U����</returns>
    public int UpStrength(int attackPower, int upStrength)
    {
        attackPower += upStrength;
        return attackPower;
    }
    /// <summary>
    /// �o�t:������
    /// ����:�s�����HP��(1�~���̃o�t�̐�)�񕜂���B
    /// </summary>
    /// <param name="hp">���݂�HP</param>
    /// <param name="autoHealing">������</param>
    /// <returns>�񕜂������݂�HP</returns>
    public int AutoHealing(int hp, int autoHealing)
    {
        hp += autoHealing;
        return hp;
    }
    /// <summary>
    /// �f�o�t:��Ԉُ햳��
    /// ����:�o�b�h�X�e�[�^�X����Ԉُ햳���̐���������������
    /// </summary>
    /// <param name="badStatus">�o�b�h�X�e�[�^�X</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>�c��̃o�b�h�X�e�[�^�X,�����̏�Ԉُ햳��</returns>
    public (int badStatus, int invalidBadStatus) InvalidBadStatus(int badStatus, int invalidBadStatus)
    {
        if (invalidBadStatus <= 0)
        {
            return (badStatus, invalidBadStatus);
        }
        int currentBadStatus = badStatus - invalidBadStatus;
        if (currentBadStatus < 0)
        {
            currentBadStatus = 0;
        }
        invalidBadStatus -= badStatus;
        if (invalidBadStatus < 0)
        {
            invalidBadStatus = 0;
        }
        return (currentBadStatus, invalidBadStatus);
    }
    /// <summary>
    /// �f�o�t:����
    /// ����:�ő�AP��(1�~���̃f�o�t�̐�)��������B
    /// </summary>
    /// <param name="constAP">�Q�[���J�n���̍ő�AP</param>
    /// <param name="chargeAP">���E���h�o�߂ŏ㏸���Ă���AP</param>
    /// <param name="curse">����</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>���̃��E���h�̍ő�AP,�����̏�Ԉُ햳��</returns>
    public (int nextRoundAP, int invalidBadStatus) Curse(int constAP, int chargeAP, int curse, int invalidBadStatus)
    {
        int nextRoundAP = constAP + chargeAP;
        //������0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (curse <= 0)
        {
            return (nextRoundAP, invalidBadStatus);
        }
        var invalidCurse = InvalidBadStatus(curse, invalidBadStatus);
        int currentCurse = invalidCurse.badStatus;
        if (currentCurse < 0)
        {
            currentCurse = 0;
        }
        nextRoundAP = constAP + chargeAP - currentCurse;
        if (nextRoundAP < 0)
        {
            nextRoundAP = 0;
        }
        return (nextRoundAP, invalidCurse.invalidBadStatus);
    }
    /// <summary>
    /// �f�o�t:�ő�
    /// ����:�s�����AP��(1�~���̃f�o�t�̐�)�����B
    /// </summary>
    /// <param name="currentAP">���݂�AP</param>
    /// <param name="impatience">�ő�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>�����������݂�AP,�����̏�Ԉُ햳��</returns>
    public (int currentAP, int invalidBadStatus) Impatience(int currentAP, int impatience, int invalidBadStatus)
    {
        //�ő���0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (impatience <= 0)
        {
            return (currentAP, invalidBadStatus);
        }
        var invalidImpatience = InvalidBadStatus(impatience, invalidBadStatus);
        int currentImpatience = invalidImpatience.badStatus;
        currentAP -= currentImpatience;
        if (currentAP < 0)
        {
            currentAP = 0;
        }
        return (currentAP, invalidImpatience.invalidBadStatus);
    }
    /// <summary>
    /// �f�o�t:����
    /// ����:�^����_���[�W��(1�~���̃o�t�̐�)��������
    /// </summary>
    /// <param name="attackPower">�U����</param>
    /// <param name="weakness">����</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>���������U����,�����̏�Ԉُ햳��</returns>
    public (int attackPower, int invalidBadStatus) Weakness(int attackPower, int weakness, int invalidBadStatus)
    {
        //���オ0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (weakness <= 0)
        {
            return (attackPower, invalidBadStatus);
        }
        var invalidWeakness = InvalidBadStatus(weakness, invalidBadStatus);
        int currentWeakness = invalidWeakness.badStatus;
        attackPower = attackPower - currentWeakness;
        if (attackPower < 0)
        {
            attackPower = 0;
        }
        return (attackPower, invalidWeakness.invalidBadStatus);
    }
    /// <summary>
    /// �f�o�t:�Ώ�
    /// ����:�s�����(1�~���̃f�o�t�̐�)�_���[�W���󂯂�
    /// </summary>
    /// <param name="currentHP">���݂�HP</param>
    /// <param name="burn">�Ώ�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>�����������݂�HP,�����̏�Ԉُ햳��</returns>
    public (int currentHP, int invalidBadStatus) Burn(int currentHP, int burn, int invalidBadStatus)
    {
        //�Ώ���0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (burn <= 0)
        {
            return (currentHP, invalidBadStatus);
        }
        var invalidBurn = InvalidBadStatus(burn, invalidBadStatus);
        int currentBurn = invalidBurn.badStatus;
        currentHP -= currentBurn;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        return (currentHP, invalidBurn.invalidBadStatus);
    }
    /// <summary>
    /// �f�o�t:�ד�
    /// ����:���E���h�I�����A���E���h���̍s�������דł̐��̃_���[�W���󂯂�
    /// </summary>
    /// <param name="currentHP">���݂�HP</param>
    /// <param name="poison">�ד�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <param name="moveCount">���E���h���̍s����</param>
    /// <returns>�����������݂�HP,�����̏�Ԉُ햳��</returns>
    public (int currentHP, int invalidBadStatus) Poison(int currentHP, int poison, int invalidBadStatus, int moveCount)
    {
        //�דł�0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (poison <= 0)
        {
            return (currentHP, invalidBadStatus);
        }
        var invalidPoison = InvalidBadStatus(poison, invalidBadStatus);
        int currentPoison = invalidPoison.badStatus;
        currentHP -= moveCount * currentPoison;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        return (currentHP, invalidPoison.invalidBadStatus);
    }
}
