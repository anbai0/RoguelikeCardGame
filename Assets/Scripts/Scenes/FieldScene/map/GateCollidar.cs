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
        // ���������̂�Player��������
        if (other.CompareTag("Player"))
        {

            Camera cam = Camera.main;

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g�����̔���������
            if (gameObject.CompareTag("GateForward"))
            {
                GameObject nextroom = roomGenerator.rooms[roomNumber];                                      // ���������egate�ɐݒ肳��Ă���roomNumber��room�̃I�u�W�F�N�g���擾����
                Transform cameraPos = nextroom.transform.GetChild(5);                                       // �q�I�u�W�F�N�g�̂U�Ԗڂł���cameraPos��Transform���擾
                cam.transform.position = cameraPos.position;                                                // �擾����Transform��Position���J������Position�ɑ��
                other.transform.position = nextroom.transform.position + new Vector3(0, -1.35f, -3.6f);     // Player�����̕����Ɉړ�������
                Debug.Log(nextroom);

            }

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g���E�̔���������
            if (gameObject.CompareTag("GateRight"))
            {
                GameObject nextroom = roomGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(-3.6f, -1.35f, 0);
                Debug.Log(nextroom);

            }

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g�����̔���������
            if (gameObject.CompareTag("GateLeft"))
            {
                GameObject nextroom = roomGenerator.rooms[roomNumber];
                Transform cameraPos = nextroom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextroom.transform.position + new Vector3(3.6f, -1.35f, 0);
                Debug.Log(nextroom);

            }

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g����O�̔���������
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
