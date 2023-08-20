using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath + "/GameSettings.json";

    public static void DataSave(GameSettings data)
    {
        string json = JsonUtility.ToJson(data);     // JSON形式にシリアライズ
        File.WriteAllText(SavePath, json);          // JSONデータを指定されたファイルパスに書き込む
    }

    public static GameSettings DataLoad()
    {
        // 指定されたファイルが存在するか
        if (File.Exists(SavePath))
        {
            Debug.Log("A");
            string json = File.ReadAllText(SavePath);           // ファイルの内容を読み込む
            return JsonUtility.FromJson<GameSettings>(json);    // GameSettingsオブジェクトにデシリアライズ
        }
        else
        {
            Debug.LogWarning("Save data not found.");
            return null;
        }
    }



#region データの読み込みの仕方
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

