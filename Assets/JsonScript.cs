using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  //AssetDatabaseを使うために追加
using System.IO;  //StreamWriterなどを使うために追加
using System.Linq;  //Selectを使うために追加

public class JsonScript : MonoBehaviour
{
    //保存先
    string datapath;

    void Awake()
    {
        //保存先の計算をする
        //これはAssets直下を指定. /以降にファイル名
        datapath = Application.dataPath + "/TestJson.json";
    }

    // Start is called before the first frame update
    void Start()
    {
        //playerデータを取得
        Player player = new Player();

        //JSONファイルがあればロード, なければ初期化関数へ
        if (FindJsonfile())
        {
            player = loadPlayerData();
        }
        else
        {
            Initialize(player);
        }
    }

    //セーブするための関数
    public void savePlayerData(Player player)
    {
        StreamWriter writer;

        //playerデータをJSONに変換
        string jsonstr = JsonUtility.ToJson(player);

        //JSONファイルに書き込み
        writer = new StreamWriter(datapath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //JSONファイルを読み込み, ロードするための関数
    public Player loadPlayerData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<Player>(datastr);
    }

    //JSONファイルがない場合に呼び出す初期化関数
    //初期値をセーブし, JSONファイルを生成する
    public void Initialize(Player player)
    {
        player.name = "aaa";
        player.hp = 12;
        player.attack = 6;
        player.defense = 5;

        savePlayerData(player);
    }

    //JSONファイルの有無を判定するための関数
    public bool FindJsonfile()
    {
        string[] assets = AssetDatabase.FindAssets(datapath);
        Debug.Log(assets.Length);
        if (assets.Length != 0)
        {
            string[] paths = assets.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
            Debug.Log($"検索結果:\n{string.Join("\n", paths)}");
            return true;
        }
        else
        {
            Debug.Log("Jsonファイルがなかった");
            return false;
        }
    }
}

//Playerのデータとなるクラスの定義
[System.Serializable]
public class Player
{
    public string name;
    public int hp;
    public int attack;
    public int defense;
}