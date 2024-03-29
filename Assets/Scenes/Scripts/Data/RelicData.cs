using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "Create RelicData")]
public class RelicData : ScriptableObject
{
    /// <summary>
    /// レリックの番号
    /// </summary>
    public int relicID;
    /// <summary>
    /// レリックの名前
    /// </summary>
    public string relicName;
    /// <summary>
    /// レリックのフリガナ
    /// </summary>
    //public string cardNameKana;
    /// <summary>
    /// レリックの効果
    /// </summary>
    [SerializeField, TextArea]
    public string relicEffect;
    /// <summary>
    /// レリックのレアリティ
    /// </summary>
    public int relicRarity;
    /// <summary>
    /// レリックのダメージ
    /// </summary>
    //public int damage;
    /// <summary>
    /// レリックの金額
    /// </summary>
    public int relicPrice;
    /// <summary>
    /// レリックのイメージ
    /// </summary>
    public Sprite relicImage;
}
