using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicDataManager
{
    public int _relicID;
    public string _relicName;
    public string _relicEffect;
    public int _relicRarity;
    public int _relicPrice;
    public Sprite _relicImage;


    public RelicDataManager(int relicID)
    {
        RelicData relicData = Resources.Load<RelicData>("RelicDataList/Relic" + relicID);
        _relicID = relicData.relicID;
        _relicName = relicData.relicName;
        _relicEffect = relicData.relicEffect;
        _relicRarity = relicData.relicRarity;
        _relicPrice = relicData.relicPrice;
        _relicImage = relicData.relicImage;
    }
}
