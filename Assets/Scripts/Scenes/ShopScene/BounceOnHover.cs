using UnityEngine;
using DG.Tweening;

public class BounceOnHover : MonoBehaviour
{
    public RectTransform targetPosition; // �ړ���̈ʒu
    public float jumpPower = 1f; // �W�����v�̋���
    public int numJumps = 1; // �W�����v��
    public float duration = 1f; // �A�j���[�V�����̎���


    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            //transform.DOJump(
            //new Vector3(0, 1, 0), // �ړ��I���n�_
            //2f,                    // �W�����v�̍���
            //1,                     // �W�����v�̑���
            //0.5f                   // ���o����
            //);


            // �J�[�\�����I�u�W�F�N�g�ɏ�����Ƃ��Ɏ��s����鏈��
            targetPosition.DOKill(); // ������Tween�A�j���[�V�������L�����Z��
            targetPosition.DOJumpAnchorPos(targetPosition.position, jumpPower, numJumps, duration); // �W�����v���Ȃ���w��̈ʒu�Ɉړ�����

        }
    }


}
