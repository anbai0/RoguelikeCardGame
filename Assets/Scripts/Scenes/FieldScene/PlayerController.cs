using SelfMadeNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed; //プレイヤーの動くスピード
    public float rotationSpeed = 10f; //向きを変える速度

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

            animator.SetBool("IsWalking", true); // 歩くアニメーションを再生
        }
        else
        {
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止

            bonfire = collision.gameObject;
            fieldManager.LoadBonfireScene();        // 焚火シーンをロード
        }

        if (collision.gameObject.CompareTag("TreasureBox"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止

            treasureBox = collision.gameObject;
            fieldManager.LoadTreasureBoxScene();   //宝箱シーンをロード
        }

        if (collision.gameObject.CompareTag("Shop"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止

            // 指定した名前のシーンを取得
            Scene sceneToHide = SceneManager.GetSceneByName("ShopScene");

            // シーンが有効で、ロードされていない場合に処理を実行
            if (!(sceneToHide.IsValid() && sceneToHide.isLoaded))
            {
                fieldManager.LoadShopScene();           // ショップシーンをロード
            }

            // ショップシーンがロードされていた場合、ショップシーンのオブジェクトを表示
            fieldManager.ActivateShopScene();
        }

        if (collision.gameObject.CompareTag("SmallEnemy") || collision.gameObject.CompareTag("StrongEnemy"))
        {
            isPlayerActive = false;
            animator.SetBool("IsWalking", false); // 歩くアニメーションを停止

            enemy = collision.gameObject;
            enemyTag = collision.gameObject.tag;
            fieldManager.LoadBattleScene();   //戦闘シーンをロード
        }
    }
}
