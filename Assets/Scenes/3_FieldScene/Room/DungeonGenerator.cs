using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    // ���Ƃ́A6x6���������A�{�X�����Ȃǂ�[�ɐ����������s����A8x8�ɂ��A���ʂ̕����͐^�񒆂�6x6�}�X�̒��Ő������܂��B
    public GameObject[,] rooms = new GameObject[8, 8];
    public Transform roomParent;

    // �X�|�[���n�_
    public int spawnY;
    public int spawnX;
    private const int spawnRandMin = 3;
    private const int spawnRandMax = 4;

    // �����_���E�H�[�N
    private int curWalkY;
    private int curWalkX;
    private int randomWalkMin = 13;
    private int randomWalkMax = 14;
    private int movementLimit;
    private int walkCount = 0;
    // �o�b�N�g���b�L���O
    private Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    // ���΂̐����ʒu
    private int bonfireRoomY;
    private int bonfireRoomX;

    // �{�X�����̐����ʒu
    private int bossRoomY;
    private int bossRoomX;

    // ��������L������Object
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject bossDoor;
    [SerializeField] private GameObject strongEnemy;
    [SerializeField] private GameObject smallEnemy;
    [SerializeField] private GameObject bonfirePrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject shopPrefab;
    
    // �����������̂��܂Ƃ߂邽�߂�Transform
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform objectParent;
    [SerializeField] private Transform enemyParent;

    // �e�����Ɉړ������Ƃ��Ɉꏏ�Ɉړ����������
    [SerializeField] public Camera cam;
    [SerializeField] public GameObject spotLight;          // �������ړ������Ƃ��Ɉꏏ�Ɉړ������܂�
    public Vector3 lightPos = new Vector3(0, -4, 0);       // �J�����̈ʒu�ɉ��Z���Ďg���܂�
    public Vector3 roomCam = new Vector3(0, 10, -10);      // �e�����̈ʒu�ɉ��Z���Ďg���܂��B

    // �v���C���[�𐶐�����ʒu
    float warriorY = -2.34f;
    float wizardY = -2.34f;

    void Start()
    {
        // �Z���̒���(4�����烉���_����)�X�|�[���n�_��ݒ�
        spawnY = Random.Range(spawnRandMin, spawnRandMax + 1);
        spawnX = Random.Range(spawnRandMin, spawnRandMax + 1);
        rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity, roomParent);
        // �킩��₷���悤�ɉ��Ԗڂɐ���������?�Ɛ��������z��̗v�f���𖼑O�ɓ���Ă��܂��B
        rooms[spawnY, spawnX].gameObject.name = $"Room: {walkCount} ({spawnY}  {spawnX}) (spawnRoom)";
        // ���݂̃����_���E�H�[�N�n�_���X�V
        curWalkY = spawnY;
        curWalkX = spawnX;
        // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
        backTracking.Push(new Vector2Int(curWalkX, curWalkY));
        
        RandomWalk();
        //CreatePlayer();
    }

    private void Update()
    {
        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform[] childTransforms = roomParent.GetComponentsInChildren<Transform>();

            // �e���̂��܂܂�Ă��邽�߁A�C���f�b�N�X1���烋�[�v���J�n
            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }

            walkCount = 0;
            backTracking.Clear();
            // ���������ׂč폜
            for (int y = 0; y < rooms.GetLength(0); y++)
            {
                for (int x = 0; x < rooms.GetLength(1); x++)
                {
                    rooms[x,y] = null;
                }
            }

            // �Z���̒���(4�����烉���_����)�X�|�[���n�_��ݒ�
            spawnY = Random.Range(spawnRandMin, spawnRandMax + 1);
            spawnX = Random.Range(spawnRandMin, spawnRandMax + 1);
            rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity, roomParent);
            // �킩��₷���悤�ɉ��Ԗڂɐ���������?�Ɛ��������z��̗v�f���𖼑O�ɓ���Ă��܂��B
            rooms[spawnY, spawnX].gameObject.name = $"Room: {walkCount} ({spawnY}  {spawnX}) (spawnRoom)";
            // ���݂̃����_���E�H�[�N�n�_���X�V
            curWalkY = spawnY;
            curWalkX = spawnX;
            // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
            backTracking.Push(new Vector2Int(curWalkX, curWalkY));

            RandomWalk();
        }


    }



    /// <summary>
    /// rooms�z��������_���E�H�[�N�����ĕ����𐶐����܂��B
    /// </summary>
    void RandomWalk()
    {
        // �����𐶐�����񐔂�ݒ�
        movementLimit = Random.Range(randomWalkMin, randomWalkMax + 1);

        // �����_���E�H�[�N�ƕ����̐�����movementLimit�񕪍s��
        while (walkCount <= movementLimit)
        {         
            int direction;
            int loopCount = -1;

            // �����_���E�H�[�N
            do
            {
                direction = Random.Range(0, 3 + 1);
                loopCount++;

            } while (!IsDirectionValid(direction) && loopCount <= 10);

            // 10��J��Ԃ��ĕ����𐶐�����ꏊ���Ȃ����Ƃ��m�F������o�b�N�g���b�L���O�ň�߂�
            if (loopCount >= 10)
            {
                direction = -1;     // switch�ɓ����Ăق����Ȃ��̂�-1
                Vector2Int latestCoordinate = backTracking.Pop();               
                curWalkY = latestCoordinate.y;
                curWalkX = latestCoordinate.x;
                Debug.Log($"{latestCoordinate} �ɖ߂�܂����B");
            }

            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            // �ړ�
            switch (direction)
            {
                case 0:
                    // ��Ɉړ�
                    curWalkX -= 1; break;

                case 1:
                    // ���Ɉړ�
                    curWalkX += 1; break;

                case 2:
                    // ���Ɉړ�
                    curWalkY -= 1; break;

                case 3:
                    // �E�Ɉړ�
                    curWalkY += 1; break;

                default:
                    break;
            }

            // �ړ������ꍇ�����ɕ����𐶐�
            if (direction >= 0)
            {
                walkCount++;

                // �����𐶐�
                rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                backTracking.Push(new Vector2Int(curWalkX, curWalkY));
            }

            // �O���ڂɕ��΂𐶐�
            if (walkCount == 3)
            {
                // ���ΐ���
                GameObject bonfire = Instantiate(bonfirePrefab, rooms[curWalkY, curWalkX].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, objectParent);
                bonfire.gameObject.name = $"bonfire1: {walkCount} ({curWalkY}  {curWalkX})";
                // ���΂𐶐������������L�^
                bonfireRoomY = curWalkY;
                bonfireRoomX = curWalkX;
            }
        }

        // �{�X��������
        GenerateBossRoom(rooms[spawnY, spawnX]);
    }

    /// <summary>
    /// �����_���E�H�[�N�ōs�����������ɍs���邩��bool�ŕԂ��܂��B
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    bool IsDirectionValid(int dir)
    {
        // 0 - forward, 1 - back, 2 - Left, 3 - Right
        switch (dir)
        {
            case 0:
                return curWalkX - 1 >= 1 && rooms[curWalkY, curWalkX - 1] == null;
            case 1:
                return curWalkX + 1 < rooms.GetLength(1) - 1 && rooms[curWalkY, curWalkX + 1] == null;            
            case 2:
                return curWalkY - 1 >= 1 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                return curWalkY + 1 < rooms.GetLength(0) - 1 && rooms[curWalkY + 1, curWalkX] == null;

            default:
                Debug.LogError($"switch���ɓn���l���Ԉ���Ă��܂��B:  {dir}");
                return false;
        }
    }



    /// <summary>
    /// �w�肳�ꂽ�Z���̈ʒu�ɉ����ĕ����𐶐�����ʒu��Ԃ��܂��B
    /// </summary>
    /// <param name="roomY"></param>
    /// <param name="roomX"></param>
    /// <returns></returns>
    Vector3 SetRoomPos(int roomY,int roomX)
    {
        // [0,0]������ɗ���悤�ɔz�u�����������̂�"roomX"�����}�C�i�X�ɂ��Ă��܂�
        return new Vector3(roomY * 20f, 0f, -roomX * 20f);
    }

    /// <summary>
    /// GenerateDoor���\�b�h���g���A���ׂĂ̕����̔��𐶐����܂��B
    /// </summary>
    void GenerateDoorsInAllRooms()
    {
        // ������S�T��
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] != null)
                {
                    GenerateDoor(y, x);
                }
            }
        }
    }

    /// <summary>
    /// rooms�̗v�f�����󂯎��A���̕����ɔ��𐶐����܂��B
    /// </summary>
    /// <param name="roomY"></param>
    /// <param name="roomX"></param>
    void GenerateDoor(int roomY, int roomX)
    {
        RoomBehaviour roomBehaviour = rooms[roomY, roomX].GetComponent<RoomBehaviour>();
        bool[] doorStatus = new bool[4];

        // �㉺���E�ɕ���������ꍇdoorStatus��true�ɂ��܂��B
        for (int i = 0; i < 4; i++)
        {
            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            switch (i)
            {
                case 0:
                    if (roomX - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̏�̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̏�̕����́@{rooms[roomY, roomX - 1]}");
                    doorStatus[0] = (rooms[roomY, roomX - 1] != null) ? true : false;
                    break;

                case 1:
                    if (roomX + 1 >= rooms.GetLength(1)) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̉��̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̉��̕����́@{rooms[roomY, roomX + 1]}");
                    doorStatus[1] = (rooms[roomY, roomX + 1] != null) ? true : false;
                    break;

                case 2:
                    if (roomY - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̍��̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̍��̕����́@{rooms[roomY - 1, roomX]}");
                    doorStatus[2] = (rooms[roomY - 1, roomX] != null) ? true : false;
                    break;

                case 3:
                    if (roomY + 1 >= rooms.GetLength(0)) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̉E�̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̉E�̕����́@{rooms[roomY + 1, roomX]}");
                    doorStatus[3] = (rooms[roomY + 1, roomX] != null) ? true : false;
                    break;

                default:
                    Debug.LogError($"switch���ɓn���l���Ԉ���Ă��܂��B:  {i}");
                    break;
            }
        }

        // �h�A���X�V
        Debug.Log(doorStatus);
        roomBehaviour.UpdateRoom(doorStatus);
    }



    /// <summary>
    /// �X�|�[���n�_�ɃL�����𐶐����܂��B
    /// </summary>
    void CreatePlayer()
    {
        // �L�����I���őI�����ꂽ�L�����̃��f���𐶐�
        if (GameManager.Instance.playerData._playerName == "��m")
        {
            // �ق��̃V�[����Prefab����������Ă��܂����߁A�e���w�肵�Ă��܂��B
            GameObject warrior = Instantiate(warriorPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity, playerParent);
        }
        if (GameManager.Instance.playerData._playerName == "���@�g��")
        {
            GameObject wizard = Instantiate(wizardPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity, playerParent);
        }

        // �J�����̈ʒu��ύX
        cam.transform.position = rooms[spawnY, spawnX].transform.position + roomCam;

        // ���C�g�̈ʒu�ύX
        spotLight.transform.position = cam.transform.position + lightPos;
    }

    /// <summary>
    /// �X�|�[���n�_�̕������󂯎��A�X�|�[���n�_�Ɉ�ԉ��������܂��́A���ׂ̗̕����Ƀ{�X�����𐶐����܂��B
    /// </summary>
    /// <param name="spawnRoom"></param>
    void GenerateBossRoom(GameObject spawnRoom)
    {
        Vector3 spawnPos = spawnRoom.transform.position;
        int farthestRoomY = 0;
        int farthestRoomX = 0;
        float maxDistance = 0f;

        // �S�T���ň�ԉ���������������
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] != null)
                {
                    Vector3 roomPos = rooms[y, x].transform.position;
                    float distance = Vector3.Distance(spawnPos, roomPos);

                    // ��ԉ����������X�V
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestRoomY = y;
                        farthestRoomX = x;
                    }
                }
            }
        }

        Debug.Log($"{rooms[farthestRoomY, farthestRoomX].name}�@����ԉ��������ł��B");

        // �{�X�̔��𕔉��̉����ɐ�������s����A�����̉��ɔ�������Ɗ����Ă��܂����߁A�����Ȃ��悤�ɂ���B
        // ��ԉ��������̏�ɕ���������ꍇ
        if (farthestRoomX - 1 >= 0 && rooms[farthestRoomY, farthestRoomX - 1] != null)
        {

            // ���E�ǂ��炩�ɐV���������𐶐�����
            int rand = Random.Range(2, 3 + 1);

            // ��
            if (rand == 2)
            {
                // ����null�̏ꍇ�A���ɕ����𐶐��Bnull����Ȃ��ꍇ���΂ɐ���
                farthestRoomY = rooms[farthestRoomY - 1, farthestRoomX] == null ? -1 : 1;
            }
            // �E
            if (rand == 3)
            {
                // �E��null�̏ꍇ�A�E�ɕ����𐶐��Bnull����Ȃ��ꍇ���΂ɐ���
                farthestRoomY = rooms[farthestRoomY + 1, farthestRoomX] == null ? 1 : -1;
            }

            // �V����������ǉ����A�����̖��O��NewRoom�ɂ���
            rooms[farthestRoomY, farthestRoomX] = Instantiate(room, SetRoomPos(farthestRoomY, farthestRoomX), Quaternion.identity, roomParent);
            rooms[farthestRoomY, farthestRoomX].gameObject.name = $"NewRoom: {walkCount} ({farthestRoomY}  {farthestRoomX}) (bossRoom)";
        }
        else// ��ԉ��������̏�ɕ������Ȃ��ꍇ
        {
            // ��ԉ��������̖��O�����ς���
            rooms[farthestRoomY, farthestRoomX].gameObject.name += " (bossRoom)";
        }

        GameObject bossRoom = rooms[farthestRoomY, farthestRoomX];
        // �{�X�̔��𐶐��B
        Instantiate(bossDoor, bossRoom.transform.position, Quaternion.identity, bossRoom.transform);
        Debug.Log($"{bossRoom}�@�Ƀ{�X�̔������B");

        // ���ΐ���
        GameObject bonfire = Instantiate(bonfirePrefab, rooms[farthestRoomY, farthestRoomX].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, objectParent);
        bonfire.gameObject.name = $"bonfire2: bossRoom ({farthestRoomY}  {farthestRoomX})";

        // �{�X�̔��𐶐������������L�^
        bossRoomY = farthestRoomY;
        bossRoomX = farthestRoomX;

        // ���G����
        GenerateStrongEnemy();
    }

    /// <summary>
    /// �{�X�����̍��E�A���̂����ꂩ�ɋ��G�𐶐����܂��B��̂��������ł��Ȃ������ꍇ���ׂ̗̕����ɂ�����̐������܂��B
    /// </summary>
    void GenerateStrongEnemy()
    {
        int generateCount = 0;
        int rand = Random.Range(1, 3 + 1);

        switch (rand)
        {
            // ��
            case 1:
                if (bossRoomX + 1 < rooms.GetLength(1) && rooms[bossRoomY, bossRoomX + 1])
                {

                }
                break;
            // ��
            case 2:
                if (bossRoomY - 1 >= 0 && rooms[bossRoomY - 1, bossRoomX])
                {

                }
                break;
            // �E
            case 3:
                if (bossRoomY + 1 < rooms.GetLength(1) && rooms[bossRoomY + 1, bossRoomX])
                {

                }
                break;
            default:
                break;
        }





        // �V���b�v�ƕ󔠂ƎG���G�𐶐�
        GenerateItemAndEnemy();
        // �ړ��p�̔����ׂĂ𐶐��B
        GenerateDoorsInAllRooms();
    }

    /// <summary>
    /// �V���b�v�ƕ󔠂ƎG���G�𐶐����܂��B
    /// </summary>
    void GenerateItemAndEnemy()
    {
        // �X�|�[���n�_�Ƃ��̏㉺���E�A���΁A�{�X�����A���G�̕��������O����rooms�̔z��
        GameObject[,] filteredRoomArray = rooms;

        // �X�|�[���n�_�Ƃ��̏㉺���E�̕��������O��
        filteredRoomArray[spawnY, spawnX] = null;
        filteredRoomArray[spawnY, spawnX - 1] = null;
        filteredRoomArray[spawnY, spawnX + 1] = null;
        filteredRoomArray[spawnY - 1, spawnX] = null;
        filteredRoomArray[spawnY + 1, spawnX] = null;

        // ���΂̏ꏊ��null��
        filteredRoomArray[bonfireRoomY, bonfireRoomX] = null;

        // �{�X�����̏ꏊ��null��
        filteredRoomArray[bossRoomY, bonfireRoomX] = null;

        // ���G�̏ꏊ��null��



        List<GameObject> emptyRoomList = new List<GameObject>();

        // �����Ȃ�������List����邽�߂ɑS�T��
        for (int y = 0; y < filteredRoomArray.GetLength(0); y++)
        {
            for (int x = 0; x < filteredRoomArray.GetLength(1); x++)
            {
                if (filteredRoomArray[y, x] != null) emptyRoomList.Add(filteredRoomArray[y, x]);
            }
        }

        // �V���b�v������
        for (int i = 0; i <= 2; i++)
        {
            int lotteryShop = Random.Range(0, emptyRoomList.Count);
            Instantiate(shopPrefab, emptyRoomList[lotteryShop].transform.position, Quaternion.identity, objectParent);
            emptyRoomList.RemoveAt(lotteryShop);
        }

        // �󔠂𐶐�
        int lotteryTreasure = Random.Range(0, emptyRoomList.Count);
        Instantiate(treasurePrefab, emptyRoomList[lotteryTreasure].transform.position, Quaternion.identity, objectParent);
        emptyRoomList.RemoveAt(lotteryTreasure);

        // �G���G�𐶐�
        foreach (GameObject room in emptyRoomList)
        {
            Instantiate(smallEnemy, room.transform.position, Quaternion.identity, enemyParent);
            emptyRoomList.Remove(room);
        }
    }
}