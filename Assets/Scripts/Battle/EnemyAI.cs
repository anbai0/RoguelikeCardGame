using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove //エネミーの行動クラス
{
    public string moveName;
    public bool isUsable;
    public int moveCost;

    public EnemyMove(string name, bool usable, int cost)
    {
        moveName = name;
        isUsable = usable;
        moveCost = cost;
    }
}
public class EnemyAI : MonoBehaviour
{
    BattleGameManager bg;
    PlayerBattleAction player;
    EnemyBattleAction enemy;

    //使用制限のある技の変数
    int snakeGlaresCount; //蛇睨みの回数
    bool isUsableGodCrusher; //神砕きを使用出来るか判定
    //Debug用変数
    int floor = 1;
    private enum EnemyState //エネミーの種別
    {
        SLIME1,                    //スライム(1階層目)
        SKELETONSWORDSMAN1,        //骸骨剣士(1階層目)
        NAGA1,                     //ナーガ(1階層目)
        CHIMERA1,                  //キマイラ(1階層目)
        DARKKNIGHT1,               //暗黒騎士(1階層目)
        CYCLOPS,                   //サイクロプス(1階層目)
        SLIME2,                    //スライム(2階層目)
        SKELETONSWORDSMAN2,        //骸骨剣士(2階層目)
        NAGA2,                     //ナーガ(2階層目)
        CHIMERA2,                  //キマイラ(2階層目)
        DARKKNIGHT2,               //暗黒騎士(2階層目)
        SCYLLA,                    //スキュラ(2階層目) 
    }
    EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<BattleGameManager>();
        player = GetComponent<PlayerBattleAction>();
        enemy = GetComponent<EnemyBattleAction>();
        snakeGlaresCount = 0;
        isUsableGodCrusher = false;
    }
    public (string moveName, int moveCost) SelectMove(int currentAP)
    {
        string moveName = null; //選択された技の名前
        int moveCost = 0; //選択されたコストの名前
        List<EnemyMove> enemyMove; //エネミーの技リスト

        enemyMove = SetMoveList(); //技をセットする
        enemyMove = APCheck(enemyMove, currentAP); //現在のAPに応じて使える技を選出
        enemyMove = CheckEnemyMove(enemyMove); //エネミーの条件に応じて使える技を選出
        var selectMove = SelectMove(enemyMove); //使用できる技の中からランダムで選択する
        moveName = selectMove.name; //選択された技の名前を代入
        moveCost = selectMove.cost; //選択された技のコストを代入
        return (moveName, moveCost);
    }
    public void SetEnemyState(string enemyName) //エネミーの名前と階層数に応じてEnemyAIのステートを変更する
    {
        //floor = GameManager.instance.GetFloor;
        if (enemyName == "スライム" && floor == 1)
        {
            enemyState = EnemyState.SLIME1;
        }
        else if (enemyName == "スライム" && floor == 2)
        {
            enemyState = EnemyState.SLIME2;
        }
        else if (enemyName == "骸骨剣士" && floor == 1)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN1;
        }
        else if (enemyName == "骸骨剣士" && floor == 2)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN2;
        }
        else if (enemyName == "ナーガ" && floor == 1)
        {
            enemyState = EnemyState.NAGA1;
        }
        else if (enemyName == "ナーガ" && floor == 2)
        {
            enemyState = EnemyState.NAGA2;
        }
        else if (enemyName == "キメラ" && floor == 1)
        {
            enemyState = EnemyState.CHIMERA1;
        }
        else if (enemyName == "キメラ" && floor == 2)
        {
            enemyState = EnemyState.CHIMERA2;
        }
        else if (enemyName == "暗黒騎士" && floor == 1)
        {
            enemyState = EnemyState.DARKKNIGHT1;
        }
        else if (enemyName == "暗黒騎士" && floor == 2)
        {
            enemyState = EnemyState.DARKKNIGHT2;
        }
        else if (enemyName == "サイクロプス" && floor == 1)
        {
            enemyState = EnemyState.CYCLOPS;
        }
        else if (enemyName == "スキュラ" && floor == 2)
        {
            enemyState = EnemyState.SCYLLA;
        }

    }
    List<EnemyMove> SetMoveList() //エネミーの技をリストとしてセットする
    {
        List<EnemyMove> enemyMove = new List<EnemyMove>();
        if (enemyState == EnemyState.SLIME1)
        {
            enemyMove.Add(new EnemyMove("Charge", false, 2)); //たいあたり
            enemyMove.Add(new EnemyMove("Solution", false, 3)); //溶解液
        }
        else if (enemyState == EnemyState.SLIME2)
        {
            enemyMove.Add(new EnemyMove("Charge", false, 2)); //たいあたり
            enemyMove.Add(new EnemyMove("Solution", false, 3)); //溶解液
            enemyMove.Add(new EnemyMove("Curing", false, 2)); //硬化
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //切りかかる
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //盾を構える
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //切りかかる
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //盾を構える
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//暴れ回る
        }
        else if (enemyState == EnemyState.NAGA1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //切りかかる
            enemyMove.Add(new EnemyMove("CreepySong", false, 3)); //不気味な歌
        }
        else if (enemyState == EnemyState.NAGA2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //切りかかる
            enemyMove.Add(new EnemyMove("CreepySong", false, 3)); //不気味な歌
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //蛇牙
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //噛みつき
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//暴れ回る
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4));//燃える息
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2));//蛇睨み
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //噛みつき
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//暴れ回る
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4));//燃える息
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2));//蛇睨み
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //蛇牙
        }
        else if (enemyState == EnemyState.DARKKNIGHT1)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3));//振り下ろす
            enemyMove.Add(new EnemyMove("RobustShield", false, 3));//堅牢なる盾
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3));//捨て身突進
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//暴れ回る
        }
        else if (enemyState == EnemyState.DARKKNIGHT2)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3));//振り下ろす
            enemyMove.Add(new EnemyMove("RobustShield", false, 3));//堅牢なる盾
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3));//捨て身突進
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//暴れ回る
            enemyMove.Add(new EnemyMove("Encouragement", false, 4));//鬨
        }
        else if (enemyState == EnemyState.CYCLOPS)
        {
            enemyMove.Add(new EnemyMove("SwingOver", false, 6));//振りかぶる
            enemyMove.Add(new EnemyMove("GodCrusher", false, 0));//神砕き
            enemyMove.Add(new EnemyMove("RandomPounding", false, 5));//乱れ打ち
            enemyMove.Add(new EnemyMove("GiantFist", false, 2));//巨拳
            enemyMove.Add(new EnemyMove("Rumbling", false, 4));//じならし
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            enemyMove.Add(new EnemyMove("SwingOver", false, 6));//振りかぶる
            enemyMove.Add(new EnemyMove("GodCrusher", false, 0));//神砕き
            enemyMove.Add(new EnemyMove("RandomPounding", false, 5));//乱れ打ち
            enemyMove.Add(new EnemyMove("WrigglingTentacles", false, 4));//蠢く触手
            enemyMove.Add(new EnemyMove("Rumbling", false, 4));//じならし
        }
        return enemyMove;
    }
    List<EnemyMove> CheckEnemyMove(List<EnemyMove> enemyMove) //エネミーの技が使用出来るかチェックする
    {
        if (enemyState == EnemyState.SLIME1)
        {
            if (player.GetSetCondition.weakness > 0) //プレイヤーに衰弱が付与されているなら
            {
                enemyMove[1].isUsable = false; //溶解液を使用不可に
            }
        }
        else if (enemyState == EnemyState.SLIME2)
        {
            if (player.GetSetCondition.weakness > 0) //プレイヤーに衰弱が付与されてるなら
            {
                enemyMove[1].isUsable = false; //溶解液を使用不可に
            }
            if (enemy.GetSetGP > 0) //エネミーにGPがあるなら
            {
                enemyMove[2].isUsable = false; //硬化を使用不可に
            }
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1)
        {
            if (enemy.GetSetGP > 0) //エネミーにGPがあるなら
            {
                enemyMove[1].isUsable = false; //盾を構えるを使用不可に
            }
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN2)
        {
            if (enemy.GetSetGP > 0) //エネミーにGPがあるなら
            {
                enemyMove[1].isUsable = false; //盾を構えるを使用不可に
            }
        }
        else if (enemyState == EnemyState.NAGA1)
        {
            if (player.GetSetCondition.impatience > 0) //プレイヤーに焦燥が付与されているなら
            {
                enemyMove[1].isUsable = false;//不気味な歌を使用不可に
            }
        }
        else if (enemyState == EnemyState.NAGA2)
        {
            if (player.GetSetCondition.impatience > 0) //プレイヤーに焦燥が付与されているなら
            {
                enemyMove[1].isUsable = false;//不気味な歌を使用不可に
            }
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            if (snakeGlaresCount > 0) //戦闘で一度でも蛇睨みを使用しているなら
            {
                enemyMove[3].isUsable = false; //蛇睨みを使用不可に
            }
            else //蛇睨みを使用していないなら
            {
                //蛇睨み以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                snakeGlaresCount++; //蛇睨みの使用回数をカウントする
            }
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            if (snakeGlaresCount > 0) //戦闘で一度でも蛇睨みを使用しているなら
            {
                enemyMove[3].isUsable = false; //蛇睨みを使用不可に
            }
            else //蛇睨みを使用していないなら
            {
                //蛇睨み以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[4].isUsable = false;
                snakeGlaresCount++; //蛇睨みの使用回数をカウントする
            }
        }
        else if (enemyState == EnemyState.DARKKNIGHT1)
        {
            if (enemy.GetSetRoundEnabled == true) //ラウンド中に堅牢なる盾を使用しているなら
            {
                enemyMove[1].isUsable = false; //堅牢なる盾を使用不可に
            }
            else //ラウンド中に堅牢なる盾を使用していないなら
            {
                //堅牢なる盾以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemy.GetSetRoundEnabled = true; //ラウンド中は堅牢なる盾を使用出来なくする
            }

            if (enemy.GetSetCurrentHP < 15) //エネミーの現在のHPが15未満なら
            {
                enemyMove[2].isUsable = false; //捨て身突進を使用不可に
            }
        }
        else if (enemyState == EnemyState.DARKKNIGHT2)
        {
            if (enemy.GetSetRoundEnabled == true) //ラウンド中に堅牢なる盾を使用しているなら
            {
                enemyMove[1].isUsable = false; //堅牢なる盾を使用不可に
            }
            else //ラウンド中に堅牢なる盾を使用していないなら
            {
                //堅牢なる盾以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                enemy.GetSetRoundEnabled = true; //ラウンド中は堅牢なる盾を使用出来なくする
            }

            if (enemy.GetSetCurrentHP < 15) //エネミーの現在のHPが15未満なら
            {
                enemyMove[2].isUsable = false; //捨て身突進を使用不可に
            }

            if (enemy.GetSetCondition.upStrength > 0) //エネミーに筋力増強が付与されているなら 
            {
                enemyMove[4].isUsable = false; //鬨を使用不可に
            }
        }
        else if (enemyState == EnemyState.CYCLOPS)
        {
            if (isUsableGodCrusher == true) //神砕きが使用可能なら
            {
                //神砕き以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                isUsableGodCrusher = false; //神砕きの使用判定を戻す
            }
            else
            {
                enemyMove[1].isUsable = false; //神砕きを使用不可に
            }

            if (player.CheckBadStatus() == 0 && enemy.CheckBadStatus() == 0) //プレイヤーとエネミーにバフやデバフが付与されていないなら
            {
                enemyMove[4].isUsable = false; //じならしを使用不可に
            }
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            if (isUsableGodCrusher == true) //神砕きが使用可能なら
            {
                //神砕き以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                isUsableGodCrusher = false; //神砕きの使用判定を戻す
            }
            else
            {
                enemyMove[1].isUsable = false; //神砕きを使用不可に
            }

            if (player.CheckBadStatus() == 0 && enemy.CheckBadStatus() == 0) //プレイヤーとエネミーにバフやデバフが付与されていないなら
            {
                enemyMove[4].isUsable = false; //じならしを使用不可に
            }
        }

        return enemyMove;
    }
    List<EnemyMove> APCheck(List<EnemyMove> enemyMove, int currentAP) //エネミーの技がAP以下かチェックする
    {
        foreach (var move in enemyMove) //エネミーの技を全探索 
        {
            if (move.moveCost <= currentAP) //コストが現在のAP以下であれば
            {
                move.isUsable = true; //使用可能にする
            }
        }
        return enemyMove;
    }
    (string name, int cost) SelectMove(List<EnemyMove> enemyMove) //使用する技を選ぶ
    {
        List<(string, int)> moveUsabledList = new List<(string, int)>(); //技の名前とコストを格納できるリスト
        foreach (var move in enemyMove) //使用可能な技をmoveUsabledListに追加する
        {
            if (move.isUsable == true)
            {
                moveUsabledList.Add((move.moveName, move.moveCost));
            }
        }
        if (moveUsabledList.Count == 0)
        {
            return ("RoundEnd", 0);
        }
        //ランダムに技を選ぶ
        int rand = Random.Range(0, moveUsabledList.Count);

        if (moveUsabledList[rand].Item1 == "SwingOver") //サイクロプス専用の神砕き使用判定
        {
            isUsableGodCrusher = true;
        }
        return moveUsabledList[rand];
    }
    /// <summary>
    /// 技の効果処理
    /// </summary>
    /// <param name="moveName">技の名前</param>
    public void ActionMove(string moveName)
    {
        switch (moveName)
        {
            case "RoundEnd":
                RoundEnd();
                break;
            case "Charge":
                Charge();
                break;
            case "Solution":
                Solution();
                break;
            case "Curing":
                Curing();
                break;
            case "Slash":
                Slash();
                break;
            case "HoldShield":
                HoldShield();
                break;
            case "CreepySong":
                CreepySong();
                break;
            case "SnakeFangs":
                SnakeFangs();
                break;
            case "Bite":
                Bite();
                break;
            case "Rampage":
                Rampage();
                break;
            case "BurningBreath":
                BurningBreath();
                break;
            case "SnakeGlares":
                SnakeGlares();
                break;
            case "Swing":
                Swing();
                break;
            case "RobustShield":
                RobustShield();
                break;
            case "DesperateLunge":
                DesperateLunge();
                break;
            case "Encouragement":
                Encouragement();
                break;
            case "SwingOver":
                SwingOver();
                break;
            case "GodCrusher":
                GodCrusher();
                break;
            case "RandomPounding":
                RandomPounding();
                break;
            case "GiantFist":
                GiantFist();
                break;
            case "Rumbling":
                Rumbling();
                break;
            case "WrigglingTentacles":
                WrigglingTentacles();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    /// <summary>
    ///行動せずにラウンドを終了する
    /////エネミーの現在のAPを0にする
    /// </summary>
    private void RoundEnd()
    {
        enemy.TurnEnd();
    }
    /// <summary>
    /// スライム:体当たり
    ///プレイヤーに2ダメージ与える
    /// </summary>
    private void Charge()
    {
        player.TakeDamage(2);
    }
    /// <summary>
    /// スライム:溶解液
    /// プレイヤーに衰弱を1つ与える
    /// </summary>
    private void Solution()
    {
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// スライム:硬化
    /// 4ガードを得る。状態異常無効を2得る。
    /// </summary>
    private void Curing()
    {
        enemy.AddGP(4);
        enemy.AddConditionStatus("InvalidBadStatus", 2);
    }
    /// <summary>
    /// 骸骨剣士とナーガ:切りかかる
    /// プレイヤーに4ダメージ与える
    /// </summary>
    private void Slash()
    {
        player.TakeDamage(4);
    }
    /// <summary>
    /// 骸骨剣士:盾を構える
    /// ガードを2得る
    /// </summary>
    private void HoldShield()
    {
        enemy.AddGP(2);
    }
    /// <summary>
    /// ナーガ:不気味な歌
    /// プレイヤーに焦燥を１つ与える
    /// </summary>
    private void CreepySong()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// ナーガとキマイラ:蛇牙
    /// プレイヤーに3ダメージ与え、邪毒を1与える。
    /// </summary>
    private void SnakeFangs()
    {
        player.TakeDamage(3);
        player.AddConditionStatus("Poison", 1);
    }
    /// <summary>
    /// キマイラ:噛みつき
    /// プレイヤーに4ダメージ与える
    /// </summary>
    private void Bite()
    {
        player.TakeDamage(4);
    }
    /// <summary>
    /// キマイラと暗黒騎士:暴れ回る
    /// プレイヤーに2ダメージを3～6回与える
    /// </summary>
    private void Rampage()
    {
        StartCoroutine(RampageEnumerator());
    }
    IEnumerator RampageEnumerator()
    {
        bg.isCoroutine = true;
        int attackCount = Random.Range(3, 6);
        for (int count = 0; count < attackCount; count++)
        {
            yield return new WaitForSeconds(1.0f);
            player.TakeDamage(2);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// キマイラ:燃える息
    /// プレイヤーに火傷を1つ与える
    /// </summary>
    private void BurningBreath()
    {
        player.AddConditionStatus("Burn", 1);
    }
    /// <summary>
    /// キマイラ:蛇睨み
    /// プレイヤーに焦燥を1つ与える
    /// </summary>
    private void SnakeGlares()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// 暗黒騎士：振り下ろす
    /// プレイヤーに5ダメージを与える。
    /// </summary>
    private void Swing()
    {
        player.TakeDamage(5);
    }
    /// <summary>
    /// 暗黒騎士：堅牢なる盾
    /// ガードを4得る。
    /// </summary>
    private void RobustShield()
    {
        enemy.AddGP(4);
    }
    /// <summary>
    /// 暗黒騎士：捨て身突進
    /// プレイヤーに7ダメージを与え、自身にも4ダメージ。
    /// </summary>
    private void DesperateLunge()
    {
        player.TakeDamage(7);
        enemy.TakeDamage(4);
    }
    /// <summary>
    /// 暗黒騎士：鬨
    /// 筋力増強を1つ得る
    /// </summary>
    private void Encouragement()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// サイクロプスとスキュラ：振りかぶる
    /// ガードを10得る。
    /// </summary>
    private void SwingOver()
    {
        enemy.AddGP(10);
    }
    /// <summary>
    /// サイクロプスとスキュラ：神砕き
    /// ガードの3倍のダメージを与える。ガードをすべて失う。
    /// </summary>
    private void GodCrusher()
    {
        int damage = enemy.GetSetGP * 3;
        player.TakeDamage(damage);
        enemy.GetSetGP = 0;
    }
    /// <summary>
    /// サイクロプスとスキュラ：乱れ打ち
    /// プレイヤーに5ダメージを0～3回与える。
    /// </summary>
    private void RandomPounding()
    {
        StartCoroutine(RandomPoundingEnumerator());
    }
    IEnumerator RandomPoundingEnumerator()
    {
        bg.isCoroutine = true;
        int attackCount = Random.Range(0, 3);
        for (int count = 0; count < attackCount; count++)
        {
            yield return new WaitForSeconds(1.0f);
            player.TakeDamage(5);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// サイクロプス：巨拳
    /// プレイヤーに4ダメージを与える。
    /// </summary>
    private void GiantFist()
    {
        player.TakeDamage(4);
    }
    /// <summary>
    /// サイクロプスとスキュラ：じならし
    /// お互いのバフ、デバフをすべて解除する。
    /// </summary>
    private void Rumbling()
    {
        player.GetSetCondition.upStrength = 0;
        player.GetSetCondition.autoHealing = 0;
        player.GetSetCondition.invalidBadStatus = 0;
        player.ReleaseBadStatus();
        enemy.GetSetCondition.upStrength = 0;
        enemy.GetSetCondition.autoHealing = 0;
        enemy.GetSetCondition.invalidBadStatus = 0;
        enemy.ReleaseBadStatus();
    }
    /// <summary>
    /// スキュラ：蠢く触手
    /// プレイヤーに3ダメージを与え、呪縛、焦燥、衰弱、邪毒のいずれかを与える。
    /// </summary>
    private void WrigglingTentacles()
    {
        player.TakeDamage(3);
        List<string> badStatus = new List<string>();
        badStatus.Add("Curse");
        badStatus.Add("Impatience");
        badStatus.Add("Weakness");
        badStatus.Add("Poison");
        int rand = Random.Range(0, badStatus.Count);
        player.AddConditionStatus(badStatus[rand], 1);
    }
}
