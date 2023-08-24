using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �J�[�h��DropPlace�Ƀh���b�v�������Ƀv���C���[�̍s�����Ăяo���X�N���v�g
/// </summary>
public class CardDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)//�J�[�h�����̃I�u�W�F�N�g�͈̔͂ɗ����Ă����Ƃ��ɍs������
    {
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); // �h���b�O���Ă�����񂩂�CardController���擾
        if (card != null && BattleGameManager.Instance.isPlayerTurn) // �����J�[�h������A�v���C���[�̃^�[���̏ꍇ
        {
            if (card.cardDataManager._cardState == 0)//�J�[�h���g�p�\�ł����
            {
                //�J�[�h�̌��ʂ𔭓�
                BattleGameManager.Instance.PlayerMove(card);
            }
        }
    }
}
