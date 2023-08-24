using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3�w�ڂ̓G�Ɏ������郌���b�N��I������X�N���v�g
/// </summary>
public class SelectEnemyRelic : MonoBehaviour
{
    /// <summary>
    /// 3�K�w�̃G�l�~�[�Ƀ����b�N��t�^���鏈��
    /// </summary>
    /// <param name="_hasEnemyRelics">�G�l�~�[�̎������b�N</param>
    /// <param name="_floor">���݂̊K�w</param>
    /// <param name="_enemyName">�G�l�~�[�̖��O</param>
    /// <returns>�G�l�~�[�̏������Ă��郌���b�N�f�B�N�V���i��</returns>
    public Dictionary<int,int> SetEnemyRelics(Dictionary<int,int> _hasEnemyRelics, int _floor, string _enemyName)
    {
        var enemyRelics = _hasEnemyRelics;

        if (_floor == 3) //3�K�w�Ȃ�
        {
            enemyRelics = ChooseEnemyRelics(enemyRelics, _enemyName);
        }

        return enemyRelics;
    }

    /// <summary>
    /// �G�l�~�[�̖��O�ɉ����ă����b�N���������鏈��
    /// </summary>
    /// <param name="enemyRelics">�G�l�~�[�̎������b�N</param>
    /// <param name="enemyName">�G�l�~�[�̖��O</param>
    /// <returns>�����b�N�擾��̃f�B�N�V���i��</returns>
    private Dictionary<int, int> ChooseEnemyRelics(Dictionary<int, int> enemyRelics, string enemyName)
    {
        if (enemyName == "Slime")
        {
            enemyRelics[8] = 1; //�^�~�̂�����1�l��
        }
        else if (enemyName == "SkeletonSwordsman")
        {
            enemyRelics[2] = 1; //���n�̌���1�l��
        }
        else if (enemyName == "Naga")
        {
            enemyRelics[3] = 1; //���h�̊���1�l��
        }
        else if (enemyName == "Chimera")
        {
            enemyRelics[11] = 1; //�f���؂�����1�l��
        }
        else if (enemyName == "DarkKnight")
        {
            enemyRelics[6] = 1; //���z�̂�����1�l��
        }
        return enemyRelics;
    }
}
