using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    // もとは、6x6だったが、ボス部屋などを端に生成したい都合上、8x8にし、普通の部屋は真ん中の6x6マスの中で生成します。
    public GameObject[,] rooms = new GameObject[8, 8];
    public Transform roomParent;

    // スポーン地点
    public Vector2Int spawnPos;
    private const int spawnRandMin = 3;
    private const int spawnRandMax = 4;

    // ランダムウォーク
    private Vector2Int curWalk;
    private int randomWalkMin = 12;
    private int randomWalkMax = 13;
    private int movementLimit;
    private int walkCount = 0;
    // バックトラッキング
    private Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    // 焚火の生成位置
    public Vector2Int bonfireRoomPos;

    // ボス部屋の生成位置
    private Vector2Int bossRoomPos;

    // 強敵の生成位置
    private List<Vector2Int> strongEnemyRooms = new List<Vector2Int>();

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
    [SerializeField] private Transform enemyParent;

    // 各部屋に移動したときに一緒に移動させるもの
    [SerializeField] public Camera cam;
    [SerializeField] public GameObject spotLight;          // 部屋を移動したときに一緒に移動させます
    public Vector3 lightPos = new Vector3(0, -4, 0);       // カメラの位置に加算して使います
    public Vector3 roomCam = new Vector3(0, 10, -10);      // 各部屋の位置に加算して使います。

    // プレイヤーを生成する位置
    float warriorY = -2.34f;
    float wizardY = -2.34f;
    
    // マップ生成
    [SerializeField] public Transform map;
    [SerializeField] GameObject mapPrefab;
    public GameObject[,] maps = new GameObject[8, 8];

    // マップアイコン
    [SerializeField] GameObject warriorIcon;
    [SerializeField] GameObject wizardIcon;
    [SerializeField] GameObject spawnIcon;
    public GameObject playerIcon;

    void Start()
    {
        // セルの中央(4個所からランダムで)スポーン地点を設定
        spawnPos.y = Random.Range(spawnRandMin, spawnRandMax + 1);
        spawnPos.x = Random.Range(spawnRandMin, spawnRandMax + 1);
        rooms[spawnPos.y, spawnPos.x] = Instantiate(room, SetRoomPos(spawnPos.y, spawnPos.x), Quaternion.identity, roomParent);
        // わかりやすいように何番目に生成したか?と生成した配列の要素数を名前に入れています。
        rooms[spawnPos.y, spawnPos.x].gameObject.name = $"Room: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        // 松明のエフェクトを表示
        rooms[spawnPos.y, spawnPos.x].GetComponent<RoomBehaviour>().ToggleTorchEffect(true);

        // マップ描画
        maps[spawnPos.y, spawnPos.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
        maps[spawnPos.y, spawnPos.x].transform.localPosition = new Vector3(spawnPos.x * 100, -spawnPos.y * 100, 0);
        maps[spawnPos.y, spawnPos.x].gameObject.name = $"Map: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        // スポーン地点の部屋をミニマップの中心にする
        map.transform.localPosition = new Vector3(-spawnPos.x * 100 - 50, spawnPos.y * 100 + 50, 0);
        // スポーン地点のマップをアクティブにする
        maps[spawnPos.y, spawnPos.x].gameObject.SetActive(true);
        // スポーン地点アイコンを生成
        Instantiate(spawnIcon, maps[spawnPos.y, spawnPos.x].transform.position, Quaternion.identity, maps[spawnPos.y, spawnPos.x].transform);

        // 現在のランダムウォーク地点を更新
        curWalk.Set(spawnPos.x, spawnPos.y);
        // バックトラッキング用にスタックにプッシュ
        backTracking.Push(new Vector2Int(curWalk.x, curWalk.y));
        
        RandomWalk();
        CreatePlayer();
    }

    private void Update()
    {

        #region　デバッグ用
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Transform[] childRooms = roomParent.GetComponentsInChildren<Transform>();

        //    // 親自体も含まれているため、インデックス1からループを開始
        //    for (int i = 1; i < childRooms.Length; i++)
        //    {
        //        Destroy(childRooms[i].gameObject);
        //    }

        //    Transform[] childEnemys = enemyParent.GetComponentsInChildren<Transform>();

        //    // 親自体も含まれているため、インデックス1からループを開始
        //    for (int i = 1; i < childEnemys.Length; i++)
        //    {
        //        Destroy(childEnemys[i].gameObject);
        //    }

        //    Transform[] m = map.GetComponentsInChildren<Transform>();

        //    // 親自体も含まれているため、インデックス1からループを開始
        //    for (int i = 1; i < m.Length; i++)
        //    {
        //        Destroy(m[i].gameObject);
        //    }

        //    walkCount = 0;
        //    backTracking.Clear();
        //    // 部屋をすべて削除
        //    for (int y = 0; y < rooms.GetLength(0); y++)
        //    {
        //        for (int x = 0; x < rooms.GetLength(1); x++)
        //        {
        //            rooms[x, y] = null;
        //            maps[x, y] = null;
        //        }
        //    }

        //    strongEnemyRooms.Clear();

        //    // セルの中央(4個所からランダムで)スポーン地点を設定
        //    spawnPos.y = Random.Range(spawnRandMin, spawnRandMax + 1);
        //    spawnPos.x = Random.Range(spawnRandMin, spawnRandMax + 1);
        //    rooms[spawnPos.y, spawnPos.x] = Instantiate(room, SetRoomPos(spawnPos.y, spawnPos.x), Quaternion.identity, roomParent);
        //    // わかりやすいように何番目に生成したか?と生成した配列の要素数を名前に入れています。
        //    rooms[spawnPos.y, spawnPos.x].gameObject.name = $"Room: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        //    // マップ描画
        //    maps[spawnPos.y, spawnPos.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
        //    maps[spawnPos.y, spawnPos.x].transform.localPosition = new Vector3(spawnPos.x * 100, -spawnPos.y * 100, 0);
        //    maps[spawnPos.y, spawnPos.x].gameObject.name = $"Map: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        //    // スポーン地点の部屋をミニマップの中心にする
        //    map.transform.localPosition = new Vector3(-spawnPos.x * 100 - 50, spawnPos.y * 100 + 50, 0);
        //    // スポーン地点のマップをアクティブにする
        //    maps[spawnPos.y, spawnPos.x].gameObject.SetActive(true);
        //    // 現在のランダムウォーク地点を更新
        //    curWalk = spawnPos;
        //    // バックトラッキング用にスタックにプッシュ
        //    backTracking.Push(new Vector2Int(curWalk.x, curWalk.y));

        //    RandomWalk();
        //}
        #endregion

    }



    /// <summary>
    /// rooms配列をランダムウォークさせて部屋を生成します。
    /// </summary>
    void RandomWalk()
    {
        // 部屋を生成する回数を設定
        movementLimit = Random.Range(randomWalkMin, randomWalkMax + 1);
        int forwardCount = 0, backCount = 0, leftCount = 0, rightCount = 0;

        // ランダムウォークと部屋の生成をmovementLimit回分行う
        while (walkCount <= movementLimit)
        {         
            int direction;
            int loopCount = 0;          

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
                curWalk.Set(latestCoordinate.x, latestCoordinate.y);
                Debug.Log($"{latestCoordinate} に戻りました。");
            }

            // 同じ方向に三回以上進んだときに別の方向に曲がるようにする
            if (forwardCount >= 3 || backCount >= 3 || leftCount >= 3 || rightCount >= 3)
            {

                Debug.LogWarning($"上{forwardCount} 下{backCount} 左{leftCount} 右{rightCount}");
                List<int> directionLottery = new List<int>();
                if (forwardCount >= 3 || backCount >= 3)
                {
                    // 範囲外になっていない、かつ、進んだ方向に部屋がない               
                    if (curWalk.x - 1 >= 1 && rooms[curWalk.y, curWalk.x - 1] == null)
                    {
                        directionLottery.Add(2);
                    }
                    if (curWalk.x + 1 < rooms.GetLength(0) - 1 && rooms[curWalk.y, curWalk.x + 1] == null)
                    {
                        directionLottery.Add(3);
                    }

                    // 進める方向を決定
                    if (directionLottery.Count != 0)
                        direction = directionLottery[Random.Range(0, directionLottery.Count)];
                    
                    forwardCount = 0; backCount = 0; leftCount = 0; rightCount = 0;
                    
                    if (directionLottery.Count != 0 && direction == 2) Debug.Log($"左に進みました");
                    if (directionLottery.Count != 0 && direction == 3) Debug.Log($"右に進みました");
                }
                if (leftCount >= 3 || rightCount >= 3)
                {
                    if (curWalk.y - 1 >= 1 && rooms[curWalk.y - 1, curWalk.x] == null)
                    {
                        directionLottery.Add(0);
                    }
                    if (curWalk.y + 1 < rooms.GetLength(1) - 1 && rooms[curWalk.y + 1, curWalk.x] == null)
                    {
                        directionLottery.Add(1);
                    }

                    // 進める方向を決定
                    if (directionLottery.Count != 0)
                        direction = directionLottery[Random.Range(0, directionLottery.Count)];         

                    forwardCount = 0; backCount = 0; leftCount = 0; rightCount = 0;

                    if (directionLottery.Count != 0 && direction == 0) Debug.Log($"上に進みました");
                    if (directionLottery.Count != 0 && direction == 1) Debug.Log($"下に進みました");
                }
            }

            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            // 移動
            switch (direction)
            {
                case 0:
                    forwardCount++; backCount = 0; leftCount = 0; rightCount = 0;
                    // 上に移動
                    curWalk.y -= 1; break;

                case 1:
                    forwardCount = 0; backCount++; leftCount = 0; rightCount = 0;
                    // 下に移動
                    curWalk.y += 1; break;

                case 2:
                    forwardCount = 0; backCount = 0; leftCount++; rightCount = 0;
                    // 左に移動
                    curWalk.x -= 1; break;

                case 3:
                    forwardCount = 0; backCount = 0; leftCount = 0; rightCount++;
                    // 右に移動
                    curWalk.x += 1; break;

                default:
                    break;
            }

            // 移動した場合そこに部屋を生成
            if (direction >= 0)
            {
                walkCount++;

                // 部屋を生成
                rooms[curWalk.y, curWalk.x] = Instantiate(room, SetRoomPos(curWalk.y, curWalk.x), Quaternion.identity, roomParent);
                rooms[curWalk.y, curWalk.x].gameObject.name = $"Room: {walkCount} ({curWalk.y}  {curWalk.x})";

                // マップ描画
                maps[curWalk.y, curWalk.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
                maps[curWalk.y, curWalk.x].transform.localPosition = new Vector3(curWalk.x * 100, -curWalk.y * 100, 0);
                maps[curWalk.y, curWalk.x].gameObject.name = $"Map: {walkCount} ({curWalk.y}  {curWalk.x})";

                // バックトラッキング用にスタックにプッシュ
                backTracking.Push(new Vector2Int(curWalk.x, curWalk.y));
            }

            // 二歩目に焚火を生成
            if (walkCount == 2)
            {
                // 焚火生成
                GameObject bonfire = Instantiate(bonfirePrefab, rooms[curWalk.y, curWalk.x].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, rooms[curWalk.y, curWalk.x].transform);
                bonfire.gameObject.name = $"bonfire1: {walkCount} ({curWalk.y}  {curWalk.x})";
                // 焚火を生成した部屋を記録
                bonfireRoomPos.Set(curWalk.x, curWalk.y);
            }

        }
        // ボス部屋生成
        GenerateBossRoom(rooms[spawnPos.y, spawnPos.x]);
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
                return curWalk.y - 1 >= 1 && rooms[curWalk.y - 1, curWalk.x] == null;
            case 1:
                return curWalk.y + 1 < rooms.GetLength(1) - 1 && rooms[curWalk.y + 1, curWalk.x] == null;            
            case 2:
                return curWalk.x - 1 >= 1 && rooms[curWalk.y, curWalk.x - 1] == null;
            case 3:
                return curWalk.x + 1 < rooms.GetLength(0) - 1 && rooms[curWalk.y, curWalk.x + 1] == null;

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
        return new Vector3(roomX * 20f, 0f, -roomY * 20f);
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
            Instantiate(warriorPrefab, rooms[spawnPos.y, spawnPos.x].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity, playerParent);
            // マップのプレイヤーアイコン生成
            playerIcon = Instantiate(warriorIcon, Vector3.zero, Quaternion.identity.normalized, maps[spawnPos.y, spawnPos.x].transform);
            playerIcon.transform.localPosition = Vector3.zero;
        }
        if (GameManager.Instance.playerData._playerName == "魔法使い")
        {
            Instantiate(wizardPrefab, rooms[spawnPos.y, spawnPos.x].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity, playerParent);
            // マップのプレイヤーアイコン生成
            playerIcon = Instantiate(wizardIcon, Vector3.zero, Quaternion.identity.normalized, maps[spawnPos.y, spawnPos.x].transform);
            playerIcon.transform.localPosition = Vector3.zero;
        }

        // カメラの位置を変更
        cam.transform.position = rooms[spawnPos.y, spawnPos.x].transform.position + roomCam;

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
        Vector2Int farthestRoomPos = new Vector2Int();
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
                        farthestRoomPos.Set(x, y);
                    }
                }
            }
        }

        Debug.Log($"{rooms[farthestRoomPos.y, farthestRoomPos.x].name}　が一番遠い部屋です。");

        // ボスの扉を部屋の奥側に生成する都合上、部屋の奥に扉があると干渉してしまうため、干渉しないようにする。
        // 一番遠い部屋の上に部屋がある場合
        if (farthestRoomPos.y - 1 >= 0 && rooms[farthestRoomPos.y - 1, farthestRoomPos.x] != null)
        {
            List<Vector2Int> filteredRoom = new List<Vector2Int>();

            // 左右に部屋があるかどうかを確認し、なかった場合Listに追加
            if (farthestRoomPos.x - 1 >= 0 && rooms[farthestRoomPos.y, farthestRoomPos.x - 1] == null)
            {
                filteredRoom.Add(new Vector2Int(farthestRoomPos.x - 1, farthestRoomPos.y));
            }
            if (farthestRoomPos.x + 1 < rooms.GetLength(0) && rooms[farthestRoomPos.y, farthestRoomPos.x + 1] == null)
            {
                filteredRoom.Add(new Vector2Int(farthestRoomPos.x + 1, farthestRoomPos.y));
            }

            
            List<Vector2Int> bossRoomCandidate = new List<Vector2Int>();
            // 左右の部屋の上に部屋があるかを確認し、なかった場合Listに追加
            for (int i = 0; i < filteredRoom.Count; i++)
            {
                if (rooms[filteredRoom[i].y - 1, filteredRoom[i].x] == null)
                {
                    bossRoomCandidate.Add(new Vector2Int(filteredRoom[i].x, filteredRoom[i].y));
                }
            }

            // 上記の処理で、部屋を取得できなかった場合、新しく部屋2回生成する
            if (bossRoomCandidate.Count == 0)
            {
                bossRoomCandidate = new List<Vector2Int>(filteredRoom);
                int rand = Random.Range(0, bossRoomCandidate.Count);

                // ボス部屋の手前の部屋を生成
                walkCount++;
                rooms[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x] = Instantiate(room, SetRoomPos(bossRoomCandidate[rand].y, bossRoomCandidate[rand].x), Quaternion.identity, roomParent);
                rooms[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x].gameObject.name = $"NewRoom1: {walkCount} ({bossRoomCandidate[rand].y}  {bossRoomCandidate[rand].x})";
                // マップ描画
                maps[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
                maps[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x].transform.localPosition = new Vector3(bossRoomCandidate[rand].x * 100, -bossRoomCandidate[rand].y * 100, 0);
                maps[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x].gameObject.name = $"NewRoom1Map: {walkCount} ({bossRoomCandidate[rand].y}  {bossRoomCandidate[rand].x})";
                // 一番遠い部屋から左右どちらに進んだかを取得
                int direction = bossRoomCandidate[rand].x - farthestRoomPos.x;
                // 一番遠い部屋を更新
                if (direction == -1) direction--;
                if (direction == +1) direction++;
                farthestRoomPos.y = bossRoomCandidate[rand].y;
                farthestRoomPos.x += direction;
            }
            else
            {
                // 左右どちらに部屋を生成するか決める
                int rand = Random.Range(0, bossRoomCandidate.Count);
                farthestRoomPos.Set(bossRoomCandidate[rand].x, bossRoomCandidate[rand].y);
            }
            // ボス部屋を生成
            walkCount++;
            rooms[farthestRoomPos.y, farthestRoomPos.x] = Instantiate(room, SetRoomPos(farthestRoomPos.y, farthestRoomPos.x), Quaternion.identity, roomParent);
            rooms[farthestRoomPos.y, farthestRoomPos.x].gameObject.name = $"NewRoom2: {walkCount} ({farthestRoomPos.y}  {farthestRoomPos.x}) (bossRoom)";
            // マップ描画
            maps[farthestRoomPos.y, farthestRoomPos.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
            maps[farthestRoomPos.y, farthestRoomPos.x].transform.localPosition = new Vector3(farthestRoomPos.x * 100, -farthestRoomPos.y * 100, 0);
            maps[farthestRoomPos.y, farthestRoomPos.x].gameObject.name = $"NewRoom2Map: {walkCount} ({farthestRoomPos.y}  {farthestRoomPos.x}) (bossRoom)";
        }
        else// 一番遠い部屋の上に部屋がない場合
        {
            // 一番遠い部屋がそのままボス部屋になるので、名前だけ変更
            rooms[farthestRoomPos.y, farthestRoomPos.x].gameObject.name += " (bossRoom)";
        }


        // ボスの扉を生成
        Instantiate(bossDoor, rooms[farthestRoomPos.y, farthestRoomPos.x].transform.position, Quaternion.identity, rooms[farthestRoomPos.y, farthestRoomPos.x].transform);
        Debug.Log($"{rooms[farthestRoomPos.y, farthestRoomPos.x]}　にボスの扉生成。");

        // 焚火生成
        GameObject bonfire = Instantiate(bonfirePrefab, rooms[farthestRoomPos.y, farthestRoomPos.x].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, rooms[farthestRoomPos.y, farthestRoomPos.x].transform);
        bonfire.gameObject.name = $"bonfire2: bossRoom ({farthestRoomPos.y}  {farthestRoomPos.x})";

        // ボス部屋の位置を記録
        bossRoomPos.Set(farthestRoomPos.x, farthestRoomPos.y);

        // 強敵生成
        GenerateStrongEnemy();
    }

    /// <summary>
    /// ボス部屋の左右、下のいずれかに強敵を生成します。一体しか生成できなかった場合その隣の部屋にもう一体生成します。
    /// </summary>
    void GenerateStrongEnemy()
    {
        // ボス部屋に隣接している部屋の位置を格納するList
        List<Vector2Int> adjacentRooms = new List<Vector2Int>();

        // 下に部屋がある場合Listに追加
        if (bossRoomPos.y + 1 < rooms.GetLength(1) && rooms[bossRoomPos.y + 1, bossRoomPos.x] != null)
        {
            // 指定した部屋に焚火がない場合Listに追加
            if (rooms[bossRoomPos.y + 1, bossRoomPos.x] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                adjacentRooms.Add(new Vector2Int(bossRoomPos.x, bossRoomPos.y + 1));
        }
        // 左に部屋がある場合Listに追加
        if (bossRoomPos.x - 1 >= 0 && rooms[bossRoomPos.y, bossRoomPos.x - 1] != null)
        {
            if (rooms[bossRoomPos.y, bossRoomPos.x - 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                adjacentRooms.Add(new Vector2Int(bossRoomPos.x - 1, bossRoomPos.y));
        }
        // 右に部屋がある場合Listに追加
        if (bossRoomPos.x + 1 < rooms.GetLength(0) && rooms[bossRoomPos.y, bossRoomPos.x + 1] != null)
        {
            if (rooms[bossRoomPos.y, bossRoomPos.x + 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                adjacentRooms.Add(new Vector2Int(bossRoomPos.x + 1, bossRoomPos.y));
        }

        // ランダムに2つの隣接するを強敵部屋に設定
        List<Vector2Int> selectedRooms = new List<Vector2Int>();
        while (selectedRooms.Count < 2 && adjacentRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, adjacentRooms.Count);
            selectedRooms.Add(adjacentRooms[randomIndex]);
            adjacentRooms.RemoveAt(randomIndex);
        }
        strongEnemyRooms = new List<Vector2Int>(selectedRooms);
        // 強敵を一体しか生成できなかった場合、強敵を生成した部屋を中心に、
        // ボス部屋を除いた上下左右の部屋のいずれかに強敵を生成
        if (strongEnemyRooms.Count == 1)
        {
            selectedRooms.Clear();

            // 強敵を生成した部屋を中心に上下左右の部屋を取得
            // 上に部屋がある場合Listに追加
            if (strongEnemyRooms[0].y -1 >= 0 && rooms[strongEnemyRooms[0].y - 1, strongEnemyRooms[0].x] != null)
            {
                // 指定した部屋に焚火がない場合Listに追加
                if (rooms[strongEnemyRooms[0].y - 1, strongEnemyRooms[0].x] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x, strongEnemyRooms[0].y - 1));
            }
            // 下に部屋がある場合Listに追加
            if (strongEnemyRooms[0].y + 1 < rooms.GetLength(1) && rooms[strongEnemyRooms[0].y + 1, strongEnemyRooms[0].x] != null)
            {
                if (rooms[strongEnemyRooms[0].y + 1, strongEnemyRooms[0].x] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x, strongEnemyRooms[0].y + 1));
            }
            // 左に部屋がある場合Listに追加
            if (strongEnemyRooms[0].x - 1 >= 0 && rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x - 1] != null)
            {
                if (rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x - 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x - 1, strongEnemyRooms[0].y));
            }
            // 右に部屋がある場合Listに追加
            if (strongEnemyRooms[0].x + 1 < rooms.GetLength(0) && rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x + 1] != null)
            {
                if (rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x + 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x + 1, strongEnemyRooms[0].y));
            }

            // ボス部屋を除外
            selectedRooms.Remove(bossRoomPos);
            // 選択された部屋からランダムで強敵部屋にする
            int randomIndex = Random.Range(0, selectedRooms.Count);
            strongEnemyRooms.Add(selectedRooms[randomIndex]);
        }

        // 強敵部屋に敵を生成
        for (int i = 0; i < strongEnemyRooms.Count; i++)
        {
            Instantiate(strongEnemy, rooms[strongEnemyRooms[i].y, strongEnemyRooms[i].x].transform.position, Quaternion.identity, enemyParent);
        }

        // ショップと宝箱と雑魚敵を生成
        GenerateItemAndEnemy();
        // 移動用の扉すべてを生成。
        GenerateDoorsInAllRooms();
    }

    /// <summary>
    /// 何もない部屋にショップと宝箱と雑魚敵を生成します。
    /// </summary>
    void GenerateItemAndEnemy()
    {
        // スポーン地点とその上下左右、焚火、ボス部屋、強敵の部屋を除外したroomsの配列
        GameObject[,] filteredRoomArray = new GameObject[rooms.GetLength(0), rooms.GetLength(1)];
        System.Array.Copy(rooms, filteredRoomArray, filteredRoomArray.Length);

        // スポーン地点とその上下左右の部屋を除外に
        filteredRoomArray[spawnPos.y, spawnPos.x] = null;
        filteredRoomArray[spawnPos.y - 1, spawnPos.x] = null;
        filteredRoomArray[spawnPos.y + 1, spawnPos.x] = null;
        filteredRoomArray[spawnPos.y, spawnPos.x - 1] = null;
        filteredRoomArray[spawnPos.y, spawnPos.x + 1] = null;

        // 焚火の場所をnullに
        Debug.Log("焚火"+filteredRoomArray[bonfireRoomPos.y, bonfireRoomPos.x]);
        filteredRoomArray[bonfireRoomPos.y, bonfireRoomPos.x] = null;

        // ボス部屋の場所をnullに
        Debug.Log("ボス部屋" + filteredRoomArray[bossRoomPos.y, bossRoomPos.x]);
        filteredRoomArray[bossRoomPos.y, bossRoomPos.x] = null;

        // 強敵の場所をnullに
        for (int i = 0; i < strongEnemyRooms.Count; i++)
        {
            Debug.Log("強敵部屋" + filteredRoomArray[strongEnemyRooms[i].y, strongEnemyRooms[i].x]);
            filteredRoomArray[strongEnemyRooms[i].y, strongEnemyRooms[i].x] = null;
        }

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
        int lotteryShop = Random.Range(0, emptyRoomList.Count);
        Instantiate(shopPrefab, emptyRoomList[lotteryShop].transform.position, Quaternion.identity, emptyRoomList[lotteryShop].transform);
        emptyRoomList.RemoveAt(lotteryShop);


        // 宝箱を生成
        int lotteryTreasure = Random.Range(0, emptyRoomList.Count);
        Instantiate(treasurePrefab, emptyRoomList[lotteryTreasure].transform.position, Quaternion.identity, emptyRoomList[lotteryTreasure].transform);
        emptyRoomList.RemoveAt(lotteryTreasure);

        // 残った部屋に雑魚敵を生成
        //Debug.Log("emptyList"+emptyRoomList.Count);
        foreach (GameObject emptyRoom in emptyRoomList)
        {
            Instantiate(smallEnemy, emptyRoom.transform.position, Quaternion.identity, enemyParent);
        }

        // スポーン地点の周りに雑魚敵を生成
        if (spawnPos.y - 1 >= 0 && rooms[spawnPos.y - 1, spawnPos.x] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y - 1, spawnPos.x].transform.position, Quaternion.identity, enemyParent);
        }
        if (spawnPos.y + 1 < rooms.GetLength(1) && rooms[spawnPos.y + 1, spawnPos.x] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y + 1, spawnPos.x].transform.position, Quaternion.identity, enemyParent);
        }
        if (spawnPos.x - 1 >= 0 && rooms[spawnPos.y, spawnPos.x - 1] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y, spawnPos.x - 1].transform.position, Quaternion.identity, enemyParent);
        }
        if (spawnPos.x + 1 < rooms.GetLength(0) && rooms[spawnPos.y, spawnPos.x + 1] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y, spawnPos.x + 1].transform.position, Quaternion.identity, enemyParent);
        }
    }


    /// <summary>
    /// DoorCheckメソッドを使い、すべての部屋の扉を生成し、マップの扉または、壁を描画します。
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
                    DoorCheck(y, x);
                }
            }
        }

        // スポーン地点の扉を全開
        rooms[spawnPos.y, spawnPos.x].GetComponent<RoomBehaviour>().OpenDoors(new Vector2Int(spawnPos.x, spawnPos.y));
    }

    /// <summary>
    /// roomsの要素数を受け取り、その部屋に扉を生成し、マップの扉または、壁を描画します。
    /// </summary>
    /// <param name="roomY"></param>
    /// <param name="roomX"></param>
    void DoorCheck(int roomY, int roomX)
    {
        bool[] doorStatus = new bool[4];

        // 上下左右に部屋がある場合doorStatusをtrueにします。
        for (int i = 0; i < 4; i++)
        {
            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            switch (i)
            {
                case 0:
                    if (roomY - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}　の上の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の上の部屋は　{rooms[roomY - 1, roomX]}");
                    doorStatus[0] = (rooms[roomY - 1, roomX] != null) ? true : false;
                    break;

                case 1:
                    if (roomY + 1 >= rooms.GetLength(1)) { /*Debug.Log($"{rooms[roomY, roomX]}　の下の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の下の部屋は　{rooms[roomY + 1, roomX]}");
                    doorStatus[1] = (rooms[roomY + 1, roomX] != null) ? true : false;
                    break;

                case 2:
                    if (roomX - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}　の左の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の左の部屋は　{rooms[roomY, roomX - 1]}");
                    doorStatus[2] = (rooms[roomY, roomX - 1] != null) ? true : false;
                    break;

                case 3:
                    if (roomX + 1 >= rooms.GetLength(0)) { /*Debug.Log($"{rooms[roomY, roomX]}　の右の部屋は　範囲外です");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}　の右の部屋は　{rooms[roomY, roomX + 1]}");
                    doorStatus[3] = (rooms[roomY, roomX + 1] != null) ? true : false;
                    break;

                default:
                    Debug.LogError($"switch文に渡す値が間違っています。:  {i}");
                    break;
            }
        }

        // 部屋のドアを配置
        rooms[roomY, roomX].GetComponent<RoomBehaviour>().InitRoom(doorStatus);
        // マップのドアまたは壁を描画
        maps[roomY, roomX].GetComponent<MapRoomBehaviour>().UpdateRoomMap(doorStatus);
    }
}