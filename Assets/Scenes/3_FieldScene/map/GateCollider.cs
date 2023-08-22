using UnityEngine;

public class GateCollider : MonoBehaviour
{
    private GameObject cameraPos;

    public int roomNumber;

    private RoomsManager roomsManager;

    float playerY = -2.33f;

    void Start()
    {
        roomsManager = GameObject.FindObjectOfType<RoomsManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        // 当たったのがPlayerだったら
        if (other.CompareTag("Player"))
        {

            Camera cam = Camera.main;

            // スクリプトをアタッチしているオブジェクトが奥の扉だったら
            if (gameObject.CompareTag("GateForward"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];                                      // 当たった各gateに設定されているroomNumberのroomのオブジェクトを取得する
                Transform cameraPos = nextRoom.transform.GetChild(5);                                       // 子オブジェクトの６番目であるcameraPosのTransformを取得
                cam.transform.position = cameraPos.position;                                                // 取得したTransformのPositionをカメラのPositionに代入
                other.transform.position = nextRoom.transform.position + new Vector3(0, playerY, -3.6f);     // Playerを次の部屋に移動させる
                //Debug.Log(nextRoom);

            }

            // スクリプトをアタッチしているオブジェクトが右の扉だったら
            if (gameObject.CompareTag("GateRight"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];
                Transform cameraPos = nextRoom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextRoom.transform.position + new Vector3(-3.6f, playerY, 0);
                //Debug.Log(nextRoom);

            }

            // スクリプトをアタッチしているオブジェクトが左の扉だったら
            if (gameObject.CompareTag("GateLeft"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];
                Transform cameraPos = nextRoom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextRoom.transform.position + new Vector3(3.6f, playerY, 0);
                //Debug.Log(nextRoom);

            }

            // スクリプトをアタッチしているオブジェクトが手前の扉だったら
            if (gameObject.CompareTag("GateBack"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];
                Transform cameraPos = nextRoom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextRoom.transform.position + new Vector3(0, playerY, 3.6f);
                //Debug.Log(nextRoom);

            }

        }
    }
}
