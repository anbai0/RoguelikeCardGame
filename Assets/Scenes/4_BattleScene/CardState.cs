using UnityEngine;

/// <summary>
/// 使用不可のカードを暗くするスクリプト
/// </summary>
public class CardState : MonoBehaviour
{
    CardDataManager cardDataManager;
    GameObject blindPanel;
    public bool isActive = true;

    void Start()
    {
        isActive = true;
        cardDataManager = GetComponent<CardController>().cardDataManager;
        blindPanel = gameObject.transform.Find("BlindPanel").gameObject;
        blindPanel.SetActive(false);
    }

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
