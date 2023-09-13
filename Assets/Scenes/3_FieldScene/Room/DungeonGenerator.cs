using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    // もとは、6x6だったが、ボス部屋などを端に生成したい都合上、8x8にし、普通の部屋は真ん中の6x6マスの中で生成します。
    public GameObject[,] rooms = new GameObject[8, 8];
    public Transform roomParent;

    // スポーン地点
    public int spawnY;
    public int spawnX;
    private const int spawnRandMin = 3;
    private const int spawnRandMax = 4;

    // ランダムウォーク
    private int curWalkY;
    private int curWalkX;
    private int randomWalkMin = 13;
    private int randomWalkMax = 14;
    private int movementLimit;
    private int walkCount = 0;
    // バックトラッキング
    private Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    // 焚火の生成位置
    private int bonfireRoomY;
    private int bonfireRoomX;

    // ボス部屋の生成位置
    private int bossRoomY;
    private int bossRoomX;

    // 生成するキャラやObject
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject bossDoor;
    [SerializeField] private GameObject strongEnemy;
    [SerializeField] private GameObject smallEnemy;
    [SerializeField] private GameObject bonfirePrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject shopPrefab;
    
    // 生成したものをまとめるためのTransform
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform objectParent;
    [SerializeField] private Transform enemyParent;

    // 各部屋に移動したときに一緒に移動させるもの
    [SerializeField] public Camera cam;
    [SerializeField] public GameObject spotLight;          // 部屋を移動したときに一緒に移動させます
    public Vector3 lightPos = new Vector3(0, -4, 0);       // カメラの位置に加算して使います
    public Vector3 roomCam = new Vector3(0, 10, -10);      // 各部屋の位置に加算して使います。

    // プレイヤーを生成する位置
    float warriorY = -2.34f;
    float wizardY = -2.34f;

    void Start()
    {
        // セルの中央(4個所からランダムで)スポーン地点を設定
        spawnY = Random.Range(spawnRandMin, spawnRandMax + 1);
        spawnX = Random.Range(spawnRandMin, spawnRandMax + 1);
        rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity, roomParent);
        // わかりやすいように何番目に生成したか?と生成した配列の要素数を名前に入れています。
        rooms[spawnY, spawnX].gameObject.name = $"Room: {walkCount} ({spawnY}  {spawnX}) (spawnRoom)";
        // 現在のランダムウォーク地点を更新
        curWalkY = spawnY;
        curWalkX = spawnX;
        // バックトラッキング用にスタックにプッシュ
        backTracking.Push(new Vector2Int(curWalkX, curWalkY));
        
        RandomWalk();
        //CreatePlayer();
    }

    private void Update()
    {
        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform[] childTransforms = roomParent.GetComponentsInChildren<Transform>();

            // 親自体も含まれているため、インデックス1からループを開始
            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }

            walkCount = 0;
            backTracking.Clear();
            // 部屋をすべて削除
            for (int y = 0; y < rooms.GetLength(0); y++)
            {
                for (int x = 0; x < rooms.GetLength(1); x++)
                {
                    rooms[x,y] = null;
                }
            }

            // セルの中央(4個所からランダムで)スポーン地点を設定
            spawnY = Random.Range(spawnRandMin, spawnRandMax + 1);
            spawnX = Random.Range(spawnRandMin, spawnRandMax + 1);
            rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity, roomParent);
            // わかりやすいように何番目に生成したか?と生成した配列の要素数を名前に入れています。
            rooms[spawnY, spawnX].gameObject.name = $"Room: {walkCount} ({spawnY}  {spawnX}) (spawnRoom)";
            // 現在のランダムウォーク地点を更新
            curWalkY = spawnY;
            curWalkX = spawnX;
            // バックトラッキング用にスタックにプッシュ
            backTracking.Push(new Vector2Int(curWalkX, curWalkY));

            RandomWalk();
        }


    }



    /// <summary>
    /// rooms配列をランダムウォークさせて部屋を生成します。
    /// </summary>
    void RandomWalk()
    {
        // 部屋を生成する回数を設定
        movementLimit = Random.Range(randomWalkMin, randomWalkMax + 1);

        // ランダムウォークと部屋の生成をmovementLimit回分行う
        while (walkCount <= movementLimit)
        {         
            int direction;
            int loopCount = -1;

            // ランダムウォーク
            do
            {
                direction = Random.Range(0, 3 + 1);
                loopCount++;

            } while (!IsDirectionValid(direction) && loopCount <= 10);

            // 10回繰り返して部屋を生成する場所がないことを確認したらバックトラッキングで一つ戻る
            if (loopCount >= 10)
            {
                direction = -1;     // switchに入ってほしくないので-1
                Vector2Int latestCoordinate = backTracking.Pop();               
                curWalkY = latestCoordinate.y;
                curWalkX = latestCoordinate.x;
                Debug.Log($"{latestCoordinate} に戻りました。");
            }

            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            // 移動
            switch (direction)
            {
                case 0:
                    // 上に移動
                    curWalkX -= 1; break;

                case 1:
                    // 下に移動
                    curWalkX += 1; break;

                case 2:
                    // 左に移動
                    curWalkY -= 1; break;

                case 3:
                    // 右に移動
                    curWalkY += 1; break;

                default:
                    break;
            }

            // 移動した場合そこに部屋を生成
            if (direction >= 0)
            {
                walkCount++;

                // 部屋を生成
                rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                // バックトラッキング用にスタックにプッシュ
                backTracking.Push(new Vector2Int(curWalkX, curWalkY));
            }

            // 三歩目に焚火を生成
            if (walkCount == 3)
            {
                // 焚火生成
                GameObject bonfire = Instantiate(bonfirePrefab, rooms[curWalkY, curWalkX].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, objectParent);
                bonfire.gameObject.name = $"bonfire1: {walkCount} ({curWalkY}  {curWalkX})";
                // 焚火を生成した部屋を記録
                bonfireRoomY = curWalkY;
                bonfireRoomX = curWalkX;
            }
        }

        // ボス部屋生成
        GenerateBossRoom(rooms[spawnY, spawnX]);
    }

    /// <summary>
    /// ランダムウォークで行きたい方向に行けるかをboolで返します。
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    bool IsDirectionValid(int dir)
    {
        // 0 - forward, 1 - back, 2 - Left, 3 - Right
        switch (dir)
        {
            case 0:
                return curWalkX - 1 >= 1 && rooms[curWalkY, curWalkX - 1] == null;
            case 1:
                return curWalkX + 1 < rooms.GetLength(1) - 1 && rooms[curWalkY, curWalkX + 1] == null;            
            case 2:
                return curWalkY - 1 >= 1 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                return curWalkY + 1 < rooms.GetLength(0) - 1 && rooms[curWalkY + 1, curWalkX] == null;

            default:
                Debug.LogError($"switch文に渡す値が間違っています。:  {dir}");
                return false;
        }
    }



    /// <summary>
    /// 指定されたセルの位置に応じて部屋を生成する位置を返します。
    /// </summary>
    /// <param name="roomY"></param>
    /// <param name="roomX"></param>
    /// <returns></returns>
    Vector3 SetRoomPos(int roomY,int roomX)
    {
        // [0,0]が左上に来るように配置したかったので"roomX"だけマイナスにしています
        return new Vector3(roomY * 20f, 0f, -roomX * 20f);
    }

    /// <summary>
    /// GenerateDoorメソッドを使い、すべての部屋の扉を生成します。
    /// </summary>
    void GenerateDoorsInAllRooms()
    {
        // 部屋を全探索
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] != null)
                {
                    GenerateDoor(y, x);
                }
            }
        }
    }

    /// <summary>
    /// roomsの要素数を受け取り、その部屋に扉を生成します。
    /// </summary>
    /// <param name="roomY"></param>
    /// <param name="roomX"></param>
    void GenerateDoor(int roomY, int roomX)
    {
        RoomBehaviour roomBehaviour = rooms[roomY, roomX].GetComponent<RoomBehaviour>();
        bool[] doorStatus = new bool[4];

        // 上下左右に部屋がある場合doorStatusをtrueにします。
        for (int i = 0; i < 4; i++)
        {
            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            switch (i)
            {
                case 0:
                    if (roomX - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}　の上の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の上の部屋は　{rooms[roomY, roomX - 1]}");
                    doorStatus[0] = (rooms[roomY, roomX - 1] != null) ? true : false;
                    break;

                case 1:
                    if (roomX + 1 >= rooms.GetLength(1)) { /*Debug.Log($"{rooms[roomY, roomX]}　の下の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の下の部屋は　{rooms[roomY, roomX + 1]}");
                    doorStatus[1] = (rooms[roomY, roomX + 1] != null) ? true : false;
                    break;

                case 2:
                    if (roomY - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}　の左の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の左の部屋は　{rooms[roomY - 1, roomX]}");
                    doorStatus[2] = (rooms[roomY - 1, roomX] != null) ? true : false;
                    break;

                case 3:
                    if (roomY + 1 >= rooms.GetLength(0)) { /*Debug.Log($"{rooms[roomY, roomX]}　の右の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の右の部屋は　{rooms[roomY + 1, roomX]}");
                    doorStatus[3] = (rooms[roomY + 1, roomX] != null) ? true : false;
                    break;

                default:
                    Debug.LogError($"switch文に渡す値が間違っています。:  {i}");
                    break;
            }
        }

        // ドアを更新
        Debug.Log(doorStatus);
        roomBehaviour.UpdateRoom(doorStatus);
    }



    /// <summary>
    /// スポーン地点にキャラを生成します。
    /// </summary>
    void CreatePlayer()
    {
        // キャラ選択で選択されたキャラのモデルを生成
        if (GameManager.Instance.playerData._playerName == "戦士")
        {
            // ほかのシーンにPrefabが生成されてしまうため、親を指定しています。
            GameObject warrior = Instantiate(warriorPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity, playerParent);
        }
        if (GameManager.Instance.playerData._playerName == "魔法使い")
        {
            GameObject wizard = Instantiate(wizardPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity, playerParent);
        }

        // カメラの位置を変更
        cam.transform.position = rooms[spawnY, spawnX].transform.position + roomCam;

        // ライトの位置変更
        spotLight.transform.position = cam.transform.position + lightPos;
    }

    /// <summary>
    /// スポーン地点の部屋を受け取り、スポーン地点に一番遠い部屋または、その隣の部屋にボス部屋を生成します。
    /// </summary>
    /// <param name="spawnRoom"></param>
    void GenerateBossRoom(GameObject spawnRoom)
    {
        Vector3 spawnPos = spawnRoom.transform.position;
        int farthestRoomY = 0;
        int farthestRoomX = 0;
        float maxDistance = 0f;

        // 全探索で一番遠い部屋を見つける
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] != null)
                {
                    Vector3 roomPos = rooms[y, x].transform.position;
                    float distance = Vector3.Distance(spawnPos, roomPos);

                    // 一番遠い部屋を更新
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestRoomY = y;
                        farthestRoomX = x;
                    }
                }
            }
        }

        Debug.Log($"{rooms[farthestRoomY, farthestRoomX].name}　が一番遠い部屋です。");

        // ボスの扉を部屋の奥側に生成する都合上、部屋の奥に扉があると干渉してしまうため、干渉しないようにする。
        // 一番遠い部屋の上に部屋がある場合
        if (farthestRoomX - 1 >= 0 && rooms[farthestRoomY, farthestRoomX - 1] != null)
        {

            // 左右どちらかに新しく部屋を生成する
            int rand = Random.Range(2, 3 + 1);

            // 左
            if (rand == 2)
            {
                // 左がnullの場合、左に部屋を生成。nullじゃない場合反対に生成
                farthestRoomY = rooms[farthestRoomY - 1, farthestRoomX] == null ? -1 : 1;
            }
            // 右
            if (rand == 3)
            {
                // 右がnullの場合、右に部屋を生成。nullじゃない場合反対に生成
                farthestRoomY = rooms[farthestRoomY + 1, farthestRoomX] == null ? 1 : -1;
            }

            // 新しく部屋を追加し、部屋の名前をNewRoomにする
            rooms[farthestRoomY, farthestRoomX] = Instantiate(room, SetRoomPos(farthestRoomY, farthestRoomX), Quaternion.identity, roomParent);
            rooms[farthestRoomY, farthestRoomX].gameObject.name = $"NewRoom: {walkCount} ({farthestRoomY}  {farthestRoomX}) (bossRoom)";
        }
        else// 一番遠い部屋の上に部屋がない場合
        {
            // 一番遠い部屋の名前だけ変えて
            rooms[farthestRoomY, farthestRoomX].gameObject.name += " (bossRoom)";
        }

        GameObject bossRoom = rooms[farthestRoomY, farthestRoomX];
        // ボスの扉を生成。
        Instantiate(bossDoor, bossRoom.transform.position, Quaternion.identity, bossRoom.transform);
        Debug.Log($"{bossRoom}　にボスの扉生成。");

        // 焚火生成
        GameObject bonfire = Instantiate(bonfirePrefab, rooms[farthestRoomY, farthestRoomX].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, objectParent);
        bonfire.gameObject.name = $"bonfire2: bossRoom ({farthestRoomY}  {farthestRoomX})";

        // ボスの扉を生成した部屋を記録
        bossRoomY = farthestRoomY;
        bossRoomX = farthestRoomX;

        // 強敵生成
        GenerateStrongEnemy();
    }

    /// <summary>
    /// ボス部屋の左右、下のいずれかに強敵を生成します。一体しか生成できなかった場合その隣の部屋にもう一体生成します。
    /// </summary>
    void GenerateStrongEnemy()
    {
        int generateCount = 0;
        int rand = Random.Range(1, 3 + 1);

        switch (rand)
        {
            // 下
            case 1:
                if (bossRoomX + 1 < rooms.GetLength(1) && rooms[bossRoomY, bossRoomX + 1])
                {

                }
                break;
            // 左
            case 2:
                if (bossRoomY - 1 >= 0 && rooms[bossRoomY - 1, bossRoomX])
                {

                }
                break;
            // 右
            case 3:
                if (bossRoomY + 1 < rooms.GetLength(1) && rooms[bossRoomY + 1, bossRoomX])
                {

                }
                break;
            default:
                break;
        }





        // ショップと宝箱と雑魚敵を生成
        GenerateItemAndEnemy();
        // 移動用の扉すべてを生成。
        GenerateDoorsInAllRooms();
    }

    /// <summary>
    /// ショップと宝箱と雑魚敵を生成します。
    /// </summary>
    void GenerateItemAndEnemy()
    {
        // スポーン地点とその上下左右、焚火、ボス部屋、強敵の部屋を除外したroomsの配列
        GameObject[,] filteredRoomArray = rooms;

        // スポーン地点とその上下左右の部屋を除外に
        filteredRoomArray[spawnY, spawnX] = null;
        filteredRoomArray[spawnY, spawnX - 1] = null;
        filteredRoomArray[spawnY, spawnX + 1] = null;
        filteredRoomArray[spawnY - 1, spawnX] = null;
        filteredRoomArray[spawnY + 1, spawnX] = null;

        // 焚火の場所をnullに
        filteredRoomArray[bonfireRoomY, bonfireRoomX] = null;

        // ボス部屋の場所をnullに
        filteredRoomArray[bossRoomY, bonfireRoomX] = null;

        // 強敵の場所をnullに



        List<GameObject> emptyRoomList = new List<GameObject>();

        // 何もない部屋のListを作るために全探索
        for (int y = 0; y < filteredRoomArray.GetLength(0); y++)
        {
            for (int x = 0; x < filteredRoomArray.GetLength(1); x++)
            {
                if (filteredRoomArray[y, x] != null) emptyRoomList.Add(filteredRoomArray[y, x]);
            }
        }

        // ショップを二つ生成
        for (int i = 0; i <= 2; i++)
        {
            int lotteryShop = Random.Range(0, emptyRoomList.Count);
            Instantiate(shopPrefab, emptyRoomList[lotteryShop].transform.position, Quaternion.identity, objectParent);
            emptyRoomList.RemoveAt(lotteryShop);
        }

        // 宝箱を生成
        int lotteryTreasure = Random.Range(0, emptyRoomList.Count);
        Instantiate(treasurePrefab, emptyRoomList[lotteryTreasure].transform.position, Quaternion.identity, objectParent);
        emptyRoomList.RemoveAt(lotteryTreasure);

        // 雑魚敵を生成
        foreach (GameObject room in emptyRoomList)
        {
            Instantiate(smallEnemy, room.transform.position, Quaternion.identity, enemyParent);
            emptyRoomList.Remove(room);
        }
    }
}