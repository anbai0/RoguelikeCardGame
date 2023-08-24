using UnityEngine;

/// <summary>
/// �G�l�~�[�̖��O��I������X�N���v�g
/// </summary>
public class SelectEnemyName : MonoBehaviour
{
    string[] enemyNameList = { "Slime", "SkeletonSwordsman", "Naga"}; //�ʏ�̓G�̃��X�g
    string[] strongEnemyNameList = { "Chimera", "DarkKnight" }; //���G�̃��X�g
    string[] bossNameList = { "Cyclops", "Scylla", "Minotaur" }; //�{�X�̃��X�g

    /// <summary>
    /// ��ނƊK�w�ʂɃ����_���ŃG�l�~�[�̖��O�����߂�
    /// </summary>
    /// <param name="floor">���݂̊K�w</param>
    /// <param name="type">�G�l�~�[�̎��(Enemy,StrongEnemy,Boss)</param>
    public string DecideEnemyName(int floor, string type)
    {
        string enemyName = null;
        if(type == "SmallEnemy")
        {
            enemyName = SelectEnemy();
        }
        else if (type == "StrongEnemy")
        {
            enemyName = SelectStrongEnemy();
        }
        else if (type == "Boss")
        {
            enemyName = SelectBoss(floor);
        }
        else
        {
            enemyName = null;
            Debug.Assert(false);
        }
        return enemyName;
    }

    /// <summary>
    /// �ʏ�G�̖��O�������_���Ō��߂�
    /// </summary>
    /// <returns>�I�����ꂽ�ʏ�G�̖��O</returns>
    string SelectEnemy()
    {
        int rndNum = Random.Range(0, enemyNameList.Length);
        return enemyNameList[rndNum];
    }

    /// <summary>
    /// ���G�̖��O�������_���Ō��߂�
    /// </summary>
    /// <returns>�I�����ꂽ���G�̖��O</returns>
    string SelectStrongEnemy()
    {
        int rndNum = Random.Range(0, strongEnemyNameList.Length);
        return strongEnemyNameList[rndNum];
    }

    /// <summary>
    /// �K�w�ɉ����ă{�X�̖��O�����߂�
    /// </summary>
    /// <param name="_floor">���݂̊K�w</param>
    /// <returns>�I�����ꂽ�{�X�̖��O</returns>
    string SelectBoss(int _floor)
    {
        string bossName = null;
        if (_floor == 1)
        {
            bossName = bossNameList[0];
        }
        else if (_floor == 2)
        {
            bossName= bossNameList[1];
        }
        else if (_floor == 3)
        {
            bossName = bossNameList[2];
        }
        return bossName;
    }
}
