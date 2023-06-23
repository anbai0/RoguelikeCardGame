using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnhancedCardData", menuName = "Create EnhancedCardData")]
public class EnhancedCardData : ScriptableObject
{
    /// <summary>
    /// �J�[�h�̔ԍ�
    /// </summary>
    public int cardID;
    /// <summary>
    /// �J�[�h�̖��O
    /// </summary>
    public string cardName;
    /// <summary>
    /// �J�[�h�̃t���K�i
    /// </summary>
    //public string cardNameKana;
    /// <summary>
    /// �J�[�h�̌���
    /// </summary>
    [SerializeField, TextArea]
    public string cardEffect;
    /// <summary>
    /// �J�[�h�̃R�X�g
    /// </summary>
    public int cardCost;
    /// <summary>
    /// �J�[�h�̃_���[�W
    /// </summary>
    //public int damage;
    /// <summary>
    /// �J�[�h�̃C���[�W
    /// </summary>
    public Sprite cardImage;
}
