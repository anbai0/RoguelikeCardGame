using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemyRelic : MonoBehaviour
{
    /// <summary>
    /// 3階層のエネミーにレリックを付与する処理
    /// </summary>
    /// <param name="floor">現在の階層</param>
    /// <param name="enemyName">エネミーの名前</param>
    /// <returns>エネミーに応じたレリックステータス</returns>
    public RelicStatus SetEnemyRelics(int floor, string enemyName)
    {
        var enemyRelics = new RelicStatus();

        if (floor == 3) //3階層なら
        {
            enemyRelics = ChooseEnemyRelics(enemyRelics, enemyName);
        }

        return enemyRelics;
    }

    /// <summary>
    /// エネミーの名前に応じてレリックを選択する処理
    /// </summary>
    /// <param name="enemyRelics">エネミーのレリックステータス</param>
    /// <param name="enemyName">エネミーの名前</param>
    /// <returns>選択されたレリックを追加したレリックステータス</returns>
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
