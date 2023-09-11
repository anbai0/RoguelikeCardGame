using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    public GameObject[,] rooms = new GameObject[6, 6];
    public Transform roomParent;

    // スポーン地点
    private int spawnY;
    private int spawnX;
    private const int spawnRandMin = 2;
    private const int spawnRandMax = 3;

    // ランダムウォーク
    private int curWalkY;
    private int curWalkX;
    private int randomWalkMin = 13;
    private int randomWalkMax = 15;
    private int movementLimit;
    private int walkCount;
    // バックトラッキング
    private Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    void Start()
    {
        // セルの中央(4個所からランダムで)スポーン地点を設定
        spawnY = Random.Range(spawnRandMin, spawnRandMax + 1);
        spawnX = Random.Range(spawnRandMin, spawnRandMax + 1);   
        rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity, roomParent);
        // 現在のランダムウォーク地点を更新
        curWalkY = spawnY;
        curWalkX = spawnX;
        // バックトラッキング用にスタックにプッシュ
        backTracking.Push(new Vector2Int(curWalkY, curWalkX));
        // わかりやすいように何番目に生成したか?と生成した配列の要素数を名前に入れています。
        rooms[curWalkY, curWalkX].gameObject.name = $"Room: 0 ({curWalkY}  {curWalkX})";
        RandomWalk();
        
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
                Debug.Log(latestCoordinate);
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
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;                   
                    break;

                case 1:
                    // 下に生成
                    curWalkX += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;                   
                    break;

                case 2:
                    // 左に生成
                    curWalkY -= 1;        
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;          
                    break;

                case 3:
                    // 右に生成
                    curWalkY += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++; 
                    break;

                default:
                    break;
            }
        }

        
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
                return curWalkX + 1 <= rooms.GetLength(1) - 1 && rooms[curWalkY, curWalkX + 1] == null;            
            case 2:
                return curWalkY - 1 >= 0 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                return curWalkY + 1 <= rooms.GetLength(0) - 1 && rooms[curWalkY + 1, curWalkX] == null;

            default:
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

    
}