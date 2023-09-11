using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] gate;  // 0 - forward, 1 - back, 2 - Left, 3 - Right
    public GameObject[] doors;

    /// <summary>
    /// 要素数が4つのbool型配列を受け取り、対応した場所のgateのSetActiveをtrueにします。
    /// <para>0 - forward, 1 - back, 2 - Left, 3 - Right</para>
    /// </summary>
    /// <param name="status"></param>
    public void UpdateRoom(bool[] status)
    {
        //Debug.Log("UpdateRoomが呼ばれた。");
        
        for (int i = 0; i < status.Length; i++)
        {
            //Debug.Log($"{transform.name}:  {i}番目:   {status[i]}");
            gate[i].SetActive(status[i]);
            //doors[i].SetActive(status[i]);
        }
    }
}
