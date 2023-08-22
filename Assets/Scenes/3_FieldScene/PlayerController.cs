using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed; //�v���C���[�̓����X�s�[�h
    public float rotationSpeed = 10f; //������ς��鑬�x

    private Animator animator;

    private float moveHorizontal;
    private float moveVertical;

    public static bool isPlayerActive = true;

    FieldSceneManager fieldManager;
    [SerializeField] RoomsManager roomsManager;
    public GameObject bonfire { get; private set; }
    public GameObject treasureBox { get; private set; }
    public GameObject enemy { get; private set; }
    public string enemyTag { get; private set; }

    public int lastRoomNum;    // �v���C���[���Ō�ɂ��������̔ԍ�

    void Start()
    {
        fieldManager = FindObjectOfType<FieldSceneManager>();
        roomsManager = FindObjectOfType<RoomsManager>();
        animator = GetComponent<Animator>();
        isPlayerActive = true;
    }

    void Update()
    {
        if (isPlayerActive)
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

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }


    private void OnCollisionEnter(Collision collision)
    {
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
            } else
            {
                // �V���b�v�V�[�������[�h����Ă����ꍇ�A�V���b�v�V�[���̃I�u�W�F�N�g��\��
                fieldManager.ActivateShopScene();
            }
        }

        if (collision.gameObject.CompareTag("SmallEnemy") || collision.gameObject.CompareTag("StrongEnemy"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //�퓬�V�[�������[�h
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (roomsManager == null) return;

        // �����镔������肷�鏈��
        // lastRoom�ƍ����镔�����Ⴄ�ꍇ
        if (roomsManager.rooms[lastRoomNum] != other.gameObject)
        {
            for (int roomNum = 1; roomNum <= roomsManager.rooms.Length-1; roomNum++)
            {
                if (roomsManager.rooms[roomNum] == other.gameObject)
                {
                    lastRoomNum = roomNum;         // lastRoom���X�V
                    break;
                }
            }
        }
    }


    /// <summary>
    /// EnableRoomDoorAccess���\�b�h���g���A�����镔���̔������ׂĊJ���܂��B
    /// </summary>
    public void AccessDoorAfterWin()
    {
        roomsManager.EnableRoomDoorAccess(lastRoomNum);
    }
}