using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool isEvents = true;
    public bool isSetting = false;
    public bool isConfimDeck = false;

    public float moveSpeed;                     //プレイヤーの動くスピード
    public float rotationSpeed = 10f;       //向きを変える速度
    private float moveHorizontal;
    private float moveVertical;

    private Animator animator;

    // 部屋関係
    private FieldSceneManager fieldManager;
    private RoomsManager roomsM;
    public GameObject bonfire { get; private set; }
    public GameObject treasureBox { get; private set; }
    public GameObject enemy { get; private set; }
    public string enemyTag { get; private set; }
    // 部屋の移動
    public int lastRoomNum;     // プレイヤーが最後にいた部屋の番号

    [SerializeField] DungeonGenerator dungeon;
    Vector2Int playerPos;


    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        // シングルトン
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
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeInBGMVolume());       // BGMを再生
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

            animator.SetBool("IsWalking", true); // 歩くアニメーションを再生
        }
        else
        {
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止
        }

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        #region シーン遷移
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGMを一時停止

            isEvents = true;
            animator.SetBool("IsWalking", false);

            bonfire = collision.gameObject;
            fieldManager.LoadBonfireScene();        // 焚火シーンをロード  
        }

        if (collision.gameObject.CompareTag("TreasureBox"))
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGMを一時停止

            // playerを動けなくする処理
            isEvents = true;
            animator.SetBool("IsWalking", false);

            treasureBox = collision.gameObject;
            fieldManager.LoadTreasureBoxScene();   //宝箱シーンをロード
        }

        if (collision.gameObject.CompareTag("Shop"))
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGMを一時停止

            // playerを動けなくする処理
            isEvents = true;
            animator.SetBool("IsWalking", false);

            // 指定した名前のシーンを取得
            Scene sceneToHide = SceneManager.GetSceneByName("ShopScene");

            // ロードされていない場合に処理を実行
            if (!sceneToHide.isLoaded)
            {
                fieldManager.LoadShopScene();           // ショップシーンをロード
            }
            else
            {
                // ショップシーンがロードされていた場合、ショップシーンのオブジェクトを表示
                fieldManager.ActivateShopScene();
            }
        }

        if (collision.gameObject.CompareTag("SmallEnemy") || collision.gameObject.CompareTag("StrongEnemy") || collision.gameObject.CompareTag("Boss"))
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());       // BGMを一時停止

            // playerを動けなくする処理
            isEvents = true;
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //戦闘シーンをロード
            Invoke("AccessDoorAfterWin", 2.0f); //ロードする時間を考慮して2秒待ってから処理を呼び出す
        }
        #endregion
    }


    private void OnTriggerStay(Collider other)
    {
        //if (roomsM == null) return;

        #region 今いる部屋を特定する処理
        // lastRoomと今いる部屋が違う場合
        //if (roomsM.rooms[lastRoomNum] != other.gameObject)
        //{
        //    for (int roomNum = 1; roomNum <= roomsM.rooms.Length - 1; roomNum++)
        //    {
        //        if (roomsM.rooms[roomNum] == other.gameObject)
        //        {
        //            lastRoomNum = roomNum;         // lastRoomを更新
        //            break;
        //        }
        //    }
        //}
        #endregion

        #region 部屋移動処理
        //if (other.gameObject.CompareTag("GateForward"))
        //{
        //    AudioManager.Instance.PlaySE("マップ切り替え");                                                   // SE再生
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum + 4];                                              // 次の部屋を取得
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;                    // カメラを次の部屋に移動
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;           // ライトを次の部屋に移動
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, -3.6f);      // Playerを次の部屋に移動

        //    // 焚火のエフェクトの切り替え
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
        //    AudioManager.Instance.PlaySE("マップ切り替え");
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum + 1];
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(-3.6f, transform.position.y, 0);

        //    // 焚火のエフェクトの切り替え
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
        //    AudioManager.Instance.PlaySE("マップ切り替え");
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum - 1];
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(3.6f, transform.position.y, 0);

        //    // 焚火のエフェクトの切り替え
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
        //    AudioManager.Instance.PlaySE("マップ切り替え");
        //    GameObject nextRoom = roomsM.rooms[lastRoomNum - 4];
        //    Camera.main.transform.position = nextRoom.transform.position + roomsM.roomCam;
        //    roomsM.spotLight.transform.position = Camera.main.transform.position + roomsM.lightPos;
        //    gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, 3.6f);

        //    // 焚火のエフェクトの切り替え
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
            AudioManager.Instance.PlaySE("マップ切り替え");                                                     // SE再生
            playerPos.x -= 1;                                                                                       // 次に参照する部屋の位置を移動
            GameObject nextRoom = dungeon.rooms[playerPos.y,playerPos.x];                                               // 次の部屋を取得
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;                     // カメラを次の部屋に移動
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;           // ライトを次の部屋に移動
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, -3.6f);      // Playerを次の部屋に移動

            BonfireParticleSwitch(nextRoom);
        }

        if (other.gameObject.CompareTag("GateBack"))
        {
            AudioManager.Instance.PlaySE("マップ切り替え");
            playerPos.x += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, 3.6f);

            BonfireParticleSwitch(nextRoom);
        }

        if (other.gameObject.CompareTag("GateLeft"))
        {
            AudioManager.Instance.PlaySE("マップ切り替え");
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
            AudioManager.Instance.PlaySE("マップ切り替え");
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
        //// 焚火のエフェクトの切り替え
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
    /// EnableRoomDoorAccessメソッドを使い、今いる部屋の扉をすべて開けます。
    /// </summary>
    public void AccessDoorAfterWin()
    {
        roomsM.EnableRoomDoorAccess(lastRoomNum);
    }
}