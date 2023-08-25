using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomStatus
{
    public GameObject roomObject;
    public GameObject stopForward;
    public GameObject stopRight;
    public GameObject stopLeft;
}

public class RoomsManager : MonoBehaviour
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

    [SerializeField] public Camera cam;    // Main.camera���Ɛ������擾�ł��Ȃ��������邽��
    [SerializeField] private GameObject objectParent;
    [SerializeField] private GameObject enemyParent;

    [SerializeField] public GameObject spotLight;          // �������ړ������Ƃ��Ɉꏏ�Ɉړ������܂�
    public Vector3 lightPos = new Vector3(0, -4, 0);       // �J�����̈ʒu�ɉ��Z���Ďg���܂�
    public Vector3 roomCam = new Vector3(0, 10, -10);      // �e�����̈ʒu�ɉ��Z���Ďg���܂��B

    float warriorY = -2.34f;
    float wizardY = -2.34f;

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
        //OpenAllDoors();       // ���ׂẴh�A���J���郁�\�b�h�B�f�o�b�O�Ɏg���܂��B

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

            // �����J����
            EnableRoomDoorAccess((int)RoomNum.Room2);

            // �ׂ̕����ɓG����
            enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room3].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
            enemy.transform.SetParent(enemyParent.transform);

            // �J�����̈ʒu��ύX
            cam.transform.position = rooms[(int)RoomNum.Room2].transform.position + roomCam;

            // ���C�g�̈ʒu�ύX
            spotLight.transform.position = cam.transform.position + lightPos;
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

            // �����J����
            EnableRoomDoorAccess((int)RoomNum.Room3);

            // �ׂ̕����ɓG����
            enemy = Instantiate(smallEnemyPrefab, rooms[(int)RoomNum.Room2].transform.position + new Vector3(0, -0.6f, 0), Quaternion.Euler(0f, 90f, 0f));
            enemy.transform.SetParent(enemyParent.transform);

            // �J�����̈ʒu��ύX
            cam.transform.position = rooms[(int)RoomNum.Room3].transform.position + roomCam;

            // ���C�g�̈ʒu�ύX
            spotLight.transform.position = cam.transform.position + lightPos;
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

            // �Q�[�g���\��
            rooms[(int)RoomNum.Room7].transform.GetChild(2).gameObject.SetActive(false);
            rooms[(int)RoomNum.Room8].transform.GetChild(1).gameObject.SetActive(false);

            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room5].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                     // ���ΐ���

        }
        else
        {
            // �󔠂����̃p�^�[��


            // �� + �Օ�
            treasureBox = Instantiate(treasureBoxPrefab, rooms[(int)RoomNum.Room5].transform.position + new Vector3(0, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));      // �󔠐���

            // �Q�[�g���\��
            rooms[(int)RoomNum.Room5].transform.GetChild(2).gameObject.SetActive(false);
            rooms[(int)RoomNum.Room6].transform.GetChild(1).gameObject.SetActive(false);  


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

            // �Q�[�g���\��
            rooms[(int)RoomNum.Room12].transform.GetChild(5).gameObject.SetActive(false);

            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room9].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                               // ���ΐ���
            enemy = Instantiate(bossEnemyPrefab, rooms[(int)RoomNum.BossRoom1].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));               // ���΂���������Ă��鎟�̕�����Boss�𐶐�

        }
        else
        {
            // �V���b�v�����̃p�^�[��


            // �V���b�v + �Օ�
            shop = Instantiate(shopPrefab, rooms[(int)RoomNum.Room9].transform.position + new Vector3(-0.3f, -2.4f, 0), Quaternion.Euler(0f, 180f, 0f));     // �V���b�v����

            // �Q�[�g���\��
            rooms[(int)RoomNum.Room9].transform.GetChild(5).gameObject.SetActive(false);

            // ����
            bonfire = Instantiate(bonfirePrefab, rooms[(int)RoomNum.Room12].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity);                              // ���ΐ���
            enemy = Instantiate(bossEnemyPrefab, rooms[(int)RoomNum.BossRoom2].transform.position + new Vector3(0, -3.5f, 0), Quaternion.Euler(0f, 90f, 0f));               // ���΂���������Ă��鎟�̕�����Boss�𐶐�

        }
        Debug.Log(rooms[(int)RoomNum.Room12].transform.GetChild(5).gameObject);
        
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
    
    /// <summary>
    /// �w�肵�������̔������ׂĊJ���܂��B
    /// </summary>
    /// <param name="roomNum">�����J�����������̔ԍ�</param>
    public void EnableRoomDoorAccess(int roomNum)
    {
        // 1��gateLeft,2��gateRight,3��Forward
        for(int i = 1; i <= 3; i++)
        {
            // �Q�[�g���A�N�e�B�u�̏ꍇ
            if (rooms[roomNum].transform.GetChild(i).gameObject.activeSelf)
            {
                // �h�A�̌����ڂ��A�N�e�B�u�̏ꍇ(�h�A���܂��Ă���ꍇ)
                if (rooms[roomNum].transform.GetChild(i).GetChild(1).gameObject.activeSelf)
                {
                    // �h�A���\��(�h�A���J����)
                    rooms[roomNum].transform.GetChild(i).GetChild(1).gameObject.SetActive(false);

                    // �h�A�̃R���C�_�[��true��
                    rooms[roomNum].transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;


                    // gateLeft�������ꍇ�A���ׂ̕�����gateRight�̔����J����
                    if(i == 1)
                    {
                        rooms[roomNum - 1].transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                        rooms[roomNum - 1].transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;
                    }
                    // gateRight�������ꍇ�A�E�ׂ̕�����gateLeft�̔����J����
                    if(i == 2)
                    {
                        rooms[roomNum + 1].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                        rooms[roomNum + 1].transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
        }
    }


    /// <summary>
    /// ���ׂẴh�A���J���܂��B
    /// �f�o�b�O�Ɏg���܂��B
    /// </summary>
    public void OpenAllDoors()
    {
        for (int roomNum = 1; roomNum <= rooms.Length - 1; roomNum++)
        {
            for (int i = 1; i <= 3; i++)
            {
                // �h�A���J����
                rooms[roomNum].transform.GetChild(i).GetChild(1).gameObject.SetActive(false);

                // �h�A�̃R���C�_�[��true��
                rooms[roomNum].transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            }
        }
    }
}