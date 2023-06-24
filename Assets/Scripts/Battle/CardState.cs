using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardState : MonoBehaviour
{
    CardDataManager cardDataManager;
    GameObject hiddenPanel;
    // Start is called before the first frame update
    void Start()
    {
        cardDataManager = GetComponent<CardController>().cardDataManager;
        hiddenPanel = gameObject.transform.GetChild(5).gameObject;
        hiddenPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cardDataManager._cardState != 0)//�J�[�h���g�p�s�ȏꍇ
        {
            //�J�[�h���Â�����p�l����\������
            hiddenPanel.SetActive(true);
        }
        else 
        {
            //�J�[�h���Â�����p�l�����\���ɂ���
            hiddenPanel.SetActive(false);
        }
    }
}
