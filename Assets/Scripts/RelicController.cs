using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicController : MonoBehaviour
{
    public RelicViewManager relicViewManager;// レリックの見た目の処理
    public RelicDataManager relicDataManager;// レリックのデータを処理

    private void Awake()
    {
        relicViewManager = GetComponent<RelicViewManager>();
    }

    public void Init(int relicID)// レリックを生成した時に呼ばれる関数
    {
        relicDataManager = new RelicDataManager(relicID);// レリックデータを生成
        relicViewManager.ViewRelic(relicDataManager);// レリックデータの表示
    }
}
