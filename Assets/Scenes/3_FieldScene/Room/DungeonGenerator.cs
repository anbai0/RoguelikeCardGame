using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject room;
    public GameObject[,] rooms = new GameObject[6, 6];

    // �X�|�[���n�_
    public int spawnY;
    public int spawnX;
    public const int spawnRandMin = 2;
    public const int spawnRandMax = 3;

    // �����_���E�H�[�N
    public int curWalkY;
    public int curWalkX;
    public int randomWalkMin = 13;
    public int randomWalkMax = 15;
    public int movementLimit;
    public int walkCount;
    public Stack<Vector2Int> backTracking = new Stack<Vector2Int>();

    void Start()
    {
        // �Z���̒���(4�����烉���_����)�X�|�[���n�_��ݒ�
        spawnY = Random.Range(spawnRandMin, spawnRandMax);
        spawnX = Random.Range(spawnRandMin, spawnRandMax);   
        rooms[spawnY, spawnX] = Instantiate(room, SetRoomPos(spawnY, spawnX), Quaternion.identity);
        // ���݂̃����_���E�H�[�N�n�_���X�V
        curWalkY = spawnY;
        curWalkX = spawnX;
        // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
        backTracking.Push(new Vector2Int(curWalkY, curWalkX));
    }


    void RandomWalk()
    {
        // �����𐶐�����񐔂�ݒ�
        movementLimit = Random.Range(randomWalkMin, randomWalkMax);



        while (walkCount <= movementLimit)
        {
            // 0 - forward, 1 - back, 2 - Left, 3 - Right
            int direction;
            int loopCount = -1;

            do
            {
                direction = Random.Range(0, 3);
                loopCount++;
            } while (!IsDirectionValid(direction) && loopCount <= 10);


            if (loopCount == 10)
            {
                direction = -1;
                Vector2Int latestCoordinate = backTracking.Pop();
                curWalkY = latestCoordinate.y;
                curWalkX = latestCoordinate.x;        
            }


            switch (direction)
            {
                case 0:
                    // ��ɐ���
                    curWalkX -= 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;
                    break;
                case 1:
                    // ���ɐ���
                    curWalkX += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;
                    break;
                case 2:
                    // ���ɐ���
                    curWalkY -= 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
                    // �o�b�N�g���b�L���O�p�ɃX�^�b�N�Ƀv�b�V��
                    backTracking.Push(new Vector2Int(curWalkY, curWalkX));
                    walkCount++;
                    break;
                case 3:
                    // �E�ɐ���
                    curWalkY += 1;
                    rooms[curWalkY, curWalkX] = Instantiate(room, SetRoomPos(curWalkY, curWalkX), Quaternion.identity);
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
        switch (dir)
        {
            case 0:
                // ��ɍs����ꍇtrue
                return curWalkX -1 >= 0 && rooms[curWalkY, curWalkX - 1] == null;
            case 1:
                // ���ɍs����ꍇtrue
                return curWalkX <= rooms.GetLength(1) - 1 && rooms[curWalkY, curWalkX + 1] == null;
            case 2:
                // ���ɍs����ꍇtrue
                return curWalkY >= 0 && rooms[curWalkY - 1, curWalkX] == null;
            case 3:
                // �E�ɍs����ꍇtrue
                return curWalkY <= rooms.GetLength(0) - 1 && rooms[curWalkY + 1, curWalkX] == null;
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
        return new Vector3(vertical * 20f, horizontal * 20f, 0f);
    }
}