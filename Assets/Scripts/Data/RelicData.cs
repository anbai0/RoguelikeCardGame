using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "Create RelicData")]
public class RelicData : ScriptableObject
{
    /// <summary>
    /// �����b�N�̔ԍ�
    /// </summary>
    public int relicID;
    /// <summary>
    /// �����b�N�̖��O
    /// </summary>
    public string relicName;
    /// <summary>
    /// �����b�N�̃t���K�i
    /// </summary>
    //public string cardNameKana;
    /// <summary>
    /// �����b�N�̌���
    /// </summary>
    [SerializeField, TextArea]
    public string relicEffect;
    /// <summary>
    /// �����b�N�̃_���[�W
    /// </summary>
    //public int damage;
    /// <summary>
    /// �����b�N�̃C���[�W
    /// </summary>
    public Sprite relicImage;
}
