using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    public GameObject[,] rooms = new GameObject[6, 6];
    public Transform roomParent;

    // スポーン地点
    public int spawnY;
    public int spawnX;
    private const int spawnRandMin = 2;
    private const int spawnRandMax = 3;

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
        rooms[spawnY, spawnX].gameObject.name = $"Room: {walkCount} ({spawnY}  {spawnX})";
        // 現在のランダムウォーク地点を更新
        curWalkY = spawnY;
        curWalkX = spawnX;
        // バックトラッキング用にスタックにプッシュ
        backTracking.Push(new Vector2Int(curWalkX, curWalkY));

        walkCount++;
        RandomWalk();
        CreatePalyer();
    }

    void CreatePalyer()
    {
        // キャラ選択で選択されたキャラのモデルを生成
        if (GameManager.Instance.playerData._playerName == "戦士")
        {
            // ほかのシーンにPrefabが生成されてしまうため、一度SetParentで親を指定して、親を解除しています。
            GameObject warrior = Instantiate(warriorPrefab, rooms[curWalkY, curWalkX].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity);
            warrior.transform.SetParent(objectParent.transform);
            warrior.transform.SetParent(null);
        }
        if (GameManager.Instance.playerData._playerName == "魔法使い")
        {
            // ほかのシーンにPrefabが生成されてしまうため、一度SetParentで親を指定して、親を解除しています。
            GameObject wizard = Instantiate(wizardPrefab, rooms[curWalkY, curWalkX].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity);
            wizard.transform.SetParent(objectParent.transform);
            wizard.transform.SetParent(null);
        }

        // カメラの位置を変更
        cam.transform.position = rooms[curWalkY, curWalkX].transform.position + roomCam;

        // ライトの位置変更
        spotLight.transform.position = cam.transform.position + lightPos;
    }

    /// <summary>
    /// rooms配列をランダムウォークさせて部屋を生成します。
    /// </summary>
    void RandomWalk()
    {
        // 部屋を生成する回数を設定
        movementLimit = Random.Range(randomWalkMin, randomWalkMax + 1);


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
                    // 上に生成
                    curWalkX -= 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++;                   
                    break;

                case 1:
                    // 下に生成
                    curWalkX += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++;                   
                    break;

                case 2:
                    // 左に生成
                    curWalkY -= 1;        
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++;          
                    break;

                case 3:
                    // 右に生成
                    curWalkY += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++; 
                    break;

                default:
                    break;
            }
        }

        CreateDoors();
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
                return curWalkX - 1 >= 0 && rooms[curWalkY, curWalkX - 1] == null;
            case 1:
                return curWalkX + 1 < rooms.GetLength(1) && rooms[curWalkY, curWalkX + 1] == null;            
            case 2:
                return curWalkY - 1 >= 0 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                return curWalkY + 1 < rooms.GetLength(0) && rooms[curWalkY + 1, curWalkX] == null;

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


    void CreateDoors()
    {
        int aCount = 0;
        RoomBehaviour roomBehaviour;
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            //Debug.Log("y  "+ y);
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] == null)
                {
                    //Debug.Log($"{y},{x}の部屋は、nullです");
                    continue;
                }
               

                bool[] doorStatus = new bool[4];           

                // 上下左右に部屋がある場合doorStatusをtrueにします。
                for (int i = 0; i < 4; i++)
                {
                    // 0 - forward, 1 - back, 2 - Left, 3 - Right
                    switch (i)
                    {
                        case 0:
                            if (x - 1 < 0)
                            {
                                //Debug.Log($"{rooms[y, x]}　の上の部屋は　範囲外です");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}　の上の部屋は　{rooms[y, x - 1]}");
                            doorStatus[0] = (rooms[y, x - 1] != null) ? true : false;
                            break;

                        case 1:
                            if (x + 1 >= rooms.GetLength(1))
                            {
                                //Debug.Log($"{rooms[y, x]}　の下の部屋は　範囲外です");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}　の下の部屋は　{rooms[y, x + 1]}");
                            doorStatus[1] = (rooms[y, x + 1] != null) ? true : false;
                            break;

                        case 2:
                            if (y - 1 < 0)
                            {
                                //Debug.Log($"{rooms[y, x]}　の左の部屋は　範囲外です");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}　の左の部屋は　{rooms[y - 1, x]}");
                            doorStatus[2] = (rooms[y - 1, x] != null) ? true : false;
                            break;

                        case 3:
                            if (y + 1 >= rooms.GetLength(0))
                            {
                                //Debug.Log($"{rooms[y, x]}　の右の部屋は　範囲外です");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}　の右の部屋は　{rooms[y + 1, x]}");
                            doorStatus[3] = (rooms[y + 1, x] != null) ? true : false;
                            break;

                        default:
                            Debug.LogError($"switch文に渡す値が間違っています。:  {i}");
                            break;
                    }
                    //Debug.Log($"{aCount}回目のi:  {i}");
                    
                }
                aCount++;
                if (rooms[y, x] != null)
                {
                    roomBehaviour = rooms[y, x].GetComponent<RoomBehaviour>();
                    roomBehaviour.UpdateRoom(doorStatus);
                }
                   
            }
        }

    }
}