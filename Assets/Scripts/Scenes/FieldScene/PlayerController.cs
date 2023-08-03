using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed; //プレイヤーの動くスピード
    public float rotationSpeed = 10f; //向きを変える速度

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

            animator.SetBool("IsWalking", true); // 歩くアニメーションを再生
        }
        else
        {
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
