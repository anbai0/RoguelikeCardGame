using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxManager : MonoBehaviour
{
    [Header("�󔠊l����ʗpUI")]
    [SerializeField] GameObject treasureBoxUI;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField] Lottery lottery;
    [SerializeField] SceneFader sceneFader;
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

    List<int> treasureCardID = null; //�󔠂���o�Ă���J�[�h��ID
    List<int> treasureRelicID = null; //�󔠂���o�Ă��郌���b�N��ID

    void Start()
    {
        Invoke("ShowTreasure", 0.05f); //�ǂݍ��݂��x��Ă���̂ŌĂяo����x�点��
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
        treasureCardID = lottery.SelectCardByRarity(new List<int> { 2, 2, 1 });
        treasureRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 1, 1 });
    }

    /// <summary>
    /// ���I���ꂽ�J�[�h�𐶐�
    /// </summary>
    void ShowCards()
    {
        for (int cardCount = 0; cardCount < treasureCardID.Count; cardCount++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPlace);
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
        }
    }

    /// <summary>
    /// �󔠃V�[�����A�����[�h
    /// </summary>
    public void UnLoadTreasureBoxScene()
    {
        lottery.gm = null;     // �Q�Ɖ���

        sceneFader.SceneChange(unLoadSceneName: "TreasureBoxScene");
    }
}