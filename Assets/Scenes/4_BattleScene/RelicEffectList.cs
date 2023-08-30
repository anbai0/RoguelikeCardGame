using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レリックの効果をまとめたスクリプト
/// </summary>
public class RelicEffectList : MonoBehaviour
{
    int id5Count = 0;
    void Start()
    {
        id5Count = 0;
    }
    /// <summary>
    /// 名前:アリアドネの糸
    /// 効果:デッキの上限を1枚増やす。
    /// </summary>
    /// <param name="ID1Quantity"></param>
    public void RelicID1(int ID1Quantity)
    {
        //このスクリプトで処理する効果はなし
    }

    /// <summary>
    ///  名前:諸刃の剣
    ///  効果:与えるダメージと受けるダメージを1増やす。
    /// </summary>
    /// <param name="ID2Quantity">レリック番号02の個数</param>
    public void RelicID2(int ID2Quantity)
    {
        var bg = BattleGameManager.Instance;
        
        if (ID2Quantity <= 0) //レリックが1個もない場合
        {
            Debug.Log("player:" + bg.relicID2Player + "enemy" + bg.relicID2Enemy);
            //リターンで返す
            return;
        }
        else
        {
            bg.relicID2Player += 1 * ID2Quantity; //プレイヤーの攻撃力の増加量はレリックの個数分
            bg.relicID2Enemy += 1 * ID2Quantity; //エネミーの攻撃力の増加量はレリックの個数分
        }
        Debug.Log("player:"+ bg.relicID2Player + "enemy"+ bg.relicID2Enemy);
    }

    /// <summary>
    /// 名前:虚栄の冠
    /// 効果:最大APを2増やすが、ラウンド終了時に増えるAPの上昇値が1少なくなる。(最大APが減ることはない)
    /// </summary>
    /// <param name="ID3Quantity">レリック番号03の個数</param>
    /// <param name="constAP">APの初期値</param>
    /// <param name="chargeAP">APの上昇値</param>
    /// <returns>増加したAPの初期値,減少したAPの上昇値</returns>
    public (int constAP, int chargeAP) RelicID3(int ID3Quantity, int constAP, int chargeAP)
    {
        constAP += 2 * ID3Quantity;
        chargeAP -= ID3Quantity;
        if (chargeAP < 0)
        {
            chargeAP = 0;
        }
        return (constAP, chargeAP);
    }

    /// <summary>
    /// 名前:神秘の髪飾り
    /// 効果:最大APを1増やす。
    /// </summary>
    /// <param name="ID4Quantity">レリック番号04の個数</param>
    /// <param name="constAP">APの初期値</param>
    /// <returns>増加したAPの初期値</returns>
    public int RelicID4(int ID4Quantity, int constAP)
    {
        constAP += ID4Quantity;
        return constAP;
    }

    /// <summary>
    /// 名前:千里眼鏡
    /// 効果:最大APを1減らすが、ラウンド終了時、APの上昇値をさらに1増やす。レリック1つにつき最大5回まで。
    /// </summary>
    /// <param name="ID5Quantity">レリック番号05の個数</param>
    /// <param name="constAP">APの初期値</param>
    /// <param name="chargeAP">APの上昇値</param>
    /// <returns>減少したAPの初期値,増加したAPの上昇値</returns>
    public (int constAP, int chargeAP) RelicID5(int ID5Quantity, int constAP, int chargeAP)
    {
        if(id5Count >= 5)
        {
            return (constAP, chargeAP);
        }
        constAP -= ID5Quantity;
        if(constAP < 0)
        {
            constAP = 0;
        }
        chargeAP += ID5Quantity;
        id5Count++;
        return (constAP, chargeAP);
    }

    /// <summary>
    /// 名前:太陽のお守り
    /// 効果:戦闘開始時、相手に火傷を1つ付与する。
    /// </summary>
    /// <param name="ID6Quantity">レリック番号06の個数</param>
    /// <returns>加算する火傷の値</returns>
    public int RelicID6(int ID6Quantity)
    {
        int addBurn = 0;

        if (ID6Quantity <= 0)
        {
            return addBurn;
        }
        else
        {
            addBurn += ID6Quantity;
        }
        return addBurn;
    }

