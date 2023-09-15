using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool isEvents = true;
    public bool isSetting = false;
    public bool isConfimDeck = false;

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
    public int lastRoomNum;     // �v���C���[���Ō�ɂ��������̔ԍ�

    [SerializeField] DungeonGenerator dungeon;
    Vector2Int playerPos;


    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        // �V���O���g��
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        fieldManager = FindObjectOfType<FieldSceneManager>();
        roomsM = FindObjectOfType<RoomsManager>();
        animator = GetComponent<Animator>();

        dungeon = FindObjectOfType<DungeonGenerator>();
        playerPos = dungeon.spawnPos;
    }

    void Update()
    {
        Debug.Log("isEvents: " + isEvents);
        Debug.Log("isSetting: " + isSetting);
        Debug.Log("isConfimDeck: " + isConfimDeck);
        if (!isEvents && !isSetting && !isConfimDeck)
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeInBGMVolume());       // BGM���Đ�
            PlayerMove();
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
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGM���ꎞ��~

            isEvents = true;
            animator.SetBool("IsWalking", false);

            bonfire = collision.gameObject;
            fieldManager.LoadBonfireScene();        // ���΃V�[�������[�h  
        }

        if (collision.gameObject.CompareTag("TreasureBox"))
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGM���ꎞ��~

            // player�𓮂��Ȃ����鏈��
            isEvents = true;
            animator.SetBool("IsWalking", false);

            treasureBox = collision.gameObject;
            fieldManager.LoadTreasureBoxScene();   //�󔠃V�[�������[�h
        }

        if (collision.gameObject.CompareTag("Shop"))
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGM���ꎞ��~

            // player�𓮂��Ȃ����鏈��
            isEvents = true;
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
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGM���ꎞ��~

            // player�𓮂��Ȃ����鏈��
            isEvents = true;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //�퓬�V�[�������[�h
            Invoke("AccessDoorAfterWin", 2.0f); //���[�h���鎞�Ԃ��l������2�b�҂��Ă��珈�����Ăяo��
        }
        #endregion
    }


    private void OnTriggerStay(Collider other)
    {
        //if (roomsM == null) return;

        #region �����镔������肷�鏈��
        // lastRoom�ƍ����镔�����Ⴄ�ꍇ
        //if (roomsM.rooms[lastRoomNum] != other.gameObject)
        //{
        //    for (int roomNum = 1; roomNum <= roomsM.rooms.Length - 1; roomNum++)
        //    {
        //        if (roomsM.rooms[roomNum] == other.gameObject)
        //        {
        //            lastRoomNum = roomNum;         // lastRoom���X�V
        //            break;
        //        }
        //    }
        //}
        #endregion

        #region �����ړ�����
        //if (other.gameObject.CompareTag("GateForward"))
        //{
        //    AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");                                                   // SE�Đ�
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum + 4];                                              // ���̕������擾
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;                    // �J���������̕����Ɉړ�
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;           // ���C�g�����̕����Ɉړ�
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, -3.6f);      // Player�����̕����Ɉړ�

        //    // ���΂̃G�t�F�N�g�̐؂�ւ�
        //    Transform bonfirePrefab = nextRoom.transform.Find("Bonfire(Clone)");
        //    if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        //    {
        //        bonfirePrefab.transform.GetComponentInChildren<ParticleSystem>().Play();
        //    }
        //    Transform lastRoomBonfire = roomsM.rooms[lastRoomNum].transform.Find("Bonfire(Clone)");
        //    if (lastRoomBonfire != null)
        //    {
        //        lastRoomBonfire.transform.GetComponentInChildren<ParticleSystem>().Stop();
        //    }
        //}

        //if (other.gameObject.CompareTag("GateRight"))
        //{
        //    AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum + 1];
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(-3.6f, transform.position.y, 0);

        //    // ���΂̃G�t�F�N�g�̐؂�ւ�
        //    Transform bonfirePrefab = nextRoom.transform.Find("Bonfire(Clone)");
        //    if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        //    {
        //        bonfirePrefab.transform.GetComponentInChildren<ParticleSystem>().Play();
        //    }
        //    Transform lastRoomBonfire = roomsM.rooms[lastRoomNum].transform.Find("Bonfire(Clone)");
        //    if (lastRoomBonfire != null)
        //    {
        //        lastRoomBonfire.transform.GetComponentInChildren<ParticleSystem>().Stop();
        //    }
        //}

        //if (other.gameObject.CompareTag("GateLeft"))
        //{
        //    AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum - 1];
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(3.6f, transform.position.y, 0);

        //    // ���΂̃G�t�F�N�g�̐؂�ւ�
        //    Transform bonfirePrefab = nextRoom.transform.Find("Bonfire(Clone)");
        //    if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        //    {
        //        bonfirePrefab.transform.GetComponentInChildren<ParticleSystem>().Play();
        //    }
        //    Transform lastRoomBonfire = roomsM.rooms[lastRoomNum].transform.Find("Bonfire(Clone)");
        //    if (lastRoomBonfire != null)
        //    {
        //        lastRoomBonfire.transform.GetComponentInChildren<ParticleSystem>().Stop();
        //    }
        //}

        //if (other.gameObject.CompareTag("GateBack"))
        //{
        //    AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum - 4];
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, 3.6f);

        //    // ���΂̃G�t�F�N�g�̐؂�ւ�
        //    Transform bonfirePrefab = nextRoom.transform.Find("Bonfire(Clone)");
        //    if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        //    {
        //        bonfirePrefab.transform.GetComponentInChildren<ParticleSystem>().Play();
        //    }
        //    Transform lastRoomBonfire = roomsM.rooms[lastRoomNum].transform.Find("Bonfire(Clone)");
        //    if (lastRoomBonfire != null)
        //    {
        //        lastRoomBonfire.transform.GetComponentInChildren<ParticleSystem>().Stop();
        //    }
        //}
        #endregion



        if (other.gameObject.CompareTag("GateForward"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");                                                     // SE�Đ�
            playerPos.x -= 1;                                                                                       // ���ɎQ�Ƃ��镔���̈ʒu���ړ�
            GameObject nextRoom = dungeon.rooms[playerPos.y,playerPos.x];                                               // ���̕������擾
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;                     // �J���������̕����Ɉړ�
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;           // ���C�g�����̕����Ɉړ�
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, -3.6f);      // Player�����̕����Ɉړ�

            BonfireParticleSwitch(nextRoom);
        }

        if (other.gameObject.CompareTag("GateBack"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            playerPos.x += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, 3.6f);

            BonfireParticleSwitch(nextRoom);
        }

        if (other.gameObject.CompareTag("GateLeft"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            playerPos.y -= 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Debug.Log(Camera.main);
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(3.6f, transform.position.y, 0);

            BonfireParticleSwitch(nextRoom);
        }

        if (other.gameObject.CompareTag("GateRight"))
        {
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            playerPos.y += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(-3.6f, transform.position.y, 0);

            BonfireParticleSwitch(nextRoom);
        }

    }

    void BonfireParticleSwitch(GameObject nextRoom)
    {
        //// ���΂̃G�t�F�N�g�̐؂�ւ�
        //Transform bonfirePrefab = nextRoom.transform.Find("Bonfire(Clone)");
        //if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        //{
        //    bonfirePrefab.transform.GetComponentInChildren<ParticleSystem>().Play();
        //}
        //Transform lastRoomBonfire = dungeon.rooms[lastRoomNum].transform.Find("Bonfire(Clone)");
        //if (lastRoomBonfire != null)
        //{
        //    lastRoomBonfire.transform.GetComponentInChildren<ParticleSystem>().Stop();
        //}
    }

    /// <summary>
    /// EnableRoomDoorAccess���\�b�h���g���A�����镔���̔������ׂĊJ���܂��B
    /// </summary>
    public void AccessDoorAfterWin()
    {
        roomsM.EnableRoomDoorAccess(lastRoomNum);
    }
}