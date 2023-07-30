using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemyRelic : MonoBehaviour
{
    /// <summary>
    /// 3�K�w�̃G�l�~�[�Ƀ����b�N��t�^���鏈��
    /// </summary>
    /// <param name="floor">���݂̊K�w</param>
    /// <param name="enemyName">�G�l�~�[�̖��O</param>
    /// <returns>�G�l�~�[�ɉ����������b�N�X�e�[�^�X</returns>
    public RelicStatus SetEnemyRelics(int floor, string enemyName)
    {
        var enemyRelics = new RelicStatus();

        if (floor == 3) //3�K�w�Ȃ�
        {
            enemyRelics = ChooseEnemyRelics(enemyRelics, enemyName);
        }

        return enemyRelics;
    }

    /// <summary>
    /// �G�l�~�[�̖��O�ɉ����ă����b�N��I�����鏈��
    /// </summary>
    /// <param name="enemyRelics">�G�l�~�[�̃����b�N�X�e�[�^�X</param>
    /// <param name="enemyName">�G�l�~�[�̖��O</param>
    /// <returns>�I�����ꂽ�����b�N��ǉ����������b�N�X�e�[�^�X</returns>
    private RelicStatus ChooseEnemyRelics(RelicStatus enemyRelics, string enemyName)
    {
        if (enemyName == "Slime")
        {
            enemyRelics.hasRelicID8++;
        }
        else if (enemyName == "SkeletonSwordsman")
        {
            enemyRelics.hasRelicID2++;
        }
        else if (enemyName == "Naga")
        {
            enemyRelics.hasRelicID3++;
        }
        else if (enemyName == "Chimera")
        {
            enemyRelics.hasRelicID11++;
        }
        else if (enemyName == "DarkKnight")
        {
            enemyRelics.hasRelicID6++;
        }
        return enemyRelics;
    }
}
