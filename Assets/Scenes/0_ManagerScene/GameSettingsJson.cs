using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  //AssetDatabase���g�����߂ɒǉ�
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�
using System.Linq;  //Select���g�����߂ɒǉ�

// �Q�[���̐ݒ�̃f�[�^�ł��B
[System.Serializable]
public class GameSettings
{
    public float overallVolume;
    public float seVolume;
    public float bgmVolume;
}

public class GameSettingsJson : MonoBehaviour
{
    //�ۑ���
    string datapath => Application.dataPath + "/GameSettingsJson.json";

    private void Awake()
    {
        //GameSettings�f�[�^���擾
        GameSettings gameSettings = new GameSettings();

        //JSON�t�@�C��������΃��[�h, �Ȃ���Ώ������֐���
        if (FindJsonfile())
        {
            gameSettings = loadGameSettingsData();
        }
        else
        {
            Initialize(gameSettings);
        }
    }

    //�Z�[�u���邽�߂̊֐�
    public void saveGameSettingsData(GameSettings gameSettings)
    {
        StreamWriter writer;

        //gameSettings�f�[�^��JSON�ɕϊ�
        string jsonstr = JsonUtility.ToJson(gameSettings);

        //JSON�t�@�C���ɏ�������
        writer = new StreamWriter(datapath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //JSON�t�@�C����ǂݍ���, ���[�h���邽�߂̊֐�
    public GameSettings loadGameSettingsData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<GameSettings>(datastr);
    }



    //JSON�t�@�C�����Ȃ��ꍇ�ɌĂяo���������֐�
    //�����l���Z�[�u��, JSON�t�@�C���𐶐�����
    public void Initialize(GameSettings gameSettings)
    {
        gameSettings.overallVolume = 1f;
        gameSettings.seVolume = 0.5f;
        gameSettings.bgmVolume = 0.5f;

        saveGameSettingsData(gameSettings);
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