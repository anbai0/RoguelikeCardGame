using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�v���C���[
    GameObject player;
    public PlayerDataManager playerData;


    //�V���O���g��
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }


    void Update()
    {
        
    }

    public void ReadPlayer(string playerJob)
    {
        if (playerJob == "Warrior")
        {
            playerData = new PlayerDataManager("Warrior");
        }
        else if (playerJob == "Wizard")
        {
            playerData = new PlayerDataManager("Wizard");
        }
    }
}
