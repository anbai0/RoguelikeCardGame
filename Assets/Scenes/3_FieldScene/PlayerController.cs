using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed; //プレイヤーの動くスピード
    public float rotationSpeed = 10f; //向きを変える速度

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

    public int lastRoomNum;    // プレイヤーが最後にいた部屋の番号

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

            animator.SetBool("IsWalking", true); // 歩くアニメーションを再生
        }
        else
        {
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            // playerを動けなくする処理
            isPlayerActive = false;
            animator.SetBool("IsWalking", false);

            bonfire = collision.gameObject;
            fieldManager.LoadBonfireScene();        // 焚火シーンをロード
        }

        if (collision.gameObject.CompareTag("TreasureBox"))
        {
            // playerを動けなくする処理
            isPlayerActive = false;
            animator.SetBool("IsWalking", false);

            treasureBox = collision.gameObject;
            fieldManager.LoadTreasureBoxScene();   //宝箱シーンをロード
        }

        if (collision.gameObject.CompareTag("Shop"))
        {
            // playerを動けなくする処理
            isPlayerActive = false;
            animator.SetBool("IsWalking", false);

            // 指定した名前のシーンを取得
            Scene sceneToHide = SceneManager.GetSceneByName("ShopScene");

            // ロードされていない場合に処理を実行
            if (!sceneToHide.isLoaded)
            {
                fieldManager.LoadShopScene();           // ショップシーンをロード
            } else
            {
                // ショップシーンがロードされていた場合、ショップシーンのオブジェクトを表示
                fieldManager.ActivateShopScene();
            }
        }

        if (collision.gameObject.CompareTag("SmallEnemy") || collision.gameObject.CompareTag("StrongEnemy"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //戦闘シーンをロード
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (roomsManager == null) return;

        // 今いる部屋を特定する処理
        // lastRoomと今いる部屋が違う場合
        if (roomsManager.rooms[lastRoomNum] != other.gameObject)
        {
            for (int roomNum = 1; roomNum <= roomsManager.rooms.Length-1; roomNum++)
            {
                if (roomsManager.rooms[roomNum] == other.gameObject)
                {
                    lastRoomNum = roomNum;         // lastRoomを更新
                    break;
                }
            }
        }
    }


    /// <summary>
    /// EnableRoomDoorAccessメソッドを使い、今いる部屋の扉をすべて開けます。
    /// </summary>
    public void AccessDoorAfterWin()
    {
        roomsManager.EnableRoomDoorAccess(lastRoomNum);
    }
}