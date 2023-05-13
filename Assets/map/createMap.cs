using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMap : MonoBehaviour
{
    [SerializeField]
    GameObject blockPrefab;
    public float spacing = 1f;
    [SerializeField]
    private int numRows = 4;
    [SerializeField]
    private int numCols = 3;

    private Vector3 spawnPosition;
    [SerializeField]
    private GameObject cameraprefab;
    List<GameObject> blocks = new List<GameObject>();
    private Vector3 cameraPos = new Vector3(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        Vector3 blockSize = blockPrefab.transform.localScale;
        Vector3 spawnOrigin = new Vector3(0, 0, 0);    
        for(int row=0;row<numRows;row++)
        {
            for(int col=0;col<numCols;col++)
            {
                Vector3 position =spawnOrigin+new Vector3(col*(blockSize.x+spacing),0,-row*(blockSize.z+spacing));
                GameObject block =Instantiate(blockPrefab,position,Quaternion.identity);
                blocks.Add(block);
                block.transform.parent = transform;
               // GameObject camera=Instantiate(cameraprefab,position,Quaternion.identity);
            }
           

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
