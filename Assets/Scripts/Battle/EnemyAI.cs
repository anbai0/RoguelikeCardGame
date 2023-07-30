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
    bool isUsedOnlyMove; //�퓬�J�n���̂ݎg�p�\
    bool isUsableGodCrusher; //�_�ӂ����g�p�o���邩����
    private enum EnemyState //�G�l�~�[�̎��
    {
        SLIME1,                    //�X���C��(1�K�w)
        SKELETONSWORDSMAN1,        //�[�����m(1�K�w)
        NAGA1,                     //�i�[�K(1�K�w)
        CHIMERA1,                  //�L�}�C��(1�K�w)
        DARKKNIGHT1,               //�Í��R�m(1�K�w)
        CYCLOPS,                   //�T�C�N���v�X(1�K�w)
        SLIME2,                    //�X���C��(2�K�w)
        SKELETONSWORDSMAN2,        //�[�����m(2�K�w)
        NAGA2,                     //�i�[�K(2�K�w)
        CHIMERA2,                  //�L�}�C��(2�K�w)
        DARKKNIGHT2,               //�Í��R�m(2�K�w)
        SCYLLA,                    //�X�L����(2�K�w)
        SLIME3,                    //�X���C��(3�K�w)
        SKELETONSWORDSMAN3,        //�[�����m(3�K�w)
        NAGA3,                     //�i�[�K(3�K�w)
        CHIMERA3,                  //�L�}�C��(3�K�w)
        DARKKNIGHT3,               //�Í��R�m(3�K�w)
        MINOTAUR,                  //�~�m�^�E���X(3�K�w)
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
        string moveName = null; //�I�����ꂽ�Z�̖��O
        int moveCost = 0; //�I�����ꂽ�R�X�g�̖��O
        List<EnemyMove> enemyMove; //�G�l�~�[�̋Z���X�g

        enemyMove = SetMoveList(); //�Z���Z�b�g����
        enemyMove = CheckAP(enemyMove, currentAP); //���݂�AP�ɉ����Ďg����Z��I�o
        enemyMove = CheckEnemyMove(enemyMove); //�G�l�~�[�̏����ɉ����Ďg����Z��I�o
        var selectMove = SelectMove(enemyMove); //�g�p�ł���Z�̒����烉���_���őI������
        moveName = selectMove.name; //�I�����ꂽ�Z�̖��O����
        moveCost = selectMove.cost; //�I�����ꂽ�Z�̃R�X�g����
        return (moveName, moveCost);
    }
    public void SetEnemyState(int floor, string enemyName) //�G�l�~�[�̖��O�ƊK�w���ɉ�����EnemyAI�̃X�e�[�g��ύX����
    {
        if (enemyName == "�X���C��" && floor == 1)
        {
            enemyState = EnemyState.SLIME1;
        }
        else if (enemyName == "�X���C��" && floor == 2)
        {
            enemyState = EnemyState.SLIME2;
        }
        else if (enemyName == "�X���C��" && floor == 3)
        {
            enemyState = EnemyState.SLIME3;
        }
        else if (enemyName == "�[�����m" && floor == 1)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN1;
        }
        else if (enemyName == "�[�����m" && floor == 2)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN2;
        }
        else if (enemyName == "�[�����m" && floor == 3)
        {
            enemyState = EnemyState.SKELETONSWORDSMAN3;
        }
        else if (enemyName == "�i�[�K" && floor == 1)
        {
            enemyState = EnemyState.NAGA1;
        }
        else if (enemyName == "�i�[�K" && floor == 2)
        {
            enemyState = EnemyState.NAGA2;
        }
        else if (enemyName == "�i�[�K" && floor == 3)
        {
            enemyState = EnemyState.NAGA3;
        }
        else if (enemyName == "�L����" && floor == 1)
        {
            enemyState = EnemyState.CHIMERA1;
        }
        else if (enemyName == "�L����" && floor == 2)
        {
            enemyState = EnemyState.CHIMERA2;
        }
        else if (enemyName == "�L����" && floor == 3)
        {
            enemyState = EnemyState.CHIMERA3;
        }
        else if (enemyName == "�Í��R�m" && floor == 1)
        {
            enemyState = EnemyState.DARKKNIGHT1;
        }
        else if (enemyName == "�Í��R�m" && floor == 2)
        {
            enemyState = EnemyState.DARKKNIGHT2;
        }
        else if (enemyName == "�Í��R�m" && floor == 3)
        {
            enemyState = EnemyState.DARKKNIGHT3;
        }
        else if (enemyName == "�T�C�N���v�X" && floor == 1)
        {
            enemyState = EnemyState.CYCLOPS;
        }
        else if (enemyName == "�X�L����" && floor == 2)
        {
            enemyState = EnemyState.SCYLLA;
        }
        else if (enemyName == "�~�m�^�E���X" && floor == 3)
        {
            enemyState = EnemyState.MINOTAUR;
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
        else if (enemyState == EnemyState.SLIME3)
        {
            enemyMove.Add(new EnemyMove("Charge", false, 2)); //����������
            enemyMove.Add(new EnemyMove("Solution", false, 3)); //�n���t
            enemyMove.Add(new EnemyMove("Curing", false, 2)); //�d��
            enemyMove.Add(new EnemyMove("Venom", false, 3)); //�ŉt
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //�؂肩����
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //�����\����
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //�؂肩����
            enemyMove.Add(new EnemyMove("HoldShield", false, 2)); //�����\����
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //�\����
        }
        else if (enemyState == EnemyState.SKELETONSWORDSMAN3)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //�؂肩����
            enemyMove.Add(new EnemyMove("HoldShield", false, 3)); //�����\����
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //�\����
            enemyMove.Add(new EnemyMove("TearOfWeathering", false, 6)); //�����̈��
        }
        else if (enemyState == EnemyState.NAGA1)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //�؂肩����
            enemyMove.Add(new EnemyMove("CreepySong", false, 2)); //�s�C���ȉ�
        }
        else if (enemyState == EnemyState.NAGA2)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //�؂肩����
            enemyMove.Add(new EnemyMove("CreepySong", false, 2)); //�s�C���ȉ�
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //�։�
        }
        else if (enemyState == EnemyState.NAGA3)
        {
            enemyMove.Add(new EnemyMove("Slash", false, 3)); //�؂肩����
            enemyMove.Add(new EnemyMove("CreepySong", false, 2)); //�s�C���ȉ�
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //�։�
            enemyMove.Add(new EnemyMove("SwordDance", false, 3)); //���̕�
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //���݂�
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //�\����
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4)); //�R���鑧
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2)); //���ɂ�
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //���݂�
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //�\����
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4)); //�R���鑧
            enemyMove.Add(new EnemyMove("SnakeGlares", false, 2)); //���ɂ�
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //�։�
        }
        else if (enemyState == EnemyState.CHIMERA3)
        {
            enemyMove.Add(new EnemyMove("Bite", false, 3)); //���݂�
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //�\����
            enemyMove.Add(new EnemyMove("BurningBreath", false, 4)); //�R���鑧
            enemyMove.Add(new EnemyMove("SnakeFangs", false, 4)); //�։�
            enemyMove.Add(new EnemyMove("BattlePosture", false, 3)); //�퓬�Ԑ�
            enemyMove.Add(new EnemyMove("Shedding", false, 4)); //�E��
        }
        else if (enemyState == EnemyState.DARKKNIGHT1)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3)); //�U�艺�낷
            enemyMove.Add(new EnemyMove("RobustShield", false, 3)); //���S�Ȃ鏂
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3)); //�̂Đg�ːi
            enemyMove.Add(new EnemyMove("Rampage", false, 7)); //�\����
        }
        else if (enemyState == EnemyState.DARKKNIGHT2)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3)); //�U�艺�낷
            enemyMove.Add(new EnemyMove("RobustShield", false, 3)); //���S�Ȃ鏂
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3)); //�̂Đg�ːi
            enemyMove.Add(new EnemyMove("Rampage", false, 6)); //�\����
            enemyMove.Add(new EnemyMove("Encouragement", false, 4)); //�
        }
        else if (enemyState == EnemyState.DARKKNIGHT3)
        {
            enemyMove.Add(new EnemyMove("Swing", false, 3)); //�U�艺�낷
            enemyMove.Add(new EnemyMove("RobustShield", false, 1)); //���S�Ȃ鏂
            enemyMove.Add(new EnemyMove("DesperateLunge", false, 3)); //�̂Đg�ːi
            enemyMove.Add(new EnemyMove("Rampage", false, 6)); //�\����
            enemyMove.Add(new EnemyMove("Encouragement", false, 3)); //�
            enemyMove.Add(new EnemyMove("Decapitation", false, 5)); //�f��
        }
        else if (enemyState == EnemyState.CYCLOPS)
        {
            enemyMove.Add(new EnemyMove("SwingOver", false, 6)); //�U�肩�Ԃ�
            enemyMove.Add(new EnemyMove("GodCrusher", false, 0)); //�_�ӂ�
            enemyMove.Add(new EnemyMove("RandomPounding", false, 5)); //����ł�
            enemyMove.Add(new EnemyMove("GiantFist", false, 2)); //����
            enemyMove.Add(new EnemyMove("Rumbling", false, 4)); //���Ȃ炵
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            enemyMove.Add(new EnemyMove("CorrosivePoison", false, 3)); //�I��
            enemyMove.Add(new EnemyMove("OneDrop", false, 5)); //��H
            enemyMove.Add(new EnemyMove("Whipping", false, 2)); //�ڑ�
            enemyMove.Add(new EnemyMove("ChewAndCrush", false, 9)); //?�ݍӂ�
            enemyMove.Add(new EnemyMove("WrigglingTentacles", false, 4)); //忂��G��
            enemyMove.Add(new EnemyMove("Rumbling", false, 4)); //���Ȃ炵
        }
        else if (enemyState == EnemyState.MINOTAUR)
        {
            enemyMove.Add(new EnemyMove("MowDown", false, 3)); //�ガ����
            enemyMove.Add(new EnemyMove("Rampage", false, 5)); //�\����
            enemyMove.Add(new EnemyMove("Labyrinth", false, 10)); //���{
            enemyMove.Add(new EnemyMove("Throwing", false, 6)); //����
            enemyMove.Add(new EnemyMove("SkullSplit", false, 4)); //���W��
            enemyMove.Add(new EnemyMove("Rumbling", false, 4)); //���Ȃ炵
        }
        return enemyMove;
    }
    List<EnemyMove> CheckAP(List<EnemyMove> enemyMove, int currentAP) //�G�l�~�[�̋Z��AP�ȉ����`�F�b�N����
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
    List<EnemyMove> CheckEnemyMove(List<EnemyMove> enemyMove) //�G�l�~�[�̋Z���g�p�o���邩�`�F�b�N����
    {
        if (enemyState == EnemyState.SLIME1)
        {
            if (player.GetSetCondition.weakness > 0) //�v���C���[�ɐ��オ�t�^����Ă���Ȃ�
            {
                enemyMove[1].isUsable = false; //�n���t���g�p�s��
            }
        }
        else if (enemyState == EnemyState.SLIME2 || enemyState == EnemyState.SLIME2)
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
        else if (enemyState == EnemyState.SKELETONSWORDSMAN1 || enemyState == EnemyState.SKELETONSWORDSMAN2 || enemyState == EnemyState.SKELETONSWORDSMAN3)
        {
            if (enemy.GetSetGP > 0) //�G�l�~�[��GP������Ȃ�
            {
                enemyMove[1].isUsable = false; //�����\������g�p�s��
            }
        }
        else if (enemyState == EnemyState.NAGA1 || enemyState == EnemyState.NAGA2 || enemyState == EnemyState.NAGA3)
        {
            if (player.GetSetCondition.impatience > 0) //�v���C���[�ɏő����t�^����Ă���Ȃ�
            {
                enemyMove[1].isUsable = false;//�s�C���ȉ̂��g�p�s��
            }
        }
        else if (enemyState == EnemyState.CHIMERA1)
        {
            if (isUsedOnlyMove) //�퓬�ň�x�ł����ɂ݂��g�p���Ă���Ȃ�
            {
                enemyMove[3].isUsable = false; //���ɂ݂��g�p�s��
            }
            else //���ɂ݂��g�p���Ă��Ȃ��Ȃ�
            {
                //���ɂ݈ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                isUsedOnlyMove = true; //���ɂ݂̎g�p�����Ɣ���
            }
        }
        else if (enemyState == EnemyState.CHIMERA2)
        {
            if (isUsedOnlyMove) //�퓬�ň�x�ł����ɂ݂��g�p���Ă���Ȃ�
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
                isUsedOnlyMove = true; //���ɂ݂̎g�p�����Ɣ���
            }
        }
        else if (enemyState == EnemyState.CHIMERA3)
        {
            if (isUsedOnlyMove) //�퓬�ň�x�ł��퓬�Ԑ����g�p���Ă���Ȃ�
            {
                enemyMove[4].isUsable = false; //�퓬�Ԑ����g�p�s��
            }
            else //�퓬�Ԑ����g�p���Ă��Ȃ��Ȃ�
            {
                //�퓬�Ԑ��ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[2].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[5].isUsable = false;
                isUsedOnlyMove = true; //�퓬�Ԑ��̎g�p�����Ɣ���
            }

            if (enemy.CheckBadStatus() == 0) //�G�l�~�[�Ƀo�b�g�X�e�[�^�X���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[5].isUsable = false; //�E����g�p�s��
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
        else if (enemyState == EnemyState.DARKKNIGHT3)
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
                enemyMove[5].isUsable = false;
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

            if (player.CheckBuffStatus() == 0) //�v���C���[�Ƀo�t���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[5].isUsable = false; //�f�����g�p�s��
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

            if (player.CheckBuffStatus() == 0 && player.CheckBadStatus() == 0 && enemy.CheckBuffStatus() == 0 && enemy.CheckBadStatus() == 0) //�v���C���[�ƃG�l�~�[�Ƀo�t��f�o�t���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[4].isUsable = false; //���Ȃ炵���g�p�s��
            }
        }
        else if (enemyState == EnemyState.SCYLLA)
        {
            if (enemy.GetSetCurrentHP > 40) //�G�l�~�[�̌��݂�HP��40������Ȃ�
            {
                enemyMove[1].isUsable = false; //��H���g�p�s��
            }

            if (player.GetSetGP > 0) //�v���C���[���K�[�h�������Ă���Ȃ�
            {
                enemyMove[3].isUsable = false; //?�ݍӂ����g�p�s��
            }

            if (player.CheckBuffStatus() == 0 && enemy.CheckBadStatus() == 0) //�v���C���[�Ƀo�t���t�^����ĂȂ��A�܂��̓G�l�~�[�Ƀf�o�t���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[5].isUsable = false; //���Ȃ炵���g�p�s��
            }
        }
        else if (enemyState == EnemyState.MINOTAUR)
        {
            if (isUsedOnlyMove) //�퓬�ň�x�ł����{���g�p���Ă���Ȃ�
            {
                enemyMove[2].isUsable = false; //���{���g�p�s��
            }
            else //���{���g�p���Ă��Ȃ��Ȃ�
            {
                //���{�ȊO���g�p�s��
                enemyMove[0].isUsable = false;
                enemyMove[1].isUsable = false;
                enemyMove[3].isUsable = false;
                enemyMove[4].isUsable = false;
                enemyMove[5].isUsable = false;
                isUsedOnlyMove = true; //���{�̎g�p�����Ɣ���
            }
            if (enemy.GetSetCurrentHP > 50) //�G�l�~�[�̌��݂�HP��50������Ȃ�
            {
                enemyMove[3].isUsable = false; //�������g�p�s��
            }

            if (player.GetSetGP == 0) //�v���C���[���K�[�h�������Ă��Ȃ��Ȃ�
            {
                enemyMove[4].isUsable = false; //���W�����g�p�s��
            }

            if (player.CheckBuffStatus() == 0 && enemy.CheckBadStatus() == 0) //�v���C���[�Ƀo�t���t�^����ĂȂ��A�܂��̓G�l�~�[�Ƀf�o�t���t�^����Ă��Ȃ��Ȃ�
            {
                enemyMove[5].isUsable = false; //���Ȃ炵���g�p�s��
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
    ///�s�������Ƀ��E���h���I������B
    /// </summary>
    private void RoundEnd()
    {
        enemy.TurnEnd();
    }
    /// <summary>
    /// �Z���F�̓�����
    /// �g�p�ҁF�X���C��(1�K�w),�X���C��(2�K�w)
    /// ���ʁF�v���C���[��2�_���[�W�^����B
    /// �g�p�ҁF�X���C��(3�K�w)
    /// ���ʁF�v���C���[��3�_���[�W�^����B
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
    /// �Z���F�n���t
    /// �g�p�ҁF�X���C��(1�K�w),�X���C��(2�K�w),�X���C��(3�K�w)
    /// ���ʁF�v���C���[�ɐ����1�^����B
    /// </summary>
    private void Solution()
    {
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// �Z���F�d��
    /// �g�p�ҁF�X���C��(2�K�w),�X���C��(3�K�w)
    /// ���ʁF4�K�[�h�𓾂�B��Ԉُ햳����2����B
    /// </summary>
    private void Curing()
    {
        enemy.AddGP(4);
        enemy.AddConditionStatus("InvalidBadStatus", 2);
    }
    /// <summary>
    /// �Z���F�ŉt
    /// �g�p�ҁF�X���C��(3�K�w)
    /// ���ʁF�v���C���[�Ɏדł�1�^����B
    /// </summary>
    private void Venom()
    {
        player.AddConditionStatus("Poison", 1);
    }
    /// <summary>
    /// �Z���F�؂肩����
    /// �g�p�ҁF�[�����m(1�K�w),�[�����m(2�K�w),�i�[�K(1�K�w),�i�[�K(2�K�w)
    /// ���ʁF�v���C���[��4�_���[�W�^����B
    /// �g�p�ҁF�[�����m(3�K�w),�i�[�K(3�K�w)
    /// ���ʁF�v���C���[��5�_���[�W�^����B
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
    /// �Z���F�����\����
    /// �g�p�ҁF�[�����m(1�K�w),�[�����m(2�K�w)
    /// ���ʁF�K�[�h��2����B
    /// �g�p�ҁF�[�����m(3�K�w)
    /// ���ʁF�K�[�h��5����B
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
    /// �Z���F�����̈��
    /// �g�p�ҁF�[�����m(3�K�w)
    /// ���ʁF�v���C���[��3�_���[�W��^���A�����1�^����B
    /// </summary>
    private void TearOfWeathering()
    {
        EnemyAttacking(3);
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// �Z���F�s�C���ȉ�
    /// �g�p�ҁF�i�[�K(1�K�w),�i�[�K(2�K�w),�i�[�K(3�K�w)
    /// ���ʁF�v���C���[�ɏő����P�^����B
    /// </summary>
    private void CreepySong()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// �Z���F�։�
    /// �g�p�ҁF�i�[�K(2�K�w),�i�[�K(3�K�w),�L�}�C��(2�K�w),�L�}�C��(3�K�w),
    /// ���ʁF�v���C���[��3�_���[�W�^���A�דł�1�^����B
    /// </summary>
    private void SnakeFangs()
    {
        EnemyAttacking(3);
        player.AddConditionStatus("Poison", 1);
    }
    /// <summary>
    /// �Z���F���̕�
    /// �g�p�ҁF�i�[�K(3�K�w)
    /// ���ʁF�ؗ͑�����1����B
    /// </summary>
    private void SwordDance()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// �Z���F���݂�
    /// �g�p�ҁF�L�}�C��(1�K�w),�L�}�C��(2�K�w),�L�}�C��(3�K�w)
    /// ���ʁF�v���C���[��4�_���[�W�^����B
    /// </summary>
    private void Bite()
    {
        EnemyAttacking(4);
    }
    /// <summary>
    /// �Z���F�\����
    /// �g�p�ҁF�[�����m(2�K�w),�[�����m(3�K�w),�Í��R�m(1�K�w),�Í��R�m(2�K�w),�Í��R�m(3�K�w)
    /// ���ʁF�v���C���[��2�_���[�W��3�`5��^����B
    /// �g�p�ҁF�L�}�C��(1�K�w),�L�}�C��(2�K�w),�L�}�C��(3�K�w)
    /// ���ʁF�v���C���[��2�_���[�W��3�`6��^����B
    /// �g�p�ҁF�~�m�^�E���X(3�K�w)
    /// ���ʁF�v���C���[��2�_���[�W��2�`8��^����B
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
    /// �Z���F�R���鑧
    /// �g�p�ҁF�L�}�C��(1�K�w),�L�}�C��(2�K�w),�L�}�C��(3�K�w)
    /// ���ʁF�v���C���[�ɉΏ���1�^����B
    /// </summary>
    private void BurningBreath()
    {
        player.AddConditionStatus("Burn", 1);
    }
    /// <summary>
    /// �Z���F���ɂ�
    /// �g�p�ҁF�L�}�C��(1�K�w),�L�}�C��(2�K�w)
    /// ���ʁF�v���C���[�ɏő���1�^����B
    /// </summary>
    private void SnakeGlares()
    {
        player.AddConditionStatus("Impatience", 1);
    }
    /// <summary>
    /// �Z���F�퓬�Ԑ�
    /// �g�p�ҁF�L�}�C��(3�K�w)
    /// ���ʁF�ؗ͑�����1����B
    /// </summary>
    private void BattlePosture()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// �Z���F�E��
    /// �g�p�ҁF�L�}�C��(3�K�w)
    /// ���ʁF�f�o�t�����ׂĉ�������B
    /// </summary>
    private void Shedding()
    {
        enemy.ReleaseBadStatus();
    }
    /// <summary>
    /// �Z���F�U�艺�낷
    /// �g�p�ҁF�Í��R�m(1�K�w),�Í��R�m(2�K�w)
    /// ���ʁF�v���C���[��5�_���[�W��^����B
    /// �g�p�ҁF�Í��R�m(3�K�w)
    /// ���ʁF�v���C���[��6�_���[�W��^����B
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
    /// �Z���F���S�Ȃ鏂
    /// �g�p�ҁF�Í��R�m(1�K�w),�Í��R�m(2�K�w),�Í��R�m(3�K�w)
    /// ���ʁF�K�[�h��4����B
    /// </summary>
    private void RobustShield()
    {
        enemy.AddGP(4);
    }
    /// <summary>
    /// �Í��R�m�F�̂Đg�ːi
    /// �g�p�ҁF�Í��R�m(1�K�w),�Í��R�m(2�K�w),�Í��R�m(3�K�w)
    /// ���ʁF�v���C���[��7�_���[�W��^���A���g�ɂ�4�_���[�W��^����B
    /// </summary>
    private void DesperateLunge()
    {
        EnemyAttacking(7);
        enemy.TakeDamage(4);
    }
    /// <summary>
    /// �Z���F�
    /// �g�p�ҁF�Í��R�m(2�K�w),�Í��R�m(3�K�w)
    /// ���ʁF�ؗ͑�����1����B
    /// </summary>
    private void Encouragement()
    {
        enemy.AddConditionStatus("UpStrength", 1);
    }
    /// <summary>
    /// �Z���F�f��
    /// �g�p�ҁF�Í��R�m(3�K�w)
    /// ���ʁF�v���C���[��7�_���[�W��^���A�v���C���[�̃o�t�����ׂĉ�������B
    /// </summary>
    private void Decapitation()
    {
        EnemyAttacking(7);
        player.ReleaseBuffStatus();
    }
    /// <summary>
    /// �Z���F�U�肩�Ԃ�
    /// �g�p�ҁF�T�C�N���v�X(1�K�w)
    /// ���ʁF�K�[�h��10����B
    /// </summary>
    private void SwingOver()
    {
        enemy.AddGP(10);
    }
    /// <summary>
    /// �Z���F�_�ӂ�
    /// �g�p�ҁF�T�C�N���v�X(1�K�w)
    /// ���ʁF�K�[�h��3�{�̃_���[�W��^����B�K�[�h�����ׂĎ����B
    /// </summary>
    private void GodCrusher()
    {
        int damage = enemy.GetSetGP * 3;
        EnemyAttacking(damage);
        enemy.GetSetGP = 0;
    }
    /// <summary>
    /// �Z���F����ł�
    /// �g�p�ҁF�T�C�N���v�X(1�K�w)
    /// ���ʁF�v���C���[��5�_���[�W��0�`3��^����B
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
    /// �Z���F����
    /// �g�p�ҁF�T�C�N���v�X(1�K�w)
    /// ���ʁF�v���C���[��4�_���[�W��^����B
    /// </summary>
    private void GiantFist()
    {
        EnemyAttacking(4);
    }
    /// <summary>
    /// �Z���F���Ȃ炵
    /// �g�p�ҁF�T�C�N���v�X(1�K�w),�X�L����(2�K�w),�~�m�^�E���X(3�K�w)
    /// ���ʁF���݂��̃o�t�A�f�o�t�����ׂĉ�������B
    /// </summary>
    private void Rumbling()
    {
        player.ReleaseBuffStatus();
        player.ReleaseBadStatus();
        enemy.ReleaseBuffStatus();
        enemy.ReleaseBadStatus();
    }
    /// <summary>
    /// �Z���F�I��
    /// �g�p�ҁF�X�L����(2�K�w)
    /// ���ʁF�v���C���[�Ɏדł�2�^����B
    /// </summary>
    private void CorrosivePoison()
    {
        player.AddConditionStatus("Poison", 2);
    }
    /// <summary>
    /// �Z���F��H
    /// �g�p�ҁF�X�L����(2�K�w)
    /// ���ʁFHP��10�񕜂���B
    /// </summary>
    private void OneDrop()
    {
        enemy.HealingHP(10);
    }
    /// <summary>
    /// �Z���F�ڑ�
    /// �g�p�ҁF�X�L����(2�K�w)
    /// ���ʁF�v���C���[��3�_���[�W��^����B
    /// </summary>
    private void Whipping()
    {
        EnemyAttacking(3);
    }
    /// <summary>
    /// �Z���F?�ݍӂ�
    /// �g�p�ҁF�X�L����(2�K�w)
    /// ���ʁF3�_���[�W��6��^����B
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
    /// �Z���F忂��G��
    /// �g�p�ҁF�X�L����(2�K�w)
    /// ���ʁF�v���C���[��3�_���[�W��^���A�����A�ő��A����A�דł̂����ꂩ��^����B
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
    /// �Z���F�ガ����
    /// �g�p�ҁF�~�m�^�E���X(3�K�w)
    /// ���ʁF�v���C���[��6�_���[�W��^����B
    /// </summary>
    private void MowDown()
    {
        EnemyAttacking(6);
    }
    /// <summary>
    /// �Z���F���{
    /// �g�p�ҁF�~�m�^�E���X(3�K�w)
    /// ���ʁF�v���C���[�Ɏ����A�ő��A������P���^����B
    /// </summary>
    private void Labyrinth()
    {
        player.AddConditionStatus("Curse", 1);
        player.AddConditionStatus("Impatience", 1);
        player.AddConditionStatus("Weakness", 1);
    }
    /// <summary>
    /// �Z���F����
    /// �g�p�ҁF�~�m�^�E���X(3�K�w)
    /// ���ʁF�v���C���[��3�_���[�W��5��^����B
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
    /// �Z���F���W��
    /// �g�p�ҁF�~�m�^�E���X(3�K�w)
    /// ���ʁF�v���C���[�̃K�[�h�����ׂĉ������A4�_���[�W��^����B
    /// </summary>
    private void SkullSplit()
    {
        player.GetSetGP = 0;
        EnemyAttacking(4);
    }

    private void EnemyAttacking(int damage)//�G�l�~�[�ւ̍U������ 
    {
        damage = ChangeAttackPower(damage);
        Debug.Log("�v�Z��̍U���͂�" + damage);
        player.TakeDamage(damage);
    }
    private int ChangeAttackPower(int damage) //��Ԉُ�ɂ��U���͂̑���
    {
        damage = enemy.UpStrength(damage);
        damage = enemy.Weakness(damage);
        return damage;
    }
}
