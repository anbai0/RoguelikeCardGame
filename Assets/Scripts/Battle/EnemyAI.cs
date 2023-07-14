using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    BattleGameManager bg;
    PlayerBattleAction player;
    EnemyBattleAction enemy;

    private int snakeGlaresCount;
    private enum EnemyState //エネミーの種別
    {
        SLIME,                    //スライム
        SKELETONSWORDSMAN,        //骸骨剣士
        NAGA,                     //ナーガ
        CHIMERA,                  //キマイラ
        DARKKNIGHT                //暗黒騎士
    }
    EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<BattleGameManager>();
        player = GetComponent<PlayerBattleAction>();
        enemy = GetComponent<EnemyBattleAction>();
        snakeGlaresCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetEnemyState(string enemyName) //エネミーの名前に応じて行動パターンのステートを変更する
    {
        if (enemyName == "スライム")
        {
            enemyState = EnemyState.SLIME;
        }
        else if (enemyName == "骸骨剣士")
        {
            enemyState = EnemyState.SKELETONSWORDSMAN;
        }
        else if (enemyName == "ナーガ")
        {
            enemyState = EnemyState.NAGA;
        }
        else if (enemyName == "キマイラ")
        {
            enemyState = EnemyState.CHIMERA;
        }
        else if (enemyName == "暗黒騎士")
        {
            enemyState = EnemyState.DARKKNIGHT;
        }
    }
    /// <summary>
    /// エネミーの行動を選択する
    /// </summary>
    /// <param name="currentAP">エネミーの現在のAP</param>
    /// <returns>技の名前,技のコスト</returns>
    public (string moveName, int moveCost) SelectMove(int currentAP)
    {
        Debug.Log("現在のエネミーステートは:" + enemyState);
        string moveName = "RoundEnd";//使用する技の名前
        int moveCost = 0;//使用する技のコスト
        if (enemyState == EnemyState.SLIME)//スライムの行動パターン
        {
            Debug.Log("Slimeの行動");
            int chargeCost = 2;//体当たりのコスト
            int solutionCost = 3;//溶解液のコスト
            int[] costArray = { chargeCost, solutionCost };//コストの配列
            bool chargeEnabled = false;//体当たりが使用可能か
            bool solutionEnabled = false;//溶解液が使用可能か
            bool[] enabledArray = { chargeEnabled, solutionEnabled };//使用可能か判定する配列
            for (int arrayNum = 0; arrayNum < costArray.Length; arrayNum++)
            {
                if (costArray[arrayNum] <= currentAP) //現在のAPがコストよりも大きい場合
                {
                    enabledArray[arrayNum] = true;//行動が可能になる
                }
            }
            chargeEnabled = enabledArray[0];
            solutionEnabled = enabledArray[1];
            Debug.Log("dostArray.Length is :" + costArray.Length);
            Debug.Log("chargeEnabled = " + chargeEnabled.ToString() + "chargeEnabled = " + solutionEnabled.ToString());
            //行動パターンの選択
            if (solutionEnabled && player.GetSetPlayerCondition.weakness == 0) //溶解液が使用可能でプレイヤーに衰弱が付いていなかった場合
            {
                moveName = "Solution";//溶解液を使用する
                chargeEnabled = false;//体当たりを使用出来なくする
                moveCost = solutionCost;//溶解液のコストをmoveCostを設定する
                Debug.Log("溶解液");
            }
            if (chargeEnabled) //体当たりが使用可能な場合
            {
                moveName = "Charge";//体当たりを使用する
                solutionEnabled = false;//溶解液を使用出来なくする
                moveCost = chargeCost;//体当たりのコストをmoveCostを設定する
                Debug.Log("体当たり");
            }
            if (!chargeEnabled && !solutionEnabled)//どの技も使用不可の場合 
            {
                moveName = "RoundEnd";//行動を終了する
            }
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN) //骸骨剣士の行動パターン
        {
            int slashCost = 2;//切りかかるのコスト
            int holdShieldCost = 3;//盾を構えるのコスト
            int[] costArray = { slashCost, holdShieldCost };
            bool slashEnabled = false;//切りかかるが使用可能か
            bool holdShieldEnabled = false;//盾を構えるが使用可能か
            bool[] enabledArray = { slashEnabled, holdShieldEnabled };
            for (int arrayNum = 0; arrayNum < costArray.Length; arrayNum++)
            {
                if (costArray[arrayNum] <= currentAP)
                {
                    enabledArray[arrayNum] = true;
                }
            }
            slashEnabled = enabledArray[0];
            holdShieldEnabled = enabledArray[1];
            if (holdShieldEnabled && enemy.GetSetEnemyGP == 0) //盾を構えるが使用可能でエネミーにガードが無かった場合
            {
                moveName = "HoldShield";//盾を構えるを使用する
                slashEnabled = false;//切りかかるを使用出来なくする
                moveCost = holdShieldCost;//盾を構えるのコストをmoveCostを設定する
                Debug.Log("盾を構える");
            }
            if (slashEnabled) //切りかかるが使用可能な場合
            {
                moveName = "Slash";//切りかかるを使用する
                holdShieldEnabled = false;//盾を構えるを使用出来なくする
                moveCost = slashCost;//切りかかるのコストをmoveCostを設定する
                Debug.Log("切りかかる");
            }
            if (!slashEnabled && !holdShieldEnabled)//どの技も使用不可の場合 
            {
                moveName = "RoundEnd";
            }
        }
        else if (enemyState == EnemyState.NAGA) //ナーガの行動パターン
        {
            int slashCost = 2;//切りかかるのコスト
            int creepySongCost = 3;//不気味な歌のコスト
            int[] costArray = { slashCost, creepySongCost };
            bool slashEnabled = false;//切りかかるが使用可能か
            bool creepySongEnabled = false;//不気味な歌が使用可能か
            bool[] enabledArray = { slashEnabled, creepySongEnabled };
            for (int arrayNum = 0; arrayNum < costArray.Length; arrayNum++)
            {
                if (costArray[arrayNum] <= currentAP)
                {
                    enabledArray[arrayNum] = true;
                }
            }
            slashEnabled = enabledArray[0];
            creepySongEnabled = enabledArray[1];
            if (creepySongEnabled && player.GetSetPlayerCondition.impatience == 0) //不気味な歌が使用可能でプレイヤーに焦燥が付いていなかった場合
            {
                moveName = "CreepySong";//不気味な歌を使用する
                slashEnabled = false;//切りかかるを使用出来なくする
                moveCost = creepySongCost;//不気味な歌のコストをmoveCostを設定する
                Debug.Log("不気味な歌");
            }
            if (slashEnabled) //切りかかるが使用可能な場合
            {
                moveName = "Slash";//切りかかるを使用する
                creepySongEnabled = false;//盾を構えるを使用出来なくする
                moveCost = slashCost;//切りかかるのコストをmoveCostを設定する
                Debug.Log("切りかかる");
            }
            if (!slashEnabled && !creepySongEnabled)//どの技も使用不可の場合
            {
                moveName = "RoundEnd";
            }
        }
        else if (enemyState == EnemyState.CHIMERA) //キマイラの行動パターン
        {
            int biteCost = 3;//噛みつきのコスト
            int rampageCost = 7;//暴れ回るのコスト
            int burningBreathCost = 4;//燃える息のコスト
            int snakeGlaresCost = 2;//蛇睨みのコスト
            int[] costArray = { biteCost, rampageCost, burningBreathCost, snakeGlaresCost };
            bool biteEnabled = false;//噛みつきが使用可能か
            bool rampageEnabled = false;//暴れ回るが使用可能か
            bool burningBreathEnabled = false;//燃える息が使用可能か
            bool snakeGlaresEnabled = false;//蛇睨みが使用可能か
            bool[] enabledArray = { biteEnabled, rampageEnabled, burningBreathEnabled, snakeGlaresEnabled };
            for (int arrayNum = 0; arrayNum < costArray.Length; arrayNum++)
            {
                if (costArray[arrayNum] <= currentAP)
                {
                    enabledArray[arrayNum] = true;
                }
            }
            biteEnabled = enabledArray[0];
            rampageEnabled = enabledArray[1];
            burningBreathEnabled = enabledArray[2];
            snakeGlaresEnabled = enabledArray[3];

            if (snakeGlaresEnabled && snakeGlaresCount == 0) //蛇睨みが使用可能で一回も使用していない場合
            {
                snakeGlaresCount++;//使用した回数を増やす
                moveName = "SnakeGlares";//蛇睨みを使用する
                biteEnabled = false;//?みつきを使用出来なくする
                rampageEnabled = false;//暴れ回るを使用出来なくする
                burningBreathEnabled = false;//燃える息を使用出来なくする
                moveCost = snakeGlaresCost;//蛇睨みのコストをmoveCostに設定する
                Debug.Log("蛇睨み");
            }
            else
            {
                snakeGlaresEnabled = false;//蛇睨みを使用出来なくする
            }

            List<string> moveEnabledArray = new List<string>();//使用可能な技をListに保存
            if (biteEnabled) //biteEnabledが使用可能であれば
            {
                moveEnabledArray.Add("Bite");//moveEnabledArrayに追加
            }
            if (rampageEnabled) //rampageEnabledが使用可能であれば
            {
                moveEnabledArray.Add("Rampage");//moveEnabledArrayに追加
            }
            if (burningBreathEnabled) //burningBreathEnabledが使用可能であれば
            {
                moveEnabledArray.Add("BurningBreath");//moveEnabledArrayに追加
            }
            if (moveEnabledArray.Count > 0)
            {
                int randSelectMove = Random.Range(0, moveEnabledArray.Count);//使用できる技の中からランダムに技を選択する

                switch (moveEnabledArray[randSelectMove])
                {
                    case "Bite":　//選択された行動噛みつきだった場合
                        moveName = "Bite";//噛みつきを使用する
                        rampageEnabled = false;//暴れ回るを使用出来なくする
                        burningBreathEnabled = false;//燃える息を使用出来なくする
                        moveCost = biteCost;//噛みつきのコストをmoveCostに設定する
                        Debug.Log("噛みつき");
                        break;
                    case "Rampage": //選択された行動が暴れ回るだった場合
                        moveName = "Rampage";//暴れ回るを使用する
                        biteEnabled = false;//噛みつきを使用出来なくする
                        burningBreathEnabled = false;//燃える息を使用出来なくする
                        moveCost = rampageCost;//暴れ回るのコストをmoveCostに設定する
                        Debug.Log("暴れ回る");
                        break;
                    case "BurningBreath": //選択された行動が燃える息だった場合
                        moveName = "BurningBreath";//燃える息を使用する
                        biteEnabled = false;//噛みつきを使用出来なくする
                        rampageEnabled = false;//暴れ回るを使用出来なくする
                        moveCost = burningBreathCost; //燃える息のコストをmoveCostに設定する
                        Debug.Log("燃える息");
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            if (!snakeGlaresEnabled && moveEnabledArray.Count == 0) //どの技も使用不可の場合
            {
                moveName = "RoundEnd";
            }
        }
        else if (enemyState == EnemyState.DARKKNIGHT) //暗黒騎士の行動パターン
        {
            int swingCost = 3;//振り下ろすのコスト
            int robustShieldCost = 3;//堅牢な盾のコスト
            int desperateLungeCost = 3;//捨て身突進のコスト
            int rampageCost = 7;//暴れ回るのコスト
            int[] costArray = { swingCost, robustShieldCost, desperateLungeCost, rampageCost };
            bool swingEnabled = false;//振り下ろすが使用可能か
            bool robustShieldEnabled = false;//堅牢な盾が使用可能か
            bool desperateLungeEnabled = false;//捨て身突進が使用可能か
            bool rampageEnabled = false;//暴れ回るが使用可能か
            bool[] enabledArray = { swingEnabled, robustShieldEnabled, desperateLungeEnabled, rampageEnabled };
            for (int arrayNum = 0; arrayNum < costArray.Length; arrayNum++)
            {
                if (costArray[arrayNum] <= currentAP)
                {
                    enabledArray[arrayNum] = true;
                }
            }
            swingEnabled = enabledArray[0];
            robustShieldEnabled = enabledArray[1];
            desperateLungeEnabled = enabledArray[2];
            rampageEnabled = enabledArray[3];

            if (robustShieldEnabled && enemy.GetSetRoundEnabled == false) //堅牢な盾が使用可能でラウンド中に1回も使用していなかった場合
            {
                enemy.GetSetRoundEnabled = true;//ラウンド中に使用した
                moveName = "RobustShield";//堅牢な盾を使用する
                swingEnabled = false;//振り下ろすを使用出来なくする
                desperateLungeEnabled = false;//捨て身突進を使用出来なくする
                rampageEnabled = false;//暴れ回るを使用出来なくする
                moveCost = robustShieldCost;//堅牢な盾のコストをmoveCostに設定する
            }
            else
            {
                robustShieldEnabled = false;
            }
            List<string> moveEnabledArray = new List<string>();//使用可能な技をListに保存
            if (swingEnabled) //swingEnabledが使用可能であれば
            {
                moveEnabledArray.Add("Swing");//moveEnabledArrayに追加
            }
            if (desperateLungeEnabled && enemy.GetSetEnemyCurrentHP >= 15) //desperateLungeEnabledが使用可能でエネミーの現在のHPが15以上であれば
            {
                moveEnabledArray.Add("DesperateLunge");//moveEnabledArrayに追加
            }
            if (rampageEnabled) //burningBreathEnabledが使用可能であれば
            {
                moveEnabledArray.Add("Rampage");//moveEnabledArrayに追加
            }
            if (moveEnabledArray.Count > 0)
            {
                int randomSelectMove = Random.Range(0, moveEnabledArray.Count);
                switch (moveEnabledArray[randomSelectMove])
                {
                    case "Swing":
                        moveName = "Swing";
                        desperateLungeEnabled = false;
                        rampageEnabled = false;
                        moveCost = swingCost;
                        Debug.Log("振り下ろす");
                        break;
                    case "DesperateLunge":
                        moveName = "DesperateLunge";
                        swingEnabled = false;
                        rampageEnabled = false;
                        moveCost = desperateLungeCost;
                        Debug.Log("捨て身突進");
                        break;
                    case "Rampage":
                        moveName = "Rampage";
                        swingEnabled = false;
                        desperateLungeEnabled = false;
                        moveCost = rampageCost;
                        Debug.Log("暴れ回る");
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            if (!robustShieldEnabled && moveEnabledArray.Count == 0)
            {
                moveName = "RoundEnd";
            }
        }
        else //どのステートでもない場合
        {
            moveName = "RoundEnd";
        }
        return (moveName, moveCost);
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
            case "Slash":
                Slash();
                break;
            case "HoldShield":
                HoldShield();
                break;
            case "CreepySong":
                CreepySong();
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
        int attackCount = Random.Range(3, 4);
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
    private void Swing()
    {
        player.TakeDamage(5);
    }
    private void RobustShield()
    {
        enemy.AddGP(4);
    }
    private void DesperateLunge()
    {
        player.TakeDamage(7);
        enemy.TakeDamage(4);
    }
}
