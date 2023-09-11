using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    public GameObject[,] rooms = new GameObject[6, 6];
    public Transform roomParent;

    // �X�|�[���n�_
    private int spawnY;
    private int spawnX;
    private const int spawnRandMin = 2;
    private const int spawnRandMax = 3;

    // �����_���E�H�[�N
    private int curWalkY;
    private int curWalkX;
    private int randomWalkMin = 13;
    private int randomWalkMax = 15;
    private int movementLimit;
    private int walkCount;
    // �o�b�N�g���b�L���O
    private Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    void Start()
    {
        // �Z���̒���(4�����烉���_����)�X�|�[���n�_��ݒ�
        spawnY = Random.Range(spawnRandMin, spawnRandMax + 1);
        spawnX = Random.Range(spawnRandMin, spawnRandMax + 1);   
        rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity, roomParent);
        // ���݂̃����_���E�H�[�N�n�_���X�V
        curWalkY = spawnY;
        curWalkX = spawnX;
        // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
        backTracking.Push(new Vector2Int(curWalkY, curWalkX));
        // �킩��₷���悤�ɉ��Ԗڂɐ���������?�Ɛ��������z��̗v�f���𖼑O�ɓ���Ă��܂��B
        rooms[curWalkY, curWalkX].gameObject.name = $"Room: 0 ({curWalkY}  {curWalkX})";
        RandomWalk();
        
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
                Debug.Log(latestCoordinate);
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
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;                   
                    break;

                case 1:
                    // ���ɐ���
                    curWalkX += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;                   
                    break;

                case 2:
                    // ���ɐ���
                    curWalkY -= 1;        
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;          
                    break;

                case 3:
                    // �E�ɐ���
                    curWalkY += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity, roomParent);
                    rooms[curWalkY, curWalkX].gameObject.name = $"Room: {walkCount} ({curWalkY}  {curWalkX})";

                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++; 
                    break;

                default:
                    break;
            }
        }

        
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
                return curWalkX + 1 <= rooms.GetLength(1) - 1 && rooms[curWalkY, curWalkX + 1] == null;            
            case 2:
                return curWalkY - 1 >= 0 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                return curWalkY + 1 <= rooms.GetLength(0) - 1 && rooms[curWalkY + 1, curWalkX] == null;

            default:
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

    
}