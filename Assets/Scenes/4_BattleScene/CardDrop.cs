using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrop : MonoBehaviour,IDropHandler
{
    [SerializeField] EmphasisDropPlace emphasisDropPlace;
    public void OnDrop(PointerEventData eventData)
    {
        //���L�̈ꕶ�́A�����̗����g�p�����UIManagerBattle��OnDrop�ɂ��Ƃ���O�ɏ������I����Ă��܂��ׁA��肪�N���Ȃ��悤�ɂ�����ɂ��������܂����B
        emphasisDropPlace.HiddenGameObject();

        if (eventData.button == PointerEventData.InputButton.Right) return;
        if (eventData.button == PointerEventData.InputButton.Middle) return;
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); // �h���b�O���Ă�����񂩂�CardController���擾
        if (card != null && BattleGameManager.Instance.isPlayerTurn) // �����J�[�h������A�v���C���[�̃^�[���̏ꍇ
        {
            if (card.cardDataManager._cardState == 0)//�J�[�h���g�p�\�ł����
            {
                Debug.Log("�J�[�h�̌��ʔ���");
                BattleGameManager.Instance.PlayerMove(card);
            }
        }
    }
}
