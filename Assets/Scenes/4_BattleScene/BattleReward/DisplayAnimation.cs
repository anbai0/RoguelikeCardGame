using UnityEngine;

/// <summary>
/// 戦闘報酬画面の拡大表示・縮小表示を行うスクリプト
/// </summary>
public class DisplayAnimation : MonoBehaviour
{
    [SerializeField, Header("表示用オブジェクト")] GameObject displayObject;
    [SerializeField, Header("表示の倍率")] float enlargeScale = 1.5f;
    [SerializeField, Header("切り替わるまでの時間")] float animationDuration = 1.0f;

    private bool isPopUP = true; //true:拡大表示　false:縮小表示
    private Vector3 originalScale;
    private Vector3 zoomScale;
    private bool isAnimating = false;
    private float animationStartTime;

    private void Start()
    {
        originalScale = displayObject.transform.localScale;
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
                if (isPopUP)
                {
                    isAnimating = false;
                }
                else
                {
                    isAnimating = false;
                    displayObject.SetActive(false);
                }

            }
            else
            {
                if (isPopUP)
                {
                    // 徐々に拡大させる
                    float currentScale = Mathf.Lerp(1.0f, enlargeScale, t);
                    displayObject.transform.localScale = originalScale * currentScale;
                }
                else
                {
                    //徐々に縮小させる
                    float currentScale = Mathf.Lerp(1.0f, 0.1f, t);
                    displayObject.transform.localScale = zoomScale * currentScale;
                }

            }
        }
    }

    /// <summary>
    /// 表示するオブジェクトを拡大表示する処理
    /// </summary>
    public void StartPopUPAnimation()
    {
        if (!isAnimating)
        {
            displayObject.SetActive(true);
            displayObject.transform.localScale = originalScale;
            isPopUP = true;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }

    /// <summary>
    /// 表示するオブジェクトを縮小表示する処理
    /// </summary>
    public void StartDisappearAnimation()
    {
        if (!isAnimating)
        {
            zoomScale = displayObject.transform.localScale;
            isPopUP = false;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }
}
