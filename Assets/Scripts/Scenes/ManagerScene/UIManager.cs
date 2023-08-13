using UnityEngine;
using TMPro;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;

/// <summary>
/// UI�̊Ǘ����s���X�N���v�g�ł��B
/// UIController���ŋN��������ɑ΂��ď������s���܂��B
/// </summary>
public class UIManager : MonoBehaviour
{
    // UIManager�ɍŏ������`���Ă���ϐ�
    [SerializeField] private GameObject canvas;
    private UIController[] UIs;
    private bool isRemoved = true;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameManager gm;
    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] GameObject cardDiscardScreen;
    [Header("�N���b�N��ɎQ�Ƃ���UI")]
    [SerializeField] GameObject overlayOptionButton;
    [SerializeField] GameObject titleOptionButton;
    [SerializeField] GameObject closeOptionButton;
    [SerializeField] GameObject titleBackButton;
    [SerializeField] GameObject closeConfirmButton;
    [SerializeField] GameObject confirmTitleBackButton;
    [SerializeField] GameObject returnButton;
    [SerializeField] GameObject discardButton;
    [Space(10)]
    [SerializeField] TextMeshProUGUI myMoneyText;   //��������\������e�L�X�g


    private bool isTitleScreen = false; // �^�C�g����ʂɂ���Ƃ���true�ɂ���
    public static bool isShowingCardDiscard { get; private set; } = false;  // �J�[�h�j����ʂ�\�����Ă���Ƃ�����true
    //public static bool cancelDiscard { get; private set; } = false;         // �J�[�h�j����ʂŖ߂�{�^�����������Ƃ���true
    //public static bool discardButtonClicked { get; private set; } = false;  // �J�[�h�j����ʂŔj���{�^�����������Ƃ���true

    private bool isSelected = false;
    private GameObject lastSelectedCards;

    private Vector3 scaleBoost = Vector3.one * 0.05f;     // ���̃X�P�[���ɏ�Z���Ďg���܂�

    private int maxRelics = 12;

    [Header("�J�[�h�\���֌W")]
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    private Vector3 cardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��
    private List<int> deckNumberList;                    // �v���C���[�̂��f�b�L�i���o�[�̃��X�g

    void Start()
    {      
        UIEventsReload();
    }

    void Update()
    {
        RefreshMoneyText();
    }

    #region UI�C�x���g���X�i�[�֌W�̏���
    /// <summary>
    /// <para> UI�C�x���g���X�i�[�̓o�^�A�ēo�^���s���܂��B</para>
    /// <para>�C�x���g�̓o�^���s������ɁA�V������������Prefab�ɑ΂��ď������s�������ꍇ�́A�ēx���̃��\�b�h���Ă�ł��������B</para>
    /// </summary>
    public void UIEventsReload()
    {
        if (!isRemoved)             // �C�x���g�̏�����
            RemoveListeners();

        UIs = canvas.GetComponentsInChildren<UIController>(true);       //�w�肵���e�̎q�I�u�W�F�N�g��UIController�R���|�[�l���g�����ׂĎ擾
        foreach (UIController UI in UIs)                            //UIs�z����̊e�v�f��UIController�^�̕ϐ�UI�ɏ��Ԃɑ�����ꏈ�������
        {
            UI.onLeftClick.AddListener(() => UILeftClick(UI.gameObject));         //UI���N���b�N���ꂽ��A�N���b�N���ꂽUI���֐��ɓn��
            UI.onEnter.AddListener(() => UIEnter(UI.gameObject));
            UI.onExit.AddListener(() => UIExit(UI.gameObject));

        }

        isRemoved = false;
    }

    /// <summary>
    /// UI�C�x���g���폜���܂��B
    /// UIEventsReload���\�b�h���ŌĂ΂�܂��B
    /// </summary>
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

    /// <summary>
    /// ���N���b�N���ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�N���b�N���ꂽObject</param>
    void UILeftClick(GameObject UIObject)
    {
        #region Overlay�̏���

        // �I�v�V������ʕ\��
        if (UIObject == overlayOptionButton || UIObject == titleOptionButton)
        {
            Debug.Log(UIObject);
            optionScreen.SetActive(true);
        }

        // �I�v�V������ʔ�\��
        if (UIObject == closeOptionButton)
        {
            Debug.Log(UIObject);
            optionScreen.SetActive(false);
        }

        // �^�C�g���֖߂�̊m�F��ʕ\��
        if (UIObject == titleBackButton)
        {
            confirmationPanel.SetActive(true);
        }

        // �^�C�g���֖߂�̊m�F��ʔ�\��
        if (UIObject == closeConfirmButton)
        {
            confirmationPanel.SetActive(false);
        }

        if (UIObject == confirmTitleBackButton)
        {
            // �^�C�g���֖߂鏈��

        }

        #endregion


        #region �J�[�h�j����ʂ̏���

        // �J�[�h�j����ʂ��\������Ă���Ƃ�
        if (isShowingCardDiscard)
        {
            // �J�[�h���N���b�N������
            if (UIObject.CompareTag("Cards"))
            {
                isSelected = true;

                // �{�^���؂�ւ�
                returnButton.SetActive(false);
                discardButton.SetActive(true);

                // �J�[�h�I����Ԃ̐؂�ւ�
                if (lastSelectedCards != null && lastSelectedCards != UIObject)              // ���ڂ̃N���b�N���N���b�N�����I�u�W�F�N�g���Ⴄ�ꍇ   
                {
                    lastSelectedCards.transform.localScale = cardScale;
                    UIObject.transform.localScale += scaleBoost;
                }

                lastSelectedCards = UIObject;

            }

            // �J�[�h���N���b�N������A�w�i���N���b�N����ƃJ�[�h�̃N���b�N��Ԃ�����
            if (isSelected && UIObject.CompareTag("BackGround"))
            {
                lastSelectedCards.transform.localScale = cardScale;
                lastSelectedCards = null;
                isSelected = false;

                // �����{�^���؂�ւ�
                returnButton.SetActive(true);
                discardButton.SetActive(false);

            }

            // �J�[�h�j����ʂ��\����
            if(UIObject == returnButton)
            {
                ToggleDiscardScreen(false);
            }

            // �J�[�h��I�񂾌�A�j���{�^���������ƁA���̃J�[�h��j��
            if (UIObject == discardButton && lastSelectedCards != null)
            {
                discardButtonClicked = true;

                int selectedCardID = lastSelectedCards.GetComponent<CardController>().cardDataManager._cardID; // �I�����ꂽ�J�[�h��ID���擾

                for (int cardIndex = 0; cardIndex < gm.playerData._deckList.Count; cardIndex++) {

                    if (gm.playerData._deckList[cardIndex] == selectedCardID)
                    {
                        // �J�[�h��j��
                        gm.playerData._deckList.RemoveAt(cardIndex);
                    }
                }
                ToggleDiscardScreen(false);
            }
        
        }

        #endregion
    }


    /// <summary>
    /// �J�[�\�����G�ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�����G�ꂽObject</param>
    void UIEnter(GameObject UIObject)
    {
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(true);
        }


        // �J�[�h�j����ʂ��\������Ă���Ƃ�
        if (isShowingCardDiscard && !isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale += scaleBoost;
            }
        }
    }


    /// <summary>
    /// �J�[�\�������ꂽ�Ƃ��ɏ������郁�\�b�h�ł��B
    /// </summary>
    /// <param name="UIObject">�J�[�\�������ꂽObject</param>
    void UIExit(GameObject UIObject)
    {
        if (UIObject.CompareTag("Relics"))
        {
            UIObject.transform.GetChild(5).gameObject.SetActive(false);
        }


        // �J�[�h�j����ʂ��\������Ă���Ƃ�
        if (isShowingCardDiscard && !isSelected)
        {
            if (UIObject.CompareTag("Cards"))
            {
                UIObject.transform.localScale = cardScale;
            }
        }
    }
    

    /// <summary>
    /// UI��؂�ւ��郁�\�b�h�ł��B
    /// �����́A�^�C�g����ʂł���΁A"Title"�A
    /// �L�����I����ʂł����"Chara"�A
    /// Overlay�����\���������ꍇ�́A"OverlayOnly"�ɂ��Ă��������B
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUI(string type)
    {
        if (type == "Title")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(true);
            titleBackButton.SetActive(false);
        }

        if (type == "None")
        {
            overlay.SetActive(false);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(false);
        }

        if (type == "OverlayOnly")
        {
            overlay.SetActive(true);
            titleOptionButton.SetActive(false);
            titleBackButton.SetActive(true);
        }
    }


    /// <summary>
    /// �������̃e�L�X�g���X�V���郁�\�b�h�ł��B
    /// </summary>
    void RefreshMoneyText()
    {
        if (gm.playerData != null)
            myMoneyText.text = gm.playerData._playerMoney.ToString();
    }

    /// <summary>
    /// �J�[�h�j����ʂ�\���܂��͔�\���ɂ��郁�\�b�h
    /// </summary>
    /// <param name="show">�\������ꍇ��true�A��\���ɂ���ꍇ��false</param>
    public void ToggleDiscardScreen(bool show)
    {
        isShowingCardDiscard = true;
        if (show)
        {
            // �{�^���̏�ԃ��Z�b�g
            returnButton.SetActive(true);
            discardButton.SetActive(false);

            // �O��\�������J�[�h��Destroy
            foreach (Transform child in upperCardPlace.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in lowerCardPlace.transform)
            {
                Destroy(child.gameObject);
            }

            cardDiscardScreen.SetActive(true);
            ShowDeck();
            UIEventsReload();
        }
        else
        {
            isShowingCardDiscard = false;
            cardDiscardScreen.SetActive(false);
        }
    }

    /// <summary>
    /// �����Ă���J�[�h�����ׂĕ\��
    /// </summary>
    private void ShowDeck()
    {
        deckNumberList = gm.playerData._deckList;
        int distribute = DistributionOfCards(deckNumberList.Count); 
        if (distribute <= 0) //�f�b�L�̖�����0���Ȃ琶�����Ȃ�
            return;
        for (int init = 1; init <= deckNumberList.Count; init++)// �f�b�L�̖�����
        {
            if (init <= distribute) //���߂�ꂽ����upperCardPlace�ɐ�������
            {
                CardController card = Instantiate(cardPrefab, upperCardPlace);//�J�[�h�𐶐�����
                card.transform.localScale = cardScale;
                card.name = "Deck" + (init - 1).ToString();//���������J�[�h�ɖ��O��t����
                card.Init(deckNumberList[init - 1]);//�f�b�L�f�[�^�̕\��
            }
            else //�c���lowerCardPlace�ɐ�������
            {
                CardController card = Instantiate(cardPrefab, lowerCardPlace);//�J�[�h�𐶐�����
                card.transform.localScale = cardScale;
                card.name = "Deck" + (init - 1).ToString();//���������J�[�h�ɖ��O��t����
                card.Init(deckNumberList[init - 1]);//�f�b�L�f�[�^�̕\��
            }
        }
    }

    /// <summary>
    /// �f�b�L�̃J�[�h�����ɂ���ď㉺��CardPlace�ɐU�蕪���鐔�����߂�
    /// </summary>
    /// <param name="deckCount">�f�b�L�̖���</param>
    /// <returns>���CardPlace�ɐ�������J�[�h�̖���</returns>
    int DistributionOfCards(int deckCount)
    {
        int distribute = 0;
        if (0 <= deckCount && deckCount <= 5)//�f�b�L�̐���0�ȏ�5���ȉ��������� 
        {
            distribute = deckCount;//�f�b�L�̖���������
        }
        else if (deckCount > 5)//�f�b�L�̐���6���ȏゾ������
        {
            if (deckCount % 2 == 0)//�f�b�L�̖�����������������
            {
                int value = deckCount / 2;
                distribute = value;//�f�b�L�̔����̖����𐶐�
            }
            else //�f�b�L�̖��������������
            {
                int value = (deckCount - 1) / 2;
                distribute = value + 1;//�f�b�L�̔���+1�̖����𐶐�
            }
        }
        else //�f�b�L�̐���0��������������
        {
            distribute = 0;//�������Ȃ�
        }
        return distribute;
    }
}