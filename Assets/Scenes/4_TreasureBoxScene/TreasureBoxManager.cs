using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureBoxManager : MonoBehaviour
{
    [Header("�󔠊l����ʗpUI")]
    [SerializeField] GameObject treasureBoxUI;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] UIManagerTreasureBox uiManagerTB;

    [Header("��������J�[�h�I�u�W�F�N�g")]
    [SerializeField] GameObject cardPrefab;
    [Header("�������郌���b�N�I�u�W�F�N�g")]
    [SerializeField] GameObject relicPrefab;
    [Header("�J�[�h�̐����ꏊ")]
    [SerializeField] Transform cardPlace;
    [Header("�����b�N�̐����ꏊ")]
    [SerializeField] Transform relicPlace;

    CardController cardController;
    RelicController relicController;

    [SerializeField] List<int> treasureCardID = null; //�󔠂���o�Ă���J�[�h��ID
    List<int> treasureRelicID = null; //�󔠂���o�Ă��郌���b�N��ID

    void Start()
    {
        Invoke("ShowTreasure", 0.05f); //�ǂݍ��݂��x��Ă���̂ŌĂяo����x�点��
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {

            treasureCardID = null;
            treasureCardID = Lottery.Instance.SelectCardByRarity(new List<int> { 2, 2, 1 });
            Debug.Log(treasureCardID);
        }
    }

    /// <summary>
    /// �󔠊l����ʂ̕\��
    /// </summary>
    void ShowTreasure()
    {
        TreasureLottery();
        ShowCards();
        ShowRelics();
        uiManagerTB.UIEventsReload();
        relicPlace.gameObject.SetActive(false);
    }

    /// <summary>
    /// �󔠂���l���ł���A�C�e���𒊑I
    /// </summary>
    void TreasureLottery()
    {
        treasureCardID = Lottery.Instance.SelectCardByRarity(new List<int> { 2, 2, 1 });
        treasureRelicID = Lottery.Instance.SelectRelicByRarity(new List<int> { 2, 1, 1 });
    }

    /// <summary>
    /// ���I���ꂽ�J�[�h�𐶐�
    /// </summary>
    void ShowCards()
    {
        for (int cardCount = 0; cardCount < treasureCardID.Count; cardCount++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPlace);
            cardObj.transform.localScale = Vector3.one * 0.25f;
            cardObj.transform.SetParent(cardPlace);
            cardController = cardObj.GetComponent<CardController>();
            cardController.Init(treasureCardID[cardCount]);
        }
    }

    /// <summary>
    /// ���I���ꂽ�����b�N�𐶐�
    /// </summary>
    void ShowRelics()
    {
        for (int relicCount = 0; relicCount < treasureRelicID.Count; relicCount++)
        {
            GameObject relicObj = Instantiate(relicPrefab, relicPlace);
            relicObj.transform.SetParent(relicPlace);
            relicController = relicObj.GetComponent<RelicController>();
            relicController.Init(treasureRelicID[relicCount]);
            Transform relicBG = relicObj.transform.GetChild(8); //�\������BackGround���擾
            TextMeshProUGUI relicName = relicBG.GetChild(0).GetComponent<TextMeshProUGUI>(); //�����b�N�̖��O
            relicName.text = relicController.relicDataManager._relicName;
            TextMeshProUGUI relicEffect = relicBG.GetChild(1).GetComponent<TextMeshProUGUI>(); //�����b�N�̌���
            relicEffect.text = relicController.relicDataManager._relicEffect;
        }
    }

    /// <summary>
    /// �󔠃V�[�����A�����[�h
    /// </summary>
    public void UnLoadTreasureBoxScene()
    {
        SceneFader.Instance.SceneChange(unLoadSceneName: "TreasureBoxScene", allowPlayerMove: true);
    }
}
