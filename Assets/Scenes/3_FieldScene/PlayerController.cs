using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

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
    public GameObject bonfire { get; private set; }
    public GameObject treasureBox { get; private set; }
    public GameObject enemy { get; private set; }
    public string enemyTag { get; private set; }

    // �����̈ړ�
    [SerializeField] DungeonGenerator dungeon;
    Vector2Int playerPos;
    // �}�b�v�̕`��
    List<Vector2Int> roomVisited = new List<Vector2Int>();  // ��x�K�ꂽ�����̈ʒu���L�^���܂�
    // �}�b�v�A�C�R��
    [SerializeField] GameObject bonfireIcon;
    [SerializeField] GameObject shopIcon;
    [SerializeField] GameObject treasureBoxIcon;
    [SerializeField] GameObject bossIcon;

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
        else
        {
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~
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
        #region �����ړ��̏���
        if (other.gameObject.CompareTag("GateForward"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];                                                 // �Ō�ɂ��������X�V
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");                                                     // SE�Đ�
            playerPos.y -= 1;                                                                                   // ���ɎQ�Ƃ��镔���̈ʒu���ړ�
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];                                      // ���̕������擾
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;                     // �J���������̕����Ɉړ�
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;           // ���C�g�����̕����Ɉړ�
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, -3.6f);      // Player�����̕����Ɉړ�

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // �ړ������������}�b�v�̒��S�ɕύX
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }

        if (other.gameObject.CompareTag("GateBack"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            playerPos.y += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, 3.6f);

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // �ړ������������}�b�v�̒��S�ɕύX
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }

        if (other.gameObject.CompareTag("GateLeft"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            playerPos.x -= 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Debug.Log(Camera.main);
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(3.6f, transform.position.y, 0);

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // �ړ������������}�b�v�̒��S�ɕύX
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }

        if (other.gameObject.CompareTag("GateRight"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];
            AudioManager.Instance.PlaySE("�}�b�v�؂�ւ�");
            playerPos.x += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(-3.6f, transform.position.y, 0);

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // �ړ������������}�b�v�̒��S�ɕύX
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }
        # endregion
    }


    /// <summary>
    /// �������ړ������Ƃ��ɕ��΂̃G�t�F�N�g�̐؂�ւ����s���܂�
    /// </summary>
    /// <param name="nextRoom"></param>
    /// <param name="lastRoom"></param>
    void BonfireParticleSwitch(GameObject nextRoom, GameObject lastRoom)
    {
        // �����������ɕ��΂��������ꍇ�G�t�F�N�g��t����B
        Transform bonfirePrefab = null;
        foreach (Transform child in nextRoom.transform)
        {
            if (child.CompareTag("Bonfire"))
            {
                bonfirePrefab = child;
                break; // �^�O�����������烋�[�v���I��
            }
        }
        if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        {
            bonfirePrefab.GetComponentInChildren<ParticleSystem>().Play();
        }

        // �O�̕����ɕ��΂��������ꍇ�G�t�F�N�g�������B
        Transform lastRoomBonfire = null;
        foreach (Transform child in lastRoom.transform)
        {
            if (child.CompareTag("Bonfire"))
            {
                lastRoomBonfire = child;
                break; // �^�O�����������烋�[�v���I��
            }
        }
        if (lastRoomBonfire != null && lastRoomBonfire.GetComponent<BoxCollider>().enabled)
        {
            lastRoomBonfire.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    /// <summary>
    /// ���������������߂ĖK�ꂽ�����������ꍇ�A�}�b�v�ɕ�����`�悵�܂�
    /// </summary>
    /// <param name="nextRoom"></param>
    private void GenerateMap(GameObject nextRoom)
    {
        bool isVisited = false;

        // ��x�K�ꂽ�����Ȃ̂����`�F�b�N
        foreach (var room in roomVisited)
        {
            // �K��Ă�����
            if (room == playerPos)
            {
                isVisited = true;
                break;
            }
        }

        // �K��Ă��Ȃ�������}�b�v��`��
        if (!isVisited)
        {
            // ������`��
            dungeon.maps[playerPos.y, playerPos.x].gameObject.SetActive(true);

            // �����ɓ���̃I�u�W�F�N�g���������ꍇ�A�C�R����\��
            if (dungeon.rooms[playerPos.y, playerPos.x].transform.childCount >= 6)
            {
                if (dungeon.rooms[playerPos.y, playerPos.x].transform.GetChild(5).CompareTag("Bonfire"))
                {
                    GameObject bonfire = Instantiate(bonfireIcon, Vector3.zero, Quaternion.identity, dungeon.maps[playerPos.y, playerPos.x].transform);
                    bonfire.transform.localPosition = new Vector3(100, -100, 0);
                }

                if (dungeon.rooms[playerPos.y, playerPos.x].transform.GetChild(5).CompareTag("Shop"))
                {
                    GameObject shop = Instantiate(shopIcon, Vector3.zero, Quaternion.identity, dungeon.maps[playerPos.y, playerPos.x].transform);
                    shop.transform.localPosition = new Vector3(100, -100, 0);
                }

                if (dungeon.rooms[playerPos.y, playerPos.x].transform.GetChild(5).CompareTag("TreasureBox"))
                {
                    GameObject box = Instantiate(treasureBoxIcon, Vector3.zero, Quaternion.identity, dungeon.maps[playerPos.y, playerPos.x].transform);
                    box.transform.localPosition = new Vector3(100, -100, 0);
                }

                if (dungeon.rooms[playerPos.y, playerPos.x].transform.GetChild(5).CompareTag("Boss"))
                {
                    GameObject boss = Instantiate(bossIcon, Vector3.zero, Quaternion.identity, dungeon.maps[playerPos.y, playerPos.x].transform);
                    boss.transform.localPosition = new Vector3(100, -100, 0);
                }
            }
        }  
    }
}