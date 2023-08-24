using UnityEngine;

/// <summary>
/// �g�p�s�̃J�[�h���Â�����X�N���v�g
/// </summary>
public class CardState : MonoBehaviour
{
    CardDataManager cardDataManager;
    GameObject blindPanel;
    public bool isActive = true;

    void Start()
    {
        isActive = true;
        cardDataManager = GetComponent<CardController>().cardDataManager;
        blindPanel = gameObject.transform.Find("BlindPanel").gameObject;
        blindPanel.SetActive(false);
    }

    void Update()
    {
        if (isActive == true && cardDataManager._cardState != 0)//�J�[�h���\������Ă��ăX�e�[�g��0�ȊO�̏ꍇ
        {
            //�J�[�h���Â�����p�l����\������
            blindPanel.SetActive(true);
        }
        else
        {
            //�J�[�h���Â�����p�l�����\���ɂ���
            blindPanel.SetActive(false);
        }
    }
}
