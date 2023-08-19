using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultAnimation : MonoBehaviour
{
    [Header("勝敗演出用オブジェクト")]
    [SerializeField]
    GameObject resultPrefab;
    [Header("勝敗演出の表示場所")]
    [SerializeField]
    Transform resultPlace;

    GameObject resultObj = null;
    TextMeshProUGUI resultText; //勝敗の表示を行うテキスト
    public float enlargeScale = 1.5f;
    public float animationDuration = 1.0f;

    private Vector3 originalScale;
    private bool isAnimating = false;
    private float animationStartTime;

    private void Start()
    {
        originalScale = resultPrefab.transform.localScale;
    }

    private void Update()
    {
        if (isAnimating)
        {
            float timeSinceStart = Time.time - animationStartTime;
            float t = Mathf.Clamp01(timeSinceStart / animationDuration);

            // アニメーションが終了
            if (t >= 1.0f)
            {
                isAnimating = false;
            }
            else
            {
                // 拡大表示から元の大きさへ徐々に変化させる
                float currentScale = Mathf.Lerp(1.0f, enlargeScale, t);
                resultObj.transform.localScale = originalScale * currentScale;
            }
        }
    }

    /// <summary>
    /// 勝敗テキストの表示
    /// </summary>
    /// <param name="vicdef">勝ちならVictory,負けならDefeatedの文字列を引数に代入</param>
    public void StartAnimation(string vicdef)
    {
        if (!isAnimating)
        {
            resultObj = Instantiate(resultPrefab, resultPlace);
            resultObj.transform.SetParent(resultPlace);
            resultText = resultObj.GetComponent<TextMeshProUGUI>();
            resultText.text = vicdef;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }

    /// <summary>
    /// 勝敗テキストを消す処理
    /// </summary>
    public void DisappearResult()
    {
        Destroy(resultObj);
    }
}
