using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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


    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject bossDoor;

    [SerializeField] public Camera cam;    // Main.cameraだと正しく取得できない時があるため
    [SerializeField] private GameObject objectParent;

    [SerializeField] public GameObject spotLight;          // 部屋を移動したときに一緒に移動させます
    public Vector3 lightPos = new Vector3(0, -4, 0);       // カメラの位置に加算して使います
    public Vector3 roomCam = new Vector3(0, 10, -10);      // 各部屋の位置に加算して使います。

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform[] childTransforms = roomParent.GetComponentsInChildren<Transform>();

            // 親自体も含まれているため、インデックス1からループを開始
            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }

            backTracking.Clear();
            walkCount = 0;
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
            walkCount++;
            // バックトラッキング用にスタックにプッシュ
            backTracking.Push(new Vector2Int(curWalkX, curWalkY));

            RandomWalk();
        }


    }

    /// <summary>
    /// スポーン地点にキャラを生成します。
    /// </summary>
    void CreatePlayer()
    {
        // キャラ選択で選択されたキャラのモデルを生成
        if (GameManager.Instance.playerData._playerName == "戦士")
        {
            // ほかのシーンにPrefabが生成されてしまうため、一度SetParentで親を指定して、親を解除しています。
            GameObject warrior = Instantiate(warriorPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity);
            warrior.transform.SetParent(objectParent.transform);
            warrior.transform.SetParent(null);
        }
        if (GameManager.Instance.playerData._playerName == "魔法使い")
        {
            // ほかのシーンにPrefabが生成されてしまうため、一度SetParentで親を指定して、親を解除しています。
            GameObject wizard = Instantiate(wizardPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity);
            wizard.transform.SetParent(objectParent.transform);
            wizard.transform.SetParent(null);
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

        // ボスの扉を部屋の奥側に生成する都合上、部屋の奥に扉があると鑑賞してしまうため、干渉しないようにする。
        // 一番遠い部屋の上に部屋がある場合
        if (farthestRoomX - 1 >= 0 && rooms[farthestRoomY, farthestRoomX - 1] != null)
        {

            // 左右どちらかに新しく部屋を生成する
            int rand = Random.Range(2, 3 + 1);

            // 左
            if (rand == 2)
            {
                // 左がnullの場合
                if (rooms[farthestRoomY - 1, farthestRoomX] == null)
                {
                    // 左に新しく部屋を生成
                    farthestRoomY -= 1;
                }
                else
                {
                    // 右に新しく部屋を生成
                    farthestRoomY += 1;
                }
            }
            // 右
            if (rand == 3)
            {
                // 右がnullの場合
                if (rooms[farthestRoomY + 1, farthestRoomX] == null)
                {
                    farthestRoomY += 1;
                }
                else
                {
                    farthestRoomY -= 1;
                }
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

        // 移動用の扉すべてを生成。
        GenerateDoorsInAllRooms();
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
                //Debug.Log(direction);

            } while (!IsDirectionValid(direction) && loopCount <= 10);

            // 10回繰り返して部屋を生成する場所がないことを確認したらバックトラッキングで一つ戻る
            if (loopCount >= 10)
            {
                direction = -1;     // switchに入ってほしくないので-1
                Vector2Int latestCoordinate = backTracking.Pop();
                Debug.Log($"{latestCoordinate} に戻りました。");
                curWalkY = latestCoordinate.y;
                curWalkX = latestCoordinate.x;        
            }

            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            switch (direction)
            {
                case 0:
                    // 上に移動
                    curWalkX -= 1;
                    walkCount++;

                    // 部屋を生成
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));                  
                    break;

                case 1:
                    // 下に移動
                    curWalkX += 1;
                    walkCount++;

                    // 部屋を生成
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));                                      
                    break;

                case 2:
                    // 左に移動
                    curWalkY -= 1;
                    walkCount++;

                    // 部屋を生成
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    break;

                case 3:
                    // 右に移動
                    curWalkY += 1;
                    walkCount++;

                    // 部屋を生成
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    break;

                default:
                    break;
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
    /// <param name="vertical"></param>
    /// <param name="horizontal"></param>
    /// <returns></returns>
    Vector3 SetRoomPos(int vertical,int horizontal)
    {
        // [0,0]が左上に来るように配置したかったので"horizontal"だけマイナスにしています
        return new Vector3(vertical * 20f, 0f, -horizontal * 20f);
    }


    /// <summary>
    /// GenerateDoorメソッドを使い、
    /// すべての部屋の扉を生成します。
    /// </summary>
    void GenerateDoorsInAllRooms()
    {
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] != null)
                {
                    GenerateDoor(y, x);
                }
                else
                {
                    //Debug.Log($"{y},{x}の部屋は、nullです");
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
}