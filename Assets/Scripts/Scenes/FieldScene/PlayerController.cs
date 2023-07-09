using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    void Update()
    {
        // プレイヤーの移動
        float moveHorizontal = Input.GetAxisRaw("Horizontal");      //コントローラなどの場合はGetAxisで書く
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;    //正規化
        transform.Translate(movement * speed * Time.deltaTime);
    }
}