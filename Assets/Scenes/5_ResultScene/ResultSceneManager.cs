using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class ResultSceneManager : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] UIManagerResult uiManager;

    [Header("�J�[�h�\���֌W")]
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform deckPlace;
    [SerializeField] GameObject scrollView;     // �f�b�L��\������UI�̐e�I�u�W�F�N�g
    private List<int> deckNumberList;                    //�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    // �����b�N
    [SerializeField] RelicController relicPrefab;
    [SerializeField] Transform relicPlace;

    private Vector3 cardScale = Vector3.one * 0.25f;     // ��������J�[�h�̃X�P�[��

    // ���U���g�̔w�i
    [SerializeField] GameObject clearBG;
    [SerializeField] GameObject gameOverBG;
    // �N���A�A�Q�[���I�[�o�[�̃e�L�X�g
    [SerializeField] TextMeshProUGUI clearText;
    [SerializeField] TextMeshProUGUI gameOverText;
    TextMeshProUGUI viewText;

    void Start()
    {
        // GameManager�擾(�ϐ����ȗ�)
        gm = GameManager.Instance;
        AudioManager.Instance.PlayBGM("Result");

        // ���U���g�̔w�i��\��
        if (gm.isClear == true)
        {
            clearBG.SetActive(true);
            viewText = clearText;
        }           
        else
        {
            gameOverBG.SetActive(true);
            viewText = gameOverText;
        }

        StartCoroutine(TextAnimCoroutine());

        InitDeck();
        ShowRelics();
        uiManager.UIEventsReload();
    }


    IEnumerator TextAnimCoroutine()
    {

        DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(viewText);

        for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
        {
            tmproAnimator.DOScaleChar(i, 0.7f, 0);
            Vector3 currCharOffset = tmproAnimator.GetCharOffset(i);
            DOTween.Sequence()
                .Append(tmproAnimator.DOOffsetChar(i, currCharOffset + new Vector3(0, 30, 0), 0.4f).SetEase(Ease.OutFlash, 2))
                .Join(tmproAnimator.DOScaleChar(i, 1, 0.4f).SetEase(Ease.OutBack))
                .SetDelay(0.07f * i);

            // �Ō�̕����̃A�j���[�V����������������ҋ@
            if (i == tmproAnimator.textInfo.characterCount - 1)
            {
                yield return new WaitForSeconds(0.07f * tmproAnimator.textInfo.characterCount + 1f);
            }
        }

        // �A�j���[�V����������������ēx���s
        StartCoroutine(TextAnimCoroutine());
    }


    private void InitDeck() //�f�b�L����
    {
        deckNumberList = GameManager.Instance.playerData._deckList;

        for (int init = 0; init < deckNumberList.Count; init++)         // �I���o����f�b�L�̖�����
        {
            CardController card = Instantiate(cardPrefab, deckPlace);   //�J�[�h�𐶐�����
            card.transform.localScale = cardScale;
            card.name = "Deck" + (init).ToString();                     //���������J�[�h�ɖ��O��t����
            card.Init(deckNumberList[init]);                            //�f�b�L�f�[�^�̕\��
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
        gm.UnloadAllScene();     // GameManager�̃f�[�^�����Z�b�g
    }
}
