using System.Collections;
using UnityEngine;

/// <summary>
/// 戦闘報酬画面の拡大表示・縮小表示を行うスクリプト
/// </summary>
public class DisplayAnimation : MonoBehaviour
{
    [SerializeField, Header("表示用オブジェクト")] GameObject displayObject;
    [SerializeField, Header("表示の倍率")] float enlargeScale = 1.5f;
    [SerializeField, Header("切り替わるまでの時間")] float animationDuration = 1.0f;

    private Vector3 originalScale; //初期スケール
    private float elapsedTime = 0f; // 経過時間

    private void Start()
    {
        // UIテキストの初期スケールを保存
        originalScale = displayObject.transform.localScale;
        // UIテキストを非表示にする
        displayObject.SetActive(false);
    }

    /// <summary>
    /// 表示するオブジェクトを拡大表示する処理
    /// </summary>
    public void StartPopUPAnimation()
    {
        // UIテキストをアクティブにする
        displayObject.SetActive(true);
        // アニメーションを開始
        elapsedTime = 0f;
        StartCoroutine(ScaleUp());
    }

    /// <summary>
    /// 表示するオブジェクトを縮小表示する処理
    /// </summary>
    public void StartDisappearAnimation()
    {
        // 非表示にする
        displayObject.SetActive(false);
    }

    // 拡大アニメーション
    private IEnumerator ScaleUp()
    {
        while (elapsedTime < animationDuration)
        {
            float scale = Mathf.Lerp(originalScale.x, enlargeScale, elapsedTime / animationDuration);
            displayObject.transform.localScale = new Vector3(scale, scale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 目標スケールに達したらアニメーション終了
        displayObject.transform.localScale = new Vector3(enlargeScale, enlargeScale, 1f);
    }
}
