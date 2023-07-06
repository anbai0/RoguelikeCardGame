using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapJenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] rooms;
    [SerializeField]
    private GameObject[] bossrooms;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject takibi;
    [SerializeField]
    private GameObject zakoteki;
    [SerializeField]
    private GameObject tuyoiteki;
    [SerializeField]
    private GameObject treasurebox;
    [SerializeField]
    private GameObject tento;

    enum roomNum
    {
        ROOM1 = 0,
        ROOM2,
        ROOM3,
        ROOM4,
        ROOM5,
        ROOM6,
        ROOM7,
        ROOM8,
        ROOM9,
        ROOM10,
        ROOM11,
        ROOM12,
    };
    enum bossRoomNum
    {
        BOSSROOM1 = 0,
        BOSSROOM2,
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpawn();
        TakibiSpawn();
        zakotekiSpawn();
        tuyoitekiSpawn();

    }

    // Update is called once per framejuc
    void Update()
    {

    }

    private void PlayerSpawn()
    {

        int random = Random.Range(0, 100);
        if (random % 2 == 0)
        {
            player.transform.position = rooms[(int)roomNum.ROOM2].transform.position;
            Instantiate(zakoteki, rooms[(int)roomNum.ROOM3].transform.position, Quaternion.identity);
            zakoteki.transform.position = rooms[(int)roomNum.ROOM3].transform.position;

            //rooms[1].transform.GetComponent<GateCollidar>().roomNumber = 1;
        }
        else
        {
            player.transform.position = rooms[(int)roomNum.ROOM3].transform.position;
            Instantiate(zakoteki, rooms[(int)roomNum.ROOM2].transform.position, Quaternion.identity);
            zakoteki.transform.position = rooms[(int)roomNum.ROOM2].transform.position;

            //rooms[2].transform.GetComponent<GateCollidar>().roomNumber = 2;
        }
        RoomsSetting(random % 2 == 0);
    }

    private void RoomsSetting(bool isPlayerleft)
    {
        if (isPlayerleft)
        {
            GameObject gaterightObj = rooms[(int)roomNum.ROOM5].transform.transform.GetChild(3).transform.GetChild(0).gameObject;
            MeshRenderer gateright = gaterightObj.GetComponent<MeshRenderer>();
            gateright.enabled = false;
            BoxCollider gaterightcol = gaterightObj.GetComponent<BoxCollider>();
            gaterightcol.isTrigger = false;
            Instantiate(treasurebox, rooms[(int)roomNum.ROOM5].transform.position, Quaternion.identity);
            treasurebox.transform.position = rooms[(int)roomNum.ROOM5].transform.position;
            Instantiate(zakoteki, rooms[(int)roomNum.ROOM8].transform.position, Quaternion.identity);
            zakoteki.transform.position = rooms[(int)roomNum.ROOM8].transform.position;
        }
        else
        {
            GameObject gateleftObj = rooms[(int)roomNum.ROOM8].transform.GetChild(0).transform.GetChild(0).gameObject;
            MeshRenderer gateleft = gateleftObj.GetComponent<MeshRenderer>();
            gateleft.enabled = false;
            BoxCollider gateleftcol = gateleftObj.GetComponent<BoxCollider>();
            gateleftcol.isTrigger = false;
            Instantiate(treasurebox, rooms[(int)roomNum.ROOM8].transform.position, Quaternion.identity);
            treasurebox.transform.position = rooms[(int)roomNum.ROOM8].transform.position;
            Instantiate(zakoteki, rooms[(int)roomNum.ROOM5].transform.position, Quaternion.identity);
            zakoteki.transform.position = rooms[(int)roomNum.ROOM5].transform.position;
        }


    }
    private void BossRoomSetting(bool isleft)
    {
        if (isleft)
        {
            bossrooms[(int)bossRoomNum.BOSSROOM1].gameObject.SetActive(true);
            GameObject gateforwardObj = rooms[(int)roomNum.ROOM9].transform.GetChild(1).transform.GetChild(0).gameObject;
            MeshRenderer gateforward = gateforwardObj.GetComponent<MeshRenderer>();
            gateforward.enabled = true;
            BoxCollider gateforwardcol = gateforwardObj.GetComponent<BoxCollider>();
            gateforwardcol.isTrigger = true;
            GameObject wallbackObj = bossrooms[(int)bossRoomNum.BOSSROOM1].transform.GetChild(2).gameObject;
            MeshRenderer wallback = wallbackObj.GetComponent<MeshRenderer>();
            wallback.enabled = false;
            BoxCollider wallbackcol = wallbackObj.GetComponent<BoxCollider>();
            wallbackcol.isTrigger = true;

        }
        else
        {
            bossrooms[(int)bossRoomNum.BOSSROOM2].gameObject.SetActive(true);
            GameObject gateforwardObj = rooms[(int)roomNum.ROOM12].transform.GetChild(1).transform.GetChild(0).gameObject;
            MeshRenderer gateforward =gateforwardObj.GetComponent<MeshRenderer>();
            gateforward.enabled = true;
            BoxCollider gateforwardcol = gateforwardObj.GetComponent<BoxCollider>();
            gateforwardcol.isTrigger = true;
            GameObject wallbackObj = bossrooms[(int)bossRoomNum.BOSSROOM2].transform.GetChild(2).gameObject;
            MeshRenderer wallback = wallbackObj.GetComponent<MeshRenderer>();
            wallback.enabled = false;
            BoxCollider wallbackcol = wallbackObj.GetComponent<BoxCollider>();
            wallbackcol.isTrigger = true;

        }
    }
    private void TakibiSpawn()
    {
        int random = Random.Range(0, 100);
        if (random % 2 == 0)
        {
            Instantiate(takibi, rooms[(int)roomNum.ROOM9].transform.position, Quaternion.identity);
            takibi.transform.position = rooms[(int)roomNum.ROOM9].transform.position;
            Instantiate(tento, rooms[(int)roomNum.ROOM12].transform.position, Quaternion.identity);
            tento.transform.position = rooms[(int)roomNum.ROOM12].transform.position;
        }
        else
        {
            Instantiate(takibi, rooms[(int)roomNum.ROOM12].transform.position, Quaternion.identity);
            takibi.transform.position = rooms[(int)roomNum.ROOM12].transform.position;
            Instantiate(tento, rooms[(int)roomNum.ROOM9].transform.position, Quaternion.identity);
            tento.transform.position = rooms[(int)roomNum.ROOM9].transform.position;
            tento.transform.Rotate(new Vector3(0, 180f, 0));
        }
        BossRoomSetting(random % 2 == 0);
    }
    private void zakotekiSpawn()
    {
        Instantiate(zakoteki, rooms[(int)roomNum.ROOM1].transform.position, Quaternion.identity);
        zakoteki.transform.position = rooms[(int)roomNum.ROOM1].transform.position;
        Instantiate(zakoteki, rooms[(int)roomNum.ROOM4].transform.position, Quaternion.identity);
        zakoteki.transform.position = rooms[(int)roomNum.ROOM4].transform.position;
        Instantiate(zakoteki, rooms[(int)roomNum.ROOM6].transform.position, Quaternion.identity);
        zakoteki.transform.position = rooms[(int)roomNum.ROOM6].transform.position;
        Instantiate(zakoteki, rooms[(int)roomNum.ROOM7].transform.position, Quaternion.identity);
        zakoteki.transform.position = rooms[(int)roomNum.ROOM7].transform.position;

    }
    private void tuyoitekiSpawn()
    {
        Instantiate(tuyoiteki, rooms[(int)roomNum.ROOM10].transform.position, Quaternion.identity);
        tuyoiteki.transform.position = rooms[(int)roomNum.ROOM10].transform.position;
        Instantiate(tuyoiteki, rooms[(int)roomNum.ROOM11].transform.position, Quaternion.identity);
        tuyoiteki.transform.position = rooms[(int)roomNum.ROOM11].transform.position;
    }
}



