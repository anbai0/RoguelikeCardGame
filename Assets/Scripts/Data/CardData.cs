using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "Create CardData")]
public class CardData : ScriptableObject 
{
    /// <summary>
    /// �J�[�h�̔ԍ�
    /// </summary>
    public int cardID;
    /// <summary>
    /// �J�[�h�̃^�C�v
    /// </summary>
    public string cardType;
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
    [SerializeField,TextArea]
    public string cardEffect;
    /// <summary>
    /// �J�[�h�̃��A���e�B
    /// </summary>
    public int cardRarity;
    /// <summary>
    /// �J�[�h�̃R�X�g
    /// </summary>
    public int cardCost;
    /// <summary>
    /// �J�[�h�̍U����
    /// </summary>
    public int cardAttackPower;
    /// <summary>
    /// �J�[�h�̎�����
    /// </summary>
    public int cardHealingPower;
    /// <summary>
    /// �J�[�h�̃K�[�h�t�^��
    /// </summary>
    public int cardGuardPoint;
    /// <summary>
    /// �J�[�h�̏��
    /// 0:�g�p�\
    /// 1:���E���h���g�p�s��
    /// 2:�퓬���g�p�s��
    /// </summary>
    public int cardState;
    /// <summary>
    /// �J�[�h�̃C���[�W
    /// </summary>
    public Sprite cardImage;
}
