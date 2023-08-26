using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// �f�b�L�̃J�[�h�ɃJ�[�\�������킹���ۂɊg��\������s�b�N�J�[�h�̃X�N���v�g
/// </summary>
public class PickCard : MonoBehaviour
{
    /// <summary>
    /// �f�b�L�̏��ԂƓ������Ԃɕ���ł���s�b�N�J�[�h��I������
    /// </summary>
    /// <param name="card">�f�b�L�̃J�[�h</param>
    /// <returns>�f�b�L�̃J�[�h�ɑΉ�����s�b�N�J�[�h</returns>
    public GameObject ChoosePickCard(GameObject card)
    {
        string pickName = card.name;
        string pattern = @"\d+";// ���p�����𒊏o���邽�߂̐��K�\���p�^�[��
        Match match = Regex.Match(pickName, pattern);// ���K�\���p�^�[���Ɉ�v���锼�p����������
        string pickCardNum = match.Value;
        GameObject pickCard = GameObject.Find("Pick" + pickCardNum);
        return pickCard;
    }

    /// <summary>
    /// �f�b�L�J�[�h�̃f�[�^���s�b�N�J�[�h�ɓ]�ʂ���
    /// </summary>
    /// <param name="deckCard">�f�b�L�̃J�[�h</param>
    /// <param name="pickCard">�s�b�N�����ۂɕ\������J�[�h</param>
    /// <returns>�f�b�L�̃f�[�^��]�ʂ����s�b�N�J�[�h</returns>
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
        //�s�b�N���ꂽ�J�[�h�͎Q�Ƃ���f�b�L�̃J�[�h��cardCost�𔽉f
        pickCard.GetComponent<CardController>().cardDataManager._cardCost = deckData._cardCost;
        //�J�[�h����\������
        pickCard.transform.Find("CardInfo").gameObject.SetActive(true);
        pickCard.GetComponent<CardState>().isActive = true;
        return pickCard;
    }
}
