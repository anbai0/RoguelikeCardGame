using UnityEngine;
using UnityEditor;  //AssetDatabase���g�����߂ɒǉ�
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�
using System.Linq;  //Select���g�����߂ɒǉ�
using System.Collections.Generic;

// �Q�[���̐ݒ�̃f�[�^�ł��B
[System.Serializable]
public class GameSettings
{
    public float overallVolume;
    public float seVolume;
    public float bgmVolume;
    //public List<bool> collectedCardHistory;     // ��x�J�[�h����ɓ��ꂽ���Ƃ����邩�𔻒肵�܂�
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
        if (File.Exists(datapath))
        {
            Debug.Log("Json�t�@�C����������܂���");
            gameSettings = loadGameSettingsData();
        }
        else
        {
            Debug.Log("Json�t�@�C����������܂���ł���");
            Initialize(gameSettings);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(File.Exists(datapath));
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
        //gameSettings.collectedCardHistory.ForEach(hasCard => hasCard = false);      // ���ׂ�false�ŏ�����
        //for(int i=0; i<= 20; i++)
        //{
        //    gameSettings.collectedCardHistory.Add(false);
        //}

        saveGameSettingsData(gameSettings);
    }
}