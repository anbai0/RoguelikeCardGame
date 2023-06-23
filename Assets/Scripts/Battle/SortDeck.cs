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
            //リストに子オブジェクトのカードを追加
            deckList.Add(transform.GetChild(i));
        }
        //カードの名前を比較する
        deckList.Sort((obj1, obj2) => string.Compare(obj1.name, obj2.name));
        //カードの名前を昇順でソートする
        foreach (var deck in deckList)
        {
            deck.SetSiblingIndex(childCount - 1);
        }
    }
}
