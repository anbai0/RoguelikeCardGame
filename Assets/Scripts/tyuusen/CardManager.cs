using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class CardManager : MonoBehaviour
{
    List<GameObject> idList = new List<GameObject>();
    List<int> commonList = new List<int>();
    List<int>  uncommonList= new List<int>();
    List<int>  rareList= new List<int>();
    public int idlength = 28;
    int rareNum;
    int normalNum;
    int normalNum2;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= idlength; i++)
        { 
            //if (idList[i].rare == 1)//
            commonList.Add(i);
        }

        for (int i = 1; i <= idlength; i++)
        {
            //if (idList[i].rare == 2)
                uncommonList.Add(i);
        }
        for (int i = 1; i <= idlength; i++)
       {
           // if (idList[i].rare == 3)
                rareList.Add(i);
        }
        // Update is called once per frame
        void Update()
        {

        }
        void enamylot()
        {
           rareNum= Random.Range(uncommonList[0],uncommonList[uncommonList.Capacity]);
           normalNum = Random.Range(commonList[0], commonList[commonList.Capacity]);
            commonList.Remove(normalNum);
            normalNum2
                -= Random.Range(commonList[0], commonList[commonList.Capacity]);
        }
        void rareenamylot()
        {
            Random.Range(1, idlength);
        }
        void bossenamylot()
        {
            Random.Range(1, idlength);
        }

    }
}
