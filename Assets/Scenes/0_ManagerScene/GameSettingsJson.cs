using UnityEngine;
using UnityEditor;  //AssetDatabaseを使うために追加
using System.IO;  //StreamWriterなどを使うために追加
using System.Linq;  //Selectを使うために追加
using System.Collections.Generic;

// ゲームの設定のデータです。
[System.Serializable]
public class GameSettings
{
    public float overallVolume;
    public float seVolume;
    public float bgmVolume;
    public List<bool> collectedCardHistory;     // 一度カードを手に入れたことがあるかを判定します
}

public class GameSettingsJson : MonoBehaviour
{
    //保存先
    string datapath => Application.dataPath + "/GameSettingsJson.json";


    private void Awake()
    {
        //GameSettingsデータを取得
        GameSettings gameSettings = new GameSettings();

        //JSONファイルがあればロード, なければ初期化関数へ
        if (File.Exists(datapath))
        {
            Debug.Log("Jsonファイルが見つかりました");
            gameSettings = loadGameSettingsData();
        }
        else
        {
            Debug.Log("Jsonファイルが見つかりませんでした");
            Initialize(gameSettings);
        }
    }

    //セーブするための関数
    public void saveGameSettingsData(GameSettings gameSettings)
    {
        StreamWriter writer;

        //gameSettingsデータをJSONに変換
        string jsonstr = JsonUtility.ToJson(gameSettings);

        //JSONファイルに書き込み
        writer = new StreamWriter(datapath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //JSONファイルを読み込み, ロードするための関数
    public GameSettings loadGameSettingsData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<GameSettings>(datastr);
    }



    //JSONファイルがない場合に呼び出す初期化関数
    //初期値をセーブし, JSONファイルを生成する
    public void Initialize(GameSettings gameSettings)
    {
        gameSettings.overallVolume = 1f;
        gameSettings.seVolume = 0.5f;
        gameSettings.bgmVolume = 0.5f;

        gameSettings.collectedCardHistory = new List<bool> { };
        for (int i = 0; i <= 20; i++)
        {
            gameSettings.collectedCardHistory.Add(false);
        }  

        saveGameSettingsData(gameSettings);
    }
}