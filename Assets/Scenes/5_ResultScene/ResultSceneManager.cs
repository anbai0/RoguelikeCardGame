using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class ResultSceneManager : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] UIManagerResult uiManager;

    [Header("カード表示関係")]
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform deckPlace;
    [SerializeField] GameObject scrollView;     // デッキを表示するUIの親オブジェクト
    private List<int> deckNumberList;                    //プレイヤーのもつデッキナンバーのリスト

    // レリック
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    private Vector3 cardScale = Vector3.one * 0.25f;     // 生成するカードのスケール

    // リザルトの背景
    [SerializeField] GameObject clearBG;
    [SerializeField] GameObject gameOverBG;
    // クリア、ゲームオーバーのテキスト
    [SerializeField] TextMeshProUGUI clearText;
    [SerializeField] TextMeshProUGUI gameOverText;
    TextMeshProUGUI viewText;

    void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;
        AudioManager.Instance.PlayBGM("Result");

        // リザルトの背景を表示
        if (gm.isClear == true)
        {
            clearBG.SetActive(true);
            viewText = clearText;
        }           
        else
        {
            gameOverBG.SetActive(true);
            viewText = gameOverText;
        }

        StartCoroutine(TextAnimCoroutine());

        InitDeck();
        ShowRelics();
        uiManager.UIEventsReload();
    }


    IEnumerator TextAnimCoroutine()
    {

        DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(viewText);

        for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
        {
            tmproAnimator.DOScaleChar(i, 0.7f, 0);
            Vector3 currCharOffset = tmproAnimator.GetCharOffset(i);
            DOTween.Sequence()
                .Append(tmproAnimator.DOOffsetChar(i, currCharOffset + new Vector3(0, 30, 0), 0.4f).SetEase(Ease.OutFlash, 2))
                .Join(tmproAnimator.DOScaleChar(i, 1, 0.4f).SetEase(Ease.OutBack))
                .SetDelay(0.07f * i);

            // 最後の文字のアニメーションが完了したら待機
            if (i == tmproAnimator.textInfo.characterCount - 1)
            {
                yield return new WaitForSeconds(0.07f * tmproAnimator.textInfo.characterCount + 1f);
            }
        }

        // アニメーションが完了したら再度実行
        StartCoroutine(TextAnimCoroutine());
    }


    private void InitDeck() //デッキ生成
    {
        deckNumberList = GameManager.Instance.playerData._deckList;

        for (int init = 0; init < deckNumberList.Count; init++)         // 選択出来るデッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, deckPlace);   //カードを生成する
            card.transform.localScale = cardScale;
            card.name = "Deck" + (init).ToString();                     //生成したカードに名前を付ける
            card.Init(deckNumberList[init]);                            //デッキデータの表示
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

        for (int RelicID = 1; RelicID <= gm.maxRelics; RelicID++)
        {
            if (gm.hasRelics.ContainsKey(RelicID) && gm.hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                //relic.transform.localScale = Vector3.one * 0.9f;                   // 生成したPrefabの大きさ調整
                relic.Init(RelicID);                                               // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = gm.hasRelics[RelicID].ToString();      // Prefabの子オブジェクトである所持数を表示するテキストを変更

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicName.ToString();        // レリックの名前を変更
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicEffect.ToString();      // レリック説明変更
            }
        }

        uiManager.UIEventsReload();
    }


    public void SceneUnLoad()
    {
        gm.UnloadAllScene();     // GameManagerのデータをリセット
    }
}
