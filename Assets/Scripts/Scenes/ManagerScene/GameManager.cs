using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�v���C���[
    GameObject player;
    public PlayerDataManager playerData;
    public List<RelicDataManager> relicDataList { private set; get; } = new List<RelicDataManager>();
    public Dictionary<int, int> hasRelics = new Dictionary<int, int>();     // �������Ă��郌���b�N���i�[    
    int maxRelics = 12;

    bool isAlreadyRead = false; // ReadPlayer�œǂݍ��񂾂��𔻒肷��

    [SerializeField] UIManager uiManager;
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    //�V���O���g��
    public static GameManager Instance;
    private void Awake()
    {
        // �V���O���g���C���X�^���X���Z�b�g�A�b�v
        if (Instance == null)
        {
            Instance = this;
        }

        InitializeRelics();

        // �e�V�[���Ńf�o�b�O����Ƃ��ɃR�����g���������Ă�������
        // ��x���ǂݍ���ł��Ȃ����
        if (!isAlreadyRead) ReadPlayer("Warrior");
        
    }

    
    /// <summary>
    /// �����b�N�f�[�^�̏��������s���܂��B
    /// </summary>
    private void InitializeRelics()
    {
        relicDataList.Add(new RelicDataManager(1));     // ID���ɊǗ����������ߍŏ��̗v�f�������
        for (int RelicID=1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics.Add(RelicID,0);

            relicDataList.Add(new RelicDataManager(RelicID));
        }
    }

    /// <summary>
    /// �I�����ꂽ�v���C���[���C���X�^���X�����A�����b�N���擾���܂��B
    /// </summary>
    /// <param name="playerJob"></param>
    public void ReadPlayer(string playerJob)
    {
        isAlreadyRead = true;
        if (playerJob == "Warrior")
        {
            playerData = new PlayerDataManager("Warrior");
            hasRelics[10] += 1;      // �����̉ʎ�
            hasRelics[4] += 2;       // �_��̃s�A�X
            ShowRelics();
        }
        if (playerJob == "Wizard")
        {
            playerData = new PlayerDataManager("Wizard");
            hasRelics[5] += 1;     // �痢�ዾ
            hasRelics[9] += 2;     // �x���̋��ݑ�
            ShowRelics();
        }
    }


    public void ShowRelics()
    {
        // relicPlace�̎q�I�u�W�F�N�g�����ׂ�Destroy
        Transform[] children = relicPlace.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            if (hasRelics.ContainsKey(RelicID) && hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                relic.transform.localScale = Vector3.one * 0.9f;                   // ��������Prefab�̑傫������
                relic.Init(RelicID);                                               // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = hasRelics[RelicID].ToString();      // Prefab�̎q�I�u�W�F�N�g�ł��鏊������\������e�L�X�g��ύX

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = relicDataList[RelicID]._relicName.ToString();        // �����b�N�̖��O��ύX
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = relicDataList[RelicID]._relicEffect.ToString();      // �����b�N�����ύX

            }
        }

        uiManager.UIEventsReload();
    }
}
