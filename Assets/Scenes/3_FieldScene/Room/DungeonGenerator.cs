using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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


    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject bossDoor;

    [SerializeField] public Camera cam;    // Main.camera���Ɛ������擾�ł��Ȃ��������邽��
    [SerializeField] private GameObject objectParent;

    [SerializeField] public GameObject spotLight;          // �������ړ������Ƃ��Ɉꏏ�Ɉړ������܂�
    public Vector3 lightPos = new Vector3(0, -4, 0);       // �J�����̈ʒu�ɉ��Z���Ďg���܂�
    public Vector3 roomCam = new Vector3(0, 10, -10);      // �e�����̈ʒu�ɉ��Z���Ďg���܂��B

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform[] childTransforms = roomParent.GetComponentsInChildren<Transform>();

            // �e���̂��܂܂�Ă��邽�߁A�C���f�b�N�X1���烋�[�v���J�n
            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }

            backTracking.Clear();
            walkCount = 0;
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
            walkCount++;
            // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
            backTracking.Push(new Vector2Int(curWalkX, curWalkY));

            RandomWalk();
        }


    }

    /// <summary>
    /// �X�|�[���n�_�ɃL�����𐶐����܂��B
    /// </summary>
    void CreatePlayer()
    {
        // �L�����I���őI�����ꂽ�L�����̃��f���𐶐�
        if (GameManager.Instance.playerData._playerName == "��m")
        {
            // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
            GameObject warrior = Instantiate(warriorPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity);
            warrior.transform.SetParent(objectParent.transform);
            warrior.transform.SetParent(null);
        }
        if (GameManager.Instance.playerData._playerName == "���@�g��")
        {
            // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
            GameObject wizard = Instantiate(wizardPrefab, rooms[spawnY, spawnX].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity);
            wizard.transform.SetParent(objectParent.transform);
            wizard.transform.SetParent(null);
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

        // �{�X�̔��𕔉��̉����ɐ�������s����A�����̉��ɔ�������Ɗӏ܂��Ă��܂����߁A�����Ȃ��悤�ɂ���B
        // ��ԉ��������̏�ɕ���������ꍇ
        if (farthestRoomX - 1 >= 0 && rooms[farthestRoomY, farthestRoomX - 1] != null)
        {

            // ���E�ǂ��炩�ɐV���������𐶐�����
            int rand = Random.Range(2, 3 + 1);

            // ��
            if (rand == 2)
            {
                // ����null�̏ꍇ
                if (rooms[farthestRoomY - 1, farthestRoomX] == null)
                {
                    // ���ɐV���������𐶐�
                    farthestRoomY -= 1;
                }
                else
                {
                    // �E�ɐV���������𐶐�
                    farthestRoomY += 1;
                }
            }
            // �E
            if (rand == 3)
            {
                // �E��null�̏ꍇ
                if (rooms[farthestRoomY + 1, farthestRoomX] == null)
                {
                    farthestRoomY += 1;
                }
                else
                {
                    farthestRoomY -= 1;
                }
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

        // �ړ��p�̔����ׂĂ𐶐��B
        GenerateDoorsInAllRooms();
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
                //Debug.Log(direction);

            } while (!IsDirectionValid(direction) && loopCount <= 10);

            // 10��J��Ԃ��ĕ����𐶐�����ꏊ���Ȃ����Ƃ��m�F������o�b�N�g���b�L���O�ň�߂�
            if (loopCount >= 10)
            {
                direction = -1;     // switch�ɓ����Ăق����Ȃ��̂�-1
                Vector2Int latestCoordinate = backTracking.Pop();
                Debug.Log($"{latestCoordinate} �ɖ߂�܂����B");
                curWalkY = latestCoordinate.y;
                curWalkX = latestCoordinate.x;        
            }

            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            switch (direction)
            {
                case 0:
                    // ��Ɉړ�
                    curWalkX -= 1;
                    walkCount++;

                    // �����𐶐�
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));                  
                    break;

                case 1:
                    // ���Ɉړ�
                    curWalkX += 1;
                    walkCount++;

                    // �����𐶐�
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));                                      
                    break;

                case 2:
                    // ���Ɉړ�
                    curWalkY -= 1;
                    walkCount++;

                    // �����𐶐�
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    break;

                case 3:
                    // �E�Ɉړ�
                    curWalkY += 1;
                    walkCount++;

                    // �����𐶐�
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    break;

                default:
                    break;
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
    /// <param name="vertical"></param>
    /// <param name="horizontal"></param>
    /// <returns></returns>
    Vector3 SetRoomPos(int vertical,int horizontal)
    {
        // [0,0]������ɗ���悤�ɔz�u�����������̂�"horizontal"�����}�C�i�X�ɂ��Ă��܂�
        return new Vector3(vertical * 20f, 0f, -horizontal * 20f);
    }


    /// <summary>
    /// GenerateDoor���\�b�h���g���A
    /// ���ׂĂ̕����̔��𐶐����܂��B
    /// </summary>
    void GenerateDoorsInAllRooms()
    {
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] != null)
                {
                    GenerateDoor(y, x);
                }
                else
                {
                    //Debug.Log($"{y},{x}�̕����́Anull�ł�");
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
}