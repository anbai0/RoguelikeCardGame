using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelfMadeNamespace;

public class BattleRewardManager : MonoBehaviour
{
    [Header("報酬画面用UI")]
    [SerializeField]
    GameObject battleRewardUI;

    [Header("参照するコンポーネント")]
    [SerializeField]
    Lottery lottery;
    [SerializeField] 
    SceneFader sceneFader;
    
    [Header("生成するカードオブジェクト")]
    [SerializeField]
    GameObject cardPrefab;
    [Header("生成するレリックオブジェクト")]
    [SerializeField]
    GameObject relicPrefab;
    [Header("カードの生成場所")]
    [SerializeField]
    Transform cardPlace;
    [Header("レリックの生成場所")]
    [SerializeField]
    Transform relicPlace;
    
    CardController cardController;
    RelicController relicController;
    
    List<int> rewardCardID = null; //報酬として選ばれたカードのID
    List<int> rewardRelicID = null; //報酬として選ばれたレリックのID
    const int RelicID1 = 1; //アリアドネの糸(レアリティ３)

    GameManager gm;
    BattleGameManager bg;

    void Start()
    {
        gm = GameManager.Instance;
        bg = BattleGameManager.Instance;
        battleRewardUI.SetActive(false);
    }

    /// <summary>
    /// 報酬画面を表示する処理
    /// </summary>
    /// <param name="type">エネミーの種類</param>
    public void ShowReward(string type)
    {
        battleRewardUI.SetActive(true); 
        SelectRewardByCards(type);
        SelectRewardByRelics(type);
        battleRewardUI.GetComponent<DisplayAnimation>().StartPopUPAnimation();
    }

    /// <summary>
    /// エネミーの種類に応じて報酬のカードを抽選、表示をする処理
    /// </summary>
    /// <param name="type">エネミーの種類</param>
    void SelectRewardByCards(string type)
    {
        if (type == "SmallEnemy")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        }
        else if (type == "StrongEnemy")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 2, 2, 1 });
        }
        else if (type == "Boss")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 3, 2, 2 });
        }
        ShowCards();
    }

    /// <summary>
    /// エネミーの種類に応じてレリックを抽選、表示をする処理
    /// </summary>
    /// <param name="type">エネミーの種類</param>
    public void SelectRewardByRelics(string type)
    {
        if (type == "StrongEnemy")
        {
            rewardRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 1, 1 });
        }
        else if (type == "Boss")
        {
            rewardRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 2 });
            rewardRelicID.Insert(0, RelicID1);
        }
        else
        {
            rewardRelicID = null;
        }
        ShowRelics();
    }

    /// <summary>
    /// 抽選されたカードの生成と表示をする処理
    /// </summary>
    void ShowCards()
    {
        for (int cardCount = 0; cardCount < rewardCardID.Count; cardCount++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPlace);
            cardObj.transform.SetParent(cardPlace);
            cardController = cardObj.GetComponent<CardController>();
            cardController.Init(rewardCardID[cardCount]);
            cardController.cardDataManager._cardState = -1; //バトル画面にあるカードと区別する為にStateを-1にする
        }
    }

    /// <summary>
    /// 抽選されたレリックの生成と表示する処理
    /// </summary>
    void ShowRelics()
    {
        if (rewardRelicID != null) //リストにIDが入っていれば
        {
            for (int relicCount = 0; relicCount < rewardRelicID.Count; relicCount++)
            {
                GameObject relicObj = Instantiate(relicPrefab, relicPlace);
                relicObj.transform.SetParent(relicPlace);
                relicController = relicObj.GetComponent<RelicController>();
                relicController.Init(rewardRelicID[relicCount]);
            }
        }
    }

    public void UnLoadBattleScene()
    {
        if (bg.enemyType == "StrongEnemy")
        {
            if (gm.floor < 3) //階層が3階まで到達していない場合
            {
                gm.floor++; //階層を1つ上げる
                // バトルシーンをアンロードし、フィールドシーンをロード
                sceneFader.SceneChange("FieldScene", "BattleScene");
            }
            else
            {
                // バトルシーンをアンロードし、リザルトシーンをロード
                sceneFader.SceneChange("ResultScene", "BattleScene");
            }
        }
        else
        {
            // バトルシーンをアンロード
            sceneFader.SceneChange(unLoadSceneName: "BattleScene");
            PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
            PlayerController.isPlayerActive = true;       // プレイヤーを動けるようにする
            playerController.enemy.SetActive(false);      // エネミーを消す
            playerController = null;
        }
    }
}