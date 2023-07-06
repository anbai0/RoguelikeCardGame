using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollidar : MonoBehaviour
{
     private GameObject cameraPos;

    [SerializeField]
    private GameObject[] bossrooms;
    private GameObject currentRoom;
    public int roomNumber; 

    public MapJenerator mapjenerator;

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
       // currentRoom = mapjenerator.rooms[roomNumber];
        
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
            if (gameObject.CompareTag("gateforward"))
            {
                //GameObject nextroom = mapjenerator.rooms[roomNumber + 3];
                GameObject nextroom = mapjenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(-4, 1, 0);
                //roomNumber += 3;
            }
            if (gameObject.CompareTag("gateright"))
            {
                //GameObject nextroom = mapjenerator.rooms[roomNumber + 1];
                GameObject nextroom = mapjenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(0,1,4);
                //roomNumber += 1;
            }
            if (gameObject.CompareTag("gateleft"))
            {
                Debug.Log(roomNumber + "roomNUM");

                //GameObject nextroom = mapjenerator.rooms[roomNumber - 1];
                GameObject nextroom = mapjenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(
                    0, 1, -4);
                //roomNumber -= 1;

            }
            if (gameObject.CompareTag("gateback"))
            {
                //Transform cameraPos = mapjenerator.rooms[roomNumber - 4].transform.GetChild(5);
                GameObject nextroom = mapjenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(4, 1, 0);
                //roomNumber -= 4;

            }



        }
    }
}
