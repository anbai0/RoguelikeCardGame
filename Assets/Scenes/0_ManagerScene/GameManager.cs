using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameSettingsJson gameSettingsJson;
    public GameSettings gameSettings;
    [SerializeField] private AudioSetting audioSetting;

    // �v���C���[
    public PlayerDataManager playerData;
    public List<CardDataManager> cardDataList { private set; get; } = new List<CardDataManager>();
    public List<RelicDataManager> relicDataList { private set; get; } = new List<RelicDataManager>();
    public Dictionary<int, int> hasRelics { private set; get; } = new Dictionary<int, int>();     // �������Ă��郌���b�N���i�[    
    public int maxCards { get; private set; } = 20;
    public int maxRelics { get; private set; } = 12;
    private const int defaultDeckSize = 4;
    public const int healCardID = 3;           // �����̗���ID
    private const int ariadnesThreadID = 1;     // �A���h�l�̎��̃����b�N��ID(�f�b�L�̏���𑝂₷�����b�N)

    private const int id7HPIncreaseAmount = 5;  //�S�̊��HP������

    public Action OnCardDiscard;      // �J�[�h�̔j�������s�������ɌĂяo�����f���Q�[�g

    private bool isAlreadyRead = false; // ReadPlayer�œǂݍ��񂾂��𔻒肷��

    //�t�B�[���h

    public int floor = 1; //�K�w

    [SerializeField] UIManager uiManager;
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;
    [SerializeField] SceneFader sceneFader;

    public static GameManager Instance;     // �V���O���g��
    protected void Awake()
    {
        gameSettings = gameSettingsJson.loadGameSettingsData();     // �Q�[���ݒ�̃��[�h
        

        // �V���O���g���C���X�^���X���Z�b�g�A�b�v
        if (Instance == null)
        {
            Instance = this;
        }

        audioSetting.InstantiateAudioSetting();
        InitializeItemData();
        
        // �e�V�[���Ńf�o�b�O����Ƃ��ɃR�����g���������Ă�������
        // ��x���ǂݍ���ł��Ȃ����
        //if (!isAlreadyRead) ReadPlayer("Debug");
    }

    private void Update()
    {
        Debug.Log("GameManager:   " + gameSettings.overallVolume);
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
            hasRelics.Add(relicID, 0);

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
        if (playerJob == "Debug")
        {
            playerData = new PlayerDataManager("Debug");
            // �S�����b�N�擾
            hasRelics[1] += 5;
            hasRelics[2] += 1;
            hasRelics[3] += 1;
            hasRelics[4] += 1;
            hasRelics[5] += 1;
            hasRelics[6] += 1;
            hasRelics[7] += 1;
            hasRelics[8] += 1;
            hasRelics[9] += 1;
            hasRelics[10] += 1;
            hasRelics[11] += 1;
            hasRelics[12] += 1;

            ShowRelics();
            return;
        }
        playerData._deckList.Clear(); //�f�b�L���X�g����ɂ���
        //�J�n���ɔz�z�����J�[�h��ǉ�����
        playerData._deckList.Add(1); //�X�C���O
        playerData._deckList.Add(2); //�q�[��
        playerData._deckList.Add(4); //�K�[�h

    }


    #region �J�[�h�֌W
    /// <summary>
    /// �n���ꂽ�J�[�hID�̃J�[�h���擾���A�J�[�h�}�ӂɓo�^���܂��B
    /// </summary>
    /// <param name="cardID"></param>
    public void AddCard(int cardID)
    {
        playerData._deckList.Add(cardID);
        //gameSettings.collectedCardHistory[cardID] = true;
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
        List<int> checkDeck = playerData._deckList.ToList();   // �f�b�L�`�F�b�N�p�Ƀf�b�L���R�s�[

        // �����̗������O
        checkDeck.Remove(healCardID);

        // �������Ă���J�[�h���f�b�L�̃T�C�Y�ȏゾ������
        if (checkDeck.Count >= maxDeckSize)
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
    #endregion


    #region �����b�N�֌W
    /// <summary>
    /// Overlay�ɏ��������b�N��\�����܂��B
    /// </summary>
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
    /// �����b�N����肵���ۂ�ID���m�F���AID7(�S�̊�)�̎��Ɍ��ʂ���������
    /// </summary>
    /// <param name="relicID">���肵�������b�N�̔ԍ�</param>
    public void CheckGetRelicID7(int relicID)
    {
        if (relicID == 7) //�����b�N��ID7(�S�̊�)�̏ꍇ
        {
            playerData._playerHP += id7HPIncreaseAmount; //�S�̊�̑����ʕ�HP���㏸������
        }
    }
    #endregion


    #region �S�V�[���A�����[�h�̏����ƃf�[�^�̏�����
    /// <summary>
    /// �Q�[���f�[�^�̃��Z�b�g�����܂��B
    /// <para>���󃊃U���g�V�[������^�C�g���V�[���ɖ߂�Ƃ��݂̂ɂ����g���Ȃ����ߌ�ŏ��������܂��B</para>
    /// </summary>
    private void ResetGameData()
    {
        Lottery.Instance.shopCards.Clear();
        PlayerController.isPlayerActive = true;
        PlayerController.isEvents = false;
        PlayerController.isSetting = false;
        PlayerController.isConfimDeck = false;

        playerData = null;

        // ���������b�N������
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasRelics[RelicID] = 0; // �L�[�ƒl��ݒ�
        }

        ShowRelics();

        floor = 1; //�K�w��1�ɖ߂�
    }

    /// <summary>
    /// UnloadAllScenes���\�b�h��FadeOutInWrapper���\�b�h�ɓn���Ď��s���܂��B
    /// </summary>
    public void UnloadAllScene()
    {
        sceneFader.FadeOutInWrapper(UnloadAllScenes);
    }

    /// <summary>
    /// ManagerScene�ȊO�̃V�[�����A�����[�h���A�������̊J�����s���A�f�[�^�����Z�b�g���āA
    /// �^�C�g���V�[�������[�h���܂��B
    /// </summary>
    public async Task UnloadAllScenes()
    {
        AsyncOperation asyncOperation;

        // �t�B�[���h�V�[���ƃ}�l�[�W���[�V�[�������O���ăA�����[�h
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "ManagerScene" && scene.name != "FieldScene")
            {
                asyncOperation = SceneManager.UnloadSceneAsync(scene);
                while (!asyncOperation.isDone) await Task.Yield();
            }
        }

        // �Q�Ɖ����̊֌W�Ńt�B�[���h�V�[�����Ō�ɃA�����[�h����B
        Scene fieldScene = SceneManager.GetSceneByName("FieldScene");
        if (fieldScene.isLoaded)
        {
            asyncOperation = SceneManager.UnloadSceneAsync(fieldScene);
            while (!asyncOperation.isDone) await Task.Yield();
        }

        // �^�C�g���V�[�����[�h
        asyncOperation = asyncOperation = SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
        while (!asyncOperation.isDone) await Task.Yield();

        Resources.UnloadUnusedAssets();

        ResetGameData();
    }
    #endregion

}
