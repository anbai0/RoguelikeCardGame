using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemyData : MonoBehaviour
{
    /// <summary>
    /// �G�l�~�[�̖��O�ɉ����ăG�l�~�[�̃f�[�^���쐬���鏈��
    /// </summary>
    /// <param name="floor">�K�w</param>
    /// <param name="enemyName">�G�l�~�[�̖��O</param>
    /// <returns>�������ꂽ�G�l�~�[�f�[�^</returns>
    public EnemyDataManager SetEnemyDataManager(int floor, string enemyName)
    {
        EnemyDataManager enemyData = null;

        if (floor == 1 && enemyName == "Slime")
        {
            enemyData = new EnemyDataManager("Slime");
        }
        else if (floor == 2 && enemyName == "Slime")
        {
            enemyData = new EnemyDataManager("Slime2");
        }
        else if (floor == 3 && enemyName == "Slime")
        {
            enemyData = new EnemyDataManager("Slime3");
        }
        else if (floor == 1 && enemyName == "SkeletonSwordsman")
        {
            enemyData = new EnemyDataManager("SkeletonSwordsman");
        }
        else if (floor == 2 && enemyName == "SkeletonSwordsman")
        {
            enemyData = new EnemyDataManager("SkeletonSwordsman2");
        }
        else if (floor == 3 && enemyName == "SkeletonSwordsman")
        {
            enemyData = new EnemyDataManager("SkeletonSwordsman3");
        }
        else if (floor == 1 && enemyName == "Naga")
        {
            enemyData = new EnemyDataManager("Naga");
        }
        else if (floor == 2 && enemyName == "Naga")
        {
            enemyData = new EnemyDataManager("Naga2");
        }
        else if (floor == 3 && enemyName == "Naga")
        {
            enemyData = new EnemyDataManager("Naga3");
        }
        else if (floor == 1 && enemyName == "Chimera")
        {
            enemyData = new EnemyDataManager("Chimera");
        }
        else if (floor == 2 && enemyName == "Chimera")
        {
            enemyData = new EnemyDataManager("Chimera2");
        }
        else if (floor == 3 && enemyName == "Chimera")
        {
            enemyData = new EnemyDataManager("Chimera3");
        }
        else if (floor == 1 && enemyName == "DarkKnight")
        {
            enemyData = new EnemyDataManager("DarkKnight");
        }
        else if (floor == 2 && enemyName == "DarkKnight")
        {
            enemyData = new EnemyDataManager("DarkKnight2");
        }
        else if (floor == 3 && enemyName == "DarkKnight")
        {
            enemyData = new EnemyDataManager("DarkKnight3");
        }
        else if (floor == 1 && enemyName == "Cyclops")
        {
            enemyData = new EnemyDataManager("Cyclops");
        }
        else if (floor == 2 && enemyName == "Scylla")
        {
            enemyData = new EnemyDataManager("Scylla");
        }
        else if (floor == 3 && enemyName == "Minotaur")
        {
            enemyData = new EnemyDataManager("Minotaur");
        }
        return enemyData;
    }
}
