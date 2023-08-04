using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�v���C���[
    GameObject player;
    public PlayerDataManager playerData;
    public RelicDataManager relicData;
    public Dictionary<int, int> hasRelics = new Dictionary<int, int>();     // �������Ă��郌���b�N���i�[
    public Dictionary<string, int> e = new Dictionary<string, int>();     
    int maxRelics = 12;

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

        InitializeRelics();

        // �e�V�[���Ńf�o�b�O����Ƃ��ɃR�����g���������Ă�������
        // ��x���ǂݍ���ł��Ȃ����
        if (!isAlreadyRead) ReadPlayer("Warrior");
    }

    private void InitializeRelics()
    {
        for(int RelicID=1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics.Add(RelicID,0);
        }
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
