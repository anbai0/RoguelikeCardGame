using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortName : MonoBehaviour
{
    /// <summary>
    /// ���O���ɕ��ѕς��鏈��
    /// </summary>
    public void Sort()
    {
        List<Transform> deckList = new List<Transform>();
        var childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            //���X�g�Ɏq�I�u�W�F�N�g��ǉ�
            deckList.Add(transform.GetChild(i));
        }
        //�q�I�u�W�F�N�g�̖��O���r����
        deckList.Sort((obj1, obj2) => string.Compare(obj1.name, obj2.name));
        //�q�I�u�W�F�N�g�̖��O�������Ń\�[�g����
        foreach (var deck in deckList)
        {
            deck.SetSiblingIndex(childCount - 1);
        }
    }
}
