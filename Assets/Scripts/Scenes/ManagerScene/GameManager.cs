using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �v���C���[
    public PlayerDataManager playerData;
    public List<CardDataManager> cardDataList { private set; get; } = new List<CardDataManager>();
    public List<RelicDataManager> relicDataList { private set; get; } = new List<RelicDataManager>();
    public Dictionary<int, int> hasRelics { private set; get; } = new Dictionary<int, int>();     // �������Ă��郌���b�N���i�[    
    public int maxCards { get; private set; } = 20;
    public int maxRelics { get; private set; } = 12;
    private const int defaultDeckSize = 3;
    private const int ariadnesThreadID = 1;     // �A���h�l�̎��̃����b�N��ID(�f�b�L�̏���𑝂₷�����b�N)

    public Action OnCardDiscard;      // �J�[�h�̔j�������s�������ɌĂяo�����f���Q�[�g

    private bool isAlreadyRead = false; // ReadPlayer�œǂݍ��񂾂��𔻒肷��

    [SerializeField] UIManager uiManager;
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    
    public static GameManager Instance;     // �V���O���g��
    private void Awake()
    {
        // �V���O���g���C���X�^���X���Z�b�g�A�b�v
        if (Instance == null)
        {
            Instance = this;
        }

        InitializeItemData();

        // �e�V�[���Ńf�o�b�O����Ƃ��ɃR�����g���������Ă�������
        // ��x���ǂݍ���ł��Ȃ����
        if (!isAlreadyRead) ReadPlayer("Warrior");
        
    }

    
    /// <summary>
    /// �A�C�e���f�[�^�̏��������s���܂��B
    /// </summary>
    private void InitializeItemData()
    {
        cardDataList.Add(new CardDataManager(1));       // ID���ɊǗ����������ߍŏ��̗v�f�������
        for (int cardID = 1; cardID <= maxCards; cardID++)
        {
            cardDataList.Add(new CardDataManager(cardID));
        }

        relicDataList.Add(new RelicDataManager(1));     // ID���ɊǗ����������ߍŏ��̗v�f�������
        for (int relicID = 1; relicID <= maxRelics; relicID++)
        {
            hasRelics.Add(relicID,0);

            relicDataList.Add(new RelicDataManager(relicID));
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
            //�������Ɏw�肵��RelicID�̃L�[�����݂��邩�ǂ����ƃ����b�N���P�ȏ㏊�����Ă��邩
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


    /// <summary>
    /// �����J�[�h���f�b�L����ɒB���Ă��邩�𔻒肵�A
    /// <para>����ɒB���Ă���ꍇ�A�J�[�h�j����ʂɑJ�ڂ��܂��B</para>
    /// <para></para>
    /// </summary>
    /// <returns>
    /// �f�b�L����ɒB���Ă���ꍇ�Atrue
    /// <para>�f�b�L����ɒB���Ă��Ȃ��ꍇ�Afalse</para>
    /// </returns>
    public bool CheckDeckFull()
    {
        int maxDeckSize = defaultDeckSize + hasRelics[ariadnesThreadID];
        if (playerData._deckList.Count >= maxDeckSize)
        {
            uiManager.ToggleDiscardScreen(true);        // �J�[�h�j�����     

            return true;
        }
        return false;
    }


    /// <summary>
    /// �J�[�h��j�������ꍇ�A���\�b�h���̃C�x���g�𔭉΂����܂��B
    /// </summary>
    /// <param name="isDiscard">�J�[�h��j�������ꍇ�Atrue,�j�����Ȃ������ꍇ�Afalse</param>
    public void TriggerDiscardAction(bool isDiscard)
    {
        if (isDiscard)
        {
            OnCardDiscard?.Invoke();
            OnCardDiscard = null;
        }
        else
        {
            OnCardDiscard = null;
            return;
        }
    }

    /// <summary>
    /// �Q�[���f�[�^�̃��Z�b�g�����܂��B
    /// <para>���󃊃U���g�V�[������^�C�g���V�[���ɖ߂�Ƃ��݂̂ɂ����g���Ȃ����ߌ�ŏ��������܂��B</para>
    /// </summary>
    public void ResetGameData()
    {
        playerData = null;
        isAlreadyRead = false;
        Instance = null;

        // ���������b�N������
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics[RelicID] = 0; // �L�[�ƒl��ݒ�
        }

        ShowRelics();
    }


    /// <summary>
    /// ManagerScene�ȊO�̃V�[�����A�����[�h���A�A�Z�b�g��
    /// </summary>
    public void UnloadAllScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "ManagerScene")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        Resources.UnloadUnusedAssets();
    }
}
