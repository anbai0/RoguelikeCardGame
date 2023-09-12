using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    public GameObject[,] rooms = new GameObject[6, 6];
    public Transform roomParent;

    // �X�|�[���n�_
    public int spawnY;
    public int spawnX;
    private const int spawnRandMin = 2;
    private const int spawnRandMax = 3;

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
        rooms[spawnY, spawnX].gameObject.name = $"Room: {walkCount} ({spawnY}  {spawnX})";
        // ���݂̃����_���E�H�[�N�n�_���X�V
        curWalkY = spawnY;
        curWalkX = spawnX;
        // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
        backTracking.Push(new Vector2Int(curWalkX, curWalkY));

        walkCount++;
        RandomWalk();
        CreatePalyer();
    }

    void CreatePalyer()
    {
        // �L�����I���őI�����ꂽ�L�����̃��f���𐶐�
        if (GameManager.Instance.playerData._playerName == "��m")
        {
            // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
            GameObject warrior = Instantiate(warriorPrefab, rooms[curWalkY, curWalkX].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity);
            warrior.transform.SetParent(objectParent.transform);
            warrior.transform.SetParent(null);
        }
        if (GameManager.Instance.playerData._playerName == "���@�g��")
        {
            // �ق��̃V�[����Prefab����������Ă��܂����߁A��xSetParent�Őe���w�肵�āA�e���������Ă��܂��B
            GameObject wizard = Instantiate(wizardPrefab, rooms[curWalkY, curWalkX].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity);
            wizard.transform.SetParent(objectParent.transform);
            wizard.transform.SetParent(null);
        }

        // �J�����̈ʒu��ύX
        cam.transform.position = rooms[curWalkY, curWalkX].transform.position + roomCam;

        // ���C�g�̈ʒu�ύX
        spotLight.transform.position = cam.transform.position + lightPos;
    }

    /// <summary>
    /// rooms�z��������_���E�H�[�N�����ĕ����𐶐����܂��B
    /// </summary>
    void RandomWalk()
    {
        // �����𐶐�����񐔂�ݒ�
        movementLimit = Random.Range(randomWalkMin, randomWalkMax + 1);


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
                    // ��ɐ���
                    curWalkX -= 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++;                   
                    break;

                case 1:
                    // ���ɐ���
                    curWalkX += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++;                   
                    break;

                case 2:
                    // ���ɐ���
                    curWalkY -= 1;        
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++;          
                    break;

                case 3:
                    // �E�ɐ���
                    curWalkY += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkX, curWalkY));
                    walkCount++; 
                    break;

                default:
                    break;
            }
        }

        CreateDoors();
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
                return curWalkX - 1 >= 0 && rooms[curWalkY, curWalkX - 1] == null;
            case 1:
                return curWalkX + 1 < rooms.GetLength(1) && rooms[curWalkY, curWalkX + 1] == null;            
            case 2:
                return curWalkY - 1 >= 0 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                return curWalkY + 1 < rooms.GetLength(0) && rooms[curWalkY + 1, curWalkX] == null;

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


    void CreateDoors()
    {
        int aCount = 0;
        RoomBehaviour roomBehaviour;
        for (int y = 0; y < rooms.GetLength(0); y++)
        {
            //Debug.Log("y  "+ y);
            for (int x = 0; x < rooms.GetLength(1); x++)
            {
                if (rooms[y, x] == null)
                {
                    //Debug.Log($"{y},{x}�̕����́Anull�ł�");
                    continue;
                }
               

                bool[] doorStatus = new bool[4];           

                // �㉺���E�ɕ���������ꍇdoorStatus��true�ɂ��܂��B
                for (int i = 0; i < 4; i++)
                {
                    // 0 - forward, 1 - back, 2 - Left, 3 - Right
                    switch (i)
                    {
                        case 0:
                            if (x - 1 < 0)
                            {
                                //Debug.Log($"{rooms[y, x]}�@�̏�̕����́@�͈͊O�ł�");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}�@�̏�̕����́@{rooms[y, x - 1]}");
                            doorStatus[0] = (rooms[y, x - 1] != null) ? true : false;
                            break;

                        case 1:
                            if (x + 1 >= rooms.GetLength(1))
                            {
                                //Debug.Log($"{rooms[y, x]}�@�̉��̕����́@�͈͊O�ł�");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}�@�̉��̕����́@{rooms[y, x + 1]}");
                            doorStatus[1] = (rooms[y, x + 1] != null) ? true : false;
                            break;

                        case 2:
                            if (y - 1 < 0)
                            {
                                //Debug.Log($"{rooms[y, x]}�@�̍��̕����́@�͈͊O�ł�");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}�@�̍��̕����́@{rooms[y - 1, x]}");
                            doorStatus[2] = (rooms[y - 1, x] != null) ? true : false;
                            break;

                        case 3:
                            if (y + 1 >= rooms.GetLength(0))
                            {
                                //Debug.Log($"{rooms[y, x]}�@�̉E�̕����́@�͈͊O�ł�");
                                break;
                            }

                            //Debug.Log($"{rooms[y, x]}�@�̉E�̕����́@{rooms[y + 1, x]}");
                            doorStatus[3] = (rooms[y + 1, x] != null) ? true : false;
                            break;

                        default:
                            Debug.LogError($"switch���ɓn���l���Ԉ���Ă��܂��B:  {i}");
                            break;
                    }
                    //Debug.Log($"{aCount}��ڂ�i:  {i}");
                    
                }
                aCount++;
                if (rooms[y, x] != null)
                {
                    roomBehaviour = rooms[y, x].GetComponent<RoomBehaviour>();
                    roomBehaviour.UpdateRoom(doorStatus);
                }
                   
            }
        }

    }
}