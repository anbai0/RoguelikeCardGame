using System.Collections;
using UnityEngine;
using SelfMadeNamespace;

public class UIManagerTreasureBox : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isEventsReset = true;
    private bool isClick = false;

    private bool isSelected = false;
    private GameObject lastSelectedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // �J�[�h�����̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 relicScaleReset = Vector3.one * 2.5f;    // �����b�N�����̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    private bool isDisplayRelics = true; //�����b�N�̕�V��\�����邩����

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] private TreasureBoxManager TBManager;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] private GameObject treasureBoxUI;
    [SerializeField] private GameObject treasureCardPlace;
    [SerializeField] private GameObject treasureRelicPlace;
    [SerializeField] private GameObject applyGetTreasure;
    [SerializeField] private GameObject closeGetTreasure;

    GameManager gm;

    void Start()
    {
        UIEventsReload();
        gm = GameManager.Instance;
    }


    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isEventsReset)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                                // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        isEventsReset = false;
    }

    private void RemoveListeners()
    {
        foreach (UIController UI in UIs)
        {
            UI.onLeftClick.RemoveAllListeners();
            UI.onEnter.RemoveAllListeners();
            UI.onExit.RemoveAllListeners();
        }
    }
    #endregion


    void UILeftClick(GameObject UIObject)
    {

        #region TreasureBoxUI���ł̏���

        // �A�C�e�����N���b�N������
        if (UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isSelected = true;

            // ����{�^���؂�ւ�
            applyGetTreasure.SetActive(true);
            closeGetTreasure.SetActive(false);
            //UIEventsReload();

            // �J�[�h�I����Ԃ̐؂�ւ�
            // �A�C�e���I����Ԃ̐؂�ւ�
            if (lastSelectedItem != null && lastSelectedItem != UIObject)    // 2��ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
                if (lastSelectedItem.CompareTag("Cards"))
                {
                    lastSelectedItem.transform.localScale = cardScaleReset;
                    lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                }

                if (lastSelectedItem.CompareTag("Relics"))
                {
                    lastSelectedItem.transform.localScale = relicScaleReset;
                    lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                    lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
                }

                // 2��ڂɑI�������A�C�e�����J�[�h�������ꍇ�A�J�[�h��I����Ԃɂ���
                if (UIObject.CompareTag("Cards"))
                {
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.Find("CardSelectImage").gameObject.SetActive(true);
                }

                // 2��ڂɑI�������A�C�e���������b�N�������ꍇ�A�����b�N��I����Ԃɂ��Đ�����\��
                if (UIObject.CompareTag("Relics"))
                {
                    UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(true);
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);
                }

            }

            lastSelectedItem = UIObject;

        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isSelected && UIObject.CompareTag("BackGround"))
        {
            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
            }
            lastSelectedItem = null;
            isSelected = false;

            // ����{�^���؂�ւ�
            applyGetTreasure.SetActive(false);
            closeGetTreasure.SetActive(true);
            //UIEventsReload();
        }

        // "���肷��"����������
        if (UIObject == applyGetTreasure && isSelected && !isClick)
        {
            isClick = true;
            
            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
            }
            lastSelectedItem = null;
            isSelected = false;

            if (gm.CheckDeckFull()) //�f�b�L�̏���ɒB���Ă���ꍇ
            {
                //�j����ʂ��Ăяo��
                gm.OnCardDiscard += ReGetTreasure;
                // ����{�^���؂�ւ�
                applyGetTreasure.SetActive(false);
                closeGetTreasure.SetActive(true);
                isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
            }
            else
            {
                //if (UIObject.CompareTag("Cards"))
                //{
                //    var cardID = UIObject.GetComponent<CardController>().cardDataManager._cardID;        //�f�b�L���X�g�ɃJ�[�h��ǉ�����
                //    GameManager.Instance.playerData._deckList.Add(cardID);
                //}
                //if (UIObject.CompareTag("Relics"))
                //{
                //    var relicID = UIObject.GetComponent<RelicController>().relicDataManager._relicID;      //�����b�N���X�g�Ƀ����b�N��ǉ�����
                //    GameManager.Instance.hasRelics[relicID] += 1;
                //}

                //�����b�N�̕�V���K�v�Ȃ�
                if (isDisplayRelics)
                {
                    isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
                    StartCoroutine(ShowRelicTreasure());
                    isDisplayRelics = false;
                }
                else
                {
                    //treasureChestUI.SetActive(false);
                    Debug.Log("�t�B�[���h�V�[���ֈڍs");
                    TBManager.UnLoadTreasureBoxScene(); // �t�B�[���h�ɖ߂�
                    PlayerController.isPlayerActive = true; // �v���C���[�𓮂���悤�ɂ���
                    ExitTreasureBox();
                }
            }
            
        }

        // "���肵�Ȃ�"����������
        if (UIObject == closeGetTreasure)
        {
            //�����b�N�̕�V���K�v�Ȃ�
            if (isDisplayRelics)
            {
                isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
                StartCoroutine(ShowRelicTreasure());
                isDisplayRelics = false;
            }
            else
            {
                Debug.Log("�t�B�[���h�V�[���ֈڍs");
                TBManager.UnLoadTreasureBoxScene(); // �t�B�[���h�ɖ߂�
                PlayerController.isPlayerActive = true; // �v���C���[�𓮂���悤�ɂ���
                ExitTreasureBox();
            }
        }

        #endregion
    }

    IEnumerator ShowRelicTreasure()
    {
        // ����{�^���؂�ւ�
        applyGetTreasure.SetActive(false);
        closeGetTreasure.SetActive(true);
        //��V��ʐ؂�ւ�
        treasureRelicPlace.SetActive(true);
        treasureCardPlace.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        //��ʂ̃|�b�v�A�b�v
        treasureBoxUI.SetActive(true);
    }

    /// <summary>
    ///  �f�b�L�j����ʂ��Ă΂ꂽ�ۂɂ�����x�󔠊l����ʂ�\��
    /// </summary>
    void ReGetTreasure()
    {
        //if (UIObject.CompareTag("Cards"))
        //{
        //    var cardID = UIObject.GetComponent<CardController>().cardDataManager._cardID;        //�f�b�L���X�g�ɃJ�[�h��ǉ�����
        //    GameManager.Instance.playerData._deckList.Add(cardID);
        //}
        //if (UIObject.CompareTag("Relics"))
        //{
        //    var relicID = UIObject.GetComponent<RelicController>().relicDataManager._relicID;      //�����b�N���X�g�Ƀ����b�N��ǉ�����
        //    GameManager.Instance.hasRelics[relicID] += 1;
        //}

        //�����b�N�̕�V���K�v�Ȃ�
        if (isDisplayRelics)
        {
            isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
            StartCoroutine(ShowRelicTreasure());
            isDisplayRelics = false;
        }
        else
        {
            //treasureChestUI.SetActive(false);
            Debug.Log("�t�B�[���h�V�[���ֈڍs");
            TBManager.UnLoadTreasureBoxScene(); // �t�B�[���h�ɖ߂�
            PlayerController.isPlayerActive = true; // �v���C���[�𓮂���悤�ɂ���
            ExitTreasureBox();
        }
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.Find("CardSelectImage").gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(true);                  // �A�C�e���̌����ڂ�I����Ԃɂ���
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);                     // �����b�N�̐�����\��
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScaleReset;
                UIObject.transform.Find("CardSelectImage").gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������

            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(false);                 // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);                    // �����b�N�̐������\��
            }
        }
    }

    void ExitTreasureBox()
    {
        PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
        playerController.treasureBox.SetActive(false); //�󔠂�����
        playerController = null;
        gm = null;
    }
}