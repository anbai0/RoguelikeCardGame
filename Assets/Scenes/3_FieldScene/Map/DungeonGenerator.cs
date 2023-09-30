using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    // ���Ƃ́A6x6���������A�{�X�����Ȃǂ�[�ɐ����������s����A8x8�ɂ��A���ʂ̕����͐^�񒆂�6x6�}�X�̒��Ő������܂��B
    public GameObject[,] rooms = new GameObject[8, 8];
    public Transform roomParent;

    // �X�|�[���n�_
    public Vector2Int spawnPos;
    private const int spawnRandMin = 3;
    private const int spawnRandMax = 4;

    // �����_���E�H�[�N
    private Vector2Int curWalk;
    private int randomWalkMin = 12;
    private int randomWalkMax = 13;
    private int movementLimit;
    private int walkCount = 0;
    // �o�b�N�g���b�L���O
    private Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    // ���΂̐����ʒu
    public Vector2Int bonfireRoomPos;

    // �{�X�����̐����ʒu
    private Vector2Int bossRoomPos;

    // ���G�̐����ʒu
    private List<Vector2Int> strongEnemyRooms = new List<Vector2Int>();

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
    [SerializeField] private Transform enemyParent;

    // �e�����Ɉړ������Ƃ��Ɉꏏ�Ɉړ����������
    [SerializeField] public Camera cam;
    [SerializeField] public GameObject spotLight;          // �������ړ������Ƃ��Ɉꏏ�Ɉړ������܂�
    public Vector3 lightPos = new Vector3(0, -4, 0);       // �J�����̈ʒu�ɉ��Z���Ďg���܂�
    public Vector3 roomCam = new Vector3(0, 10, -10);      // �e�����̈ʒu�ɉ��Z���Ďg���܂��B

    // �v���C���[�𐶐�����ʒu
    float warriorY = -2.34f;
    float wizardY = -2.34f;
    
    // �}�b�v����
    [SerializeField] public Transform map;
    [SerializeField] GameObject mapPrefab;
    public GameObject[,] maps = new GameObject[8, 8];

    // �}�b�v�A�C�R��
    [SerializeField] GameObject warriorIcon;
    [SerializeField] GameObject wizardIcon;
    [SerializeField] GameObject spawnIcon;
    public GameObject playerIcon;

    void Start()
    {
        // �Z���̒���(4�����烉���_����)�X�|�[���n�_��ݒ�
        spawnPos.y = Random.Range(spawnRandMin, spawnRandMax + 1);
        spawnPos.x = Random.Range(spawnRandMin, spawnRandMax + 1);
        rooms[spawnPos.y, spawnPos.x] = Instantiate(room, SetRoomPos(spawnPos.y, spawnPos.x), Quaternion.identity, roomParent);
        // �킩��₷���悤�ɉ��Ԗڂɐ���������?�Ɛ��������z��̗v�f���𖼑O�ɓ���Ă��܂��B
        rooms[spawnPos.y, spawnPos.x].gameObject.name = $"Room: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        // �����̃G�t�F�N�g��\��
        rooms[spawnPos.y, spawnPos.x].GetComponent<RoomBehaviour>().ToggleTorchEffect(true);

        // �}�b�v�`��
        maps[spawnPos.y, spawnPos.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
        maps[spawnPos.y, spawnPos.x].transform.localPosition = new Vector3(spawnPos.x * 100, -spawnPos.y * 100, 0);
        maps[spawnPos.y, spawnPos.x].gameObject.name = $"Map: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        // �X�|�[���n�_�̕������~�j�}�b�v�̒��S�ɂ���
        map.transform.localPosition = new Vector3(-spawnPos.x * 100 - 50, spawnPos.y * 100 + 50, 0);
        // �X�|�[���n�_�̃}�b�v���A�N�e�B�u�ɂ���
        maps[spawnPos.y, spawnPos.x].gameObject.SetActive(true);
        // �X�|�[���n�_�A�C�R���𐶐�
        Instantiate(spawnIcon, maps[spawnPos.y, spawnPos.x].transform.position, Quaternion.identity, maps[spawnPos.y, spawnPos.x].transform);

        // ���݂̃����_���E�H�[�N�n�_���X�V
        curWalk.Set(spawnPos.x, spawnPos.y);
        // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
        backTracking.Push(new Vector2Int(curWalk.x, curWalk.y));
        
        RandomWalk();
        CreatePlayer();
    }

    private void Update()
    {

        #region�@�f�o�b�O�p
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Transform[] childRooms = roomParent.GetComponentsInChildren<Transform>();

        //    // �e���̂��܂܂�Ă��邽�߁A�C���f�b�N�X1���烋�[�v���J�n
        //    for (int i = 1; i < childRooms.Length; i++)
        //    {
        //        Destroy(childRooms[i].gameObject);
        //    }

        //    Transform[] childEnemys = enemyParent.GetComponentsInChildren<Transform>();

        //    // �e���̂��܂܂�Ă��邽�߁A�C���f�b�N�X1���烋�[�v���J�n
        //    for (int i = 1; i < childEnemys.Length; i++)
        //    {
        //        Destroy(childEnemys[i].gameObject);
        //    }

        //    Transform[] m = map.GetComponentsInChildren<Transform>();

        //    // �e���̂��܂܂�Ă��邽�߁A�C���f�b�N�X1���烋�[�v���J�n
        //    for (int i = 1; i < m.Length; i++)
        //    {
        //        Destroy(m[i].gameObject);
        //    }

        //    walkCount = 0;
        //    backTracking.Clear();
        //    // ���������ׂč폜
        //    for (int y = 0; y < rooms.GetLength(0); y++)
        //    {
        //        for (int x = 0; x < rooms.GetLength(1); x++)
        //        {
        //            rooms[x, y] = null;
        //            maps[x, y] = null;
        //        }
        //    }

        //    strongEnemyRooms.Clear();

        //    // �Z���̒���(4�����烉���_����)�X�|�[���n�_��ݒ�
        //    spawnPos.y = Random.Range(spawnRandMin, spawnRandMax + 1);
        //    spawnPos.x = Random.Range(spawnRandMin, spawnRandMax + 1);
        //    rooms[spawnPos.y, spawnPos.x] = Instantiate(room, SetRoomPos(spawnPos.y, spawnPos.x), Quaternion.identity, roomParent);
        //    // �킩��₷���悤�ɉ��Ԗڂɐ���������?�Ɛ��������z��̗v�f���𖼑O�ɓ���Ă��܂��B
        //    rooms[spawnPos.y, spawnPos.x].gameObject.name = $"Room: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        //    // �}�b�v�`��
        //    maps[spawnPos.y, spawnPos.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
        //    maps[spawnPos.y, spawnPos.x].transform.localPosition = new Vector3(spawnPos.x * 100, -spawnPos.y * 100, 0);
        //    maps[spawnPos.y, spawnPos.x].gameObject.name = $"Map: {walkCount} ({spawnPos.y}  {spawnPos.x}) (spawnRoom)";
        //    // �X�|�[���n�_�̕������~�j�}�b�v�̒��S�ɂ���
        //    map.transform.localPosition = new Vector3(-spawnPos.x * 100 - 50, spawnPos.y * 100 + 50, 0);
        //    // �X�|�[���n�_�̃}�b�v���A�N�e�B�u�ɂ���
        //    maps[spawnPos.y, spawnPos.x].gameObject.SetActive(true);
        //    // ���݂̃����_���E�H�[�N�n�_���X�V
        //    curWalk = spawnPos;
        //    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
        //    backTracking.Push(new Vector2Int(curWalk.x, curWalk.y));

        //    RandomWalk();
        //}
        #endregion

    }



    /// <summary>
    /// rooms�z��������_���E�H�[�N�����ĕ����𐶐����܂��B
    /// </summary>
    void RandomWalk()
    {
        // �����𐶐�����񐔂�ݒ�
        movementLimit = Random.Range(randomWalkMin, randomWalkMax + 1);
        int forwardCount = 0, backCount = 0, leftCount = 0, rightCount = 0;

        // �����_���E�H�[�N�ƕ����̐�����movementLimit�񕪍s��
        while (walkCount <= movementLimit)
        {         
            int direction;
            int loopCount = 0;          

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
                curWalk.Set(latestCoordinate.x, latestCoordinate.y);
                Debug.Log($"{latestCoordinate} �ɖ߂�܂����B");
            }

            // ���������ɎO��ȏ�i�񂾂Ƃ��ɕʂ̕����ɋȂ���悤�ɂ���
            if (forwardCount >= 3 || backCount >= 3 || leftCount >= 3 || rightCount >= 3)
            {

                Debug.LogWarning($"��{forwardCount} ��{backCount} ��{leftCount} �E{rightCount}");
                List<int> directionLottery = new List<int>();
                if (forwardCount >= 3 || backCount >= 3)
                {
                    // �͈͊O�ɂȂ��Ă��Ȃ��A���A�i�񂾕����ɕ������Ȃ�               
                    if (curWalk.x - 1 >= 1 && rooms[curWalk.y, curWalk.x - 1] == null)
                    {
                        directionLottery.Add(2);
                    }
                    if (curWalk.x + 1 < rooms.GetLength(0) - 1 && rooms[curWalk.y, curWalk.x + 1] == null)
                    {
                        directionLottery.Add(3);
                    }

                    // �i�߂����������
                    if (directionLottery.Count != 0)
                        direction = directionLottery[Random.Range(0, directionLottery.Count)];
                    
                    forwardCount = 0; backCount = 0; leftCount = 0; rightCount = 0;
                    
                    if (directionLottery.Count != 0 && direction == 2) Debug.Log($"���ɐi�݂܂���");
                    if (directionLottery.Count != 0 && direction == 3) Debug.Log($"�E�ɐi�݂܂���");
                }
                if (leftCount >= 3 || rightCount >= 3)
                {
                    if (curWalk.y - 1 >= 1 && rooms[curWalk.y - 1, curWalk.x] == null)
                    {
                        directionLottery.Add(0);
                    }
                    if (curWalk.y + 1 < rooms.GetLength(1) - 1 && rooms[curWalk.y + 1, curWalk.x] == null)
                    {
                        directionLottery.Add(1);
                    }

                    // �i�߂����������
                    if (directionLottery.Count != 0)
                        direction = directionLottery[Random.Range(0, directionLottery.Count)];         

                    forwardCount = 0; backCount = 0; leftCount = 0; rightCount = 0;

                    if (directionLottery.Count != 0 && direction == 0) Debug.Log($"��ɐi�݂܂���");
                    if (directionLottery.Count != 0 && direction == 1) Debug.Log($"���ɐi�݂܂���");
                }
            }

            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            // �ړ�
            switch (direction)
            {
                case 0:
                    forwardCount++; backCount = 0; leftCount = 0; rightCount = 0;
                    // ��Ɉړ�
                    curWalk.y -= 1; break;

                case 1:
                    forwardCount = 0; backCount++; leftCount = 0; rightCount = 0;
                    // ���Ɉړ�
                    curWalk.y += 1; break;

                case 2:
                    forwardCount = 0; backCount = 0; leftCount++; rightCount = 0;
                    // ���Ɉړ�
                    curWalk.x -= 1; break;

                case 3:
                    forwardCount = 0; backCount = 0; leftCount = 0; rightCount++;
                    // �E�Ɉړ�
                    curWalk.x += 1; break;

                default:
                    break;
            }

            // �ړ������ꍇ�����ɕ����𐶐�
            if (direction >= 0)
            {
                walkCount++;

                // �����𐶐�
                rooms[curWalk.y, curWalk.x] = Instantiate(room, SetRoomPos(curWalk.y, curWalk.x), Quaternion.identity, roomParent);
                rooms[curWalk.y, curWalk.x].gameObject.name = $"Room: {walkCount} ({curWalk.y}  {curWalk.x})";

                // �}�b�v�`��
                maps[curWalk.y, curWalk.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
                maps[curWalk.y, curWalk.x].transform.localPosition = new Vector3(curWalk.x * 100, -curWalk.y * 100, 0);
                maps[curWalk.y, curWalk.x].gameObject.name = $"Map: {walkCount} ({curWalk.y}  {curWalk.x})";

                // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                backTracking.Push(new Vector2Int(curWalk.x, curWalk.y));
            }

            // ����ڂɕ��΂𐶐�
            if (walkCount == 2)
            {
                // ���ΐ���
                GameObject bonfire = Instantiate(bonfirePrefab, rooms[curWalk.y, curWalk.x].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, rooms[curWalk.y, curWalk.x].transform);
                bonfire.gameObject.name = $"bonfire1: {walkCount} ({curWalk.y}  {curWalk.x})";
                // ���΂𐶐������������L�^
                bonfireRoomPos.Set(curWalk.x, curWalk.y);
            }

        }
        // �{�X��������
        GenerateBossRoom(rooms[spawnPos.y, spawnPos.x]);
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
                return curWalk.y - 1 >= 1 && rooms[curWalk.y - 1, curWalk.x] == null;
            case 1:
                return curWalk.y + 1 < rooms.GetLength(1) - 1 && rooms[curWalk.y + 1, curWalk.x] == null;            
            case 2:
                return curWalk.x - 1 >= 1 && rooms[curWalk.y, curWalk.x - 1] == null;
            case 3:
                return curWalk.x + 1 < rooms.GetLength(0) - 1 && rooms[curWalk.y, curWalk.x + 1] == null;

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
        return new Vector3(roomX * 20f, 0f, -roomY * 20f);
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
            Instantiate(warriorPrefab, rooms[spawnPos.y, spawnPos.x].transform.position + new Vector3(0, warriorY, 0), Quaternion.identity, playerParent);
            // �}�b�v�̃v���C���[�A�C�R������
            playerIcon = Instantiate(warriorIcon, Vector3.zero, Quaternion.identity.normalized, maps[spawnPos.y, spawnPos.x].transform);
            playerIcon.transform.localPosition = Vector3.zero;
        }
        if (GameManager.Instance.playerData._playerName == "���@�g��")
        {
            Instantiate(wizardPrefab, rooms[spawnPos.y, spawnPos.x].transform.position + new Vector3(0, wizardY, 0), Quaternion.identity, playerParent);
            // �}�b�v�̃v���C���[�A�C�R������
            playerIcon = Instantiate(wizardIcon, Vector3.zero, Quaternion.identity.normalized, maps[spawnPos.y, spawnPos.x].transform);
            playerIcon.transform.localPosition = Vector3.zero;
        }

        // �J�����̈ʒu��ύX
        cam.transform.position = rooms[spawnPos.y, spawnPos.x].transform.position + roomCam;

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
        Vector2Int farthestRoomPos = new Vector2Int();
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
                        farthestRoomPos.Set(x, y);
                    }
                }
            }
        }

        Debug.Log($"{rooms[farthestRoomPos.y, farthestRoomPos.x].name}�@����ԉ��������ł��B");

        // �{�X�̔��𕔉��̉����ɐ�������s����A�����̉��ɔ�������Ɗ����Ă��܂����߁A�����Ȃ��悤�ɂ���B
        // ��ԉ��������̏�ɕ���������ꍇ
        if (farthestRoomPos.y - 1 >= 0 && rooms[farthestRoomPos.y - 1, farthestRoomPos.x] != null)
        {
            List<Vector2Int> filteredRoom = new List<Vector2Int>();

            // ���E�ɕ��������邩�ǂ������m�F���A�Ȃ������ꍇList�ɒǉ�
            if (farthestRoomPos.x - 1 >= 0 && rooms[farthestRoomPos.y, farthestRoomPos.x - 1] == null)
            {
                filteredRoom.Add(new Vector2Int(farthestRoomPos.x - 1, farthestRoomPos.y));
            }
            if (farthestRoomPos.x + 1 < rooms.GetLength(0) && rooms[farthestRoomPos.y, farthestRoomPos.x + 1] == null)
            {
                filteredRoom.Add(new Vector2Int(farthestRoomPos.x + 1, farthestRoomPos.y));
            }

            
            List<Vector2Int> bossRoomCandidate = new List<Vector2Int>();
            // ���E�̕����̏�ɕ��������邩���m�F���A�Ȃ������ꍇList�ɒǉ�
            for (int i = 0; i < filteredRoom.Count; i++)
            {
                if (rooms[filteredRoom[i].y - 1, filteredRoom[i].x] == null)
                {
                    bossRoomCandidate.Add(new Vector2Int(filteredRoom[i].x, filteredRoom[i].y));
                }
            }

            // ��L�̏����ŁA�������擾�ł��Ȃ������ꍇ�A�V��������2�񐶐�����
            if (bossRoomCandidate.Count == 0)
            {
                bossRoomCandidate = new List<Vector2Int>(filteredRoom);
                int rand = Random.Range(0, bossRoomCandidate.Count);

                // �{�X�����̎�O�̕����𐶐�
                walkCount++;
                rooms[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x] = Instantiate(room, SetRoomPos(bossRoomCandidate[rand].y, bossRoomCandidate[rand].x), Quaternion.identity, roomParent);
                rooms[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x].gameObject.name = $"NewRoom1: {walkCount} ({bossRoomCandidate[rand].y}  {bossRoomCandidate[rand].x})";
                // �}�b�v�`��
                maps[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
                maps[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x].transform.localPosition = new Vector3(bossRoomCandidate[rand].x * 100, -bossRoomCandidate[rand].y * 100, 0);
                maps[bossRoomCandidate[rand].y, bossRoomCandidate[rand].x].gameObject.name = $"NewRoom1Map: {walkCount} ({bossRoomCandidate[rand].y}  {bossRoomCandidate[rand].x})";
                // ��ԉ����������獶�E�ǂ���ɐi�񂾂����擾
                int direction = bossRoomCandidate[rand].x - farthestRoomPos.x;
                // ��ԉ����������X�V
                if (direction == -1) direction--;
                if (direction == +1) direction++;
                farthestRoomPos.y = bossRoomCandidate[rand].y;
                farthestRoomPos.x += direction;
            }
            else
            {
                // ���E�ǂ���ɕ����𐶐����邩���߂�
                int rand = Random.Range(0, bossRoomCandidate.Count);
                farthestRoomPos.Set(bossRoomCandidate[rand].x, bossRoomCandidate[rand].y);
            }
            // �{�X�����𐶐�
            walkCount++;
            rooms[farthestRoomPos.y, farthestRoomPos.x] = Instantiate(room, SetRoomPos(farthestRoomPos.y, farthestRoomPos.x), Quaternion.identity, roomParent);
            rooms[farthestRoomPos.y, farthestRoomPos.x].gameObject.name = $"NewRoom2: {walkCount} ({farthestRoomPos.y}  {farthestRoomPos.x}) (bossRoom)";
            // �}�b�v�`��
            maps[farthestRoomPos.y, farthestRoomPos.x] = Instantiate(mapPrefab, map.transform.position, Quaternion.identity, map);
            maps[farthestRoomPos.y, farthestRoomPos.x].transform.localPosition = new Vector3(farthestRoomPos.x * 100, -farthestRoomPos.y * 100, 0);
            maps[farthestRoomPos.y, farthestRoomPos.x].gameObject.name = $"NewRoom2Map: {walkCount} ({farthestRoomPos.y}  {farthestRoomPos.x}) (bossRoom)";
        }
        else// ��ԉ��������̏�ɕ������Ȃ��ꍇ
        {
            // ��ԉ������������̂܂܃{�X�����ɂȂ�̂ŁA���O�����ύX
            rooms[farthestRoomPos.y, farthestRoomPos.x].gameObject.name += " (bossRoom)";
        }


        // �{�X�̔��𐶐�
        Instantiate(bossDoor, rooms[farthestRoomPos.y, farthestRoomPos.x].transform.position, Quaternion.identity, rooms[farthestRoomPos.y, farthestRoomPos.x].transform);
        Debug.Log($"{rooms[farthestRoomPos.y, farthestRoomPos.x]}�@�Ƀ{�X�̔������B");

        // ���ΐ���
        GameObject bonfire = Instantiate(bonfirePrefab, rooms[farthestRoomPos.y, farthestRoomPos.x].transform.position + new Vector3(0, -2.4f, 0), Quaternion.identity, rooms[farthestRoomPos.y, farthestRoomPos.x].transform);
        bonfire.gameObject.name = $"bonfire2: bossRoom ({farthestRoomPos.y}  {farthestRoomPos.x})";

        // �{�X�����̈ʒu���L�^
        bossRoomPos.Set(farthestRoomPos.x, farthestRoomPos.y);

        // ���G����
        GenerateStrongEnemy();
    }

    /// <summary>
    /// �{�X�����̍��E�A���̂����ꂩ�ɋ��G�𐶐����܂��B��̂��������ł��Ȃ������ꍇ���ׂ̗̕����ɂ�����̐������܂��B
    /// </summary>
    void GenerateStrongEnemy()
    {
        // �{�X�����ɗאڂ��Ă��镔���̈ʒu���i�[����List
        List<Vector2Int> adjacentRooms = new List<Vector2Int>();

        // ���ɕ���������ꍇList�ɒǉ�
        if (bossRoomPos.y + 1 < rooms.GetLength(1) && rooms[bossRoomPos.y + 1, bossRoomPos.x] != null)
        {
            // �w�肵�������ɕ��΂��Ȃ��ꍇList�ɒǉ�
            if (rooms[bossRoomPos.y + 1, bossRoomPos.x] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                adjacentRooms.Add(new Vector2Int(bossRoomPos.x, bossRoomPos.y + 1));
        }
        // ���ɕ���������ꍇList�ɒǉ�
        if (bossRoomPos.x - 1 >= 0 && rooms[bossRoomPos.y, bossRoomPos.x - 1] != null)
        {
            if (rooms[bossRoomPos.y, bossRoomPos.x - 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                adjacentRooms.Add(new Vector2Int(bossRoomPos.x - 1, bossRoomPos.y));
        }
        // �E�ɕ���������ꍇList�ɒǉ�
        if (bossRoomPos.x + 1 < rooms.GetLength(0) && rooms[bossRoomPos.y, bossRoomPos.x + 1] != null)
        {
            if (rooms[bossRoomPos.y, bossRoomPos.x + 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                adjacentRooms.Add(new Vector2Int(bossRoomPos.x + 1, bossRoomPos.y));
        }

        // �����_����2�̗אڂ�������G�����ɐݒ�
        List<Vector2Int> selectedRooms = new List<Vector2Int>();
        while (selectedRooms.Count < 2 && adjacentRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, adjacentRooms.Count);
            selectedRooms.Add(adjacentRooms[randomIndex]);
            adjacentRooms.RemoveAt(randomIndex);
        }
        strongEnemyRooms = new List<Vector2Int>(selectedRooms);
        // ���G����̂��������ł��Ȃ������ꍇ�A���G�𐶐����������𒆐S�ɁA
        // �{�X�������������㉺���E�̕����̂����ꂩ�ɋ��G�𐶐�
        if (strongEnemyRooms.Count == 1)
        {
            selectedRooms.Clear();

            // ���G�𐶐����������𒆐S�ɏ㉺���E�̕������擾
            // ��ɕ���������ꍇList�ɒǉ�
            if (strongEnemyRooms[0].y -1 >= 0 && rooms[strongEnemyRooms[0].y - 1, strongEnemyRooms[0].x] != null)
            {
                // �w�肵�������ɕ��΂��Ȃ��ꍇList�ɒǉ�
                if (rooms[strongEnemyRooms[0].y - 1, strongEnemyRooms[0].x] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x, strongEnemyRooms[0].y - 1));
            }
            // ���ɕ���������ꍇList�ɒǉ�
            if (strongEnemyRooms[0].y + 1 < rooms.GetLength(1) && rooms[strongEnemyRooms[0].y + 1, strongEnemyRooms[0].x] != null)
            {
                if (rooms[strongEnemyRooms[0].y + 1, strongEnemyRooms[0].x] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x, strongEnemyRooms[0].y + 1));
            }
            // ���ɕ���������ꍇList�ɒǉ�
            if (strongEnemyRooms[0].x - 1 >= 0 && rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x - 1] != null)
            {
                if (rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x - 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x - 1, strongEnemyRooms[0].y));
            }
            // �E�ɕ���������ꍇList�ɒǉ�
            if (strongEnemyRooms[0].x + 1 < rooms.GetLength(0) && rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x + 1] != null)
            {
                if (rooms[strongEnemyRooms[0].y, strongEnemyRooms[0].x + 1] != rooms[bonfireRoomPos.y, bonfireRoomPos.x])
                    selectedRooms.Add(new Vector2Int(strongEnemyRooms[0].x + 1, strongEnemyRooms[0].y));
            }

            // �{�X���������O
            selectedRooms.Remove(bossRoomPos);
            // �I�����ꂽ�������烉���_���ŋ��G�����ɂ���
            int randomIndex = Random.Range(0, selectedRooms.Count);
            strongEnemyRooms.Add(selectedRooms[randomIndex]);
        }

        // ���G�����ɓG�𐶐�
        for (int i = 0; i < strongEnemyRooms.Count; i++)
        {
            Instantiate(strongEnemy, rooms[strongEnemyRooms[i].y, strongEnemyRooms[i].x].transform.position, Quaternion.identity, enemyParent);
        }

        // �V���b�v�ƕ󔠂ƎG���G�𐶐�
        GenerateItemAndEnemy();
        // �ړ��p�̔����ׂĂ𐶐��B
        GenerateDoorsInAllRooms();
    }

    /// <summary>
    /// �����Ȃ������ɃV���b�v�ƕ󔠂ƎG���G�𐶐����܂��B
    /// </summary>
    void GenerateItemAndEnemy()
    {
        // �X�|�[���n�_�Ƃ��̏㉺���E�A���΁A�{�X�����A���G�̕��������O����rooms�̔z��
        GameObject[,] filteredRoomArray = new GameObject[rooms.GetLength(0), rooms.GetLength(1)];
        System.Array.Copy(rooms, filteredRoomArray, filteredRoomArray.Length);

        // �X�|�[���n�_�Ƃ��̏㉺���E�̕��������O��
        filteredRoomArray[spawnPos.y, spawnPos.x] = null;
        filteredRoomArray[spawnPos.y - 1, spawnPos.x] = null;
        filteredRoomArray[spawnPos.y + 1, spawnPos.x] = null;
        filteredRoomArray[spawnPos.y, spawnPos.x - 1] = null;
        filteredRoomArray[spawnPos.y, spawnPos.x + 1] = null;

        // ���΂̏ꏊ��null��
        Debug.Log("����"+filteredRoomArray[bonfireRoomPos.y, bonfireRoomPos.x]);
        filteredRoomArray[bonfireRoomPos.y, bonfireRoomPos.x] = null;

        // �{�X�����̏ꏊ��null��
        Debug.Log("�{�X����" + filteredRoomArray[bossRoomPos.y, bossRoomPos.x]);
        filteredRoomArray[bossRoomPos.y, bossRoomPos.x] = null;

        // ���G�̏ꏊ��null��
        for (int i = 0; i < strongEnemyRooms.Count; i++)
        {
            Debug.Log("���G����" + filteredRoomArray[strongEnemyRooms[i].y, strongEnemyRooms[i].x]);
            filteredRoomArray[strongEnemyRooms[i].y, strongEnemyRooms[i].x] = null;
        }

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
        int lotteryShop = Random.Range(0, emptyRoomList.Count);
        Instantiate(shopPrefab, emptyRoomList[lotteryShop].transform.position, Quaternion.identity, emptyRoomList[lotteryShop].transform);
        emptyRoomList.RemoveAt(lotteryShop);


        // �󔠂𐶐�
        int lotteryTreasure = Random.Range(0, emptyRoomList.Count);
        Instantiate(treasurePrefab, emptyRoomList[lotteryTreasure].transform.position, Quaternion.identity, emptyRoomList[lotteryTreasure].transform);
        emptyRoomList.RemoveAt(lotteryTreasure);

        // �c���������ɎG���G�𐶐�
        //Debug.Log("emptyList"+emptyRoomList.Count);
        foreach (GameObject emptyRoom in emptyRoomList)
        {
            Instantiate(smallEnemy, emptyRoom.transform.position, Quaternion.identity, enemyParent);
        }

        // �X�|�[���n�_�̎���ɎG���G�𐶐�
        if (spawnPos.y - 1 >= 0 && rooms[spawnPos.y - 1, spawnPos.x] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y - 1, spawnPos.x].transform.position, Quaternion.identity, enemyParent);
        }
        if (spawnPos.y + 1 < rooms.GetLength(1) && rooms[spawnPos.y + 1, spawnPos.x] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y + 1, spawnPos.x].transform.position, Quaternion.identity, enemyParent);
        }
        if (spawnPos.x - 1 >= 0 && rooms[spawnPos.y, spawnPos.x - 1] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y, spawnPos.x - 1].transform.position, Quaternion.identity, enemyParent);
        }
        if (spawnPos.x + 1 < rooms.GetLength(0) && rooms[spawnPos.y, spawnPos.x + 1] != null)
        {
            Instantiate(smallEnemy, rooms[spawnPos.y, spawnPos.x + 1].transform.position, Quaternion.identity, enemyParent);
        }
    }


    /// <summary>
    /// DoorCheck���\�b�h���g���A���ׂĂ̕����̔��𐶐����A�}�b�v�̔��܂��́A�ǂ�`�悵�܂��B
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
                    DoorCheck(y, x);
                }
            }
        }

        // �X�|�[���n�_�̔���S�J
        rooms[spawnPos.y, spawnPos.x].GetComponent<RoomBehaviour>().OpenDoors(new Vector2Int(spawnPos.x, spawnPos.y));
    }

    /// <summary>
    /// rooms�̗v�f�����󂯎��A���̕����ɔ��𐶐����A�}�b�v�̔��܂��́A�ǂ�`�悵�܂��B
    /// </summary>
    /// <param name="roomY"></param>
    /// <param name="roomX"></param>
    void DoorCheck(int roomY, int roomX)
    {
        bool[] doorStatus = new bool[4];

        // �㉺���E�ɕ���������ꍇdoorStatus��true�ɂ��܂��B
        for (int i = 0; i < 4; i++)
        {
            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            switch (i)
            {
                case 0:
                    if (roomY - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̏�̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̏�̕����́@{rooms[roomY - 1, roomX]}");
                    doorStatus[0] = (rooms[roomY - 1, roomX] != null) ? true : false;
                    break;

                case 1:
                    if (roomY + 1 >= rooms.GetLength(1)) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̉��̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̉��̕����́@{rooms[roomY + 1, roomX]}");
                    doorStatus[1] = (rooms[roomY + 1, roomX] != null) ? true : false;
                    break;

                case 2:
                    if (roomX - 1 < 0) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̍��̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̍��̕����́@{rooms[roomY, roomX - 1]}");
                    doorStatus[2] = (rooms[roomY, roomX - 1] != null) ? true : false;
                    break;

                case 3:
                    if (roomX + 1 >= rooms.GetLength(0)) { /*Debug.Log($"{rooms[roomY, roomX]}�@�̉E�̕����́@�͈͊O�ł�");*/ break; }

                    //Debug.Log($"{rooms[roomY, roomX]}�@�̉E�̕����́@{rooms[roomY, roomX + 1]}");
                    doorStatus[3] = (rooms[roomY, roomX + 1] != null) ? true : false;
                    break;

                default:
                    Debug.LogError($"switch���ɓn���l���Ԉ���Ă��܂��B:  {i}");
                    break;
            }
        }

        // �����̃h�A��z�u
        rooms[roomY, roomX].GetComponent<RoomBehaviour>().InitRoom(doorStatus);
        // �}�b�v�̃h�A�܂��͕ǂ�`��
        maps[roomY, roomX].GetComponent<MapRoomBehaviour>().UpdateRoomMap(doorStatus);
    }
}