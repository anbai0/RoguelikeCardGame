using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    BattleGameManager bg;
    PlayerBattleAction player;
    EnemyBattleAction enemy;

    private int snakeGlaresCount;
    private enum EnemyState //�G�l�~�[�̎��
    {
        SLIME,                    //�X���C��
        SKELETONSWORDSMAN,        //�[�����m
        NAGA,                     //�i�[�K
        CHIMERA,                  //�L�}�C��
        DARKKNIGHT                //�Í��R�m
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
    public void SetEnemyState(string enemyName) //�G�l�~�[�̖��O�ɉ����čs���p�^�[���̃X�e�[�g��ύX����
    {
        if (enemyName == "�X���C��")
        {
            enemyState = EnemyState.SLIME;
        }
        else if (enemyName == "�[�����m")
        {
            enemyState = EnemyState.SKELETONSWORDSMAN;
        }
        else if (enemyName == "�i�[�K")
        {
            enemyState = EnemyState.NAGA;
        }
        else if (enemyName == "�L�}�C��")
        {
            enemyState = EnemyState.CHIMERA;
        }
        else if (enemyName == "�Í��R�m")
        {
            enemyState = EnemyState.DARKKNIGHT;
        }
    }
    /// <summary>
    /// �G�l�~�[�̍s����I������
    /// </summary>
    /// <param name="currentAP">�G�l�~�[�̌��݂�AP</param>
    /// <returns>�Z�̖��O,�Z�̃R�X�g</returns>
    public (string moveName, int moveCost) SelectMove(int currentAP)
    {
        Debug.Log("���݂̃G�l�~�[�X�e�[�g��:" + enemyState);
        string moveName = "RoundEnd";//�g�p����Z�̖��O
        int moveCost = 0;//�g�p����Z�̃R�X�g
        if (enemyState == EnemyState.SLIME)//�X���C���̍s���p�^�[��
        {
            Debug.Log("Slime�̍s��");
            int chargeCost = 2;//�̓�����̃R�X�g
            int solutionCost = 3;//�n���t�̃R�X�g
            int[] costArray = { chargeCost, solutionCost };//�R�X�g�̔z��
            bool chargeEnabled = false;//�̓����肪�g�p�\��
            bool solutionEnabled = false;//�n���t���g�p�\��
            bool[] enabledArray = { chargeEnabled, solutionEnabled };//�g�p�\�����肷��z��
            for (int arrayNum = 0; arrayNum < costArray.Length; arrayNum++)
            {
                if (costArray[arrayNum] <= currentAP) //���݂�AP���R�X�g�����傫���ꍇ
                {
                    enabledArray[arrayNum] = true;//�s�����\�ɂȂ�
                }
            }
            chargeEnabled = enabledArray[0];
            solutionEnabled = enabledArray[1];
            Debug.Log("dostArray.Length is :" + costArray.Length);
            Debug.Log("chargeEnabled = " + chargeEnabled.ToString() + "chargeEnabled = " + solutionEnabled.ToString());
            //�s���p�^�[���̑I��
            if (solutionEnabled && player.GetSetPlayerCondition.weakness == 0) //�n���t���g�p�\�Ńv���C���[�ɐ��オ�t���Ă��Ȃ������ꍇ
            {
                moveName = "Solution";//�n���t���g�p����
                chargeEnabled = false;//�̓�������g�p�o���Ȃ�����
                moveCost = solutionCost;//�n���t�̃R�X�g��moveCost��ݒ肷��
                Debug.Log("�n���t");
            }
            if (chargeEnabled) //�̓����肪�g�p�\�ȏꍇ
            {
                moveName = "Charge";//�̓�������g�p����
                solutionEnabled = false;//�n���t���g�p�o���Ȃ�����
                moveCost = chargeCost;//�̓�����̃R�X�g��moveCost��ݒ肷��
                Debug.Log("�̓�����");
            }
            if (!chargeEnabled && !solutionEnabled)//�ǂ̋Z���g�p�s�̏ꍇ 
            {
                moveName = "RoundEnd";//�s�����I������
            }
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN) //�[�����m�̍s���p�^�[��
        {
            int slashCost = 2;//�؂肩����̃R�X�g
            int holdShieldCost = 3;//�����\����̃R�X�g
            int[] costArray = { slashCost, holdShieldCost };
            bool slashEnabled = false;//�؂肩���邪�g�p�\��
            bool holdShieldEnabled = false;//�����\���邪�g�p�\��
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
            if (holdShieldEnabled && enemy.GetSetEnemyGP == 0) //�����\���邪�g�p�\�ŃG�l�~�[�ɃK�[�h�����������ꍇ
            {
                moveName = "HoldShield";//�����\������g�p����
                slashEnabled = false;//�؂肩������g�p�o���Ȃ�����
                moveCost = holdShieldCost;//�����\����̃R�X�g��moveCost��ݒ肷��
                Debug.Log("�����\����");
            }
            if (slashEnabled) //�؂肩���邪�g�p�\�ȏꍇ
            {
                moveName = "Slash";//�؂肩������g�p����
                holdShieldEnabled = false;//�����\������g�p�o���Ȃ�����
                moveCost = slashCost;//�؂肩����̃R�X�g��moveCost��ݒ肷��
                Debug.Log("�؂肩����");
            }
            if (!slashEnabled && !holdShieldEnabled)//�ǂ̋Z���g�p�s�̏ꍇ 
            {
                moveName = "RoundEnd";
            }
        }
        else if (enemyState == EnemyState.NAGA) //�i�[�K�̍s���p�^�[��
        {
            int slashCost = 2;//�؂肩����̃R�X�g
            int creepySongCost = 3;//�s�C���ȉ̂̃R�X�g
            int[] costArray = { slashCost, creepySongCost };
            bool slashEnabled = false;//�؂肩���邪�g�p�\��
            bool creepySongEnabled = false;//�s�C���ȉ̂��g�p�\��
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
            if (creepySongEnabled && player.GetSetPlayerCondition.impatience == 0) //�s�C���ȉ̂��g�p�\�Ńv���C���[�ɏő����t���Ă��Ȃ������ꍇ
            {
                moveName = "CreepySong";//�s�C���ȉ̂��g�p����
                slashEnabled = false;//�؂肩������g�p�o���Ȃ�����
                moveCost = creepySongCost;//�s�C���ȉ̂̃R�X�g��moveCost��ݒ肷��
                Debug.Log("�s�C���ȉ�");
            }
            if (slashEnabled) //�؂肩���邪�g�p�\�ȏꍇ
            {
                moveName = "Slash";//�؂肩������g�p����
                creepySongEnabled = false;//�����\������g�p�o���Ȃ�����
                moveCost = slashCost;//�؂肩����̃R�X�g��moveCost��ݒ肷��
                Debug.Log("�؂肩����");
            }
            if (!slashEnabled && !creepySongEnabled)//�ǂ̋Z���g�p�s�̏ꍇ
            {
                moveName = "RoundEnd";
            }
        }
        else if (enemyState == EnemyState.CHIMERA) //�L�}�C���̍s���p�^�[��
        {
            int biteCost = 3;//���݂��̃R�X�g
            int rampageCost = 7;//�\����̃R�X�g
            int burningBreathCost = 4;//�R���鑧�̃R�X�g
            int snakeGlaresCost = 2;//���ɂ݂̃R�X�g
            int[] costArray = { biteCost, rampageCost, burningBreathCost, snakeGlaresCost };
            bool biteEnabled = false;//���݂����g�p�\��
            bool rampageEnabled = false;//�\���邪�g�p�\��
            bool burningBreathEnabled = false;//�R���鑧���g�p�\��
            bool snakeGlaresEnabled = false;//���ɂ݂��g�p�\��
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

            if (snakeGlaresEnabled && snakeGlaresCount == 0) //���ɂ݂��g�p�\�ň����g�p���Ă��Ȃ��ꍇ
            {
                snakeGlaresCount++;//�g�p�����񐔂𑝂₷
                moveName = "SnakeGlares";//���ɂ݂��g�p����
                biteEnabled = false;//?�݂����g�p�o���Ȃ�����
                rampageEnabled = false;//�\������g�p�o���Ȃ�����
                burningBreathEnabled = false;//�R���鑧���g�p�o���Ȃ�����
                moveCost = snakeGlaresCost;//���ɂ݂̃R�X�g��moveCost�ɐݒ肷��
                Debug.Log("���ɂ�");
            }
            else
            {
                snakeGlaresEnabled = false;//���ɂ݂��g�p�o���Ȃ�����
            }

            List<string> moveEnabledArray = new List<string>();//�g�p�\�ȋZ��List�ɕۑ�
            if (biteEnabled) //biteEnabled���g�p�\�ł����
            {
                moveEnabledArray.Add("Bite");//moveEnabledArray�ɒǉ�
            }
            if (rampageEnabled) //rampageEnabled���g�p�\�ł����
            {
                moveEnabledArray.Add("Rampage");//moveEnabledArray�ɒǉ�
            }
            if (burningBreathEnabled) //burningBreathEnabled���g�p�\�ł����
            {
                moveEnabledArray.Add("BurningBreath");//moveEnabledArray�ɒǉ�
            }
            if (moveEnabledArray.Count > 0)
            {
                int randSelectMove = Random.Range(0, moveEnabledArray.Count);//�g�p�ł���Z�̒����烉���_���ɋZ��I������

                switch (moveEnabledArray[randSelectMove])
                {
                    case "Bite":�@//�I�����ꂽ�s�����݂��������ꍇ
                        moveName = "Bite";//���݂����g�p����
                        rampageEnabled = false;//�\������g�p�o���Ȃ�����
                        burningBreathEnabled = false;//�R���鑧���g�p�o���Ȃ�����
                        moveCost = biteCost;//���݂��̃R�X�g��moveCost�ɐݒ肷��
                        Debug.Log("���݂�");
                        break;
                    case "Rampage": //�I�����ꂽ�s�����\���邾�����ꍇ
                        moveName = "Rampage";//�\������g�p����
                        biteEnabled = false;//���݂����g�p�o���Ȃ�����
                        burningBreathEnabled = false;//�R���鑧���g�p�o���Ȃ�����
                        moveCost = rampageCost;//�\����̃R�X�g��moveCost�ɐݒ肷��
                        Debug.Log("�\����");
                        break;
                    case "BurningBreath": //�I�����ꂽ�s�����R���鑧�������ꍇ
                        moveName = "BurningBreath";//�R���鑧���g�p����
                        biteEnabled = false;//���݂����g�p�o���Ȃ�����
                        rampageEnabled = false;//�\������g�p�o���Ȃ�����
                        moveCost = burningBreathCost; //�R���鑧�̃R�X�g��moveCost�ɐݒ肷��
                        Debug.Log("�R���鑧");
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            if (!snakeGlaresEnabled && moveEnabledArray.Count == 0) //�ǂ̋Z���g�p�s�̏ꍇ
            {
                moveName = "RoundEnd";
            }
        }
        else if (enemyState == EnemyState.DARKKNIGHT) //�Í��R�m�̍s���p�^�[��
        {
            int swingCost = 3;//�U�艺�낷�̃R�X�g
            int robustShieldCost = 3;//���S�ȏ��̃R�X�g
            int desperateLungeCost = 3;//�̂Đg�ːi�̃R�X�g
            int rampageCost = 7;//�\����̃R�X�g
            int[] costArray = { swingCost, robustShieldCost, desperateLungeCost, rampageCost };
            bool swingEnabled = false;//�U�艺�낷���g�p�\��
            bool robustShieldEnabled = false;//���S�ȏ����g�p�\��
            bool desperateLungeEnabled = false;//�̂Đg�ːi���g�p�\��
            bool rampageEnabled = false;//�\���邪�g�p�\��
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

            if (robustShieldEnabled && enemy.GetSetRoundEnabled == false) //���S�ȏ����g�p�\�Ń��E���h����1����g�p���Ă��Ȃ������ꍇ
            {
                enemy.GetSetRoundEnabled = true;//���E���h���Ɏg�p����
                moveName = "RobustShield";//���S�ȏ����g�p����
                swingEnabled = false;//�U�艺�낷���g�p�o���Ȃ�����
                desperateLungeEnabled = false;//�̂Đg�ːi���g�p�o���Ȃ�����
                rampageEnabled = false;//�\������g�p�o���Ȃ�����
                moveCost = robustShieldCost;//���S�ȏ��̃R�X�g��moveCost�ɐݒ肷��
            }
            else
            {
                robustShieldEnabled = false;
            }
            List<string> moveEnabledArray = new List<string>();//�g�p�\�ȋZ��List�ɕۑ�
            if (swingEnabled) //swingEnabled���g�p�\�ł����
            {
                moveEnabledArray.Add("Swing");//moveEnabledArray�ɒǉ�
            }
            if (desperateLungeEnabled && enemy.GetSetEnemyCurrentHP >= 15) //desperateLungeEnabled���g�p�\�ŃG�l�~�[�̌��݂�HP��15�ȏ�ł����
            {
                moveEnabledArray.Add("DesperateLunge");//moveEnabledArray�ɒǉ�
            }
            if (rampageEnabled) //burningBreathEnabled���g�p�\�ł����
            {
                moveEnabledArray.Add("Rampage");//moveEnabledArray�ɒǉ�
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
                        Debug.Log("�U�艺�낷");
                        break;
                    case "DesperateLunge":
                        moveName = "DesperateLunge";
                        swingEnabled = false;
                        rampageEnabled = false;
                        moveCost = desperateLungeCost;
                        Debug.Log("�̂Đg�ːi");
                        break;
                    case "Rampage":
                        moveName = "Rampage";
                        swingEnabled = false;
                        desperateLungeEnabled = false;
                        moveCost = rampageCost;
                        Debug.Log("�\����");
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
        else //�ǂ̃X�e�[�g�ł��Ȃ��ꍇ
        {
            moveName = "RoundEnd";
        }
        return (moveName, moveCost);
    }
    /// <summary>
    /// �Z�̌��ʏ���
    /// </summary>
    /// <param name="moveName">�Z�̖��O</param>
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
    ///�s�������Ƀ��E���h���I������
    /////�G�l�~�[�̌��݂�AP��0�ɂ���
    /// </summary>
    private void RoundEnd()
    {
        enemy.TurnEnd();
    }
    /// <summary>
    /// �X���C��:�̓�����
    ///�v���C���[��2�_���[�W�^����
    /// </summary>
    private void Charge()
    {
        player.TakeDamage(2);
    }
    /// <summary>
    /// �X���C��:�n���t
    /// �v���C���[�ɐ����1�^����
    /// </summary>
    private void Solution()
    {
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// �[�����m�ƃi�[�K:�؂肩����
    /// �v���C���[��4�_���[�W�^����
    /// </summary>
    private void Slash()
    {
        player.TakeDamage(4);
    }
    /// <summary>
    /// �[�����m:�����\����
    /// �K�[�h��2����
    /// </summary>
    private void HoldShield()
    {
        enemy.AddGP(2);
    }
    /// <summary>
    /// �i�[�K:�s�C���ȉ�
    /// �v���C���[�ɏő����P�^����
    /// </summary>
    private void CreepySong()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// �L�}�C��:���݂�
    /// �v���C���[��4�_���[�W�^����
    /// </summary>
    private void Bite()
    {
        player.TakeDamage(4);
    }
    /// <summary>
    /// �L�}�C���ƈÍ��R�m:�\����
    /// �v���C���[��2�_���[�W��3�`6��^����
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
    /// �L�}�C��:�R���鑧
    /// �v���C���[�ɉΏ���1�^����
    /// </summary>
    private void BurningBreath()
    {
        player.AddConditionStatus("Burn", 1);
    }
    /// <summary>
    /// �L�}�C��:���ɂ�
    /// �v���C���[�ɏő���1�^����
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
