using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

//このスクリプトはカードの効果をまとめた物です。
//フィールド上にドロップされたカードの情報を引数で受け取り効果の処理をします。
public class CardEffectList : MonoBehaviour
{
    BattleGameManager bg;
    PlayerBattleAction player;
    EnemyBattleAction enemy;
    int cardID;
    int cardAttackPower;
    int cardHealingPower;
    int cardGuardPoint;

    public void ActiveCardEffect(CardController card)
    {
        bg = BattleGameManager.Instance;
        player = GetComponent<PlayerBattleAction>();
        enemy = GetComponent<EnemyBattleAction>();
        cardID = card.cardDataManager._cardID;
        cardAttackPower = card.cardDataManager._cardAttackPower;
        cardHealingPower = card.cardDataManager._cardHealingPower;
        cardGuardPoint = card.cardDataManager._cardGuardPoint;
        Debug.Log("Cardのナンバーは" + cardID);
        //カードのIDに応じて処理を呼び出す
        switch (cardID)
        {
            case 1:
                CardID1();
                break;
            case 101:
                CardID1();
                break;
            case 2:
                CardID2();
                break;
            case 102:
                CardID2();
                break;
            case 3:
                CardID3(card);
                break;
            case 4:
                CardID4();
                break;
            case 104:
                CardID4();
                break;
            case 5:
                CardID5(card);
                break;
            case 105:
                CardID105(card);
                break;
            case 6:
                CardID6();
                break;
            case 106:
                CardID6();
                break;
            case 7:
                CardID7();
                break;
            case 107:
                CardID107();
                break;
            case 8:
                CardID8(card);
                break;
            case 108:
                CardID8(card);
                break;
            case 9:
                CardID9();
                break;
            case 109:
                CardID109();
                break;
            case 10:
                CardID10();
                break;
            case 110:
                CardID10();
                break;
            case 11:
                CardID11();
                break;
            case 111:
                CardID111();
                break;
            case 12:
                CardID12();
                break;
            case 112:
                CardID112();
                break;
            case 13:
                CardID13(card);
                break;
            case 113:
                CardID113(card);
                break;
            case 14:
                CardID14();
                break;
            case 114:
                CardID114();
                break;
            case 15:
                CardID15(card);
                break;
            case 115:
                CardID15(card);
                break;
            case 16:
                CardID16();
                break;
            case 116:
                CardID16();
                break;
            case 17:
                CardID17(card);
                break;
            case 117:
                CardID17(card);
                break;
            case 18:
                CardID18(card);
                break;
            case 118:
                CardID18(card);
                break;
            case 19:
                CardID19(card);
                break;
            case 119:
                CardID19(card);
                break;
            case 20:
                CardID20();
                break;
            case 120:
                CardID120();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    /// <summary>
    /// 技名：スイング,強化スイング
    /// 効果：3(4)ダメージを与える。
    /// </summary>
    private void CardID1()
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// 技名：ヒール,強化ヒール
    /// 効果：自分のHPを3(4)回復。
    /// </summary>
    private void CardID2()
    {
        //HPを回復
        PlayerHealing(cardHealingPower);
    }
    /// <summary>
    /// 技名：魔女の霊薬
    /// 効果：自分のHPを20回復できる。（ただし、一度使うとこのカードは消滅する）
    /// </summary>
    private void CardID3(CardController card)
    {
        //HPを回復
        PlayerHealing(cardHealingPower);
        Destroy(card.gameObject);
    }
    /// <summary>
    /// 技名：ガード,強化ガード
    /// 効果：3(4)ガードを得る
    /// </summary>
    private void CardID4()
    {
        //ガードを追加
        PlayerAddGP(cardGuardPoint);
    }
    /// <summary>
    /// 技名：ツバメ返し
    /// 効果：2ダメージを2回与える。
    /// </summary>
    private void CardID5(CardController card)
    {
        //時間差でエネミーを攻撃
        StartCoroutine(ID5Attacking(cardAttackPower, 0.8f));
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    IEnumerator ID5Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：強化ツバメ返し
    /// 効果：2ダメージを3回与える。
    /// </summary>
    private void CardID105(CardController card)
    {
        //時間差でエネミーを攻撃
        StartCoroutine(ID105Attacking(cardAttackPower, 0.8f));
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    IEnumerator ID105Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
        yield return new WaitForSeconds(attackInterval);
        PlayerAttacking(attackMethod);
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：シールドバッシュ,強化シールドバッシュ
    /// 効果：ガードを3得て、自分のガードの数と同じダメージを与える。
    /// </summary>
    private void CardID6()
    {
        //ガードを追加
        PlayerAddGP(cardGuardPoint);
        //エネミーを攻撃
        PlayerAttacking(player.GetSetGP);
    }

    /// <summary>
    /// 技名：逆鱗
    /// 効果：自分にデバフがあるならすべてを解除して筋力増強を2得る。5ダメージ与える。
    /// </summary>
    private void CardID7()
    {
        //バッドステータスが１つでもあった場合
        if (player.CheckBadStatus() > 0)
        {
            //バッドステータスを解除
            PlayerReleaseBadStatus();
            //筋力増強を2付与
            player.AddConditionStatus("UpStrength", 2);
        }
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// 技名：強化逆鱗
    /// 効果：自分にデバフがあるならすべてを解除して筋力増強を3得る。5ダメージ与える。
    /// </summary>
    private void CardID107()
    {
        //バッドステータスが１つでもあった場合
        if (player.CheckBadStatus() > 0)
        {
            //バッドステータスを解除
            PlayerReleaseBadStatus();
            //筋力増強を3付与
            player.AddConditionStatus("UpStrength", 3);
        }
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
    }
    /// <summary>
    /// 技名：リコシェト,強化リコシェト
    /// 効果：相手に1(2)ダメージを与える。このゲーム中、このコマンドのダメージが1増える。
    /// </summary>
    private void CardID8(CardController card)
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //カードの攻撃力を１増加
        card.cardDataManager._cardAttackPower++;
        //増加した攻撃力をカードに反映
        GameObject cardEffect = card.gameObject.transform.Find("CardInfo/CardEffect").gameObject;//効果表示のオブジェクト
        TextMeshProUGUI cardText = cardEffect.GetComponent<TextMeshProUGUI>();
        string text = cardText.text;
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
            {
                // 最初の半角数字のみを+1した数字に変更
                text = text.Remove(i, 1).Insert(i, card.cardDataManager._cardAttackPower.ToString());
                break;
            }
        }
        cardText.text = text; //カードのTextに反映
    }
    /// <summary>
    /// 技名：浄化の風
    /// 効果：自分が受けているデバフをすべて解除する。状態異常無効を1つ得る。
    /// </summary>
    private void CardID9()
    {
        //全てのバッドステータスを解除
        PlayerReleaseBadStatus();
        //状態異常無効を1付与
        player.AddConditionStatus("InvalidBadStatus", 1);
    }
    /// <summary>
    /// 技名：強化浄化の風
    /// 効果：自分が受けているデバフをすべて解除する。状態異常無効を2つ得る。
    /// </summary>
    private void CardID109()
    {
        //全てのバッドステータスを解除
        PlayerReleaseBadStatus();
        //状態異常無効を2付与
        player.AddConditionStatus("InvalidBadStatus", 2);
    }
    /// <summary>
    /// 技名：クリアヴェール,強化クリアヴェール
    /// 効果：ガードを5得る。状態異常無効を3つ得る。
    /// </summary>
    private void CardID10()
    {
        //ガードを追加
        PlayerAddGP(cardGuardPoint);
        //状態異常無効を3付与
        player.AddConditionStatus("InvalidBadStatus", 3);
    }
    /// <summary>
    /// 技名：アクセラレート
    /// 効果：相手に1ダメージを与える。このゲーム中、このコマンドのダメージが1増える。
    /// </summary>
    private void CardID11()
    {
        //BattleGameManagerのアクセラレート機能をON
        bg.isAccelerate = true;
        bg.accelerateValue = 1;//カードのコストを1下げる
    }
    /// <summary>
    /// 技名：強化アクセラレート
    /// 効果：相手に2ダメージを与える。このゲーム中、このコマンドのダメージが1増える。
    /// </summary>
    private void CardID111()
    {
        //BattleGameManagerのアクセラレート機能をON
        bg.isAccelerate = true;
        bg.accelerateValue = 2;//カードのコストを2下げる
    }
    /// <summary>
    /// 技名：乱れ鞭
    /// 効果：2ダメージを4回与える。
    /// </summary>
    private void CardID12()
    {
        //時間差でエネミーを攻撃
        StartCoroutine(ID12Attacking(cardAttackPower, 0.2f));
    }
    IEnumerator ID12Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        for (int count = 0; count < 3; count++)
        {
            yield return new WaitForSeconds(attackInterval);
            PlayerAttacking(attackMethod);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：強化乱れ鞭
    /// 効果：2ダメージを5回与える。
    /// </summary>
    private void CardID112()
    {
        //時間差でエネミーを攻撃
        StartCoroutine(ID112Attacking(cardAttackPower, 0.2f));
    }
    IEnumerator ID112Attacking(int attackMethod, float attackInterval)
    {
        bg.isCoroutine = true;
        PlayerAttacking(attackMethod);
        for (int count = 0; count < 4; count++)
        {
            yield return new WaitForSeconds(attackInterval);
            PlayerAttacking(attackMethod);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：フルバースト
    /// 効果：APをすべて消費し、消費した分の2倍ダメージを与える。
    /// </summary>
    private void CardID13(CardController card)
    {
        //APを全消費
        //現在のAP分の2倍のダメージを計算
        int damage = player.GetSetCurrentAP * 2;
        player.GetSetCurrentAP = 0;
        //エネミーを攻撃
        PlayerAttacking(damage);
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// 技名：強化フルバースト
    /// 効果：APをすべて消費し、消費した分の3倍ダメージを与える。
    /// </summary>
    private void CardID113(CardController card)
    {
        //APを全消費
        //現在のAP分の3倍のダメージを計算
        int damage = player.GetSetCurrentAP * 3;
        player.GetSetCurrentAP = 0;
        //エネミーを攻撃
        PlayerAttacking(damage);
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// 技名：ハイボルケーノ 
    /// 効果：9ダメージを与え、相手に火傷を2つ与える。
    /// </summary>
    private void CardID14()
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //火傷を2付与
        enemy.AddConditionStatus("Burn", 2);
    }
    /// <summary>
    /// 技名：強化ハイボルケーノ 
    /// 効果：9ダメージを与え、相手に火傷を4つ与える。
    /// </summary>
    private void CardID114()
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //火傷を4付与
        enemy.AddConditionStatus("Burn", 4);
    }
    /// <summary>
    /// 技名：デビルドレイン,強化デビルドレイン
    /// 効果：4ダメージを与え、自分の体力を4回復する。相手に衰弱を1つ与える。
    /// </summary>
    private void CardID15(CardController card)
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //HPを回復
        PlayerHealing(cardHealingPower);
        //エネミーに衰弱を1付与
        enemy.AddConditionStatus("Weakness", 1);
        //このラウンド中カードを使用不可にする
        card.cardDataManager._cardState = 1;
    }
    /// <summary>
    /// 技名：ガラティーン,強化ガラティーン
    /// 効果：5(7)ダメージを与える。相手が火傷を持つならかわりに3倍のダメージを与える。
    /// </summary>
    private void CardID16()
    {
        if (enemy.GetSetCondition.burn > 0)//エネミーが火傷状態だった場合
        {
            //火傷の3倍のダメージでエネミーを攻撃
            PlayerAttacking(enemy.GetSetCondition.burn * 3);
        }
        else
        {
            //エネミーを攻撃
            PlayerAttacking(cardAttackPower);
        }
    }
    /// <summary>
    /// 技名：雷霆の衝撃,強化雷霆の衝撃
    /// 効果：5(10)ダメージを与え、相手をこのラウンド中行動不能状態にする。
    /// </summary>
    private void CardID17(CardController card)
    {
        //エネミーを攻撃
        PlayerAttacking(cardAttackPower);
        //エネミーを行動不能にする
        enemy.TurnEnd();
        //この戦闘中カードを使用不可にする
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// 技名：エクスカリバー,強化エクスカリバー
    /// 効果：10(12)ダメージを与える。これが3ラウンド目以降ならAPを7回復する。
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
        bg.isCoroutine = true;
        yield return new WaitForSeconds(attackInterval);
        //プレイヤーのAPを7回復する
        player.HealingAP(7);
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：アヴァロンヴェール,強化アヴァロンヴェール
    /// 効果：自然治癒を1得る。状態異常無効を2つ得る。
    /// </summary>
    private void CardID19(CardController card)
    {
        //自動回復を1付与
        player.AddConditionStatus("AutoHealing", 1);
        //状態異常無効を2付与
        player.AddConditionStatus("InvalidBadStatus", 2);
        //この戦闘中カードを使用不可にする
        card.cardDataManager._cardState = 2;
    }
    /// <summary>
    /// 技名：鬼火
    /// 効果：相手に火傷を1つ与える
    /// </summary>
    private void CardID20()
    {
        //火傷を1付与
        enemy.AddConditionStatus("Burn", 1);
    }
    /// <summary>
    /// 技名：強化鬼火
    /// 効果：相手に火傷を2つ与える
    /// </summary>
    private void CardID120()
    {
        //火傷を1付与
        enemy.AddConditionStatus("Burn", 2);
    }
    private void PlayerAttacking(int attackMethod)//エネミーへの攻撃処理 
    {
        attackMethod = ChangeAttackPower(attackMethod);
        Debug.Log("計算後の攻撃力は" + attackMethod);
        enemy.TakeDamage(attackMethod);
    }
    private int ChangeAttackPower(int attackPower) //状態異常による攻撃力の増減
    {
        attackPower = player.UpStrength(attackPower);
        attackPower = player.Weakness(attackPower);
        return attackPower;
    }
    private void PlayerHealing(int healingPower)//プレイヤーのHP回復処理
    {
        player.HealingHP(healingPower);
    }
    private void PlayerAddGP(int addGP)//プレイヤーにガードを追加
    {
        player.AddGP(addGP);
    }
    private void PlayerReleaseBadStatus()//プレイヤーに付与されたデバフの解除
    {
        player.ReleaseBadStatus();
    }
}
