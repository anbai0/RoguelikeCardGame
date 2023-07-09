using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollidar : MonoBehaviour
{
    private GameObject cameraPos;

    public int roomNumber;

    private RoomGenerator roomGenerator;

    void Start()
    {
        roomGenerator = GameObject.FindObjectOfType<RoomGenerator>();

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
                GameObject nextroom = roomGenerator.rooms[roomNumber];                                      // 当たった各gateに設定されているroomNumberのroomのオブジェクトを取得する
                Transform cameraPos = nextroom.transform.GetChild(5);                                       // 子オブジェクトの６番目であるcameraPosのTransformを取得
                cam.transform.position = cameraPos.position;                                                // 取得したTransformのPositionをカメラのPositionに代入
                other.transform.position = nextroom.transform.position + new Vector3(0, -1.35f, -3.6f);     // Playerを次の部屋に移動させる
                Debug.Log(nextroom);

            }

            // スクリプトをアタッチしているオブジェクトが右の扉だったら
            if (gameObject.CompareTag("GateRight"))
            {
                GameObject nextroom = roomGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(-3.6f, -1.35f, 0);
                Debug.Log(nextroom);

            }

            // スクリプトをアタッチしているオブジェクトが左の扉だったら
            if (gameObject.CompareTag("GateLeft"))
            {
                GameObject nextroom = roomGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(3.6f, -1.35f, 0);
                Debug.Log(nextroom);

            }

            // スクリプトをアタッチしているオブジェクトが手前の扉だったら
            if (gameObject.CompareTag("GateBack"))
            {
                GameObject nextroom = roomGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(0, -1.35f, 3.6f);
                Debug.Log(nextroom);

            }

        }
    }
}
