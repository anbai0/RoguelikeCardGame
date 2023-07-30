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
    bool isUsedOnlyMove; //戦闘開始時のみ使用可能
    bool isUsableGodCrusher; //神砕きを使用出来るか判定
    private enum EnemyState //エネミーの種別
    {
        SLIME1,                    //スライム(1階層)
        SKELETONSWORDSMAN1,        //骸骨剣士(1階層)
        NAGA1,                     //ナーガ(1階層)
        CHIMERA1,                  //キマイラ(1階層)
        DARKKNIGHT1,               //暗黒騎士(1階層)
        CYCLOPS,                   //サイクロプス(1階層)
        SLIME2,                    //スライム(2階層)
        SKELETONSWORDSMAN2,        //骸骨剣士(2階層)
        NAGA2,                     //ナーガ(2階層)
        CHIMERA2,                  //キマイラ(2階層)
        DARKKNIGHT2,               //暗黒騎士(2階層)
        SCYLLA,                    //スキュラ(2階層)
        SLIME3,                    //スライム(3階層)
        SKELETONSWORDSMAN3,        //骸骨剣士(3階層)
        NAGA3,                     //ナーガ(3階層)
        CHIMERA3,                  //キマイラ(3階層)
        DARKKNIGHT3,               //暗黒騎士(3階層)
        MINOTAUR,                  //ミノタウロス(3階層)
    }
    EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<BattleGameManager>();
        player = GetComponent<PlayerBattleAction>();
        enemy = GetComponent<EnemyBattleAction>();
        isUsedOnlyMove = false;
        isUsableGodCrusher = false;
    }
    public (string moveName, int moveCost) SelectMove(int currentAP)
    {
        string moveName = null; //選択された技の名前
        int moveCost = 0; //選択されたコストの名前
        List<EnemyMove> enemyMove; //エネミーの技リスト

        enemyMove = SetMoveList(); //技をセットする
        enemyMove = CheckAP(enemyMove, currentAP); //現在のAPに応じて使える技を選出
        enemyMove = CheckEnemyMove(enemyMove); //エネミーの条件に応じて使える技を選出
        var selectMove = SelectMove(enemyMove); //使用できる技の中からランダムで選択する
        moveName = selectMove.name; //選択された技の名前を代入
        moveCost = selectMove.cost; //選択された技のコストを代入
        return (moveName, moveCost);
    }
    public void SetEnemyState(int floor, string enemyName) //エネミーの名前と階層数に応じてEnemyAIのステートを変更する
    {
        if (enemyName == "スライム" && floor == 1)
        {
            enemyState = EnemyState.SLIME1;
        }
        else if (enemyName == "スライム" && floor == 2)
        {
            enemyState = EnemyState.SLIME2;
        }
        else if (enemyName == "スライム" && floor == 3)
        {
            enemyState = EnemyState.SLIME3;
        }
        else if (enemyName == "骸骨剣士" && floor == 1)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN1;
        }
        else if (enemyName == "骸骨剣士" && floor == 2)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN2;
        }
        else if (enemyName == "骸骨剣士" && floor == 3)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN3;
        }
        else if (enemyName == "ナーガ" && floor == 1)
        {
            enemyState = EnemyState.NAGA1;
        }
        else if (enemyName == "ナーガ" && floor == 2)
        {
            enemyState = EnemyState.NAGA2;
        }
        else if (enemyName == "ナーガ" && floor == 3)
        {
            enemyState = EnemyState.NAGA3;
        }
        else if (enemyName == "キメラ" && floor == 1)
        {
            enemyState = EnemyState.CHIMERA1;
        }
        else if (enemyName == "キメラ" && floor == 2)
        {
            enemyState = EnemyState.CHIMERA2;
        }
        else if (enemyName == "キメラ" && floor == 3)
        {
            enemyState = EnemyState.CHIMERA3;
        }
        else if (enemyName == "暗黒騎士" && floor == 1)
        {
            enemyState = EnemyState.DARKKNIGHT1;
        }
        else if (enemyName == "暗黒騎士" && floor == 2)
        {
            enemyState = EnemyState.DARKKNIGHT2;
        }
        else if (enemyName == "暗黒騎士" && floor == 3)
        {
            enemyState = EnemyState.DARKKNIGHT3;
        }
        else if (enemyName == "サイクロプス" && floor == 1)
        {
            enemyState = EnemyState.CYCLOPS;
        }
        else if (enemyName == "スキュラ" && floor == 2)
        {
            enemyState = EnemyState.SCYLLA;
        }
        else if (enemyName == "ミノタウロス" && floor == 3)
        {
            enemyState = EnemyState.MINOTAUR;
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
        else if (enemyState == EnemyState.SLIME3)
        {
            enemyMove.Add(new EnemyMove("Charge", false, 2)); //たいあたり
            enemyMove.Add(new EnemyMove("Solution", false, 3)); //溶解液
            enemyMove.Add(new EnemyMove("Curing", false, 2)); //硬化
            enemyMove.Add(new EnemyMove("Venom", false, 3)); //毒液
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //切りかかる
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //盾を構える
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //切りかかる
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //盾を構える
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //暴れ回る
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN3)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //切りかかる
            enemyMove.Add(new EnemyMove("HoldShield", false, 3)); //盾を構える
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //暴れ回る
            enemyMove.Add(new EnemyMove("TearOfWeathering", false, 6)); //風化の一裂き
        }
        else if (enemyState == EnemyState.NAGA1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //切りかかる
            enemyMove.Add(new EnemyMove("CreepySong", false, 2)); //不気味な歌
        }
        else if (enemyState == EnemyState.NAGA2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //切りかかる
            enemyMove.Add(new EnemyMove("CreepySong", false, 2)); //不気味な歌
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //蛇牙
        }
        else if (enemyState == EnemyState.NAGA3)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //切りかかる
            enemyMove.Add(new EnemyMove("CreepySong", false, 2)); //不気味な歌
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //蛇牙
            enemyMove.Add(new EnemyMove("SwordDance", false, 3)); //剣の舞
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //噛みつき
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //暴れ回る
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4)); //燃える息
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2)); //蛇睨み
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //噛みつき
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //暴れ回る
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4)); //燃える息
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2)); //蛇睨み
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //蛇牙
        }
        else if (enemyState == EnemyState.CHIMERA3)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //噛みつき
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //暴れ回る
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4)); //燃える息
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //蛇牙
            enemyMove.Add(new EnemyMove("BattlePosture", false, 3)); //戦闘態勢
            enemyMove.Add(new EnemyMove("Shedding", false, 4)); //脱皮
        }
        else if (enemyState == EnemyState.DARKKNIGHT1)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3)); //振り下ろす
            enemyMove.Add(new EnemyMove("RobustShield", false, 3)); //堅牢なる盾
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3)); //捨て身突進
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //暴れ回る
        }
        else if (enemyState == EnemyState.DARKKNIGHT2)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3)); //振り下ろす
            enemyMove.Add(new EnemyMove("RobustShield", false, 3)); //堅牢なる盾
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3)); //捨て身突進
            enemyMove.Add(new EnemyMove("Rampage", false, 6)); //暴れ回る
            enemyMove.Add(new EnemyMove("Encouragement", false, 4)); //鬨
        }
        else if (enemyState == EnemyState.DARKKNIGHT3)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3)); //振り下ろす
            enemyMove.Add(new EnemyMove("RobustShield", false, 1)); //堅牢なる盾
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3)); //捨て身突進
            enemyMove.Add(new EnemyMove("Rampage", false, 6)); //暴れ回る
            enemyMove.Add(new EnemyMove("Encouragement", false, 3)); //鬨
            enemyMove.Add(new EnemyMove("Decapitation", false, 5)); //断頭
        }
        else if (enemyState == EnemyState.CYCLOPS)
        {
            enemyMove.Add(new EnemyMove("SwingOver", false, 6)); //振りかぶる
            enemyMove.Add(new EnemyMove("GodCrusher", false, 0)); //神砕き
            enemyMove.Add(new EnemyMove("RandomPounding", false, 5)); //乱れ打ち
            enemyMove.Add(new EnemyMove("GiantFist", false, 2)); //巨拳
            enemyMove.Add(new EnemyMove("Rumbling", false, 4)); //じならし
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            enemyMove.Add(new EnemyMove("CorrosivePoison", false, 3)); //蝕毒
            enemyMove.Add(new EnemyMove("OneDrop", false, 5)); //一滴
            enemyMove.Add(new EnemyMove("Whipping", false, 2)); //鞭打
            enemyMove.Add(new EnemyMove("ChewAndCrush", false, 9)); //?み砕き
            enemyMove.Add(new EnemyMove("WrigglingTentacles", false, 4)); //蠢く触手
            enemyMove.Add(new EnemyMove("Rumbling", false, 4)); //じならし
        }
        else if (enemyState == EnemyState.MINOTAUR)
        {
            enemyMove.Add(new EnemyMove("MowDown", false, 3)); //薙ぎ払い
            enemyMove.Add(new EnemyMove("Rampage", false, 5)); //暴れ回る
            enemyMove.Add(new EnemyMove("Labyrinth", false, 10)); //迷宮
            enemyMove.Add(new EnemyMove("Throwing", false, 6)); //投擲
            enemyMove.Add(new EnemyMove("SkullSplit", false, 4)); //頭蓋割
            enemyMove.Add(new EnemyMove("Rumbling", false, 4)); //じならし
        }
        return enemyMove;
    }
    List<EnemyMove> CheckAP(List<EnemyMove> enemyMove, int currentAP) //エネミーの技がAP以下かチェックする
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
    List<EnemyMove> CheckEnemyMove(List<EnemyMove> enemyMove) //エネミーの技が使用出来るかチェックする
    {
        if (enemyState == EnemyState.SLIME1)
        {
            if (player.GetSetCondition.weakness > 0) //プレイヤーに衰弱が付与されているなら
            {
                enemyMove[1].isUsable = false; //溶解液を使用不可に
            }
        }
        else if (enemyState == EnemyState.SLIME2 || enemyState == EnemyState.SLIME2)
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
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1 || enemyState == EnemyState.SKELETONSWORDSMAN2 || enemyState == EnemyState.SKELETONSWORDSMAN3)
        {
            if (enemy.GetSetGP > 0) //エネミーにGPがあるなら
            {
                enemyMove[1].isUsable = false; //盾を構えるを使用不可に
            }
        }
        else if (enemyState == EnemyState.NAGA1 || enemyState == EnemyState.NAGA2 || enemyState == EnemyState.NAGA3)
        {
            if (player.GetSetCondition.impatience > 0) //プレイヤーに焦燥が付与されているなら
            {
                enemyMove[1].isUsable = false;//不気味な歌を使用不可に
            }
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            if (isUsedOnlyMove) //戦闘で一度でも蛇睨みを使用しているなら
            {
                enemyMove[3].isUsable = false; //蛇睨みを使用不可に
            }
            else //蛇睨みを使用していないなら
            {
                //蛇睨み以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                isUsedOnlyMove = true; //蛇睨みの使用したと判定
            }
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            if (isUsedOnlyMove) //戦闘で一度でも蛇睨みを使用しているなら
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
                isUsedOnlyMove = true; //蛇睨みの使用したと判定
            }
        }
        else if (enemyState == EnemyState.CHIMERA3)
        {
            if (isUsedOnlyMove) //戦闘で一度でも戦闘態勢を使用しているなら
            {
                enemyMove[4].isUsable = false; //戦闘態勢を使用不可に
            }
            else //戦闘態勢を使用していないなら
            {
                //戦闘態勢以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[5].isUsable = false;
                isUsedOnlyMove = true; //戦闘態勢の使用したと判定
            }

            if (enemy.CheckBadStatus() == 0) //エネミーにバットステータスが付与されていないなら
            {
                enemyMove[5].isUsable = false; //脱皮を使用不可に
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
        else if (enemyState == EnemyState.DARKKNIGHT3)
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
                enemyMove[5].isUsable = false;
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

            if (player.CheckBuffStatus() == 0) //プレイヤーにバフが付与されていないなら
            {
                enemyMove[5].isUsable = false; //断頭を使用不可に
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

            if (player.CheckBuffStatus() == 0 && player.CheckBadStatus() == 0 && enemy.CheckBuffStatus() == 0 && enemy.CheckBadStatus() == 0) //プレイヤーとエネミーにバフやデバフが付与されていないなら
            {
                enemyMove[4].isUsable = false; //じならしを使用不可に
            }
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            if (enemy.GetSetCurrentHP > 40) //エネミーの現在のHPが40よりも上なら
            {
                enemyMove[1].isUsable = false; //一滴を使用不可に
            }

            if (player.GetSetGP > 0) //プレイヤーがガードを持っているなら
            {
                enemyMove[3].isUsable = false; //?み砕きを使用不可に
            }

            if (player.CheckBuffStatus() == 0 && enemy.CheckBadStatus() == 0) //プレイヤーにバフが付与されてなく、またはエネミーにデバフが付与されていないなら
            {
                enemyMove[5].isUsable = false; //じならしを使用不可に
            }
        }
        else if (enemyState == EnemyState.MINOTAUR)
        {
            if (isUsedOnlyMove) //戦闘で一度でも迷宮を使用しているなら
            {
                enemyMove[2].isUsable = false; //迷宮を使用不可に
            }
            else //迷宮を使用していないなら
            {
                //迷宮以外を使用不可に
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                enemyMove[5].isUsable = false;
                isUsedOnlyMove = true; //迷宮の使用したと判定
            }
            if (enemy.GetSetCurrentHP > 50) //エネミーの現在のHPが50よりも上なら
            {
                enemyMove[3].isUsable = false; //投擲を使用不可に
            }

            if (player.GetSetGP == 0) //プレイヤーがガードを持っていないなら
            {
                enemyMove[4].isUsable = false; //頭蓋割を使用不可に
            }

            if (player.CheckBuffStatus() == 0 && enemy.CheckBadStatus() == 0) //プレイヤーにバフが付与されてなく、またはエネミーにデバフが付与されていないなら
            {
                enemyMove[5].isUsable = false; //じならしを使用不可に
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
            case "Venom":
                Venom();
                break;
            case "Slash":
                Slash();
                break;
            case "HoldShield":
                HoldShield();
                break;
            case "TearOfWeathering":
                TearOfWeathering();
                break;
            case "CreepySong":
                CreepySong();
                break;
            case "SnakeFangs":
                SnakeFangs();
                break;
            case "SwordDance":
                SwordDance();
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
            case "BattlePosture":
                BattlePosture();
                break;
            case "Shedding":
                Shedding();
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
            case "Decapitation":
                Decapitation();
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
            case "CorrosivePoison":
                CorrosivePoison();
                break;
            case "OneDrop":
                OneDrop();
                break;
            case "Whipping":
                Whipping();
                break;
            case "ChewAndCrush":
                ChewAndCrush();
                break;
            case "WrigglingTentacles":
                WrigglingTentacles();
                break;
            case "MowDown":
                MowDown();
                break;
            case "Labyrinth":
                Labyrinth();
                break;
            case "Throwing":
                Throwing();
                break;
            case "SkullSplit":
                SkullSplit();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    /// <summary>
    ///行動せずにラウンドを終了する。
    /// </summary>
    private void RoundEnd()
    {
        enemy.TurnEnd();
    }
    /// <summary>
    /// 技名：体当たり
    /// 使用者：スライム(1階層),スライム(2階層)
    /// 効果：プレイヤーに2ダメージ与える。
    /// 使用者：スライム(3階層)
    /// 効果：プレイヤーに3ダメージ与える。
    /// </summary>
    private void Charge()
    {
        if (enemyState == EnemyState.SLIME3)
        {
            EnemyAttacking(3);
        }
        else
        {
            EnemyAttacking(2);
        }

    }
    /// <summary>
    /// 技名：溶解液
    /// 使用者：スライム(1階層),スライム(2階層),スライム(3階層)
    /// 効果：プレイヤーに衰弱を1つ与える。
    /// </summary>
    private void Solution()
    {
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// 技名：硬化
    /// 使用者：スライム(2階層),スライム(3階層)
    /// 効果：4ガードを得る。状態異常無効を2得る。
    /// </summary>
    private void Curing()
    {
        enemy.AddGP(4);
        enemy.AddConditionStatus("InvalidBadStatus", 2);
    }
    /// <summary>
    /// 技名：毒液
    /// 使用者：スライム(3階層)
    /// 効果：プレイヤーに邪毒を1つ与える。
    /// </summary>
    private void Venom()
    {
        player.AddConditionStatus("Poison", 1);
    }
    /// <summary>
    /// 技名：切りかかる
    /// 使用者：骸骨剣士(1階層),骸骨剣士(2階層),ナーガ(1階層),ナーガ(2階層)
    /// 効果：プレイヤーに4ダメージ与える。
    /// 使用者：骸骨剣士(3階層),ナーガ(3階層)
    /// 効果：プレイヤーに5ダメージ与える。
    /// </summary>
    private void Slash()
    {
        if (enemyState == EnemyState.SKELETONSWORDSMAN3 || enemyState == EnemyState.NAGA3)
        {
            EnemyAttacking(5);
        }
        else
        {
            EnemyAttacking(4);
        }
    }
    /// <summary>
    /// 技名：盾を構える
    /// 使用者：骸骨剣士(1階層),骸骨剣士(2階層)
    /// 効果：ガードを2得る。
    /// 使用者：骸骨剣士(3階層)
    /// 効果：ガードを5得る。
    /// </summary>
    private void HoldShield()
    {
        if (enemyState == EnemyState.SKELETONSWORDSMAN3)
        {
            enemy.AddGP(5);
        }
        else
        {
            enemy.AddGP(2);
        }
    }
    /// <summary>
    /// 技名：風化の一裂き
    /// 使用者：骸骨剣士(3階層)
    /// 効果：プレイヤーに3ダメージを与え、衰弱を1つ与える。
    /// </summary>
    private void TearOfWeathering()
    {
        EnemyAttacking(3);
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// 技名：不気味な歌
    /// 使用者：ナーガ(1階層),ナーガ(2階層),ナーガ(3階層)
    /// 効果：プレイヤーに焦燥を１つ与える。
    /// </summary>
    private void CreepySong()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// 技名：蛇牙
    /// 使用者：ナーガ(2階層),ナーガ(3階層),キマイラ(2階層),キマイラ(3階層),
    /// 効果：プレイヤーに3ダメージ与え、邪毒を1与える。
    /// </summary>
    private void SnakeFangs()
    {
        EnemyAttacking(3);
        player.AddConditionStatus("Poison", 1);
    }
    /// <summary>
    /// 技名：剣の舞
    /// 使用者：ナーガ(3階層)
    /// 効果：筋力増強を1つ得る。
    /// </summary>
    private void SwordDance()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// 技名：噛みつき
    /// 使用者：キマイラ(1階層),キマイラ(2階層),キマイラ(3階層)
    /// 効果：プレイヤーに4ダメージ与える。
    /// </summary>
    private void Bite()
    {
        EnemyAttacking(4);
    }
    /// <summary>
    /// 技名：暴れ回る
    /// 使用者：骸骨剣士(2階層),骸骨剣士(3階層),暗黒騎士(1階層),暗黒騎士(2階層),暗黒騎士(3階層)
    /// 効果：プレイヤーに2ダメージを3～5回与える。
    /// 使用者：キマイラ(1階層),キマイラ(2階層),キマイラ(3階層)
    /// 効果：プレイヤーに2ダメージを3～6回与える。
    /// 使用者：ミノタウロス(3階層)
    /// 効果：プレイヤーに2ダメージを2～8回与える。
    /// </summary>
    private void Rampage()
    {
        if (enemyState == EnemyState.MINOTAUR)
        {
            StartCoroutine(RampageEnumerator(2, 8));
        }
        else if (enemyState == EnemyState.CHIMERA1 || enemyState == EnemyState.CHIMERA2 || enemyState == EnemyState.CHIMERA3)
        {
            StartCoroutine(RampageEnumerator(3, 6));
        }
        else
        {
            StartCoroutine(RampageEnumerator(3, 5));
        }
    }
    IEnumerator RampageEnumerator(int startCount, int endCount)
    {
        bg.isCoroutine = true;
        int attackCount = Random.Range(startCount, endCount);
        for (int count = 0; count < attackCount; count++)
        {
            yield return new WaitForSeconds(1.0f);
            EnemyAttacking(2);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：燃える息
    /// 使用者：キマイラ(1階層),キマイラ(2階層),キマイラ(3階層)
    /// 効果：プレイヤーに火傷を1つ与える。
    /// </summary>
    private void BurningBreath()
    {
        player.AddConditionStatus("Burn", 1);
    }
    /// <summary>
    /// 技名：蛇睨み
    /// 使用者：キマイラ(1階層),キマイラ(2階層)
    /// 効果：プレイヤーに焦燥を1つ与える。
    /// </summary>
    private void SnakeGlares()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// 技名：戦闘態勢
    /// 使用者：キマイラ(3階層)
    /// 効果：筋力増強を1つ得る。
    /// </summary>
    private void BattlePosture()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// 技名：脱皮
    /// 使用者：キマイラ(3階層)
    /// 効果：デバフをすべて解除する。
    /// </summary>
    private void Shedding()
    {
        enemy.ReleaseBadStatus();
    }
    /// <summary>
    /// 技名：振り下ろす
    /// 使用者：暗黒騎士(1階層),暗黒騎士(2階層)
    /// 効果：プレイヤーに5ダメージを与える。
    /// 使用者：暗黒騎士(3階層)
    /// 効果：プレイヤーに6ダメージを与える。
    /// </summary>
    private void Swing()
    {
        if (enemyState == EnemyState.DARKKNIGHT3)
        {
            EnemyAttacking(6);
        }
        else
        {
            EnemyAttacking(5);
        }
    }
    /// <summary>
    /// 技名：堅牢なる盾
    /// 使用者：暗黒騎士(1階層),暗黒騎士(2階層),暗黒騎士(3階層)
    /// 効果：ガードを4得る。
    /// </summary>
    private void RobustShield()
    {
        enemy.AddGP(4);
    }
    /// <summary>
    /// 暗黒騎士：捨て身突進
    /// 使用者：暗黒騎士(1階層),暗黒騎士(2階層),暗黒騎士(3階層)
    /// 効果：プレイヤーに7ダメージを与え、自身にも4ダメージを与える。
    /// </summary>
    private void DesperateLunge()
    {
        EnemyAttacking(7);
        enemy.TakeDamage(4);
    }
    /// <summary>
    /// 技名：鬨
    /// 使用者：暗黒騎士(2階層),暗黒騎士(3階層)
    /// 効果：筋力増強を1つ得る。
    /// </summary>
    private void Encouragement()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// 技名：断頭
    /// 使用者：暗黒騎士(3階層)
    /// 効果：プレイヤーに7ダメージを与え、プレイヤーのバフをすべて解除する。
    /// </summary>
    private void Decapitation()
    {
        EnemyAttacking(7);
        player.ReleaseBuffStatus();
    }
    /// <summary>
    /// 技名：振りかぶる
    /// 使用者：サイクロプス(1階層)
    /// 効果：ガードを10得る。
    /// </summary>
    private void SwingOver()
    {
        enemy.AddGP(10);
    }
    /// <summary>
    /// 技名：神砕き
    /// 使用者：サイクロプス(1階層)
    /// 効果：ガードの3倍のダメージを与える。ガードをすべて失う。
    /// </summary>
    private void GodCrusher()
    {
        int damage = enemy.GetSetGP * 3;
        EnemyAttacking(damage);
        enemy.GetSetGP = 0;
    }
    /// <summary>
    /// 技名：乱れ打ち
    /// 使用者：サイクロプス(1階層)
    /// 効果：プレイヤーに5ダメージを0～3回与える。
    /// </summary>
    private void RandomPounding()
    {
        StartCoroutine(RandomPoundingEnumerator());
    }
    IEnumerator RandomPoundingEnumerator()
    {
        bg.isCoroutine = true;
        int attackCount = Random.Range(1, 3);
        for (int count = 0; count < attackCount; count++)
        {
            yield return new WaitForSeconds(1.0f);
            EnemyAttacking(5);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：巨拳
    /// 使用者：サイクロプス(1階層)
    /// 効果：プレイヤーに4ダメージを与える。
    /// </summary>
    private void GiantFist()
    {
        EnemyAttacking(4);
    }
    /// <summary>
    /// 技名：じならし
    /// 使用者：サイクロプス(1階層),スキュラ(2階層),ミノタウロス(3階層)
    /// 効果：お互いのバフ、デバフをすべて解除する。
    /// </summary>
    private void Rumbling()
    {
        player.ReleaseBuffStatus();
        player.ReleaseBadStatus();
        enemy.ReleaseBuffStatus();
        enemy.ReleaseBadStatus();
    }
    /// <summary>
    /// 技名：蝕毒
    /// 使用者：スキュラ(2階層)
    /// 効果：プレイヤーに邪毒を2つ与える。
    /// </summary>
    private void CorrosivePoison()
    {
        player.AddConditionStatus("Poison", 2);
    }
    /// <summary>
    /// 技名：一滴
    /// 使用者：スキュラ(2階層)
    /// 効果：HPを10回復する。
    /// </summary>
    private void OneDrop()
    {
        enemy.HealingHP(10);
    }
    /// <summary>
    /// 技名：鞭打
    /// 使用者：スキュラ(2階層)
    /// 効果：プレイヤーに3ダメージを与える。
    /// </summary>
    private void Whipping()
    {
        EnemyAttacking(3);
    }
    /// <summary>
    /// 技名：?み砕き
    /// 使用者：スキュラ(2階層)
    /// 効果：3ダメージを6回与える。
    /// </summary>
    private void ChewAndCrush()
    {
        StartCoroutine(ChewAndCrushEnumerator());
    }
    IEnumerator ChewAndCrushEnumerator()
    {
        bg.isCoroutine = true;
        int attackCount = 6;
        for (int count = 0; count < attackCount; count++)
        {
            yield return new WaitForSeconds(1.0f);
            EnemyAttacking(3);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：蠢く触手
    /// 使用者：スキュラ(2階層)
    /// 効果：プレイヤーに3ダメージを与え、呪縛、焦燥、衰弱、邪毒のいずれかを与える。
    /// </summary>
    private void WrigglingTentacles()
    {
        EnemyAttacking(3);
        List<string> badStatus = new List<string>();
        badStatus.Add("Curse");
        badStatus.Add("Impatience");
        badStatus.Add("Weakness");
        badStatus.Add("Poison");
        int rand = Random.Range(0, badStatus.Count);
        player.AddConditionStatus(badStatus[rand], 1);
    }
    /// <summary>
    /// 技名：薙ぎ払う
    /// 使用者：ミノタウロス(3階層)
    /// 効果：プレイヤーに6ダメージを与える。
    /// </summary>
    private void MowDown()
    {
        EnemyAttacking(6);
    }
    /// <summary>
    /// 技名：迷宮
    /// 使用者：ミノタウロス(3階層)
    /// 効果：プレイヤーに呪縛、焦燥、衰弱を１つずつ与える。
    /// </summary>
    private void Labyrinth()
    {
        player.AddConditionStatus("Curse", 1);
        player.AddConditionStatus("Impatience", 1);
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// 技名：投擲
    /// 使用者：ミノタウロス(3階層)
    /// 効果：プレイヤーに3ダメージを5回与える。
    /// </summary>
    private void Throwing()
    {
        StartCoroutine(ThrowingEnumerator());
    }
    IEnumerator ThrowingEnumerator()
    {
        bg.isCoroutine = true;
        int attackCount = 5;
        for (int count = 0; count < attackCount; count++)
        {
            yield return new WaitForSeconds(1.0f);
            EnemyAttacking(3);
        }
        bg.isCoroutine = false;
        bg.TurnCalc();
    }
    /// <summary>
    /// 技名：頭蓋割
    /// 使用者：ミノタウロス(3階層)
    /// 効果：プレイヤーのガードをすべて解除し、4ダメージを与える。
    /// </summary>
    private void SkullSplit()
    {
        player.GetSetGP = 0;
        EnemyAttacking(4);
    }

    private void EnemyAttacking(int damage)//エネミーへの攻撃処理 
    {
        damage = ChangeAttackPower(damage);
        Debug.Log("計算後の攻撃力は" + damage);
        player.TakeDamage(damage);
    }
    private int ChangeAttackPower(int damage) //状態異常による攻撃力の増減
    {
        damage = enemy.UpStrength(damage);
        damage = enemy.Weakness(damage);
        return damage;
    }
}
