using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����b�N�̌��ʂ��܂Ƃ߂��X�N���v�g
/// </summary>
public class RelicEffectList : MonoBehaviour
{
    int id5Count = 0;
    void Start()
    {
        id5Count = 0;
    }
    /// <summary>
    /// ���O:�A���A�h�l�̎�
    /// ����:�f�b�L�̏����1�����₷�B
    /// </summary>
    /// <param name="ID1Quantity"></param>
    public void RelicID1(int ID1Quantity)
    {
        //���̃X�N���v�g�ŏ���������ʂ͂Ȃ�
    }

    /// <summary>
    ///  ���O:���n�̌�
    ///  ����:�^����_���[�W�Ǝ󂯂�_���[�W��1���₷�B
    /// </summary>
    /// <param name="ID2Quantity">�����b�N�ԍ�02�̌�</param>
    public void RelicID2(int ID2Quantity)
    {
        var bg = BattleGameManager.Instance;
        
        if (ID2Quantity <= 0) //�����b�N��1���Ȃ��ꍇ
        {
            Debug.Log("player:" + bg.relicID2Player + "enemy" + bg.relicID2Enemy);
            //���^�[���ŕԂ�
            return;
        }
        else
        {
            bg.relicID2Player += 1 * ID2Quantity; //�v���C���[�̍U���͂̑����ʂ̓����b�N�̌���
            bg.relicID2Enemy += 1 * ID2Quantity; //�G�l�~�[�̍U���͂̑����ʂ̓����b�N�̌���
        }
        Debug.Log("player:"+ bg.relicID2Player + "enemy"+ bg.relicID2Enemy);
    }

    /// <summary>
    /// ���O:���h�̊�
    /// ����:�ő�AP��2���₷���A���E���h�I�����ɑ�����AP�̏㏸�l��1���Ȃ��Ȃ�B(�ő�AP�����邱�Ƃ͂Ȃ�)
    /// </summary>
    /// <param name="ID3Quantity">�����b�N�ԍ�03�̌�</param>
    /// <param name="constAP">AP�̏����l</param>
    /// <param name="chargeAP">AP�̏㏸�l</param>
    /// <returns>��������AP�̏����l,��������AP�̏㏸�l</returns>
    public (int constAP, int chargeAP) RelicID3(int ID3Quantity, int constAP, int chargeAP)
    {
        constAP += 2 * ID3Quantity;
        chargeAP -= ID3Quantity;
        if (chargeAP < 0)
        {
            chargeAP = 0;
        }
        return (constAP, chargeAP);
    }

    /// <summary>
    /// ���O:�_��̔�����
    /// ����:�ő�AP��1���₷�B
    /// </summary>
    /// <param name="ID4Quantity">�����b�N�ԍ�04�̌�</param>
    /// <param name="constAP">AP�̏����l</param>
    /// <returns>��������AP�̏����l</returns>
    public int RelicID4(int ID4Quantity, int constAP)
    {
        constAP += ID4Quantity;
        return constAP;
    }

    /// <summary>
    /// ���O:�痢�ዾ
    /// ����:�ő�AP��1���炷���A���E���h�I�����AAP�̏㏸�l�������1���₷�B�����b�N1�ɂ��ő�5��܂ŁB
    /// </summary>
    /// <param name="ID5Quantity">�����b�N�ԍ�05�̌�</param>
    /// <param name="constAP">AP�̏����l</param>
    /// <param name="chargeAP">AP�̏㏸�l</param>
    /// <returns>��������AP�̏����l,��������AP�̏㏸�l</returns>
    public (int constAP, int chargeAP) RelicID5(int ID5Quantity, int constAP, int chargeAP)
    {
        if(id5Count >= 5)
        {
            return (constAP, chargeAP);
        }
        constAP -= ID5Quantity;
        if(constAP < 0)
        {
            constAP = 0;
        }
        chargeAP += ID5Quantity;
        id5Count++;
        return (constAP, chargeAP);
    }

    /// <summary>
    /// ���O:���z�̂����
    /// ����:�퓬�J�n���A����ɉΏ���1�t�^����B
    /// </summary>
    /// <param name="ID6Quantity">�����b�N�ԍ�06�̌�</param>
    /// <returns>���Z����Ώ��̒l</returns>
    public int RelicID6(int ID6Quantity)
    {
        int addBurn = 0;

        if (ID6Quantity <= 0)
        {
            return addBurn;
        }
        else
        {
            addBurn += ID6Quantity;
        }
        return addBurn;
    }

