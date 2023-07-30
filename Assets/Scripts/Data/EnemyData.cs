using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Create EnemyData")]
public class EnemyData : ScriptableObject
{
    /// <summary>
    /// エネミーの名前
    /// </summary>
    public string enemyName;
    /// <summary>
    /// エネミーのHP
    /// </summary>
    public int enemyHP;
    /// <summary>
    /// エネミーの現在のHP
    /// </summary>
    public int enemyCurrentHP;
    /// <summary>
    ///エネミーのAP
    /// </summary>
    public int enemyAP;
    /// <summary>
    /// エネミーの現在のAP
    /// </summary>
    public int enemyCurrentAP;
    /// <summary>
    /// エネミーのGP
    /// </summary>
    public int enemyGP;
    /// <summary>
    /// エネミーが落とすお金
    /// </summary>
    public int dropMoney;
    /// <summary>
    /// エネミーのイメージ
    /// </summary>
    public Sprite enemyImage;
}
