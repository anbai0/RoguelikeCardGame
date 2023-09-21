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

    public float moveSpeed;                     //プレイヤーの動くスピード
    public float rotationSpeed = 10f;       //向きを変える速度
    private float moveHorizontal;
    private float moveVertical;

    private Animator animator;

    // 部屋関係
    private FieldSceneManager fieldManager;
    public GameObject bonfire { get; private set; }
    public GameObject treasureBox { get; private set; }
    public GameObject enemy { get; private set; }
    public string enemyTag { get; private set; }

    // 部屋の移動
    [SerializeField] DungeonGenerator dungeon;
    Vector2Int playerPos;
    // マップの描画
    List<Vector2Int> roomVisited = new List<Vector2Int>();  // 一度訪れた部屋の位置を記録します
    // マップアイコン
    [SerializeField] GameObject bonfireIcon;
    [SerializeField] GameObject shopIcon;
    [SerializeField] GameObject treasureBoxIcon;
    [SerializeField] GameObject bossIcon;

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
        else
        {
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止
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
        #region 部屋移動の処理
        if (other.gameObject.CompareTag("GateForward"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];                                                 // 最後にいた部屋更新
            AudioManager.Instance.PlaySE("マップ切り替え");                                                     // SE再生
            playerPos.y -= 1;                                                                                   // 次に参照する部屋の位置を移動
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];                                      // 次の部屋を取得
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;                     // カメラを次の部屋に移動
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;           // ライトを次の部屋に移動
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, -3.6f);      // Playerを次の部屋に移動

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // 移動した部屋をマップの中心に変更
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }

        if (other.gameObject.CompareTag("GateBack"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];
            AudioManager.Instance.PlaySE("マップ切り替え");
            playerPos.y += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(0, transform.position.y, 3.6f);

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // 移動した部屋をマップの中心に変更
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }

        if (other.gameObject.CompareTag("GateLeft"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];
            AudioManager.Instance.PlaySE("マップ切り替え");
            playerPos.x -= 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Debug.Log(Camera.main);
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(3.6f, transform.position.y, 0);

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // 移動した部屋をマップの中心に変更
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }

        if (other.gameObject.CompareTag("GateRight"))
        {
            GameObject lastRoom = dungeon.rooms[playerPos.y, playerPos.x];
            AudioManager.Instance.PlaySE("マップ切り替え");
            playerPos.x += 1;
            GameObject nextRoom = dungeon.rooms[playerPos.y, playerPos.x];
            Camera.main.transform.position = nextRoom.transform.position + dungeon.roomCam;
            dungeon.spotLight.transform.position = Camera.main.transform.position + dungeon.lightPos;
            gameObject.transform.position = nextRoom.transform.position + new Vector3(-3.6f, transform.position.y, 0);

            BonfireParticleSwitch(nextRoom, lastRoom);
            GenerateMap(nextRoom);
            // 移動した部屋をマップの中心に変更
            dungeon.map.transform.localPosition = new Vector3(-playerPos.x * 100 - 50, playerPos.y * 100 + 50, 0);
        }
        # endregion
    }


    /// <summary>
    /// 部屋を移動したときに焚火のエフェクトの切り替えを行います
    /// </summary>
    /// <param name="nextRoom"></param>
    /// <param name="lastRoom"></param>
    void BonfireParticleSwitch(GameObject nextRoom, GameObject lastRoom)
    {
        // 入った部屋に焚火があった場合エフェクトを付ける。
        Transform bonfirePrefab = null;
        foreach (Transform child in nextRoom.transform)
        {
            if (child.CompareTag("Bonfire"))
            {
                bonfirePrefab = child;
                break; // タグが見つかったらループを終了
            }
        }
        if (bonfirePrefab != null && bonfirePrefab.GetComponent<BoxCollider>().enabled)
        {
            bonfirePrefab.GetComponentInChildren<ParticleSystem>().Play();
        }

        // 前の部屋に焚火があった場合エフェクトを消す。
        Transform lastRoomBonfire = null;
        foreach (Transform child in lastRoom.transform)
        {
            if (child.CompareTag("Bonfire"))
            {
                lastRoomBonfire = child;
                break; // タグが見つかったらループを終了
            }
        }
        if (lastRoomBonfire != null && lastRoomBonfire.GetComponent<BoxCollider>().enabled)
        {
            lastRoomBonfire.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    /// <summary>
    /// 入った部屋が初めて訪れた部屋だった場合、マップに部屋を描画します
    /// </summary>
    /// <param name="nextRoom"></param>
    private void GenerateMap(GameObject nextRoom)
    {
        bool isVisited = false;

        // 一度訪れた部屋なのかをチェック
        foreach (var room in roomVisited)
        {
            // 訪れていたら
            if (room == playerPos)
            {
                isVisited = true;
                break;
            }
        }

        // 訪れていなかったらマップを描画
        if (!isVisited)
        {
            // 部屋を描画
            dungeon.maps[playerPos.y, playerPos.x].gameObject.SetActive(true);

            // 部屋に特定のオブジェクトがあった場合アイコンを表示
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