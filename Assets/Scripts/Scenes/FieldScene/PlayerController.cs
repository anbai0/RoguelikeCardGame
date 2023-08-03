using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed; //�v���C���[�̓����X�s�[�h
    public float rotationSpeed = 10f; //������ς��鑬�x

    private Animator animator;

    [SerializeField] float moveHorizontal;
    [SerializeField] float moveVertical;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;


        if (movement.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("IsWalking", true); // �����A�j���[�V�������Đ�
        }
        else
        {
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
