using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create PlayerData")]
public class PlayerData : ScriptableObject
{
    /// <summary>
    /// キャラクターの名前
    /// </summary>
    public string playerName;
    /// <summary>
    /// キャラクターのHP
    /// </summary>
    public int playerHP;
    /// <summary>
    /// キャラクターの現在のHP
    /// </summary>
    public int playerCurrentHP;
    /// <summary>
    ///キャラクターのAP
    /// </summary>
    public int playerAP;
    /// <summary>
    /// キャラクターの現在のAP
    /// </summary>
    public int playerCurrentAP;
    /// <summary>
    /// キャラクターのGP
    /// </summary>
    public int playerGP;
    /// <summary>
    /// キャラクターの所持金
    /// </summary>
    public int money;
    /// <summary>
    /// キャラクターのデッキ
    /// </summary>
    public List<int> deckList;
}
