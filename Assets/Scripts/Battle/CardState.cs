using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardState : MonoBehaviour
{
    CardDataManager cardDataManager;
    GameObject blindPanel;
    public bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        cardDataManager = GetComponent<CardController>().cardDataManager;
        blindPanel = gameObject.transform.Find("BlindPanel").gameObject;
        blindPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true && cardDataManager._cardState != 0)//カードが表示されていてステートが0以外の場合
        {
            //カードを暗くするパネルを表示する
            blindPanel.SetActive(true);
        }
        else
        {
            //カードを暗くするパネルを非表示にする
            blindPanel.SetActive(false);
        }
    }
}
