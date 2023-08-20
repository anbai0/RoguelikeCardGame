using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  //AssetDatabase���g�����߂ɒǉ�
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�
using System.Linq;  //Select���g�����߂ɒǉ�

public class JsonScript : MonoBehaviour
{
    //�ۑ���
    string datapath;

    void Awake()
    {
        //�ۑ���̌v�Z������
        //�����Assets�������w��. /�ȍ~�Ƀt�@�C����
        datapath = Application.dataPath + "/TestJson.json";
    }

    // Start is called before the first frame update
    void Start()
    {
        //player�f�[�^���擾
        Player player = new Player();

        //JSON�t�@�C��������΃��[�h, �Ȃ���Ώ������֐���
        if (FindJsonfile())
        {
            player = loadPlayerData();
        }
        else
        {
            Initialize(player);
        }
    }

    //�Z�[�u���邽�߂̊֐�
    public void savePlayerData(Player player)
    {
        StreamWriter writer;

        //player�f�[�^��JSON�ɕϊ�
        string jsonstr = JsonUtility.ToJson(player);

        //JSON�t�@�C���ɏ�������
        writer = new StreamWriter(datapath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //JSON�t�@�C����ǂݍ���, ���[�h���邽�߂̊֐�
    public Player loadPlayerData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<Player>(datastr);
    }

    //JSON�t�@�C�����Ȃ��ꍇ�ɌĂяo���������֐�
    //�����l���Z�[�u��, JSON�t�@�C���𐶐�����
    public void Initialize(Player player)
    {
        player.name = "aaa";
        player.hp = 12;
        player.attack = 6;
        player.defense = 5;

        savePlayerData(player);
    }

    //JSON�t�@�C���̗L���𔻒肷�邽�߂̊֐�
    public bool FindJsonfile()
    {
        string[] assets = AssetDatabase.FindAssets(datapath);
        Debug.Log(assets.Length);
        if (assets.Length != 0)
        {
            string[] paths = assets.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
            Debug.Log($"��������:\n{string.Join("\n", paths)}");
            return true;
        }
        else
        {
            Debug.Log("Json�t�@�C�����Ȃ�����");
            return false;
        }
    }
}

//Player�̃f�[�^�ƂȂ�N���X�̒�`
[System.Serializable]
public class Player
{
    public string name;
    public int hp;
    public int attack;
    public int defense;
}