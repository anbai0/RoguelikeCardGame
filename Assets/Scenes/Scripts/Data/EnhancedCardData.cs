using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnhancedCardData", menuName = "Create EnhancedCardData")]
public class EnhancedCardData : ScriptableObject
{
    /// <summary>
    /// カードの番号
    /// </summary>
    public int cardID;
    /// <summary>
    /// カードの名前
    /// </summary>
    public string cardName;
    /// <summary>
    /// カードのフリガナ
    /// </summary>
    //public string cardNameKana;
    /// <summary>
    /// カードの効果
    /// </summary>
    [SerializeField, TextArea]
    public string cardEffect;
    /// <summary>
    /// カードのコスト
    /// </summary>
    public int cardCost;
    /// <summary>
    /// カードのダメージ
    /// </summary>
    //public int damage;
    /// <summary>
    /// カードのイメージ
    /// </summary>
    public Sprite cardImage;
}
