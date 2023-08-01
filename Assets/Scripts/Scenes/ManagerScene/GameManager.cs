using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�v���C���[
    GameObject player;
    public PlayerDataManager playerData;

    bool isAlreadyRead = false; // ReadPlayer�œǂݍ��񂾂��𔻒肷��

    //�V���O���g��
    public static GameManager Instance;
    private void Awake()
    {
        // �V���O���g���C���X�^���X���Z�b�g�A�b�v
        if (Instance == null)
        {
            Instance = this;
        }

        // �e�V�[���Ńf�o�b�O����Ƃ��ɃR�����g���������Ă�������
        // ��x���ǂݍ���ł��Ȃ����
        if (!isAlreadyRead) ReadPlayer("Warrior");

    }



    public void ReadPlayer(string playerJob)
    {
        isAlreadyRead = true;
        if (playerJob == "Warrior")
        {
            playerData = new PlayerDataManager("Warrior");
        }
        else if (playerJob == "Wizard")
        {
            playerData = new PlayerDataManager("Wizard");
        }
    }
}
