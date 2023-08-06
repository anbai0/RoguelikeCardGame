using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //プレイヤー
    GameObject player;
    public PlayerDataManager playerData;
    public List<RelicDataManager> relicDataList { private set; get; } = new List<RelicDataManager>();
    public Dictionary<int, int> hasRelics = new Dictionary<int, int>();     // 所持しているレリックを格納    
    int maxRelics = 12;

    bool isAlreadyRead = false; // ReadPlayerで読み込んだかを判定する

    [SerializeField] UIManager uiManager;
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

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

    
    /// <summary>
    /// レリックデータの初期化を行います。
    /// </summary>
    private void InitializeRelics()
    {
        relicDataList.Add(new RelicDataManager(1));     // ID順に管理したいため最初の要素だけ代入
        for (int RelicID=1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics.Add(RelicID,0);

            relicDataList.Add(new RelicDataManager(RelicID));
        }
    }

    /// <summary>
    /// 選択されたプレイヤーをインスタンス化し、レリックを取得します。
    /// </summary>
    /// <param name="playerJob"></param>
    public void ReadPlayer(string playerJob)
    {
        isAlreadyRead = true;
        if (playerJob == "Warrior")
        {
            playerData = new PlayerDataManager("Warrior");
            hasRelics[10] += 1;      // 黄金の果実
            hasRelics[4] += 2;       // 神秘のピアス
            ShowRelics();
        }
        if (playerJob == "Wizard")
        {
            playerData = new PlayerDataManager("Wizard");
            hasRelics[5] += 1;     // 千里眼鏡
            hasRelics[9] += 2;     // 富豪の金貨袋
            ShowRelics();
        }
    }


    public void ShowRelics()
    {
        // relicPlaceの子オブジェクトをすべてDestroy
        Transform[] children = relicPlace.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            if (hasRelics.ContainsKey(RelicID) && hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                relic.transform.localScale = Vector3.one * 0.9f;                   // 生成したPrefabの大きさ調整
                relic.Init(RelicID);                                               // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = hasRelics[RelicID].ToString();      // Prefabの子オブジェクトである所持数を表示するテキストを変更

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = relicDataList[RelicID]._relicName.ToString();        // レリックの名前を変更
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = relicDataList[RelicID]._relicEffect.ToString();      // レリック説明変更

            }
        }

        uiManager.UIEventsReload();
    }
}
