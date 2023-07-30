using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Create EnemyData")]
public class EnemyData : ScriptableObject
{
    /// <summary>
    /// �G�l�~�[�̖��O
    /// </summary>
    public string enemyName;
    /// <summary>
    /// �G�l�~�[��HP
    /// </summary>
    public int enemyHP;
    /// <summary>
    /// �G�l�~�[�̌��݂�HP
    /// </summary>
    public int enemyCurrentHP;
    /// <summary>
    ///�G�l�~�[��AP
    /// </summary>
    public int enemyAP;
    /// <summary>
    /// �G�l�~�[�̌��݂�AP
    /// </summary>
    public int enemyCurrentAP;
    /// <summary>
    /// �G�l�~�[��GP
    /// </summary>
    public int enemyGP;
    /// <summary>
    /// �G�l�~�[�����Ƃ�����
    /// </summary>
    public int dropMoney;
    /// <summary>
    /// �G�l�~�[�̃C���[�W
    /// </summary>
    public Sprite enemyImage;
}
