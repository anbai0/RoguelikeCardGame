using UnityEngine;

/// <summary>
/// �n���h���[�ɂ��J�[�h�̋������܂Ƃ߂��X�N���v�g
/// </summary>
public class CardMovement : MonoBehaviour
{
    [SerializeField] UIManagerBattle uIManagerBattle;
    
    PickCard pickCardScript;
    GameObject pickCard = null;

    public Transform cardParent;
    bool isInitialize = false;
    
    private void Start()
    {
        uIManagerBattle = FindObjectOfType<UIManagerBattle>();
        pickCardScript = GetComponent<PickCard>();
    }

    private void Update()
    {
        if (!isInitialize)
        {
            uIManagerBattle.UIEventsReload();
            isInitialize = true;
        }
    }

    public void CardBeginDrag(GameObject Card)
    {
        cardParent = transform.parent;                          // �J�[�h�̐e���擾
        transform.SetParent(cardParent.parent, false);          // �J�[�h�̐e���甲����

        if (pickCard.transform.Find("CardInfo").gameObject.activeSelf)
        {
            pickCard.transform.Find("CardInfo").gameObject.SetActive(false);
            pickCard.GetComponent<CardState>().isActive = false;
        }
    }

    public void CardDorp(GameObject Card)
    {
        transform.SetParent(cardParent, false);
        GameObject.Find("CardPlace").GetComponent<SortName>().Sort();       // ���O���Ƀ\�[�g������
        GameObject.Find("PickCardPlace").GetComponent<SortName>().Sort();
    }

    public void CardEnter(GameObject Card)
    {
        pickCard = pickCardScript.ChoosePickCard(this.gameObject);
        pickCard = pickCardScript.SetPickCardStatus(this.gameObject, pickCard);
    }
    public void CardExit(GameObject Card)
    {
        if (pickCard.transform.Find("CardInfo").gameObject.activeSelf)
        {
            pickCard.transform.Find("CardInfo").gameObject.SetActive(false);
            pickCard.GetComponent<CardState>().isActive = false;
        }
    }
}
