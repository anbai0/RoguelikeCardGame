using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameMode : MonoBehaviour
{
    [SerializeField] BattleRewardManager battleRewardManager;

    public void SelectContinue()
    {
        battleRewardManager.selectLevel = 3;
        battleRewardManager.UnLoadBattleScene();
    }

    public void SelectQuit()
    {
        battleRewardManager.selectLevel = 1;
        battleRewardManager.UnLoadBattleScene();
    }
}
