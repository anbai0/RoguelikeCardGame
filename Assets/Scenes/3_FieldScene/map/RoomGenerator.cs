using UnityEngine;

public class RoomStatus
{
    public GameObject roomObject;
    public GameObject stopForward;
    public GameObject stopRight;
    public GameObject stopLeft;
}

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] rooms;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject bonfirePrefab;
    [SerializeField]
    private GameObject smallEnemyPrefab;
    [SerializeField]
    private GameObject strongEnemyPrefab;
    [SerializeField]
    private GameObject bossEnemyPrefab;
    [SerializeField]
    private GameObject treasureBoxPrefab;
    [SerializeField]
    private GameObject shopPrefab;
    [SerializeField]
    private RoomStatus[] roomStatuses = new RoomStatus[12];

    [SerializeField] private Camera cam;    // Main.cameraだと正しく取得できない時があるため
    [SerializeField] private GameObject cameraPos2, cameraPos3;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private GameObject enemyParent;

    float playerY = -2.33f;

    enum RoomNum
    {
        Room1 = 1,
        Room2,
        Room3,
        Room4,
        Room5,
        Room6,
        Room7,
        Room8,
        Room9,
        Room10,
        Room11,
        Room12,
        BossRoom1,
        BossRoom2,
    };


    void Start()
    {
        TreasureBoxOrBonfire();
        ShopOrBonfire();
        SmallEnemySpawn();
        StrongEnemySpawn();
        PlayerSpawn();

    }

    /// <summary>
    /// room2またはroom3でプレイヤーを生成し、隣の部屋に敵を生成します。
    /// </summary>
    private void PlayerSpawn()
    {
        GameObject enemy;
        if (Random.Range(0,2) == 0)
        {
            playerPrefab.transform.position = rooms[(int)RoomNum.Room2].transform.position + new Vector3 (0, playerY, 0);
            enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
            enemy.transform.SetParent(enemyParent.transform);
            cam.transform.position = cameraPos2.transform.position;
        }
        else
        {
            playerPrefab.transform.position = rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, playerY, 0);
            enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room2].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
            enemy.transform.SetParent(enemyParent.transform);
            cam.transform.position = cameraPos3.transform.position;
        }
    }

    /// <summary>
    /// room5またはroom8で宝箱を生成し次の部屋につながる扉を閉めます。
    /// 宝箱が生成されなかった部屋には焚火を生成します。
    /// </summary>
    void TreasureBoxOrBonfire()
    {
        GameObject treasureBox;
        GameObject bonfire;

        if (Random.Range(0, 2) == 0)
        {
            // 宝箱が右のパターン


            // 宝箱 + 遮蔽
            treasureBox = Instantiate(treasureBoxPrefab, rooms[(int)RoomNum.Room8].transform.position + new Vector3(0, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));      // 宝箱生成

            rooms[(int)RoomNum.Room7].transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;                                            // gateRightのコライダーを無効化
            rooms[(int)RoomNum.Room7].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);                                                 // ドアを表示

            rooms[(int)RoomNum.Room8].transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;                                            // gateLeftのコライダーを無効化
            rooms[(int)RoomNum.Room8].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);                                                 // ドアを表示

            // 焚火
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room5].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                     // 焚火生成

        }
        else
        {
            // 宝箱が左のパターン


            // 宝箱 + 遮蔽
            treasureBox = Instantiate(treasureBoxPrefab, rooms[(int)RoomNum.Room5].transform.position + new Vector3(0, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));      // 宝箱生成

            rooms[(int)RoomNum.Room5].transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;                                            // gateRightのコライダーを無効化
            rooms[(int)RoomNum.Room5].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);                                                 // ドアを表示

            rooms[(int)RoomNum.Room6].transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;                                            // gateLeftのコライダーを無効化
            rooms[(int)RoomNum.Room6].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);                                                 // ドアを表示  


            // 焚火
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room8].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                     // 焚火生成

        }

        treasureBox.transform.SetParent(objectParent.transform);
        bonfire.transform.SetParent(objectParent.transform);
    }

    /// <summary>
    /// room9またはroom12でショップを生成し次の部屋につながる扉を閉めます。
    /// ショップが生成されなかった部屋には焚火を生成します。
    /// </summary>
    void ShopOrBonfire()
    {
        GameObject shop;
        GameObject bonfire;
        GameObject enemy;

        if (Random.Range(0, 2) == 0)
        {
            // ショップが右のパターン 


            // ショップ + 遮蔽
            shop = Instantiate(shopPrefab, rooms[(int)RoomNum.Room12].transform.position + new Vector3(-0.3f, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));    // ショップ生成

            rooms[(int)RoomNum.BossRoom2].transform.GetChild(4).GetComponent<BoxCollider>().enabled = false;                                    // gateBackのコライダーを無効化

            rooms[(int)RoomNum.Room12].transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;                                       // gateForwardのコライダーを無効化
            rooms[(int)RoomNum.Room12].transform.GetChild(3).GetChild(1).gameObject.SetActive(true);                                            // ドアを表示

            // 焚火
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room9].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                               // 焚火生成
            enemy = Instantiate(bossEnemyPrefab, rooms[(int)RoomNum.BossRoom1].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));               // 焚火が生成されている次の部屋にBossを生成

        }
        else
        {
            // ショップが左のパターン


            // ショップ + 遮蔽
            shop = Instantiate(shopPrefab, rooms[(int)RoomNum.Room9].transform.position + new Vector3(-0.3f, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));     // ショップ生成

            rooms[(int)RoomNum.BossRoom1].transform.GetChild(4).GetComponent<BoxCollider>().enabled = false;                                    // gateBackのコライダーを無効化

            rooms[(int)RoomNum.Room9].transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;                                        // gateForwardのコライダーを無効化
            rooms[(int)RoomNum.Room9].transform.GetChild(3).GetChild(1).gameObject.SetActive(true);                                             // ドアを表示

            // 焚火
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room12].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                              // 焚火生成
            enemy = Instantiate(bossEnemyPrefab, rooms[(int)RoomNum.BossRoom2].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));               // 焚火が生成されている次の部屋にBossを生成

        }
        
        shop.transform.SetParent(objectParent.transform);
        bonfire.transform.SetParent(objectParent.transform);
        enemy.transform.SetParent(enemyParent.transform);
    }


    /// <summary>
    /// 生成位置が固定されている雑魚敵を生成します。
    /// </summary>
    private void SmallEnemySpawn()
    {
        GameObject enemy;
        enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room1].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
        enemy.transform.SetParent(enemyParent.transform);
        enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room4].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
        enemy.transform.SetParent(enemyParent.transform);
        enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room6].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
        enemy.transform.SetParent(enemyParent.transform);
        enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room7].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
        enemy.transform.SetParent(enemyParent.transform);
    }

    /// <summary>
    /// 生成位置が固定されている強敵を生成します。
    /// </summary>
    private void StrongEnemySpawn()
    {
        GameObject enemy;
        enemy = Instantiate(strongEnemyPrefab, rooms[(int)RoomNum.Room10].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));
        enemy.transform.SetParent(enemyParent.transform);
        enemy = Instantiate(strongEnemyPrefab, rooms[(int)RoomNum.Room11].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));
        enemy.transform.SetParent(enemyParent.transform);
    }
}