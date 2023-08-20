using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomStatus
{
    public GameObject roomObject;
    public GameObject stopForward;
    public GameObject stopRight;
    public GameObject stopLeft;
}

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] public GameObject[] rooms;
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject bonfirePrefab;
    [SerializeField] private GameObject smallEnemyPrefab;
    [SerializeField] private GameObject strongEnemyPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;
    [SerializeField] private GameObject treasureBoxPrefab;
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private RoomStatus[] roomStatuses = new RoomStatus[12];

    [SerializeField] private Camera cam;    // Main.camera���Ɛ������擾�ł��Ȃ��������邽��
    [SerializeField] private GameObject cameraPos2, cameraPos3;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private GameObject enemyParent;

    float warriorY = -2.34f;
    float wizardY = -2.37f;

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
    /// room2�܂���room3�Ńv���C���[�𐶐����A�ׂ̕����ɓG�𐶐����܂��B
    /// </summary>
    private void PlayerSpawn()
    {
        Scene targetScene = SceneManager.GetSceneByName("FieldScene");
        GameObject enemy;
        if (Random.Range(0,2) == 0)
        {
            // �L�����I���őI�����ꂽ�L�����̃��f���𐶐�
            if (GameManager.Instance.playerData._playerName == "��m")
            {
                // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
                GameObject warrior = Instantiate(warriorPrefab, rooms[(int)RoomNum.Room2].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity);
                warrior.transform.SetParent(objectParent.transform);
                warrior.transform.SetParent(null);
            }            
            if (GameManager.Instance.playerData._playerName == "���@�g��")
            {
                // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
                GameObject wizard = Instantiate(wizardPrefab, rooms[(int)RoomNum.Room2].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity);
                wizard.transform.SetParent(objectParent.transform);
                wizard.transform.SetParent(null);
            }

            // �ׂ̕����ɓG����
            enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
            enemy.transform.SetParent(enemyParent.transform);
            // �J�����̈ʒu��ύX
            cam.transform.position = cameraPos2.transform.position;
        }
        else
        {
            // �L�����I���őI�����ꂽ�L�����̃��f���𐶐�
            if (GameManager.Instance.playerData._playerName == "��m")
            {
                // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
                GameObject warrior = Instantiate(warriorPrefab, rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity);
                warrior.transform.SetParent(objectParent.transform);
                warrior.transform.SetParent(null);
            }
            if (GameManager.Instance.playerData._playerName == "���@�g��")
            {
                // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
                GameObject wizard = Instantiate(wizardPrefab, rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity);
                wizard.transform.SetParent(objectParent.transform);
                wizard.transform.SetParent(null);
            }

            // �ׂ̕����ɓG����
            enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room2].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
            enemy.transform.SetParent(enemyParent.transform);
            // �J�����̈ʒu��ύX
            cam.transform.position = cameraPos3.transform.position;
        }
    }

    /// <summary>
    /// room5�܂���room8�ŕ󔠂𐶐������̕����ɂȂ������߂܂��B
    /// �󔠂���������Ȃ����������ɂ͕��΂𐶐����܂��B
    /// </summary>
    void TreasureBoxOrBonfire()
    {
        GameObject treasureBox;
        GameObject bonfire;

        if (Random.Range(0, 2) == 0)
        {
            // �󔠂��E�̃p�^�[��


            // �� + �Օ�
            treasureBox = Instantiate(treasureBoxPrefab, rooms[(int)RoomNum.Room8].transform.position + new Vector3(0, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));      // �󔠐���

            rooms[(int)RoomNum.Room7].transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;                                            // gateRight�̃R���C�_�[�𖳌���
            rooms[(int)RoomNum.Room7].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);                                                 // �h�A��\��

            rooms[(int)RoomNum.Room8].transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;                                            // gateLeft�̃R���C�_�[�𖳌���
            rooms[(int)RoomNum.Room8].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);                                                 // �h�A��\��

            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room5].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                     // ���ΐ���

        }
        else
        {
            // �󔠂����̃p�^�[��


            // �� + �Օ�
            treasureBox = Instantiate(treasureBoxPrefab, rooms[(int)RoomNum.Room5].transform.position + new Vector3(0, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));      // �󔠐���

            rooms[(int)RoomNum.Room5].transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;                                            // gateRight�̃R���C�_�[�𖳌���
            rooms[(int)RoomNum.Room5].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);                                                 // �h�A��\��

            rooms[(int)RoomNum.Room6].transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;                                            // gateLeft�̃R���C�_�[�𖳌���
            rooms[(int)RoomNum.Room6].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);                                                 // �h�A��\��  


            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room8].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                     // ���ΐ���

        }

        treasureBox.transform.SetParent(objectParent.transform);
        bonfire.transform.SetParent(objectParent.transform);
    }

    /// <summary>
    /// room9�܂���room12�ŃV���b�v�𐶐������̕����ɂȂ������߂܂��B
    /// �V���b�v����������Ȃ����������ɂ͕��΂𐶐����܂��B
    /// </summary>
    void ShopOrBonfire()
    {
        GameObject shop;
        GameObject bonfire;
        GameObject enemy;

        if (Random.Range(0, 2) == 0)
        {
            // �V���b�v���E�̃p�^�[�� 


            // �V���b�v + �Օ�
            shop = Instantiate(shopPrefab, rooms[(int)RoomNum.Room12].transform.position + new Vector3(-0.3f, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));    // �V���b�v����

            rooms[(int)RoomNum.BossRoom2].transform.GetChild(4).GetComponent<BoxCollider>().enabled = false;                                    // gateBack�̃R���C�_�[�𖳌���

            rooms[(int)RoomNum.Room12].transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;                                       // gateForward�̃R���C�_�[�𖳌���
            rooms[(int)RoomNum.Room12].transform.GetChild(3).GetChild(1).gameObject.SetActive(true);                                            // �h�A��\��

            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room9].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                               // ���ΐ���
            enemy = Instantiate(bossEnemyPrefab, rooms[(int)RoomNum.BossRoom1].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));               // ���΂���������Ă��鎟�̕�����Boss�𐶐�

        }
        else
        {
            // �V���b�v�����̃p�^�[��


            // �V���b�v + �Օ�
            shop = Instantiate(shopPrefab, rooms[(int)RoomNum.Room9].transform.position + new Vector3(-0.3f, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));     // �V���b�v����

            rooms[(int)RoomNum.BossRoom1].transform.GetChild(4).GetComponent<BoxCollider>().enabled = false;                                    // gateBack�̃R���C�_�[�𖳌���

            rooms[(int)RoomNum.Room9].transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;                                        // gateForward�̃R���C�_�[�𖳌���
            rooms[(int)RoomNum.Room9].transform.GetChild(3).GetChild(1).gameObject.SetActive(true);                                             // �h�A��\��

            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room12].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                              // ���ΐ���
            enemy = Instantiate(bossEnemyPrefab, rooms[(int)RoomNum.BossRoom2].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));               // ���΂���������Ă��鎟�̕�����Boss�𐶐�

        }
        
        shop.transform.SetParent(objectParent.transform);
        bonfire.transform.SetParent(objectParent.transform);
        enemy.transform.SetParent(enemyParent.transform);
    }


    /// <summary>
    /// �����ʒu���Œ肳��Ă���G���G�𐶐����܂��B
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
    /// �����ʒu���Œ肳��Ă��鋭�G�𐶐����܂��B
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