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
    /// <param name="badStatus">�t�^�����o�b�h�X�e�[�^�X�̐�</param>
    /// <param name="invalidBadStatus">��Ԉُ햳���̐�</param>
    /// <returns>�c��̃o�b�h�X�e�[�^�X�̐�,�g�p��̏�Ԉُ햳���̐�</returns>
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
    /// <returns>���̃��E���h�̍ő�AP</returns>
    public int Curse(int constAP, int chargeAP, int curse)
    {
        int nextRoundAP = constAP + chargeAP;
        //������0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (curse <= 0)
        {
            return nextRoundAP;
        }
        nextRoundAP = constAP + chargeAP - curse;
        if (nextRoundAP < 0)
        {
            nextRoundAP = 0;
        }
        return nextRoundAP;
    }

    /// <summary>
    /// �f�o�t:�ő�
    /// ����:�s�����AP��(1�~���̃f�o�t�̐�)�����B
    /// </summary>
    public void Impatience()
    {
        //���̂Ƃ��돈���̕K�v�͖���
    }

    /// <summary>
    /// �f�o�t:����
    /// ����:�^����_���[�W��(1�~���̃o�t�̐�)��������
    /// </summary>
    /// <param name="attackPower">�U����</param>
    /// <param name="weakness">����</param>
    /// <returns>���������U����</returns>
    public int Weakness(int attackPower, int weakness)
    {
        //���オ0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (weakness <= 0)
        {
            return attackPower;
        }
        attackPower = attackPower - weakness;
        if (attackPower < 0)
        {
            attackPower = 0;
        }
        return attackPower;
    }

    /// <summary>
    /// �f�o�t:�Ώ�
    /// ����:�s�����(1�~���̃f�o�t�̐�)�_���[�W���󂯂�
    /// </summary>
    public void Burn()
    {
        //���̂Ƃ��돈���͖���
    }

    /// <summary>
    /// �f�o�t:�ד�
    /// ����:���E���h�I�����A���E���h���̍s�������דł̐��̃_���[�W���󂯂�
    /// </summary>
    /// <param name="poison">�ד�</param>
    /// <param name="moveCount">���E���h���̍s����</param>
    /// <returns>�󂯂�_���[�W</returns>
    public int Poison(int poison, int moveCount)
    {
        int damage = 0;
        //�דł�0�̂Ƃ��͌��ʂ𔭓����Ȃ�
        if (poison <= 0)
        {
            return damage;
        }
        damage = moveCount * poison;
        return damage;
    }
}
