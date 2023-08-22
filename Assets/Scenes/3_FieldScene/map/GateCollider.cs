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
        // ���������̂�Player��������
        if (other.CompareTag("Player"))
        {

            Camera cam = Camera.main;

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g�����̔���������
            if (gameObject.CompareTag("GateForward"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];                                      // ���������egate�ɐݒ肳��Ă���roomNumber��room�̃I�u�W�F�N�g���擾����
                Transform cameraPos = nextRoom.transform.GetChild(5);                                       // �q�I�u�W�F�N�g�̂U�Ԗڂł���cameraPos��Transform���擾
                cam.transform.position = cameraPos.position;                                                // �擾����Transform��Position���J������Position�ɑ��
                other.transform.position = nextRoom.transform.position + new Vector3(0, playerY, -3.6f);     // Player�����̕����Ɉړ�������
                //Debug.Log(nextRoom);

            }

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g���E�̔���������
            if (gameObject.CompareTag("GateRight"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];
                Transform cameraPos = nextRoom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextRoom.transform.position + new Vector3(-3.6f, playerY, 0);
                //Debug.Log(nextRoom);

            }

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g�����̔���������
            if (gameObject.CompareTag("GateLeft"))
            {
                GameObject nextRoom = roomsManager.rooms[roomNumber];
                Transform cameraPos = nextRoom.transform.GetChild(5);
                cam.transform.position = cameraPos.position;
                other.transform.position = nextRoom.transform.position + new Vector3(3.6f, playerY, 0);
                //Debug.Log(nextRoom);

            }

            // �X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g����O�̔���������
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
