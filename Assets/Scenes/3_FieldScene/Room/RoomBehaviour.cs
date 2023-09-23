using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] gate;  // 0 - forward, 1 - back, 2 - Left, 3 - Right
    public GameObject[] doors;

    /// <summary>
    /// �v�f����4��bool�^�z����󂯎��A�Ή������ꏊ��gate��SetActive��true�ɂ��܂��B
    /// <para>0 - forward, 1 - back, 2 - Left, 3 - Right</para>
    /// </summary>
    /// <param name="status"></param>
    public void UpdateRoom(bool[] status)
    {
        //Debug.Log("UpdateRoom���Ă΂ꂽ�B");
        
        for (int i = 0; i < status.Length; i++)
        {
            //Debug.Log($"{transform.name}:  {i}�Ԗ�:   {status[i]}");
            gate[i].SetActive(status[i]);
            //doors[i].SetActive(status[i]);
        }
    }
}
