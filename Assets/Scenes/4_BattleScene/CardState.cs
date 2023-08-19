using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardState : MonoBehaviour
{
    CardDataManager cardDataManager;
    GameObject blindPanel;
    public bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        cardDataManager = GetComponent<CardController>().cardDataManager;
        blindPanel = gameObject.transform.Find("BlindPanel").gameObject;
        blindPanel.SetActive(false);
    }

    // Update is called once per frame
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
