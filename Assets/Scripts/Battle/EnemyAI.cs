using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove //�G�l�~�[�̍s���N���X
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

    //�g�p�����̂���Z�̕ϐ�
    int snakeGlaresCount; //���ɂ݂̉�
    bool isUsableGodCrusher; //�_�ӂ����g�p�o���邩����
    //Debug�p�ϐ�
    int floor = 1;
    private enum EnemyState //�G�l�~�[�̎��
    {
        SLIME1,                    //�X���C��(1�K�w��)
        SKELETONSWORDSMAN1,        //�[�����m(1�K�w��)
        NAGA1,                     //�i�[�K(1�K�w��)
        CHIMERA1,                  //�L�}�C��(1�K�w��)
        DARKKNIGHT1,               //�Í��R�m(1�K�w��)
        CYCLOPS,                   //�T�C�N���v�X(1�K�w��)
        SLIME2,                    //�X���C��(2�K�w��)
        SKELETONSWORDSMAN2,        //�[�����m(2�K�w��)
        NAGA2,                     //�i�[�K(2�K�w��)
        CHIMERA2,                  //�L�}�C��(2�K�w��)
        DARKKNIGHT2,               //�Í��R�m(2�K�w��)
        SCYLLA,                    //�X�L����(2�K�w��) 
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
        string moveName = null; //�I�����ꂽ�Z�̖��O
        int moveCost = 0; //�I�����ꂽ�R�X�g�̖��O
        List<EnemyMove> enemyMove; //�G�l�~�[�̋Z���X�g

        enemyMove = SetMoveList(); //�Z���Z�b�g����
        enemyMove = APCheck(enemyMove, currentAP); //���݂�AP�ɉ����Ďg����Z��I�o
        enemyMove = CheckEnemyMove(enemyMove); //�G�l�~�[�̏����ɉ����Ďg����Z��I�o
        var selectMove = SelectMove(enemyMove); //�g�p�ł���Z�̒����烉���_���őI������
        moveName = selectMove.name; //�I�����ꂽ�Z�̖��O����
        moveCost = selectMove.cost; //�I�����ꂽ�Z�̃R�X�g����
        return (moveName, moveCost);
    }
    public void SetEnemyState(string enemyName) //�G�l�~�[�̖��O�ƊK�w���ɉ�����EnemyAI�̃X�e�[�g��ύX����
    {
        //floor = GameManager.instance.GetFloor;
        if (enemyName == "�X���C��" && floor == 1)
        {
            enemyState = EnemyState.SLIME1;
        }
        else if (enemyName == "�X���C��" && floor == 2)
        {
            enemyState = EnemyState.SLIME2;
        }
        else if (enemyName == "�[�����m" && floor == 1)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN1;
        }
        else if (enemyName == "�[�����m" && floor == 2)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN2;
        }
        else if (enemyName == "�i�[�K" && floor == 1)
        {
            enemyState = EnemyState.NAGA1;
        }
        else if (enemyName == "�i�[�K" && floor == 2)
        {
            enemyState = EnemyState.NAGA2;
        }
        else if (enemyName == "�L����" && floor == 1)
        {
            enemyState = EnemyState.CHIMERA1;
        }
        else if (enemyName == "�L����" && floor == 2)
        {
            enemyState = EnemyState.CHIMERA2;
        }
        else if (enemyName == "�Í��R�m" && floor == 1)
        {
            enemyState = EnemyState.DARKKNIGHT1;
        }
        else if (enemyName == "�Í��R�m" && floor == 2)
        {
            enemyState = EnemyState.DARKKNIGHT2;
        }
        else if (enemyName == "�T�C�N���v�X" && floor == 1)
        {
            enemyState = EnemyState.CYCLOPS;
        }
        else if (enemyName == "�X�L����" && floor == 2)
        {
            enemyState = EnemyState.SCYLLA;
        }

    }
    List<EnemyMove> SetMoveList() //�G�l�~�[�̋Z�����X�g�Ƃ��ăZ�b�g����
    {
        List<EnemyMove> enemyMove = new List<EnemyMove>();
        if (enemyState == EnemyState.SLIME1)
        {
            enemyMove.Add(new EnemyMove("Charge", false, 2)); //����������
            enemyMove.Add(new EnemyMove("Solution", false, 3)); //�n���t
        }
        else if (enemyState == EnemyState.SLIME2)
        {
            enemyMove.Add(new EnemyMove("Charge", false, 2)); //����������
            enemyMove.Add(new EnemyMove("Solution", false, 3)); //�n���t
            enemyMove.Add(new EnemyMove("Curing", false, 2)); //�d��
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //�؂肩����
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //�����\����
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //�؂肩����
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //�����\����
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//�\����
        }
        else if (enemyState == EnemyState.NAGA1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //�؂肩����
            enemyMove.Add(new EnemyMove("CreepySong", false, 3)); //�s�C���ȉ�
        }
        else if (enemyState == EnemyState.NAGA2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 2)); //�؂肩����
            enemyMove.Add(new EnemyMove("CreepySong", false, 3)); //�s�C���ȉ�
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //�։�
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //���݂�
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//�\����
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4));//�R���鑧
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2));//���ɂ�
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //���݂�
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//�\����
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4));//�R���鑧
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2));//���ɂ�
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //�։�
        }
        else if (enemyState == EnemyState.DARKKNIGHT1)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3));//�U�艺�낷
            enemyMove.Add(new EnemyMove("RobustShield", false, 3));//���S�Ȃ鏂
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3));//�̂Đg�ːi
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//�\����
        }
        else if (enemyState == EnemyState.DARKKNIGHT2)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3));//�U�艺�낷
            enemyMove.Add(new EnemyMove("RobustShield", false, 3));//���S�Ȃ鏂
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3));//�̂Đg�ːi
            enemyMove.Add(new EnemyMove("Rampage", false, 7));//�\����
            enemyMove.Add(new EnemyMove("Encouragement", false, 4));//�
        }
        else if (enemyState == EnemyState.CYCLOPS)
        {
            enemyMove.Add(new EnemyMove("SwingOver", false, 6));//�U�肩�Ԃ�
            enemyMove.Add(new EnemyMove("GodCrusher", false, 0));//�_�ӂ�
            enemyMove.Add(new EnemyMove("RandomPounding", false, 5));//����ł�
            enemyMove.Add(new EnemyMove("GiantFist", false, 2));//����
            enemyMove.Add(new EnemyMove("Rumbling", false, 4));//���Ȃ炵
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            enemyMove.Add(new EnemyMove("SwingOver", false, 6));//�U�肩�Ԃ�
            enemyMove.Add(new EnemyMove("GodCrusher", false, 0));//�_�ӂ�
            enemyMove.Add(new EnemyMove("RandomPounding", false, 5));//����ł�
            enemyMove.Add(new EnemyMove("WrigglingTentacles", false, 4));//忂��G��
            enemyMove.Add(new EnemyMove("Rumbling", false, 4));//���Ȃ炵
        }
        return enemyMove;
    }
    List<EnemyMove> CheckEnemyMove(List<EnemyMove> enemyMove) //�G�l�~�[�̋Z���g�p�o���邩�`�F�b�N����
    {
        if (enemyState == EnemyState.SLIME1)
        {
            if (player.GetSetCondition.weakness > 0) //�v���C���[�ɐ��オ�t�^����Ă���Ȃ�
            {
                enemyMove[1].isUsable = false; //�n���t���g�p�s��
            }
        }
        else if (enemyState == EnemyState.SLIME2)
        {
            if (player.GetSetCondition.weakness > 0) //�v���C���[�ɐ��オ�t�^����Ă�Ȃ�
            {
                enemyMove[1].isUsable = false; //�n���t���g�p�s��
            }
            if (enemy.GetSetGP > 0) //�G�l�~�[��GP������Ȃ�
            {
                enemyMove[2].isUsable = false; //�d�����g�p�s��
            }
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1)
        {
            if (enemy.GetSetGP > 0) //�G�l�~�[��GP������Ȃ�
            {
                enemyMove[1].isUsable = false; //�����\������g�p�s��
            }
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN2)
        {
            if (enemy.GetSetGP > 0) //�G�l�~�[��GP������Ȃ�
            {
                enemyMove[1].isUsable = false; //�����\������g�p�s��
            }
        }
        else if (enemyState == EnemyState.NAGA1)
        {
            if (player.GetSetCondition.impatience > 0) //�v���C���[�ɏő����t�^����Ă���Ȃ�
            {
                enemyMove[1].isUsable = false;//�s�C���ȉ̂��g�p�s��
            }
        }
        else if (enemyState == EnemyState.NAGA2)
        {
            if (player.GetSetCondition.impatience > 0) //�v���C���[�ɏő����t�^����Ă���Ȃ�
            {
                enemyMove[1].isUsable = false;//�s�C���ȉ̂��g�p�s��
            }
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            if (snakeGlaresCount > 0) //�퓬�ň�x�ł����ɂ݂��g�p���Ă���Ȃ�
            {
                enemyMove[3].isUsable = false; //���ɂ݂��g�p�s��
            }
            else //���ɂ݂��g�p���Ă��Ȃ��Ȃ�
            {
                //���ɂ݈ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                snakeGlaresCount++; //���ɂ݂̎g�p�񐔂��J�E���g����
            }
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            if (snakeGlaresCount > 0) //�퓬�ň�x�ł����ɂ݂��g�p���Ă���Ȃ�
            {
                enemyMove[3].isUsable = false; //���ɂ݂��g�p�s��
            }
            else //���ɂ݂��g�p���Ă��Ȃ��Ȃ�
            {
                //���ɂ݈ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[4].isUsable = false;
                snakeGlaresCount++; //���ɂ݂̎g�p�񐔂��J�E���g����
            }
        }
        else if (enemyState == EnemyState.DARKKNIGHT1)
        {
            if (enemy.GetSetRoundEnabled == true) //���E���h���Ɍ��S�Ȃ鏂���g�p���Ă���Ȃ�
            {
                enemyMove[1].isUsable = false; //���S�Ȃ鏂���g�p�s��
            }
            else //���E���h���Ɍ��S�Ȃ鏂���g�p���Ă��Ȃ��Ȃ�
            {
                //���S�Ȃ鏂�ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemy.GetSetRoundEnabled = true; //���E���h���͌��S�Ȃ鏂���g�p�o���Ȃ�����
            }

            if (enemy.GetSetCurrentHP < 15) //�G�l�~�[�̌��݂�HP��15�����Ȃ�
            {
                enemyMove[2].isUsable = false; //�̂Đg�ːi���g�p�s��
            }
        }
        else if (enemyState == EnemyState.DARKKNIGHT2)
        {
            if (enemy.GetSetRoundEnabled == true) //���E���h���Ɍ��S�Ȃ鏂���g�p���Ă���Ȃ�
            {
                enemyMove[1].isUsable = false; //���S�Ȃ鏂���g�p�s��
            }
            else //���E���h���Ɍ��S�Ȃ鏂���g�p���Ă��Ȃ��Ȃ�
            {
                //���S�Ȃ鏂�ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                enemy.GetSetRoundEnabled = true; //���E���h���͌��S�Ȃ鏂���g�p�o���Ȃ�����
            }

            if (enemy.GetSetCurrentHP < 15) //�G�l�~�[�̌��݂�HP��15�����Ȃ�
            {
                enemyMove[2].isUsable = false; //�̂Đg�ːi���g�p�s��
            }

            if (enemy.GetSetCondition.upStrength > 0) //�G�l�~�[�ɋؗ͑������t�^����Ă���Ȃ� 
            {
                enemyMove[4].isUsable = false; //騂��g�p�s��
            }
        }
        else if (enemyState == EnemyState.CYCLOPS)
        {
            if (isUsableGodCrusher == true) //�_�ӂ����g�p�\�Ȃ�
            {
                //�_�ӂ��ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                isUsableGodCrusher = false; //�_�ӂ��̎g�p�����߂�
            }
            else
            {
                enemyMove[1].isUsable = false; //�_�ӂ����g�p�s��
            }

            if (player.CheckBadStatus() == 0 && enemy.CheckBadStatus() == 0) //�v���C���[�ƃG�l�~�[�Ƀo�t��f�o�t���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[4].isUsable = false; //���Ȃ炵���g�p�s��
            }
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            if (isUsableGodCrusher == true) //�_�ӂ����g�p�\�Ȃ�
            {
                //�_�ӂ��ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                isUsableGodCrusher = false; //�_�ӂ��̎g�p�����߂�
            }
            else
            {
                enemyMove[1].isUsable = false; //�_�ӂ����g�p�s��
            }

            if (player.CheckBadStatus() == 0 && enemy.CheckBadStatus() == 0) //�v���C���[�ƃG�l�~�[�Ƀo�t��f�o�t���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[4].isUsable = false; //���Ȃ炵���g�p�s��
            }
        }

        return enemyMove;
    }
    List<EnemyMove> APCheck(List<EnemyMove> enemyMove, int currentAP) //�G�l�~�[�̋Z��AP�ȉ����`�F�b�N����
    {
        foreach (var move in enemyMove) //�G�l�~�[�̋Z��S�T�� 
        {
            if (move.moveCost <= currentAP) //�R�X�g�����݂�AP�ȉ��ł����
            {
                move.isUsable = true; //�g�p�\�ɂ���
            }
        }
        return enemyMove;
    }
    (string name, int cost) SelectMove(List<EnemyMove> enemyMove) //�g�p����Z��I��
    {
        List<(string, int)> moveUsabledList = new List<(string, int)>(); //�Z�̖��O�ƃR�X�g���i�[�ł��郊�X�g
        foreach (var move in enemyMove) //�g�p�\�ȋZ��moveUsabledList�ɒǉ�����
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
        //�����_���ɋZ��I��
        int rand = Random.Range(0, moveUsabledList.Count);

        if (moveUsabledList[rand].Item1 == "SwingOver") //�T�C�N���v�X��p�̐_�ӂ��g�p����
        {
            isUsableGodCrusher = true;
        }
        return moveUsabledList[rand];
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
    /// �X���C��:�d��
    /// 4�K�[�h�𓾂�B��Ԉُ햳����2����B
    /// </summary>
    private void Curing()
    {
        enemy.AddGP(4);
        enemy.AddConditionStatus("InvalidBadStatus", 2);
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
    /// �i�[�K�ƃL�}�C��:�։�
    /// �v���C���[��3�_���[�W�^���A�דł�1�^����B
    /// </summary>
    private void SnakeFangs()
    {
        player.TakeDamage(3);
        player.AddConditionStatus("Poison", 1);
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
    /// <summary>
    /// �Í��R�m�F�U�艺�낷
    /// �v���C���[��5�_���[�W��^����B
    /// </summary>
    private void Swing()
    {
        player.TakeDamage(5);
    }
    /// <summary>
    /// �Í��R�m�F���S�Ȃ鏂
    /// �K�[�h��4����B
    /// </summary>
    private void RobustShield()
    {
        enemy.AddGP(4);
    }
    /// <summary>
    /// �Í��R�m�F�̂Đg�ːi
    /// �v���C���[��7�_���[�W��^���A���g�ɂ�4�_���[�W�B
    /// </summary>
    private void DesperateLunge()
    {
        player.TakeDamage(7);
        enemy.TakeDamage(4);
    }
    /// <summary>
    /// �Í��R�m�F�
    /// �ؗ͑�����1����
    /// </summary>
    private void Encouragement()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// �T�C�N���v�X�ƃX�L�����F�U�肩�Ԃ�
    /// �K�[�h��10����B
    /// </summary>
    private void SwingOver()
    {
        enemy.AddGP(10);
    }
    /// <summary>
    /// �T�C�N���v�X�ƃX�L�����F�_�ӂ�
    /// �K�[�h��3�{�̃_���[�W��^����B�K�[�h�����ׂĎ����B
    /// </summary>
    private void GodCrusher()
    {
        int damage = enemy.GetSetGP * 3;
        player.TakeDamage(damage);
        enemy.GetSetGP = 0;
    }
    /// <summary>
    /// �T�C�N���v�X�ƃX�L�����F����ł�
    /// �v���C���[��5�_���[�W��0�`3��^����B
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
    /// �T�C�N���v�X�F����
    /// �v���C���[��4�_���[�W��^����B
    /// </summary>
    private void GiantFist()
    {
        player.TakeDamage(4);
    }
    /// <summary>
    /// �T�C�N���v�X�ƃX�L�����F���Ȃ炵
    /// ���݂��̃o�t�A�f�o�t�����ׂĉ�������B
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
    /// �X�L�����F忂��G��
    /// �v���C���[��3�_���[�W��^���A�����A�ő��A����A�דł̂����ꂩ��^����B
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
