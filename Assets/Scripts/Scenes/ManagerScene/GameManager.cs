using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //プレイヤー
    GameObject player;
    public PlayerDataManager playerData;

    bool isAlreadyRead = false; // ReadPlayerで読み込んだかを判定する

    //シングルトン
    public static GameManager Instance;
    private void Awake()
    {
        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }

        // 各シーンでデバッグするときにコメントを解除してください
        // 一度も読み込んでいなければ
        if (!isAlreadyRead) ReadPlayer("Warrior");

    }



    public void ReadPlayer(string playerJob)
    {
        isAlreadyRead = true;
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
