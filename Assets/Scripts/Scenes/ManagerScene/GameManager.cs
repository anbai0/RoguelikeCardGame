using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //プレイヤー
    GameObject player;
    public PlayerDataManager playerData;


    //シングルトン
    public static GameManager Instance;
    private void Awake()
    {
        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }

        // これでReadPlayerを呼び出してplayerDataを初期化できます
        ReadPlayer("Warrior");
    }



    public void ReadPlayer(string playerJob)
    {
        if (playerJob == "Warrior")
        {
            playerData = new PlayerDataManager("Warrior");
        }
        else if (playerJob == "Wizard")
        {
            playerData = new PlayerDataManager("Wizard");
        }
    }
}
