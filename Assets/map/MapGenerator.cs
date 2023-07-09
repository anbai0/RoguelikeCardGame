using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStatus
{
    public GameObject roomObject;
    public GameObject stopForward;
    public GameObject stopRight;
    public GameObject stopLeft;
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] rooms;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject bonfire;
    [SerializeField]
    private GameObject smallEnemy;
    [SerializeField]
    private GameObject strongEnemy;
    [SerializeField]
    private GameObject treasureBox;
    [SerializeField]
    private GameObject tent;
    [SerializeField]
    private RoomStatus[] roomStatuses = new RoomStatus[12];

    [SerializeField] private GameObject cameraPos2, cameraPos3;

    enum RoomNum
    {
        Room1 = 1,
        Room2,
        Room3,
        Room4,
        Room5,
        Room6,
        Room7,
        Room8,
        Room9,
        Room10,
        Room11,
        Room12,
        BossRoom1,
        BossRoom2,
    };



    // Start is called before the first frame update
    void Start()
    {
        BonfireSpawn();
        SmallEnemySpawn();
        StrongEnemySpawn();
        PlayerSpawn();
    }

    private void PlayerSpawn()
    {
        int random = Random.Range(0, 100);
        if (random % 2 == 0)
        {
            player.transform.position = rooms[(int)RoomNum.Room2].transform.position + new Vector3 (0, -1.35f, 0);
            Instantiate(smallEnemy, rooms[(int)RoomNum.Room3].transform.position, Quaternion.Euler(0f, 180f, 0f));
            Camera cam = Camera.main;
            cam.transform.position = cameraPos2.transform.position;
            RoomsSetting(true);
        }
        else
        {
            player.transform.position = rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, -1.35f, 0);
            Instantiate(smallEnemy, rooms[(int)RoomNum.Room2].transform.position, Quaternion.identity);
            Camera cam = Camera.main;
            cam.transform.position = cameraPos3.transform.position;
            RoomsSetting(false);
        }
    }

    private void RoomsSetting(bool isPlayerLeft)
    {
        if (isPlayerLeft)
        {
        //    GameObject gateRightObj = rooms[(int)RoomNum.Room5].transform.GetChild(3).transform.gameObject;         //gateRightというObjectを取得
        //    //MeshRenderer gateRight = gateRightObj.GetComponent<MeshRenderer>();                                             //メッシュレンダラーを取得し
        //    //gateRight.enabled = false;                                                                                      //非表示に
        //    BoxCollider gateRightCol = gateRightObj.GetComponent<BoxCollider>();                                    //BOXColliderを取得し
        //    gateRightCol.isTrigger = false;
        //    Instantiate(treasureBox, rooms[(int)RoomNum.Room5].transform.position, Quaternion.identity);
        //    Instantiate(smallEnemy, rooms[(int)RoomNum.Room8].transform.position, Quaternion.identity);
        //}
        //else
        //{
        //    GameObject gateLeftObj = rooms[(int)RoomNum.Room8].transform.GetChild(0).transform.gameObject;
        //    //MeshRenderer gateLeft = gateLeftObj.GetComponent<MeshRenderer>();
        //    //gateLeft.enabled = false;
        //    BoxCollider gateLeftCol = gateLeftObj.GetComponent<BoxCollider>();
        //    gateLeftCol.isTrigger = false;
        //    Instantiate(treasureBox, rooms[(int)RoomNum.Room8].transform.position, Quaternion.identity);
        //    Instantiate(smallEnemy, rooms[(int)RoomNum.Room5].transform.position, Quaternion.identity);
        }
    }

    private void BossRoomSetting(bool isLeft)
    {
        if (isLeft)
        {
            rooms[(int)RoomNum.BossRoom1].SetActive(true);
            GameObject gateForwardObj = rooms[(int)RoomNum.Room9].transform.GetChild(1).transform.GetChild(0).gameObject;
            MeshRenderer gateForward = gateForwardObj.GetComponent<MeshRenderer>();
            gateForward.enabled = true;
            BoxCollider gateForwardCol = gateForwardObj.GetComponent<BoxCollider>();
            gateForwardCol.isTrigger = true;
            GameObject wallBackObj = rooms[(int)RoomNum.BossRoom1].transform.GetChild(2).gameObject;
            MeshRenderer wallBack = wallBackObj.GetComponent<MeshRenderer>();
            wallBack.enabled = false;
            BoxCollider wallBackCol = wallBackObj.GetComponent<BoxCollider>();
            wallBackCol.isTrigger = true;
        }
        else
        {
            rooms[(int)RoomNum.BossRoom2].SetActive(true);
            GameObject gateForwardObj = rooms[(int)RoomNum.Room12].transform.GetChild(1).transform.GetChild(0).gameObject;
            MeshRenderer gateForward = gateForwardObj.GetComponent<MeshRenderer>();
            gateForward.enabled = true;
            BoxCollider gateForwardCol = gateForwardObj.GetComponent<BoxCollider>();
            gateForwardCol.isTrigger = true;
            GameObject wallBackObj = rooms[(int)RoomNum.BossRoom2].transform.GetChild(2).gameObject;
            MeshRenderer wallBack = wallBackObj.GetComponent<MeshRenderer>();
            wallBack.enabled = false;
            BoxCollider wallBackCol = wallBackObj.GetComponent<BoxCollider>();
            wallBackCol.isTrigger = true;
        }
    }

    private void BonfireSpawn()
    {
        //int random = Random.Range(0, 100);
        //if (random % 2 == 0)
        //{
        //    Instantiate(bonfire, rooms[(int)RoomNum.Room9].transform.position, Quaternion.identity);
        //    Instantiate(tent, rooms[(int)RoomNum.Room12].transform.position, Quaternion.identity);
        //}
        //else
        //{
        //    Instantiate(bonfire, rooms[(int)RoomNum.Room12].transform.position, Quaternion.identity);
        //    Instantiate(tent, rooms[(int)RoomNum.Room9].transform.position, Quaternion.identity);
        //    tent.transform.Rotate(new Vector3(0, 180f, 0));
        //}
        //BossRoomSetting(random % 2 == 0);
    }

    private void SmallEnemySpawn()
    {
        Instantiate(smallEnemy, rooms[(int)RoomNum.Room1].transform.position, Quaternion.identity);
        Instantiate(smallEnemy, rooms[(int)RoomNum.Room4].transform.position, Quaternion.identity);
        Instantiate(smallEnemy, rooms[(int)RoomNum.Room6].transform.position, Quaternion.identity);
        Instantiate(smallEnemy, rooms[(int)RoomNum.Room7].transform.position, Quaternion.identity);
    }

    private void StrongEnemySpawn()
    {
        Instantiate(strongEnemy, rooms[(int)RoomNum.Room10].transform.position, Quaternion.identity);
        Instantiate(strongEnemy, rooms[(int)RoomNum.Room11].transform.position, Quaternion.identity);
    }
}