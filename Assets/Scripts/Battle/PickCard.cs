using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class PickCard : MonoBehaviour
{
    string thisName = null; //���̃I�u�W�F�N�g�̖��O
    GameObject deckCard = null; //���̃J�[�h�ɑΉ�����f�b�L�̃J�[�h
    CardController cardController;
    // Start is called before the first frame update
    void Start()
    {
        thisName = gameObject.name;
        cardController = gameObject.GetComponent<CardController>();
        string pattern = @"\d+";// ���p�����𒊏o���邽�߂̐��K�\���p�^�[��
        Match match = Regex.Match(thisName, pattern);// ���K�\���p�^�[���Ɉ�v���锼�p����������
        string thisCardNum = match.Value;
        deckCard = GameObject.Find("Deck" + thisCardNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject ChoosePickCard(GameObject card)
    {
        string pickName = card.name;
        string pattern = @"\d+";// ���p�����𒊏o���邽�߂̐��K�\���p�^�[��
        Match match = Regex.Match(pickName, pattern);// ���K�\���p�^�[���Ɉ�v���锼�p����������
        string pickCardNum = match.Value;
        Debug.Log("pickCardNum number is: " + pickCardNum);
        GameObject pickCard = GameObject.Find("Pick" + pickCardNum);
        return pickCard;
    }
    public GameObject SetPickCardStatus(GameObject deckCard, GameObject pickCard)
    {
        //�s�b�N���ꂽ�J�[�h�͎Q�Ƃ���f�b�L�̃J�[�h��dataManager�𔽉f
        var deckData = deckCard.GetComponent<CardController>().cardDataManager;
        pickCard.GetComponent<CardViewManager>().ViewCard(deckData);
        //�s�b�N���ꂽ�J�[�h�͎Q�Ƃ���f�b�L�̃J�[�h�̌���Text�𔽉f
        var deckEffect = deckCard.transform.Find("CardInfo/CardEffect").GetComponent<TextMeshProUGUI>().text;
        pickCard.transform.Find("CardInfo/CardEffect").GetComponent<TextMeshProUGUI>().text = deckEffect;
        //�s�b�N���ꂽ�J�[�h�͎Q�Ƃ���f�b�L�̃J�[�h��cardState�𔽉f
        pickCard.GetComponent<CardController>().cardDataManager._cardState = deckData._cardState;
        Debug.Log("pickCard name is: " + pickCard);
        //�J�[�h����\������
        pickCard.transform.GetChild(0).gameObject.SetActive(true);
        pickCard.GetComponent<CardState>().isActive = true;
        return pickCard;
    }
}
