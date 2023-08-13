using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SelfMadeNamespace;

public class UIManagerBattleReward : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;
    private bool isClick = false;

    PlayerController playerController;

    private bool isSelected = false;
    private GameObject lastSelectedItem;

    private Vector3 cardScaleReset = Vector3.one * 0.37f;    // �J�[�h�����̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 relicScaleReset = Vector3.one * 2.5f;    // �����b�N�����̃X�P�[���ɖ߂��Ƃ��Ɏg���܂�
    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    public bool isDisplayRelics = false; //�����b�N�̕�V��\�����邩����

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] private SceneFader sceneController;
    [SerializeField] private BattleRewardManager battleRewardManager;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] private GameObject battleRewardUI;
    [SerializeField] private GameObject cardRewardPlace;
    [SerializeField] private GameObject relicRewardPlace;
    [SerializeField] private GameObject applyGetItem;
    [SerializeField] private GameObject closeGetItem;


    void Start()
    {
        UIEventsReload();
    }

    private void Update()
    {
        //if (playerController == null)
        //{
        //    playerController = "FieldScene".GetComponentInScene<PlayerController>();
        //}
    }


    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isRemoved)
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       // �w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                                // UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         // UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));
        }

        isRemoved = false;
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

        #region BattleRewardUI���ł̏���

        // �A�C�e�����N���b�N������
        if (UIObject == UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isSelected = true;

            // ����{�^���؂�ւ�
            applyGetItem.SetActive(true);
            closeGetItem.SetActive(false);
            //UIEventsReload();

            // �J�[�h�I����Ԃ̐؂�ւ�
            // �A�C�e���I����Ԃ̐؂�ւ�
            if (lastSelectedItem != null && lastSelectedItem != UIObject)    // 2��ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
                if (lastSelectedItem == lastSelectedItem.CompareTag("Cards"))
                {
                    lastSelectedItem.transform.localScale = cardScaleReset;
                    lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                }

                if (lastSelectedItem == lastSelectedItem.CompareTag("Relics"))
                {
                    lastSelectedItem.transform.localScale = relicScaleReset;
                    lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
                    lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
                }

                // 2��ڂɑI�������A�C�e����I����Ԃɂ���
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.Find("CardSelectImage").gameObject.SetActive(true);

                // 2��ڂɑI�������A�C�e���������b�N�������ꍇ�A�����b�N�̐�����\��
                if (UIObject == UIObject.CompareTag("Relics"))
                    UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(true);

            }

            lastSelectedItem = UIObject;

        }

        // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
        if (isSelected && UIObject == UIObject.CompareTag("BackGround"))
        {
            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastSelectedItem == lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem == lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
            }
            lastSelectedItem = null;
            isSelected = false;

            // ����{�^���؂�ւ�
            applyGetItem.SetActive(false);
            closeGetItem.SetActive(true);
            //UIEventsReload();
        }

        // "���肷��"����������
        if (UIObject == applyGetItem && isSelected && !isClick)
        {
            isClick = true;
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

            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastSelectedItem == lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.Find("CardSelectImage").gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem == lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.Find("RelicSelectImage").gameObject.SetActive(false);
                lastSelectedItem.transform.Find("RelicEffectBG").gameObject.SetActive(false);       // �����b�N�̐������\��
            }
            lastSelectedItem = null;
            isSelected = false;

            //�����b�N�̕�V���K�v�Ȃ�
            if (isDisplayRelics)
            {
                battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //��V��ʂ����
                isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
                StartCoroutine(ShowRelicReward());
                isDisplayRelics = false;
            }
            else
            {
                Debug.Log("�t�B�[���h�V�[���ֈڍs");
                battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //��V��ʂ����
                //battleRewardManager.UnLoadBattleScene();      // �t�B�[���h�ɖ߂�
                //PlayerController.isPlayerActive = true;       // �v���C���[�𓮂���悤�ɂ���
            }
        }

        // "���肵�Ȃ�"����������
        if (UIObject == closeGetItem)
        {
            Debug.Log("�t�B�[���h�V�[���ֈڍs");
            battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //��V��ʂ����
            //battleRewardManager.UnLoadBattleScene();      // �t�B�[���h�ɖ߂�
            //PlayerController.isPlayerActive = true;       // �v���C���[�𓮂���悤�ɂ���
        }

        #endregion
    }

    IEnumerator ShowRelicReward()
    {
        // ����{�^���؂�ւ�
        applyGetItem.SetActive(false);
        closeGetItem.SetActive(true);
        //��V��ʐ؂�ւ�
        relicRewardPlace.SetActive(true);
        cardRewardPlace.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        //��ʂ̃|�b�v�A�b�v
        battleRewardUI.GetComponent<DisplayAnimation>().StartPopUPAnimation();
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                if(UIObject.GetComponent<CardController>().cardDataManager._cardState == -1)             //��V�p�̃J�[�h��������
                {
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.Find("CardSelectImage").gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
                }
            }

            if (UIObject == UIObject.CompareTag("Relics"))
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
            if (UIObject == UIObject.CompareTag("Cards"))
            {
                if (UIObject.GetComponent<CardController>().cardDataManager._cardState == -1)           //��V�p�̃J�[�h��������
                {
                    UIObject.transform.localScale = cardScaleReset;
                    UIObject.transform.Find("CardSelectImage").gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������
                } 
                    
            }

            if (UIObject == UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.Find("RelicSelectImage").gameObject.SetActive(false);                 // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.Find("RelicEffectBG").gameObject.SetActive(false);                    // �����b�N�̐������\��
            }
        }
    }
}