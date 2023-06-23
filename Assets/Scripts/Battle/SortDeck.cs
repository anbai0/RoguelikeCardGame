using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortDeck : MonoBehaviour
{
    public void Sort()
    {
        List<Transform> deckList = new List<Transform>();
        var childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            //���X�g�Ɏq�I�u�W�F�N�g�̃J�[�h��ǉ�
            deckList.Add(transform.GetChild(i));
        }
        //�J�[�h�̖��O���r����
        deckList.Sort((obj1, obj2) => string.Compare(obj1.name, obj2.name));
        //�J�[�h�̖��O�������Ń\�[�g����
        foreach (var deck in deckList)
        {
            deck.SetSiblingIndex(childCount - 1);
        }
    }
}
