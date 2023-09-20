using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRoomBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] door = new GameObject[4];
    [SerializeField] GameObject[] wall = new GameObject[4];

    void ChooseDoorOrWall(bool[] isDoor)
    {
        for (int i = 0; i < 4; i++)
        {
            if (isDoor[i] == true)
            {
                door[i].SetActive(true);
            }
            else
            {
                wall[i].SetActive(true);
            }
        }
    }
}
