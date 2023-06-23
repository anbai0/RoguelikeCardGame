using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CardEffectList : MonoBehaviour
{
    [SerializeField] Text playerHPText;
    [SerializeField] Text playerAPText;
    [SerializeField] Text playerGPText;
    [SerializeField] Text enemyHPText;
    [SerializeField] Slider enemyHPSlider;
    int cardID;
    int cardAttackPower;
    int cardHealingPower;
    int cardGuardPoint;
    BattleGameManager bg;

    public void ActiveCardEffect(CardController card)
    {
        bg = BattleGameManager.Instance;
        cardID = card.cardDataManager._cardID;
        cardAttackPower = card.cardDataManager._cardAttackPower;
        cardHealingPower = card.cardDataManager._cardHealingPower;
        cardGuardPoint = card.cardDataManager._cardGuardPoint;
        Debug.Log("Cardのナンバーは" + cardID);
        //カードのIDに応じて処理を呼び出す
        switch (cardID)
        {
            case 1: CardID1();
                break;
            case 2: CardID2();
                break;
            case 3: CardID3(card);
                break;
            case 4: CardID4();
                break;
            case 5: CardID5(card);
                break;
            case 6: CardID6();
                break;
            case 7: CardID7();
                break;
            case 8: CardID8(card);
                break;
            case 9: CardID9();
                break;
            case 10: CardID10();
                break;
            case 11: CardID11();
                break;
            case 12: CardID12();
                break;
            case 13: CardID13(card);
                break;
            case 14: CardID14();
                break;
            case 15: CardID15(card);
                break;
            case 16: CardID16();
                break;
            case 17: CardID17(card);
                break;
            case 18: CardID18(card);
                break;
            case 19: CardID19(card);
                break;
            case 20: CardID20();
                break;
            default:Debug.Assert(false);
                break;
        }
    }
    /// <summary>
    /// 技名：スイング
    /// </summary>
    private void CardID1()
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// 技名：ヒール
    /// </summary>
    private void CardID2()
    {
        Debug.Log("CardID2が呼び出されました");
        //HPを回復
        PlayerHealing();
    }
    /// <summary>
    /// 技名：魔女の霊薬
    /// </summary>
    private void CardID3(CardController card)
    {
        //HPを回復
        PlayerHealing();
        Destroy(card.gameObject);
    }
    /// <summary>
    /// 技名：ガード
    /// </summary>
    private void CardID4()
    {
        //ガードを追加
        PlayerAddGP();
    }
    /// <summary>
    /// 技名：ツバメ返し
    /// </summary>
    private void CardID5(CardController card)
    {
        //時間差でエネミーを攻撃
        StartCoroutine(ID5Attacking(cardAttackPower, 0.8f));
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }

    IEnumerator ID5Attacking(int attackMethod,float attackInterval) 
    {
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
    }
    /// <summary>
    /// 技名：シールドバッシュ
    /// </summary>
    private void CardID6()
    {
        //ガードを追加
        PlayerAddGP();
        //エネミーを攻撃
        PlayerAttacking(bg.playerGP);
    }
    /// <summary>
    /// 技名：逆鱗
    /// </summary>
    private void CardID7()
    {
        //バッドステータスが１つでもあった場合
        if (bg.playerCondition.curse > 0 
            || bg.playerCondition.impatience > 0 
            || bg.playerCondition.weakness > 0 
            || bg.playerCondition.burn > 0 
            || bg.playerCondition.poison > 0) 
        {
            //バッドステータスを解除
            ReleaseBadStatus();
            //筋力増強を2付与
            bg.playerCondition.upStrength += 2;
        }
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// 技名：リコシェト
    /// </summary>
    private void CardID8(CardController card)
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //カードの攻撃力を１増加
        card.cardDataManager._cardAttackPower++;
        //増加した攻撃力をカードに反映
        GameObject cardEffect = card.gameObject.transform.GetChild(2).gameObject;//効果表示のオブジェクト
        Text cardText = cardEffect.GetComponent<Text>();
        string text = cardText.text;
        string pattern = @"\d+";// 半角数字を抽出するための正規表現パターン
        MatchCollection matches = Regex.Matches(text, pattern);// 正規表現パターンに一致する半角数字を検索
        foreach (Match match in matches)// 半角数字に+1する
        {
            string numberString = match.Value;
            int number;
            if (Int32.TryParse(numberString, out number))
            {
                int addNumber = number + 1;

                // 元の文字列内の半角数字を置換する
                text = text.Replace(numberString, addNumber.ToString());
            }
            else
            {
                Debug.LogWarning("Invalid number format: " + numberString);
            }
        }
        cardText.text = text;
    }
    /// <summary>
    /// 技名：浄化の風
    /// </summary>
    private void CardID9()
    {
        //バッドステータスを解除
        ReleaseBadStatus();
        //状態異常無効を1付与
        bg.playerCondition.invalidBadStatus += 1;
    }
    /// <summary>
    /// 技名：クリアヴェール
    /// </summary>
    private void CardID10()
    {
        //ガードを追加
        PlayerAddGP();
        //状態異常無効を3付与
        bg.playerCondition.invalidBadStatus += 3;
    }
    /// <summary>
    /// 技名：アクセラレート
    /// </summary>
    private void CardID11()
    {
        //BattleGameManagerのアクセラレート機能をON
        bg.isAccelerate = true;
    }
    /// <summary>
    /// 技名：乱れ鞭
    /// </summary>
    private void CardID12()
    {
        //時間差でエネミーを攻撃
        StartCoroutine(ID12Attacking(cardAttackPower, 0.2f));
    }
    IEnumerator ID12Attacking(int attackMethod, float attackInterval)
    {
        PlayerAttacking(attackMethod);
        for (int count = 0; count < 3; count++)
        {
            yield return new WaitForSeconds(attackInterval);
            PlayerAttacking(attackMethod);
        }
    }
    /// <summary>
    /// 技名：フルバースト
    /// </summary>
    private void CardID13(CardController card)
    {
        //現在のAP分のダメージを計算
        int damage = bg.playerCurrentAP * 2;
        //APを全消費
        bg.playerCurrentAP = 0;
        bg.playerAPText.text = bg.playerCurrentAP + "/" + bg.playerAP;
        //エネミーを攻撃
        PlayerAttacking(damage);
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// 技名：ハイボルケーノ  
    /// </summary>
    private void CardID14()
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //火傷を2付与
        bg.enemyCondition.burn += 2;
    }
    /// <summary>
    /// 技名：デビルドレイン
    /// </summary>
    private void CardID15(CardController card)
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //HPを回復
        PlayerHealing();
        //エネミーに衰弱を1付与
        bg.enemyCondition.weakness += 1;
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// 技名：ガラティーン
    /// </summary>
    private void CardID16()
    {
        if (bg.enemyCondition.burn > 0)//エネミーが火傷状態だった場合
        {
            //火傷の3倍のダメージでエネミーを攻撃
            PlayerAttacking(bg.enemyCondition.burn * 3);
        }
        else 
        {
            //エネミーを攻撃
            PlayerAttacking(cardAttackPower);
        }
    }
    /// <summary>
    /// 技名：雷霆の衝撃
    /// </summary>
    private void CardID17(CardController card)
    {
        //行動不能の処理をもう少し弄りたい
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //エネミーを行動不能にする
        bg.enemyCurrentAP = 0;
        //この戦闘中カードを使用不可にする
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// 技名：エクスカリバー
    /// </summary>
    private void CardID18(CardController card)
    {
        PlayerAttacking(cardAttackPower);
        if (bg.roundCount >= 3)//3ラウンド目以降の場合
        {
            //プレイヤーのAPを回復する
            StartCoroutine(ID18APHealing(1.0f));
        }
        //この戦闘中カードを使用不可にする
        card.cardDataManager._cardState = 2;
    }
    IEnumerator ID18APHealing(float attackInterval) 
    { 
        yield return new WaitForSeconds(attackInterval);
        //プレイヤーのAPを7回復する
        bg.playerCurrentAP += 7;
        if (bg.playerCurrentAP > bg.playerAP)
        {
            bg.playerCurrentAP = bg.playerAP;
        }
        playerAPText.text = bg.playerCurrentAP + "/" + bg.playerAP;
    }
    /// <summary>
    /// 技名：アヴァロンヴェール
    /// </summary>
    private void CardID19(CardController card)
    {
        //自動回復を1付与
        bg.playerCondition.autoHealing += 1;
        //状態異常無効を2付与
        bg.playerCondition.invalidBadStatus += 2;
        //この戦闘中カードを使用不可にする
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// 技名：鬼火
    /// </summary>
    private void CardID20()
    {
        //火傷を1付与
        bg.enemyCondition.burn += 1;
    }
    private void PlayerAttacking(int attackMethod)//エネミーへの攻撃処理 
    {
        attackMethod += bg.playerCondition.upStrength;//筋力増強の効果
        attackMethod -= bg.playerCondition.weakness;//衰弱の効果
        if (attackMethod < 0) 
        {
            attackMethod = 0;
        }
        int attackPower = attackMethod - bg.enemyGP;
        if (bg.enemyGP > 0) 
        {
            bg.enemyGP -= attackMethod;
            if (bg.enemyGP <= 0) 
            {
                bg.enemyGP = 0;
            }
        }
        if (attackPower <= 0) 
        {
            attackPower = 0;
        }
        Debug.Log("計算後の攻撃力は" + attackPower);
        bg.enemyCurrentHP -= attackPower;
        enemyHPText.text = bg.enemyCurrentHP + "/" + bg.enemyHP;
        enemyHPSlider.value = bg.enemyCurrentHP / (float)bg.enemyHP;
    }
    private void PlayerHealing()//プレイヤーのHP回復処理
    {
        bg.playerCurrentHP += cardHealingPower;
    }
    private void PlayerAddGP()//プレイヤーにガードを追加
    {
        bg.playerGP += cardGuardPoint;
    }
    private void ReleaseBadStatus()//プレイヤーに付与されたデバフの解除
    {
        bg.playerCondition.curse = 0;
        bg.playerCondition.impatience = 0;
        bg.playerCondition.weakness = 0;
        bg.playerCondition.burn = 0;
        bg.playerCondition.poison = 0;
    }
}
