using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{

    public string _playerName;
    public int _playerHP;
    public int _playerCurrentHP;
    public int _playerAP;
    public int _playerCurrentAP;
    public int _playerGP;
    public int _money;
    public List<int> _deckList;

    public PlayerDataManager(string name)
    {
        PlayerData playerData = Resources.Load<PlayerData>("PlayerDataList/" + name);
        _playerName = playerData.playerName;
        _playerHP = playerData.playerHP;
        _playerCurrentHP = playerData.playerCurrentHP;
        _playerAP = playerData.playerAP;
        _playerCurrentAP = playerData.playerCurrentAP;
        _playerGP = playerData.playerGP;
        _money = playerData.money;
        _deckList = playerData.deckList;
    }
}
