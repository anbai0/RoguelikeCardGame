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
    /// �v�f����4��bool�^�z����󂯎��A�Ή������ꏊ��gate��SetActive��true�ɂ��܂��B
    /// <para>0 - forward, 1 - back, 2 - Left, 3 - Right</para>
    /// </summary>
    /// <param name="status"></param>
    public void InitRoom(bool[] status)
    {
        // �h�A���J����Ƃ��Ɏg���̂�doorsStatus�֑��
        doorsStatus = status;

        for (int i = 0; i < status.Length; i++)
        {
            gate[i].SetActive(status[i]);
            doors[i].SetActive(status[i]);
        }
    }

    /// <summary>
    /// �����ɂ�����ƁA����ɑΉ������ׂ̕����ɂ���������ׂĊJ�����\�b�h�ł��B
    /// </summary>
    public void OpenDoors(Vector2Int curRoomPos)
    {

        // ��
        if (doorsStatus[0])
        {
            // ���̕����̏�̃h�A���J���A�R���C�_�[��true��
            doors[0].SetActive(false);
            gate[0].GetComponent<BoxCollider>().enabled = true;

            RoomBehaviour forwardRoom = dungeon.rooms[curRoomPos.y - 1, curRoomPos.x].transform.GetComponent<RoomBehaviour>();
            // ��̕����̉��̃h�A���J���A���̏���\��(�R���C�_�[��true�ɂȂ�܂�)
            forwardRoom.doors[1].SetActive(false);
            forwardRoom.GetComponent<RoomBehaviour>().backFloor.SetActive(true);
        }

        // ��
        if (doorsStatus[1])
        {
            // ���̕����̃h�A���J���A���̏���\��(�R���C�_�[��true�ɂȂ�܂�)
            doors[1].SetActive(false);
            backFloor.SetActive(true);

            RoomBehaviour backRoom = dungeon.rooms[curRoomPos.y + 1, curRoomPos.x].transform.GetComponent<RoomBehaviour>();
            // ���̕����̏�̔����J���A�R���C�_�[��true��
            backRoom.doors[0].SetActive(false);
            backRoom.gate[0].GetComponent<BoxCollider>().enabled = true;
        }

        // ��
        if (doorsStatus[2])
        {
            // ���̕����̃h�A���J���A�R���C�_�[��true��
            doors[2].SetActive(false);
            gate[2].GetComponent<BoxCollider>().enabled = true;

            RoomBehaviour leftRoom = dungeon.rooms[curRoomPos.y, curRoomPos.x - 1].transform.GetComponent<RoomBehaviour>();
            // ���̕����̉E�̔����J���A�R���C�_�[��true��
            leftRoom.doors[3].SetActive(false);
            leftRoom.gate[3].GetComponent<BoxCollider>().enabled = true;
        }

        // �E
        if (doorsStatus[3])
        {
            // ���̕����̃h�A���J���A�R���C�_�[��true��
            doors[3].SetActive(false);
            gate[3].GetComponent<BoxCollider>().enabled = true;

            RoomBehaviour rightRoom = dungeon.rooms[curRoomPos.y, curRoomPos.x + 1].transform.GetComponent<RoomBehaviour>();
            // �E�̕����̍��̔����J���A�R���C�_�[��true��
            rightRoom.doors[2].SetActive(false);
            rightRoom.gate[2].GetComponent<BoxCollider>().enabled = true;
        }
    }
}
