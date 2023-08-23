using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    public static bool isPlayerActive = true;

    public float moveSpeed;                     //�v���C���[�̓����X�s�[�h
    public float rotationSpeed = 10f;       //������ς��鑬�x
    private float moveHorizontal;
    private float moveVertical;

    private Animator animator;   

    // �����֌W
    private FieldSceneManager fieldManager;
    private RoomsManager roomsM;
    public GameObject bonfire { get; private set; }
    public GameObject treasureBox { get; private set; }
    public GameObject enemy { get; private set; }
    public string enemyTag { get; private set; }
    // �����̈ړ�
    private Vector3 pPos;
    public int lastRoomNum;     // �v���C���[���Ō�ɂ��������̔ԍ�

    void Start()
    {
        pPos = gameObject.transform.position;

        fieldManager = FindObjectOfType<FieldSceneManager>();
        roomsM = FindObjectOfType<RoomsManager>();
        animator = GetComponent<Animator>();
        isPlayerActive = true;
    }

    void Update()
    {
        // isPlayerActive��true���t�F�[�h�C�����I����Ă���Ƃ�
        if (isPlayerActive && !FadeController.fadeInDone)
        {
            PlayerMove();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AccessDoorAfterWin();
        }
    }

    private void PlayerMove()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;


        if (movement.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("IsWalking", true); // �����A�j���[�V�������Đ�
        }
        else
        {
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~
        }

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }


    private void OnCollisionEnter(Collision collision)
    {
        #region �V�[���J��
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            // player�𓮂��Ȃ����鏈��
            isPlayerActive = false;
            animator.SetBool("IsWalking", false);

            bonfire = collision.gameObject;
            fieldManager.LoadBonfireScene();        // ���΃V�[�������[�h
        }

        if (collision.gameObject.CompareTag("TreasureBox"))
        {
            // player�𓮂��Ȃ����鏈��
            isPlayerActive = false;
            animator.SetBool("IsWalking", false);

            treasureBox = collision.gameObject;
            fieldManager.LoadTreasureBoxScene();   //�󔠃V�[�������[�h
        }

        if (collision.gameObject.CompareTag("Shop"))
        {
            // player�𓮂��Ȃ����鏈��
            isPlayerActive = false;
            animator.SetBool("IsWalking", false);

            // �w�肵�����O�̃V�[�����擾
            Scene sceneToHide = SceneManager.GetSceneByName("ShopScene");

            // ���[�h����Ă��Ȃ��ꍇ�ɏ��������s
            if (!sceneToHide.isLoaded)
            {
                fieldManager.LoadShopScene();           // �V���b�v�V�[�������[�h
            }
            else
            {
                // �V���b�v�V�[�������[�h����Ă����ꍇ�A�V���b�v�V�[���̃I�u�W�F�N�g��\��
                fieldManager.ActivateShopScene();
            }
        }

        if (collision.gameObject.CompareTag("SmallEnemy") || collision.gameObject.CompareTag("StrongEnemy") || collision.gameObject.CompareTag("Boss"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //�퓬�V�[�������[�h
        }
        #endregion
    }


    private void OnTriggerStay(Collider other)
    {
        if (roomsM == null) return;

        #region �����镔������肷�鏈��
        // lastRoom�ƍ����镔�����Ⴄ�ꍇ
        if (roomsM.rooms[lastRoomNum] != other.gameObject)
        {
            for (int roomNum = 1; roomNum <= roomsM.rooms.Length - 1; roomNum++)
            {
                if (roomsM.rooms[roomNum] == other.gameObject)
                {
                    lastRoomNum = roomNum;         // lastRoom���X�V
                    break;
                }
            }
        }
        #endregion

        
        #region �����ړ�����
        if (other.gameObject.CompareTag("GateForward"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");                                                   // SE�Đ�
            GameObject nextRoom = roomsM.rooms[lastRoomNum + 4];                                              // ���̕������擾
            Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;                    // �J���������̕����Ɉړ�
            roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;           // ���C�g�����̕����Ɉړ�
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, pPos.y, -3.6f);      // Player�����̕����Ɉړ�
        }

        if (other.gameObject.CompareTag("GateRight"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            GameObject nextRoom = roomsM.rooms[lastRoomNum + 1];
            Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
            roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(-3.6f, pPos.y, 0);
        }

        if (other.gameObject.CompareTag("GateLeft"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            GameObject nextRoom = roomsM.rooms[lastRoomNum - 1];
            Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
            roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(3.6f, pPos.y, 0);
        }

        if (other.gameObject.CompareTag("GateBack"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            GameObject nextRoom = roomsM.rooms[lastRoomNum - 4];
            Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
            roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, pPos.y, 3.6f);
        }
        #endregion
    }


    /// <summary>
    /// EnableRoomDoorAccess���\�b�h���g���A�����镔���̔������ׂĊJ���܂��B
    /// </summary>
    public void AccessDoorAfterWin()
    {
        roomsM.EnableRoomDoorAccess(lastRoomNum);
    }
}