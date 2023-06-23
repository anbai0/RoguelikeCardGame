using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create PlayerData")]
public class PlayerData : ScriptableObject
{
    /// <summary>
    /// �L�����N�^�[�̖��O
    /// </summary>
    public string playerName;
    /// <summary>
    /// �L�����N�^�[��HP
    /// </summary>
    public int playerHP;
    /// <summary>
    /// �L�����N�^�[�̌��݂�HP
    /// </summary>
    public int playerCurrentHP;
    /// <summary>
    ///�L�����N�^�[��AP
    /// </summary>
    public int playerAP;
    /// <summary>
    /// �L�����N�^�[�̌��݂�AP
    /// </summary>
    public int playerCurrentAP;
    /// <summary>
    /// �L�����N�^�[��GP
    /// </summary>
    public int playerGP;
    /// <summary>
    /// �L�����N�^�[�̏�����
    /// </summary>
    public int money;
    /// <summary>
    /// �L�����N�^�[�̃f�b�L
    /// </summary>
    public List<int> deckList;
}
