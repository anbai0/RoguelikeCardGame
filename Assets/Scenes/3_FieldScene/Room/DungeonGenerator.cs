using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    public GameObject[,] rooms = new GameObject[6, 6];

    // スポーン地点
    public int spawnY;
    public int spawnX;
    public const int spawnRandMin = 2;
    public const int spawnRandMax = 3;

    // ランダムウォーク
    public int curWalkY;
    public int curWalkX;
    public int randomWalkMin = 13;
    public int randomWalkMax = 15;
    public int movementLimit;
    public int walkCount;
    public Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    void Start()
    {
        // セルの中央(4個所からランダムで)スポーン地点を設定
        spawnY = Random.Range(spawnRandMin, spawnRandMax);
        spawnX = Random.Range(spawnRandMin, spawnRandMax);   
        rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity);
        // 現在のランダムウォーク地点を更新
        curWalkY = spawnY;
        curWalkX = spawnX;
        // バックトラッキング用にスタックにプッシュ
        backTracking.Push(new Vector2Int(curWalkY, curWalkX));
    }


    void RandomWalk()
    {
        // 部屋を生成する回数を設定
        movementLimit = Random.Range(randomWalkMin, randomWalkMax);



        while (walkCount <= movementLimit)
        {
            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            int direction;
            int loopCount = -1;

            do
            {
                direction = Random.Range(0, 3);
                loopCount++;
            } while (!IsDirectionValid(direction) && loopCount <= 10);


            if (loopCount == 10)
            {
                direction = -1;
                Vector2Int latestCoordinate = backTracking.Pop();
                curWalkY = latestCoordinate.y;
                curWalkX = latestCoordinate.x;        
            }


            switch (direction)
            {
                case 0:
                    // 上に生成
                    curWalkX -= 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;
                    break;
                case 1:
                    // 下に生成
                    curWalkX += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;
                    break;
                case 2:
                    // 左に生成
                    curWalkY -= 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
                    // バックトラッキング用にスタックにプッシュ
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;
                    break;
                case 3:
                    // 右に生成
                    curWalkY += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
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
        switch (dir)
        {
            case 0:
                // 上に行ける場合true
                return curWalkX -1 >= 0 && rooms[curWalkY, curWalkX - 1] == null;
            case 1:
                // 下に行ける場合true
                return curWalkX <= rooms.GetLength(1) - 1 && rooms[curWalkY, curWalkX + 1] == null;
            case 2:
                // 左に行ける場合true
                return curWalkY >= 0 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                // 右に行ける場合true
                return curWalkY <= rooms.GetLength(0) - 1 && rooms[curWalkY + 1, curWalkX] == null;
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
        return new Vector3(vertical * 20f, horizontal * 20f, 0f);
    }
}