using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ResultSceneManager : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] SceneFader sceneFader;
    [SerializeField] UIManagerResult uiManager;
    
    // �J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    private List<int> deckNumberList;                    //�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    // �����b�N
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    private Vector3 CardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��


    void Start()
    {
        // GameManager�擾(�ϐ����ȗ�)
        gm = GameManager.Instance;

        InitDeck();
        ShowRelics();
        uiManager.UIEventsReload();
    }

    public void InitDeck() //�f�b�L����
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
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init - 1).ToString();//���������J�[�h�ɖ��O��t����
                card.Init(deckNumberList[init - 1]);//�f�b�L�f�[�^�̕\��
            }
            else //�c���lowerCardPlace�ɐ�������
            {
                CardController card = Instantiate(cardPrefab, lowerCardPlace);//�J�[�h�𐶐�����
                card.transform.localScale = CardScale;
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


    public void ShowRelics()
    {
        // relicPlace�̎q�I�u�W�F�N�g�����ׂ�Destroy
        Transform[] children = relicPlace.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        for (int RelicID = 1; RelicID <= gm.maxRelics; RelicID++)
        {
            if (gm.hasRelics.ContainsKey(RelicID) && gm.hasRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(relicPrefab, relicPlace);
                //relic.transform.localScale = Vector3.one * 0.9f;                   // ��������Prefab�̑傫������
                relic.Init(RelicID);                                               // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = gm.hasRelics[RelicID].ToString();      // Prefab�̎q�I�u�W�F�N�g�ł��鏊������\������e�L�X�g��ύX

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicName.ToString();        // �����b�N�̖��O��ύX
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicEffect.ToString();      // �����b�N�����ύX
            }
        }

        uiManager.UIEventsReload();
    }


    public void SceneUnLoad()
    {
        gm.ResetGameData();     // GameManager�̃f�[�^�����Z�b�g

        gm = null;      // �Q�Ɖ���

        // Result�V�[�����A�����[�h���A�^�C�g���V�[�������[�h
        sceneFader.SceneChange("TitleScene", "ResultScene");
    }
}
