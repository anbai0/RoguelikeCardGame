using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    private DungeonGenerator dungeon;
    public GameObject[] gate;  // 0 - forward, 1 - back, 2 - Left, 3 - Right
    public GameObject[] doors;
    public GameObject backFloor;
    private bool[] doorsStatus = new bool[4];

    private void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
    }

    /// <summary>
    /// 要素数が4つのbool型配列を受け取り、対応した場所のgateのSetActiveをtrueにします。
    /// <para>0 - forward, 1 - back, 2 - Left, 3 - Right</para>
    /// </summary>
    /// <param name="status"></param>
    public void InitRoom(bool[] status)
    {
        // ドアを開けるときに使うのでdoorsStatusへ代入
        doorsStatus = status;

        for (int i = 0; i < status.Length; i++)
        {
            gate[i].SetActive(status[i]);
            doors[i].SetActive(status[i]);
        }
    }

    /// <summary>
    /// 部屋にある扉と、それに対応した隣の部屋にある扉をすべて開くメソッドです。
    /// </summary>
    public void OpenDoors(Vector2Int curRoomPos)
    {

        // 上
        if (doorsStatus[0])
        {
            // この部屋の上のドアを開け、コライダーをtrueに
            doors[0].SetActive(false);
            gate[0].GetComponent<BoxCollider>().enabled = true;

            RoomBehaviour forwardRoom = dungeon.rooms[curRoomPos.y - 1, curRoomPos.x].transform.GetComponent<RoomBehaviour>();
            // 上の部屋の下のドアを開け、下の床を表示(コライダーもtrueになります)
            forwardRoom.doors[1].SetActive(false);
            forwardRoom.GetComponent<RoomBehaviour>().backFloor.SetActive(true);
        }

        // 下
        if (doorsStatus[1])
        {
            // この部屋のドアを開け、下の床を表示(コライダーもtrueになります)
            doors[1].SetActive(false);
            backFloor.SetActive(true);

            RoomBehaviour backRoom = dungeon.rooms[curRoomPos.y + 1, curRoomPos.x].transform.GetComponent<RoomBehaviour>();
            // 下の部屋の上の扉を開け、コライダーをtrueに
            backRoom.doors[0].SetActive(false);
            backRoom.gate[0].GetComponent<BoxCollider>().enabled = true;
        }

        // 左
        if (doorsStatus[2])
        {
            // この部屋のドアを開け、コライダーをtrueに
            doors[2].SetActive(false);
            gate[2].GetComponent<BoxCollider>().enabled = true;

            RoomBehaviour leftRoom = dungeon.rooms[curRoomPos.y, curRoomPos.x - 1].transform.GetComponent<RoomBehaviour>();
            // 左の部屋の右の扉を開け、コライダーをtrueに
            leftRoom.doors[3].SetActive(false);
            leftRoom.gate[3].GetComponent<BoxCollider>().enabled = true;
        }

        // 右
        if (doorsStatus[3])
        {
            // この部屋のドアを開け、コライダーをtrueに
            doors[3].SetActive(false);
            gate[3].GetComponent<BoxCollider>().enabled = true;

            RoomBehaviour rightRoom = dungeon.rooms[curRoomPos.y, curRoomPos.x + 1].transform.GetComponent<RoomBehaviour>();
            // 右の部屋の左の扉を開け、コライダーをtrueに
            rightRoom.doors[2].SetActive(false);
            rightRoom.gate[2].GetComponent<BoxCollider>().enabled = true;
        }
    }
}