    /// <summary>
    /// 名前:心の器
    /// 効果:最大HPを5増やす。現在HPは変化しない。
    /// </summary>
    /// <param name="ID7Quantity">レリック番号07の個数</param>
    /// <param name="HP">最大HP</param>
    /// <returns>増加した最大HP</returns>
    public int RelicID7(int ID7Quantity, int HP)
    {
        HP += 5 * ID7Quantity;
        return HP;
    }

    /// <summary>
    /// 名前: 真円のお守り
    /// 効果:戦闘開始時にガードを3獲得する。
    /// </summary>
    /// <param name="ID8Quantity">レリック番号08の個数</param>
    /// <param name="GP">ガードポイント</param>
    /// <returns>増加したガードポイント</returns>
    public int RelicID8(int ID8Quantity, int GP)
    {
        GP += 3 * ID8Quantity;
        return GP;
    }

    /// <summary>
    /// 名前:ご褒美袋
    /// 効果:戦闘終了後に獲得するゴールドを10増やす
    /// </summary>
    /// <param name="ID9Quantity">レリック番号09の個数</param>
    /// <returns>増加したゴールド</returns>
    public int RelicID9(int ID9Quantity)
    {
        int money = 0;
        money += 10 * ID9Quantity;
        return money;
    }

    /// <summary>
    /// 名前:ほかほかおにぎり
    /// 効果:戦闘終了時に自分の現在のHPを5回復する。
    /// </summary>
    /// <param name="ID10Quantity">レリック番号10の個数</param>
    /// <returns>戦闘終了時に回復する量</returns>
    public int RelicID10(int ID10Quantity)
    {
        int healingPower = 0;
        healingPower += 5 * ID10Quantity;
        return healingPower;
    }

    /// <summary>
    /// 名前:断ち切り鋏
    /// 効果:ラウンド終了時にデバフを1つ解除する。
    /// </summary>
    /// <param name="ID11Quantity">レリック番号11の個数</param>
    /// <param name="condition">状態異常のステータス</param>
    /// <returns>ランダムに減少したバッドステータス</returns>
    public Dictionary<string, int> RelicID11(int ID11Quantity, Dictionary<string, int> condition)
    {
        //バッドステータスをリストに追加
        List<int> badStatus = new List<int> { condition["Curse"], condition["Impatience"], condition["Weakness"], condition["Burn"], condition["Poison"] };
        //解除できる数がバッドステータスの数より多い場合はバッドステータスの数だけ解除
        int totalBadStatus = condition["Curse"] + condition["Impatience"] + condition["Weakness"] + condition["Burn"] + condition["Poison"];
        if (totalBadStatus < ID11Quantity)
        {
            ID11Quantity = totalBadStatus;
        }

        //ID11の個数分ランダムな数字を選んでその数のList番目に入っている数が0以上なら-1する
        for (int i = 0; i < ID11Quantity; i++)
        {
            int chooseNumber = Random.Range(0, badStatus.Count - 1);
            while (badStatus[chooseNumber] == 0)
            {
                chooseNumber = Random.Range(0, badStatus.Count - 1);
            }
            badStatus[chooseNumber] -= 1;
        }
        //減少後の数値を代入する
        condition["Curse"] = badStatus[0];
        condition["Impatience"] = badStatus[1];
        condition["Weakness"] = badStatus[2];
        condition["Burn"] = badStatus[3];
        condition["Poison"] = badStatus[4];
        return condition;
    }

    /// <summary>
    /// 名前:銀の火薬
    /// 効果:ボスとの戦闘時、与えるダメージが1増加する。
    /// </summary>
    /// <param name="ID12Quantity">レリック番号12の個数</param>
    /// <param name="type">エネミーの種類</param>
    /// <returns>加算する筋力増強の値</returns>
    public int RelicID12(int ID12Quantity, string type)
    {
        int addPlayerUpStrength = 0;

        if (ID12Quantity > 0 && type == "Boss")
        {
            addPlayerUpStrength += 1 * ID12Quantity;
        }
        else
        {
            return addPlayerUpStrength;
        }

        return addPlayerUpStrength;
    }
}
