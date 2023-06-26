using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardController cardController;

    bool isHeelPotion = false;

    private void Start()
    {
        playerDataManager = GameManager.Instance.playerData;
        foreach (var deck in playerDataManager._deckList)
        {
            if (deck == 3)
            {
                isHeelPotion = true;
            }

        }

    }
    void Update()
    {


        playerDataManager._money -= 50;
        playerDataManager._deckList.Add(1);

    }

    public void ShowCards(int rare2, int rare1_1, int rare1_2)
    {
        cardController.Init(rare2); 
        
    }
}
