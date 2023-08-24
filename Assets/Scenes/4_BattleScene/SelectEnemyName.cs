using UnityEngine;

/// <summary>
/// エネミーの名前を選択するスクリプト
/// </summary>
public class SelectEnemyName : MonoBehaviour
{
    string[] enemyNameList = { "Slime", "SkeletonSwordsman", "Naga"}; //通常の敵のリスト
    string[] strongEnemyNameList = { "Chimera", "DarkKnight" }; //強敵のリスト
    string[] bossNameList = { "Cyclops", "Scylla", "Minotaur" }; //ボスのリスト

    /// <summary>
    /// 種類と階層別にランダムでエネミーの名前を決める
    /// </summary>
    /// <param name="floor">現在の階層</param>
    /// <param name="type">エネミーの種類(Enemy,StrongEnemy,Boss)</param>
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
    /// 通常敵の名前をランダムで決める
    /// </summary>
    /// <returns>選択された通常敵の名前</returns>
    string SelectEnemy()
    {
        int rndNum = Random.Range(0, enemyNameList.Length);
        return enemyNameList[rndNum];
    }

    /// <summary>
    /// 強敵の名前をランダムで決める
    /// </summary>
    /// <returns>選択された強敵の名前</returns>
    string SelectStrongEnemy()
    {
        int rndNum = Random.Range(0, strongEnemyNameList.Length);
        return strongEnemyNameList[rndNum];
    }

    /// <summary>
    /// 階層に応じてボスの名前を決める
    /// </summary>
    /// <param name="_floor">現在の階層</param>
    /// <returns>選択されたボスの名前</returns>
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
