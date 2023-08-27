using UnityEngine;

/// <summary>
/// ��Ԉُ�̌��ʂ��܂Ƃ߂��X�N���v�g
/// </summary>
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
    /// <param name="autoHealing">������</param>
    /// <returns>�����񕜂̒l</returns>
    public int AutoHealing(int autoHealing)
    {
        //���ʌ����Ȃǂ̏���������΂����ɏ���
        return autoHealing;
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
    /// <param name="impatience">�ő�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>���ʂ𔭊�����ő��̐�,�����̏�Ԉُ햳��</returns>
    public (int impatience, int invalidBadStatus) Impatience(int impatience, int invalidBadStatus)
    {
        //�ő���0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (impatience <= 0)
        {
            return (impatience, invalidBadStatus);
        }
        var invalidImpatience = InvalidBadStatus(impatience, invalidBadStatus);
        int currentImpatience = invalidImpatience.badStatus;

        return (currentImpatience, invalidImpatience.invalidBadStatus);
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
    /// <param name="burn">�Ώ�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <returns>�󂯂�_���[�W,�����̏�Ԉُ햳��</returns>
    public (int damage, int invalidBadStatus) Burn(int burn, int invalidBadStatus)
    {
        int damage = 0;
        //�Ώ���0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (burn <= 0)
        {
            return (damage, invalidBadStatus);
        }
        var invalidBurn = InvalidBadStatus(burn, invalidBadStatus);
        int currentBurn = invalidBurn.badStatus;
        damage = currentBurn;
        return (damage, invalidBurn.invalidBadStatus);
    }
    /// <summary>
    /// �f�o�t:�ד�
    /// ����:���E���h�I�����A���E���h���̍s�������דł̐��̃_���[�W���󂯂�
    /// </summary>
    /// <param name="poison">�ד�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳��</param>
    /// <param name="moveCount">���E���h���̍s����</param>
    /// <returns>�󂯂�_���[�W,�����̏�Ԉُ햳��</returns>
    public (int damage, int invalidBadStatus) Poison(int poison, int invalidBadStatus, int moveCount)
    {
        int damage = 0;
        //�דł�0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (poison <= 0)
        {
            return (damage, invalidBadStatus);
        }
        var invalidPoison = InvalidBadStatus(poison, invalidBadStatus);
        int currentPoison = invalidPoison.badStatus;
        damage = moveCount * currentPoison;
        return (damage, invalidPoison.invalidBadStatus);
    }
}
