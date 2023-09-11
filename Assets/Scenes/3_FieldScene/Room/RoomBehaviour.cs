using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] gate;  // 0 - forward, 1 - back, 2 - Left, 3 - Right
    public GameObject[] doors;

    public bool[] status;

    void Start()
    {
        UpdateRoom(status);
    }

    void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            gate[i].SetActive(status[i]);
            //doors[i].SetActive(status[i]);
        }
    }
}
