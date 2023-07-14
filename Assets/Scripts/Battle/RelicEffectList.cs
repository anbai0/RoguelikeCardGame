using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicEffectList : MonoBehaviour
{
    /// <summary>
    /// 名前:アリアドネの糸
    /// 効果:デッキの上限を1枚増やす。
    /// </summary>
    /// <param name="ID1Quantity"></param>
    public void RelicID1(int ID1Quantity) 
    {
    }
    /// <summary>
    ///  名前:諸刃の剣
    ///  効果:与えるダメージと受けるダメージを1増やす。
    /// </summary>
    /// <param name="ID2Quantity">レリック番号02の個数</param>
    /// <param name="playerUpStrength">プレイヤーの筋力増強</param>
    /// <param name="enemyUpStrength">エネミーの筋力増強</param>
    /// <returns>増加したプレイヤーの筋力増強,増強したエネミーの筋力増強</returns>
    public (int playerUpStrength, int enemyUpStrength) RelicID2(int ID2Quantity,int playerUpStrength,int enemyUpStrength) 
    {
        if (ID2Quantity <= 0)
        {
            return (playerUpStrength, enemyUpStrength);
        }
        else 
        {
            playerUpStrength += 1 * ID2Quantity;
            enemyUpStrength += 1 * ID2Quantity;
        }
        return (playerUpStrength, enemyUpStrength);
    }
    /// <summary>
    /// 名前:虚栄の冠
    /// 効果:最大APを2増やすが、ラウンド終了時に増えるAPの上昇値が1少なくなる。(最大APが減ることはない)
    /// </summary>
    /// <param name="ID3Quantity">レリック番号03の個数</param>
    /// <param name="playerConstAP">APの初期値</param>
    /// <param name="playerChargeAP">APの上昇値</param>
    /// <returns>増加したAPの初期値,減少したAPの上昇値</returns>
    public (int playerConstAP, int playerChargeAP) RelicID3(int ID3Quantity, int playerConstAP,int playerChargeAP)
    {
        playerConstAP += 2 * ID3Quantity;
        playerChargeAP -= ID3Quantity;
        if (playerChargeAP < 0) 
        {
            playerChargeAP = 0;
        }
        return (playerConstAP, playerChargeAP);
    }
    /// <summary>
    /// 名前:神秘の髪飾り
    /// 効果:最大APを1増やす。
    /// </summary>
    /// <param name="ID4Quantity">レリック番号04の個数</param>
    /// <param name="playerConstAP">APの初期値</param>
    /// <returns>増加したAPの初期値</returns>
    public int RelicID4(int ID4Quantity, int playerConstAP)
    {
        playerConstAP += ID4Quantity;
        return playerConstAP;
    }
    /// <summary>
    /// 名前:千里眼鏡
    /// 効果:最大APを1減らすが、ラウンド終了時、APの上昇値をさらに1増やす。レリック1つにつき最大5まで。
    /// </summary>
    /// <param name="ID5Quantity">レリック番号05の個数</param>
    /// <param name="playerConstAP">APの初期値</param>
    /// <param name="playerChargeAP">APの上昇値</param>
    /// <returns>減少したAPの初期値,増加したAPの上昇値</returns>
    public (int playerConstAP, int playerChargeAP) RelicID5(int ID5Quantity, int playerConstAP, int playerChargeAP)
    {
        if (ID5Quantity > 5)
        {
            playerConstAP = 5;
            playerChargeAP = 5;
        }
        playerConstAP -= ID5Quantity;
        playerChargeAP += ID5Quantity;
        return (playerConstAP, playerChargeAP);
    }
    /// <summary>
    /// 名前:太陽のお守り
    /// 効果:戦闘開始時、相手に火傷を1つ付与する。
    /// </summary>
    /// <param name="ID6Quantity">レリック番号06の個数</param>
    /// <param name="enemyBurn">エネミーの火傷</param>
    /// <returns>増加したエネミーの火傷</returns>
    public int RelicID6(int ID6Quantity, int enemyBurn)
    {
        if (ID6Quantity <= 0)
        {
            return enemyBurn;
        }
        else
        {
            enemyBurn += ID6Quantity;
        }
        return enemyBurn;
    }
    /// <summary>
    /// 名前:心の器
    /// 効果:最大HPを5増やす。現在HPは変化しない。
    /// </summary>
    /// <param name="ID7Quantity">レリック番号07の個数</param>
    /// <param name="playerHP">最大HP</param>
    /// <returns>増加した最大HP</returns>
    public int RelicID7(int ID7Quantity, int playerHP)
    {
        playerHP += 5 * ID7Quantity;
        return playerHP;
    }
    /// <summary>
    /// 名前: 真円のお守り
    /// 効果:戦闘開始時にガードを3獲得する。
    /// </summary>
    /// <param name="ID8Quantity">レリック番号08の個数</param>
    /// <param name="playerGP">ガードポイント</param>
    /// <returns>増加したガードポイント</returns>
    public int RelicID8(int ID8Quantity, int playerGP)
    {
        playerGP += 3 * ID8Quantity;
        return playerGP;
    }
    /// <summary>
    /// 名前:ご褒美袋
    /// 効果:戦闘終了後に獲得するゴールドを10増やす
    /// </summary>
    /// <param name="ID9Quantity">レリック番号09の個数</param>
    /// <param name="money">エネミーの持つゴールド</param>
    /// <returns>増加したゴールド</returns>
    public int RelicID9(int ID9Quantity, int money)
    {
        money += 10 * ID9Quantity;
        return money;
    }
    /// <summary>
    /// 名前:ほかほかおにぎり
    /// 効果:戦闘終了時に自分のHPを5回復する。
    /// </summary>
    /// <param name="ID10Quantity">レリック番号10の個数</param>
    /// <param name="playerCurrentHP">現在のHP</param>
    /// <returns>回復した現在のHP</returns>
    public int RelicID10(int ID10Quantity, int playerCurrentHP)
    {
        playerCurrentHP += 5 * ID10Quantity;
        return playerCurrentHP;
    }
    /// <summary>
    /// 名前:断ち切り鋏
    /// 効果:ラウンド終了時にデバフを1つ解除する。
    /// </summary>
    /// <param name="ID11Quantity">レリック番号11の個数</param>
    /// <param name="curse">呪縛</param>
    /// <param name="impatience">焦燥</param>
    /// <param name="weakness">衰弱</param>
    /// <param name="burn">火傷</param>
    /// <param name="poison">邪毒</param>
    /// <returns>ランダムに減少したバッドステータス</returns>
    public (int curse, int impatience, int weakness, int burn, int poison) RelicID11(int ID11Quantity, int curse, int impatience, int weakness, int burn, int poison)
    {
        //バッドステータスをリストに追加
        List<int> badStatus = new List<int> { curse, impatience, weakness, burn, poison };
        Debug.Log("Relicの処理によるBurnの数：変更前：" + badStatus[3]);
        //解除できる数がバッドステータスの数より多い場合はバッドステータスの数だけ解除
        int totalBadStatus = curse + impatience + weakness + burn + poison;
        if (totalBadStatus < ID11Quantity) 
        {
            ID11Quantity = totalBadStatus;
        }

        //ID11の個数分ランダムな数字を選んでその数のList番目に入っている数が0以上なら-1する
        for (int i = 0; i < ID11Quantity; i++)
        {
            int chooseNumber = Random.Range(0, badStatus.Count-1);
            while (badStatus[chooseNumber] == 0)
            {
                chooseNumber = Random.Range(0, badStatus.Count-1);
            }
            badStatus[chooseNumber] -= 1;
        }
        Debug.Log("Relicの処理によるBurnの数：変更後：" + badStatus[3]);
        //減少後の数値を代入する
        curse = badStatus[0];
        impatience = badStatus[1];
        weakness = badStatus[2];
        burn = badStatus[3];
        poison = badStatus[4];
        return (curse, impatience, weakness, burn, poison);
    }
    /// <summary>
    /// 名前:銀の火薬
    /// 効果:ボスとの戦闘時、与えるダメージが1増加する。
    /// </summary>
    /// <param name="ID12Quantity">レリック番号12の個数</param>
    /// <param name="tag">エネミーのタグ</param>
    public int RelicID12(int ID12Quantity,string tag, int playerUpStrength)
    {
        if (ID12Quantity <= 0 || !(tag == "Boss"))
        {
            return playerUpStrength;
        }
        else 
        {
            playerUpStrength += 1 * ID12Quantity;
        }
        return playerUpStrength;
    }
}
