using UnityEngine;

/// <summary>
/// �J�[�h�f�[�^�̐����ƕ\�����s���X�N���v�g
/// </summary>
public class CardController : MonoBehaviour
{
    public CardViewManager cardViewManager;// �J�[�h�̌����ڂ̏���
    public CardDataManager cardDataManager;// �J�[�h�̃f�[�^������

    private void Awake()
    {
        cardViewManager = GetComponent<CardViewManager>();
    }

    public void Init(int cardID)// �J�[�h�𐶐��������ɌĂ΂��֐�
    {
        cardDataManager = new CardDataManager(cardID);// �J�[�h�f�[�^�𐶐�
        cardViewManager.ViewCard(cardDataManager);// �J�[�h�f�[�^�̕\��
    }
}
