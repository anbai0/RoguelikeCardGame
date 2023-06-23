using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager
{
    
    public string _enemyName;
    public int _enemyHP;
    public int _enemyCurrentHP;
    public int _enemyAP;
    public int _enemyCurrentAP;
    public int _enemyGP;
    public int _dropMoney;
    public List<int> _enemyDeckList;
    public Sprite _enemyImage;

    public EnemyDataManager(string name) 
    {
        EnemyData enemyData = Resources.Load<EnemyData>("EnemyDataList/" + name);
        _enemyName = enemyData.enemyName;
        _enemyHP = enemyData.enemyHP;
        _enemyCurrentHP = enemyData.enemyCurrentHP;
        _enemyAP = enemyData.enemyAP;
        _enemyCurrentAP = enemyData.enemyAP;
        _enemyGP = enemyData.enemyGP;
        _dropMoney = enemyData.dropMoney;
        _enemyDeckList = enemyData.enemyDeckList;
        _enemyImage = enemyData.enemyImage;
    }
}
