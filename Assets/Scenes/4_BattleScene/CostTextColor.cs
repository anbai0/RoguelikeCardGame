using UnityEngine;
using TMPro;

public class CostTextColor : MonoBehaviour
{
    [SerializeField]
    CardController cardController;
    [SerializeField]
    TextMeshProUGUI costText;

    PlayerBattleAction player;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBattleAction>();
    }
    void Update()
    {
        //現在のAPをカードのコストが超えていなければ
        if (cardController.cardDataManager._cardCost <= player.GetSetCurrentAP)
        {
            WithinCostColor();
        }
        else //現在のAPをカードのコストが超えていれば
        {
            OverCostColor();
        }
    }

    /// <summary>
    /// カードのコストの色を白にする
    /// </summary>
    void WithinCostColor()
    {
        costText.color = Color.white;
    }

    /// <summary>
    /// カードのコストの色を赤にする
    /// </summary>
    void OverCostColor()
    {
        costText.color = Color.red;
    }
}
