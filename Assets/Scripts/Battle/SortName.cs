using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortName : MonoBehaviour
{
    /// <summary>
    /// 名前順に並び変える処理
    /// </summary>
    public void Sort()
    {
        List<Transform> deckList = new List<Transform>();
        var childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            //リストに子オブジェクトを追加
            deckList.Add(transform.GetChild(i));
        }
        //子オブジェクトの名前を比較する
        deckList.Sort((obj1, obj2) => string.Compare(obj1.name, obj2.name));
        //子オブジェクトの名前を昇順でソートする
        foreach (var deck in deckList)
        {
            deck.SetSiblingIndex(childCount - 1);
        }
    }
}