    /// <summary>
    /// ���O:�S�̊�
    /// ����:�ő�HP��5���₷�B����HP�͕ω����Ȃ��B
    /// </summary>
    /// <param name="ID7Quantity">�����b�N�ԍ�07�̌�</param>
    /// <param name="HP">�ő�HP</param>
    /// <returns>���������ő�HP</returns>
    public int RelicID7(int ID7Quantity, int HP)
    {
        HP += 5 * ID7Quantity;
        return HP;
    }

    /// <summary>
    /// ���O: �^�~�̂����
    /// ����:�퓬�J�n���ɃK�[�h��3�l������B
    /// </summary>
    /// <param name="ID8Quantity">�����b�N�ԍ�08�̌�</param>
    /// <param name="GP">�K�[�h�|�C���g</param>
    /// <returns>���������K�[�h�|�C���g</returns>
    public int RelicID8(int ID8Quantity, int GP)
    {
        GP += 3 * ID8Quantity;
        return GP;
    }

    /// <summary>
    /// ���O:���J����
    /// ����:�퓬�I����Ɋl������S�[���h��10���₷
    /// </summary>
    /// <param name="ID9Quantity">�����b�N�ԍ�09�̌�</param>
    /// <returns>���������S�[���h</returns>
    public int RelicID9(int ID9Quantity)
    {
        int money = 0;
        money += 10 * ID9Quantity;
        return money;
    }

    /// <summary>
    /// ���O:�ق��ق����ɂ���
    /// ����:�퓬�I�����Ɏ����̌��݂�HP��5�񕜂���B
    /// </summary>
    /// <param name="ID10Quantity">�����b�N�ԍ�10�̌�</param>
    /// <returns>�퓬�I�����ɉ񕜂����</returns>
    public int RelicID10(int ID10Quantity)
    {
        int healingPower = 0;
        healingPower += 5 * ID10Quantity;
        return healingPower;
    }

    /// <summary>
    /// ���O:�f���؂���
    /// ����:���E���h�I�����Ƀf�o�t��1��������B
    /// </summary>
    /// <param name="ID11Quantity">�����b�N�ԍ�11�̌�</param>
    /// <param name="condition">��Ԉُ�̃X�e�[�^�X</param>
    /// <returns>�����_���Ɍ��������o�b�h�X�e�[�^�X</returns>
    public Dictionary<string, int> RelicID11(int ID11Quantity, Dictionary<string, int> condition)
    {
        //�o�b�h�X�e�[�^�X�����X�g�ɒǉ�
        List<int> badStatus = new List<int> { condition["Curse"], condition["Impatience"], condition["Weakness"], condition["Burn"], condition["Poison"] };
        //�����ł��鐔���o�b�h�X�e�[�^�X�̐���葽���ꍇ�̓o�b�h�X�e�[�^�X�̐���������
        int totalBadStatus = condition["Curse"] + condition["Impatience"] + condition["Weakness"] + condition["Burn"] + condition["Poison"];
        if (totalBadStatus < ID11Quantity)
        {
            ID11Quantity = totalBadStatus;
        }

        //ID11�̌��������_���Ȑ�����I��ł��̐���List�Ԗڂɓ����Ă��鐔��0�ȏ�Ȃ�-1����
        for (int i = 0; i < ID11Quantity; i++)
        {
            int chooseNumber = Random.Range(0, badStatus.Count - 1);
            while (badStatus[chooseNumber] == 0)
            {
                chooseNumber = Random.Range(0, badStatus.Count - 1);
            }
            badStatus[chooseNumber] -= 1;
        }
        //������̐��l��������
        condition["Curse"] = badStatus[0];
        condition["Impatience"] = badStatus[1];
        condition["Weakness"] = badStatus[2];
        condition["Burn"] = badStatus[3];
        condition["Poison"] = badStatus[4];
        return condition;
    }

    /// <summary>
    /// ���O:��̉Ζ�
    /// ����:�{�X�Ƃ̐퓬���A�^����_���[�W��1��������B
    /// </summary>
    /// <param name="ID12Quantity">�����b�N�ԍ�12�̌�</param>
    /// <param name="type">�G�l�~�[�̎��</param>
    /// <returns>���Z����ؗ͑����̒l</returns>
    public int RelicID12(int ID12Quantity, string type)
    {
        int addPlayerUpStrength = 0;

        if (ID12Quantity > 0 && type == "Boss")
        {
            addPlayerUpStrength += 1 * ID12Quantity;
        }
        else
        {
            return addPlayerUpStrength;
        }

        return addPlayerUpStrength;
    }
}
