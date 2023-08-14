using SelfMadeNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed; //�v���C���[�̓����X�s�[�h
    public float rotationSpeed = 10f; //������ς��鑬�x

    private Animator animator;

    private float moveHorizontal;
    private float moveVertical;

    public static bool isPlayerActive = true;

    private FieldSceneManager fieldManager;
    public GameObject bonfire { get; private set; }
    public GameObject treasureBox { get; private set; }
    public GameObject enemy { get; private set; }
    public string enemyTag { get; private set; }

    void Start()
    {
        fieldManager = FindObjectOfType<FieldSceneManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerActive)
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            bonfire = collision.gameObject;
            fieldManager.LoadBonfireScene();        // ���΃V�[�������[�h
        }

        if (collision.gameObject.CompareTag("TreasureBox"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            treasureBox = collision.gameObject;
            fieldManager.LoadTreasureBoxScene();   //�󔠃V�[�������[�h
        }

        if (collision.gameObject.CompareTag("Shop"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            // �w�肵�����O�̃V�[�����擾
            Scene sceneToHide = SceneManager.GetSceneByName("ShopScene");

            // �V�[�����L���ŁA���[�h����Ă��Ȃ��ꍇ�ɏ��������s
            if (!(sceneToHide.IsValid() && sceneToHide.isLoaded))
            {
                fieldManager.LoadShopScene();           // �V���b�v�V�[�������[�h
            }

            // �V���b�v�V�[�������[�h����Ă����ꍇ�A�V���b�v�V�[���̃I�u�W�F�N�g��\��
            fieldManager.ActivateShopScene();
        }

        if (collision.gameObject.CompareTag("SmallEnemy") || collision.gameObject.CompareTag("StrongEnemy"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // �����A�j���[�V�������~

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //�퓬�V�[�������[�h
        }
    }
}
