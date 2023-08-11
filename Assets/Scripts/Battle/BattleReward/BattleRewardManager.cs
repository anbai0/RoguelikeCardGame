using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRewardManager : MonoBehaviour
{
    [Header("��V��ʗpUI")]
    [SerializeField]
    GameObject battleRewardUI;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField]
    Lottery lottery;
    [SerializeField] 
    SceneFader sceneFader;
    
    [Header("��������J�[�h�I�u�W�F�N�g")]
    [SerializeField]
    GameObject cardPrefab;
    [Header("�������郌���b�N�I�u�W�F�N�g")]
    [SerializeField]
    GameObject relicPrefab;
    [Header("�J�[�h�̐����ꏊ")]
    [SerializeField]
    Transform cardPlace;
    [Header("�����b�N�̐����ꏊ")]
    [SerializeField]
    Transform relicPlace;
    
    CardController cardController;
    RelicController relicController;
    
    List<int> rewardCardID = null; //��V�Ƃ��đI�΂ꂽ�J�[�h��ID
    List<int> rewardRelicID = null; //��V�Ƃ��đI�΂ꂽ�����b�N��ID
    const int RelicID1 = 1; //�A���A�h�l�̎�(���A���e�B�R)

    void Start()
    {
        battleRewardUI.SetActive(false);
    }

    /// <summary>
    /// ��V��ʂ�\�����鏈��
    /// </summary>
    /// <param name="type">�G�l�~�[�̎��</param>
    public void ShowReward(string type)
    {
        battleRewardUI.SetActive(true); 
        SelectRewardByCards(type);
        SelectRewardByRelics(type);
        battleRewardUI.GetComponent<DisplayAnimation>().StartPopUPAnimation();
    }

    /// <summary>
    /// �G�l�~�[�̎�ނɉ����ĕ�V�̃J�[�h�𒊑I�A�\�������鏈��
    /// </summary>
    /// <param name="type">�G�l�~�[�̎��</param>
    void SelectRewardByCards(string type)
    {
        if (type == "Enemy")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        }
        else if (type == "StrongEnemy")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 2, 2, 1 });
        }
        else if (type == "Boss")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 3, 2, 2 });
        }
        ShowCards();
    }

    /// <summary>
    /// �G�l�~�[�̎�ނɉ����ă����b�N�𒊑I�A�\�������鏈��
    /// </summary>
    /// <param name="type">�G�l�~�[�̎��</param>
    public void SelectRewardByRelics(string type)
    {
        if (type == "StrongEnemy")
        {
            rewardRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 1, 1 });
        }
        else if (type == "Boss")
        {
            rewardRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 2 });
            rewardRelicID.Insert(0, RelicID1);
        }
        else
        {
            rewardRelicID = null;
        }
        ShowRelics();
    }

    /// <summary>
    /// ���I���ꂽ�J�[�h�̐����ƕ\�������鏈��
    /// </summary>
    void ShowCards()
    {
        for (int cardCount = 0; cardCount < rewardCardID.Count; cardCount++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPlace);
            cardObj.transform.SetParent(cardPlace);
            cardController = cardObj.GetComponent<CardController>();
            cardController.Init(rewardCardID[cardCount]);
            cardController.cardDataManager._cardState = -1; //�o�g����ʂɂ���J�[�h�Ƌ�ʂ���ׂ�State��-1�ɂ���
        }
    }

    /// <summary>
    /// ���I���ꂽ�����b�N�̐����ƕ\�����鏈��
    /// </summary>
    void ShowRelics()
    {
        if (rewardRelicID != null) //���X�g��ID�������Ă����
        {
            for (int relicCount = 0; relicCount < rewardRelicID.Count; relicCount++)
            {
                GameObject relicObj = Instantiate(relicPrefab, relicPlace);
                relicObj.transform.SetParent(relicPlace);
                relicController = relicObj.GetComponent<RelicController>();
                relicController.Init(rewardRelicID[relicCount]);
            }
        }
    }

    public void UnLoadBattleScene()
    {
        // �o�g���V�[�����A�����[�h
        sceneFader.SceneChange(unLoadSceneName: "BattleScene");
    } 
}
