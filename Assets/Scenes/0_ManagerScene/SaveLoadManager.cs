using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath + "/GameSettings.json";

    public static void DataSave(GameSettings data)
    {
        string json = JsonUtility.ToJson(data);     // JSON�`���ɃV���A���C�Y
        File.WriteAllText(SavePath, json);          // JSON�f�[�^���w�肳�ꂽ�t�@�C���p�X�ɏ�������
    }

    public static GameSettings DataLoad()
    {
        // �w�肳�ꂽ�t�@�C�������݂��邩
        if (File.Exists(SavePath))
        {
            Debug.Log("A");
            string json = File.ReadAllText(SavePath);           // �t�@�C���̓��e��ǂݍ���
            return JsonUtility.FromJson<GameSettings>(json);    // GameSettings�I�u�W�F�N�g�Ƀf�V���A���C�Y
        }
        else
        {
            Debug.LogWarning("Save data not found.");
            return null;
        }
    }



#region �f�[�^�̓ǂݍ��݂̎d��
    //private GameSettings gameSettings;
    //private void Start()
    //{
    //    gameSettings = SaveLoadManager.DataLoad();
    //    gameSettings.overallVolume = 1.0f;
    //    gameSettings.bgmVolume = 1.0f;
    //    gameSettings.seVolume = 1.0f;
    //    SaveLoadManager.DataSave(gameSettings);
    //}   
#endregion
}

