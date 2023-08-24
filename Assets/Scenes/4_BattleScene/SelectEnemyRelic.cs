using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3層目の敵に持たせるレリックを選択するスクリプト
/// </summary>
public class SelectEnemyRelic : MonoBehaviour
{
    /// <summary>
    /// 3階層のエネミーにレリックを付与する処理
    /// </summary>
    /// <param name="_hasEnemyRelics">エネミーの持つレリック</param>
    /// <param name="_floor">現在の階層</param>
    /// <param name="_enemyName">エネミーの名前</param>
    /// <returns>エネミーの所持しているレリックディクショナリ</returns>
    public Dictionary<int,int> SetEnemyRelics(Dictionary<int,int> _hasEnemyRelics, int _floor, string _enemyName)
    {
        var enemyRelics = _hasEnemyRelics;

        if (_floor == 3) //3階層なら
        {
            enemyRelics = ChooseEnemyRelics(enemyRelics, _enemyName);
        }

        return enemyRelics;
    }

    /// <summary>
    /// エネミーの名前に応じてレリックを所得する処理
    /// </summary>
    /// <param name="enemyRelics">エネミーの持つレリック</param>
    /// <param name="enemyName">エネミーの名前</param>
    /// <returns>レリック取得後のディクショナリ</returns>
    private Dictionary<int, int> ChooseEnemyRelics(Dictionary<int, int> enemyRelics, string enemyName)
    {
        if (enemyName == "Slime")
        {
            enemyRelics[8] = 1; //真円のお守りを1つ獲得
        }
        else if (enemyName == "SkeletonSwordsman")
        {
            enemyRelics[2] = 1; //諸刃の剣を1つ獲得
        }
        else if (enemyName == "Naga")
        {
            enemyRelics[3] = 1; //虚栄の冠を1つ獲得
        }
        else if (enemyName == "Chimera")
        {
            enemyRelics[11] = 1; //断ち切り鋏を1つ獲得
        }
        else if (enemyName == "DarkKnight")
        {
            enemyRelics[6] = 1; //太陽のお守りを1つ獲得
        }
        return enemyRelics;
    }
}
