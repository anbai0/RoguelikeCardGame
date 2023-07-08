using UnityEngine;
using DG.Tweening;

public class BounceOnHover : MonoBehaviour
{
    public RectTransform targetPosition; // 移動先の位置
    public float jumpPower = 1f; // ジャンプの強さ
    public int numJumps = 1; // ジャンプ回数
    public float duration = 1f; // アニメーションの時間


    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            //transform.DOJump(
            //new Vector3(0, 1, 0), // 移動終了地点
            //2f,                    // ジャンプの高さ
            //1,                     // ジャンプの総数
            //0.5f                   // 演出時間
            //);


            // カーソルがオブジェクトに乗ったときに実行される処理
            targetPosition.DOKill(); // 既存のTweenアニメーションをキャンセル
            targetPosition.DOJumpAnchorPos(targetPosition.position, jumpPower, numJumps, duration); // ジャンプしながら指定の位置に移動する

        }
    }


}
