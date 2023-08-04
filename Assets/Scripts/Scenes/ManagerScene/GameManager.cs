using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //プレイヤー
    GameObject player;
    public PlayerDataManager playerData;
    public RelicDataManager relicData;
    public Dictionary<int, int> hasRelics = new Dictionary<int, int>();     // 所持しているレリックを格納
    public Dictionary<string, int> e = new Dictionary<string, int>();     
    int maxRelics = 12;

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

        InitializeRelics();

        // 各シーンでデバッグするときにコメントを解除してください
        // 一度も読み込んでいなければ
        if (!isAlreadyRead) ReadPlayer("Warrior");
    }

    private void InitializeRelics()
    {
        for(int RelicID=1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics.Add(RelicID,0);
        }
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
