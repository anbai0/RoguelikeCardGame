using System.Collections;
using UnityEngine;

/// <summary>
/// BattleReward��ʂ�UIManager�ł��B
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManagerBattleReward : MonoBehaviour
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

    public bool isDisplayRelics = false; //�����b�N�̕�V��\�����邩����

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private BattleRewardManager battleRewardManager;

    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] private GameObject battleRewardUI;
    [SerializeField] private GameObject cardRewardPlace;
    [SerializeField] private GameObject relicRewardPlace;
    [SerializeField] private GameObject applyGetItem;
    [SerializeField] private GameObject closeGetItem;

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

        #region BattleRewardUI���ł̏���

        // �A�C�e�����N���b�N������
        if (UIObject.CompareTag("Cards") || UIObject.CompareTag("Relics"))
        {
            isSelected = true;
            AudioManager.Instance.PlaySE("�I����1");

            // ����{�^���؂�ւ�
            applyGetItem.SetActive(true);
            closeGetItem.SetActive(false);

            // �J�[�h�I����Ԃ̐؂�ւ�
            // �A�C�e���I����Ԃ̐؂�ւ�
            if (lastSelectedItem != null && lastSelectedItem != UIObject)    // 2��ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
            {
                // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
                if (lastSelectedItem.CompareTag("Cards"))
                {
                    lastSelectedItem.transform.localScale = cardScaleReset;
                    lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
                }

                if (lastSelectedItem.CompareTag("Relics"))
                {
                    lastSelectedItem.transform.localScale = relicScaleReset;
                    lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
                    lastSelectedItem.transform.GetChild(8).gameObject.SetActive(false);       // RelicEffectBG(BattleReward)���\��
                }

                // 2��ڂɑI�������A�C�e�����J�[�h�������ꍇ�A�J�[�h��I����Ԃɂ���
                if (UIObject.CompareTag("Cards"))
                {
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.GetChild(0).gameObject.SetActive(true);
                }

                // 2��ڂɑI�������A�C�e���������b�N�������ꍇ�A�����b�N��I����Ԃɂ��Đ�����\��
                if (UIObject.CompareTag("Relics"))
                {
                    UIObject.transform.GetChild(0).gameObject.SetActive(true);
                    UIObject.transform.GetChild(8).gameObject.SetActive(true);         // RelicEffectBG(BattleReward)��\��
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
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
                lastSelectedItem.transform.GetChild(8).gameObject.SetActive(false);       // RelicEffectBG(BattleReward)���\��
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
            AudioManager.Instance.PlaySE("�I����1");

            // �Ō�ɃN���b�N�����A�C�e���̑I����Ԃ���������
            if (lastSelectedItem.CompareTag("Cards"))
            {
                lastSelectedItem.transform.localScale = cardScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);       // �A�C�e���̌����ڂ̑I����Ԃ���������
            }

            if (lastSelectedItem.CompareTag("Relics"))
            {
                lastSelectedItem.transform.localScale = relicScaleReset;
                lastSelectedItem.transform.GetChild(0).gameObject.SetActive(false);
                lastSelectedItem.transform.GetChild(8).gameObject.SetActive(false);       // RelicEffectBG(BattleReward)���\��
            }

            if (lastSelectedItem.CompareTag("Cards"))
            {
                if (gm.CheckDeckFull()) //�f�b�L�̏���ɒB���Ă���ꍇ
                {
                    //�j����ʂ��Ăяo��
                    gm.OnCardDiscard += ReGetReward;
                    // ����{�^���؂�ւ�
                    applyGetItem.SetActive(false);
                    closeGetItem.SetActive(true);
                    isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
                    return;
                }
                else
                {
                    var cardID = lastSelectedItem.GetComponent<CardController>().cardDataManager._cardID;        //�f�b�L���X�g�ɃJ�[�h��ǉ�����
                    gm.AddCard(cardID);
                }
            }
            if (lastSelectedItem.CompareTag("Relics"))
            {
                var relicID = lastSelectedItem.GetComponent<RelicController>().relicDataManager._relicID;      //�����b�N���X�g�Ƀ����b�N��ǉ�����
                gm.hasRelics[relicID]++;
                gm.ShowRelics();
                gm.CheckGetRelicID7();
            }

            lastSelectedItem = null;
            isSelected = false;

            //�����b�N�擾��ʂɈړ�
            if (isDisplayRelics)
            {
                battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //��V��ʂ����
                isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
                StartCoroutine(ShowRelicReward());
                isDisplayRelics = false;
            }
            else
            {
                battleRewardManager.UnLoadBattleScene();      // �t�B�[���h�ɖ߂�
            }
        }

        // "���肵�Ȃ�"����������
        if (UIObject == closeGetItem)
        {
            AudioManager.Instance.PlaySE("�I����1");

            //�����b�N�擾��ʂɈړ�
            if (isDisplayRelics)
            {
                battleRewardUI.GetComponent<DisplayAnimation>().StartDisappearAnimation(); //��V��ʂ����
                isClick = false; //applyGetItem��������x�N���b�N�o����悤�ɂ���
                StartCoroutine(ShowRelicReward());
                isDisplayRelics = false;
            }
            else
            {
                battleRewardManager.UnLoadBattleScene();      // �t�B�[���h�ɖ߂�
            }
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

    /// <summary>
    /// �f�b�L�j����ʂ��Ă΂ꂽ�ۂɂ�����x��V��ʂ�\��
    /// </summary>
    void ReGetReward()
    {
        if (lastSelectedItem.CompareTag("Cards"))
        {
            var cardID = lastSelectedItem.GetComponent<CardController>().cardDataManager._cardID;        //�f�b�L���X�g�ɃJ�[�h��ǉ�����
            gm.AddCard(cardID);  
        }

        if (lastSelectedItem.CompareTag("Relics"))
        {
            var relicID = lastSelectedItem.GetComponent<RelicController>().relicDataManager._relicID;      //�����b�N���X�g�Ƀ����b�N��ǉ�����
            gm.hasRelics[relicID]++;
            gm.ShowRelics();
            gm.CheckGetRelicID7();
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
            battleRewardManager.UnLoadBattleScene();      // �t�B�[���h�ɖ߂�
        }
    }

    void UIEnter(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                if(UIObject.GetComponent<CardController>().cardDataManager._cardState == -1)             //��V�p�̃J�[�h��������
                {
                    UIObject.transform.localScale += scaleBoost;
                    UIObject.transform.GetChild(0).gameObject.SetActive(true);              // �A�C�e���̌����ڂ�I����Ԃɂ���
                }
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale += scaleBoost;
                UIObject.transform.GetChild(0).gameObject.SetActive(true);                  // �A�C�e���̌����ڂ�I����Ԃɂ���
                UIObject.transform.GetChild(8).gameObject.SetActive(true);                  // RelicEffectBG(BattleReward)��\��
            }
        }
    }

    void UIExit(GameObject UIObject)
    {
        if (!isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                if (UIObject.GetComponent<CardController>().cardDataManager._cardState == -1)           //��V�p�̃J�[�h��������
                {
                    UIObject.transform.localScale = cardScaleReset;
                    UIObject.transform.GetChild(0).gameObject.SetActive(false);             // �A�C�e���̌����ڂ̑I����Ԃ���������
                } 
                    
            }

            if (UIObject.CompareTag("Relics"))
            {
                UIObject.transform.localScale = relicScaleReset;
                UIObject.transform.GetChild(0).gameObject.SetActive(false);                 // �A�C�e���̌����ڂ̑I����Ԃ���������
                UIObject.transform.GetChild(8).gameObject.SetActive(false);                 // RelicEffectBG(BattleReward)���\��
            }
        }
    }
}
