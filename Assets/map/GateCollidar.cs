using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollidar : MonoBehaviour
{
    private GameObject cameraPos;

    private GameObject currentRoom;
    public int roomNumber; 


    private MapGenerator mapGenerator;

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
        BOSSROOM1,
        BOSSROOM2,
    };

    // Start is called before the first frame update
    void Start()
    {
        // currentRoom = mapGenerator.rooms[roomNumber];

        mapGenerator = GameObject.FindObjectOfType<MapGenerator>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {

            Camera cam = Camera.main;


            if (gameObject.CompareTag("GateForward"))
            {
                GameObject nextroom = mapGenerator.rooms[roomNumber];       //当たった各gateに設定されているroomNumberのroomのオブジェクトを取得する
                Transform cameraPos = nextroom.transform.GetChild(5);       //子オブジェクトの６番目であるcameraPosのTransformを取得
                cam.transform.position = cameraPos.position;                //取得したTransformのPositionをカメラのPositionに代入
                other.transform.position = nextroom.transform.position + new Vector3(0, -1.35f, -4);     //Playerを次の部屋に移動させる
                Debug.Log(nextroom);

            }
            if (gameObject.CompareTag("GateRight"))
            {
                GameObject nextroom = mapGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(-4, -1.35f, 0);
                Debug.Log(nextroom);

            }
            if (gameObject.CompareTag("GateLeft"))
            {

                GameObject nextroom = mapGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(4, -1.35f, 0);
                Debug.Log(nextroom);

            }
            if (gameObject.CompareTag("GateBack"))
            {
                GameObject nextroom = mapGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(0, -1.35f, 4);
                Debug.Log(nextroom);

            }



        }
    }
}
