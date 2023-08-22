using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureBoxManager : MonoBehaviour
{
    [Header("宝箱獲得画面用UI")]
    [SerializeField] GameObject treasureBoxUI;

    [Header("参照するコンポーネント")]
    [SerializeField] Lottery lottery;
    [SerializeField] SceneFader sceneFader;
    [SerializeField] UIManagerTreasureBox uiManagerTB;

    [Header("生成するカードオブジェクト")]
    [SerializeField] GameObject cardPrefab;
    [Header("生成するレリックオブジェクト")]
    [SerializeField] GameObject relicPrefab;
    [Header("カードの生成場所")]
    [SerializeField] Transform cardPlace;
    [Header("レリックの生成場所")]
    [SerializeField] Transform relicPlace;

    CardController cardController;
    RelicController relicController;

    List<int> treasureCardID = null; //宝箱から出てくるカードのID
    List<int> treasureRelicID = null; //宝箱から出てくるレリックのID

    void Start()
    {
        Invoke("ShowTreasure", 0.05f); //読み込みが遅れているので呼び出しを遅らせる
    }

    /// <summary>
    /// 宝箱獲得画面の表示
    /// </summary>
    void ShowTreasure()
    {
        TreasureLottery();
        ShowCards();
        ShowRelics();
        uiManagerTB.UIEventsReload();
        relicPlace.gameObject.SetActive(false);
    }

    /// <summary>
    /// 宝箱から獲得できるアイテムを抽選
    /// </summary>
    void TreasureLottery()
    {
        treasureCardID = lottery.SelectCardByRarity(new List<int> { 2, 2, 1 });
        treasureRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 1, 1 });
    }

    /// <summary>
    /// 抽選されたカードを生成
    /// </summary>
    void ShowCards()
    {
        for (int cardCount = 0; cardCount < treasureCardID.Count; cardCount++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPlace);
            cardObj.transform.SetParent(cardPlace);
            cardController = cardObj.GetComponent<CardController>();
            cardController.Init(treasureCardID[cardCount]);
        }
    }

    /// <summary>
    /// 抽選されたレリックを生成
    /// </summary>
    void ShowRelics()
    {
        for (int relicCount = 0; relicCount < treasureRelicID.Count; relicCount++)
        {
            GameObject relicObj = Instantiate(relicPrefab, relicPlace);
            relicObj.transform.SetParent(relicPlace);
            relicController = relicObj.GetComponent<RelicController>();
            relicController.Init(treasureRelicID[relicCount]);
            Transform relicBG = relicObj.transform.GetChild(8); //表示するBackGroundを取得
            TextMeshProUGUI relicName = relicBG.GetChild(0).GetComponent<TextMeshProUGUI>(); //レリックの名前
            relicName.text = relicController.relicDataManager._relicName;
            TextMeshProUGUI relicEffect = relicBG.GetChild(1).GetComponent<TextMeshProUGUI>(); //レリックの効果
            relicEffect.text = relicController.relicDataManager._relicEffect;
        }
    }

    /// <summary>
    /// 宝箱シーンをアンロード
    /// </summary>
    public void UnLoadTreasureBoxScene()
    {
        sceneFader.SceneChange(unLoadSceneName: "TreasureBoxScene");
    }
}
