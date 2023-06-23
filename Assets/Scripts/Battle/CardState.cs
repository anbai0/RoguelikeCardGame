using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardState : MonoBehaviour
{
    CardDataManager cardDataManager;
    GameObject hiddenPanel;
    // Start is called before the first frame update
    void Start()
    {
        cardDataManager = GetComponent<CardController>().cardDataManager;
        hiddenPanel = gameObject.transform.GetChild(5).gameObject;
        hiddenPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cardDataManager._cardState != 0)//カードが使用不可な場合
        {
            //カードを暗くするパネルを表示する
            hiddenPanel.SetActive(true);
        }
        else 
        {
            //カードを暗くするパネルを非表示にする
            hiddenPanel.SetActive(false);
        }
    }
}
