using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    void Update()
    {
        // �v���C���[�̈ړ�
        float moveHorizontal = Input.GetAxisRaw("Horizontal");      //�R���g���[���Ȃǂ̏ꍇ��GetAxis�ŏ���
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;    //���K��
        transform.Translate(movement * speed * Time.deltaTime);
    }
}