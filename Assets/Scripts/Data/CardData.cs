using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "Create CardData")]
public class CardData : ScriptableObject 
{
    /// <summary>
    /// カードの番号
    /// </summary>
    public int cardID;
    /// <summary>
    /// カードのタイプ
    /// </summary>
    public string cardType;
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
    [SerializeField,TextArea]
    public string cardEffect;
    /// <summary>
    /// カードのレアリティ
    /// </summary>
    public int cardRarity;
    /// <summary>
    /// カードのコスト
    /// </summary>
    public int cardCost;
    /// <summary>
    /// カードの攻撃力
    /// </summary>
    public int cardAttackPower;
    /// <summary>
    /// カードの治癒力
    /// </summary>
    public int cardHealingPower;
    /// <summary>
    /// カードのガード付与力
    /// </summary>
    public int cardGuardPoint;
    /// <summary>
    /// カードの状態
    /// 0:使用可能
    /// 1:ラウンド中使用不可
    /// 2:戦闘中使用不可
    /// </summary>
    public int cardState;
    /// <summary>
    /// カードのイメージ
    /// </summary>
    public Sprite cardImage;
}
