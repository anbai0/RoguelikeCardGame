using UnityEngine;
using TMPro;

public class RoundTextAnimation : MonoBehaviour
{
    [Header("ラウンド数表示用オブジェクト")]
    [SerializeField]
    GameObject roundPrefab;
    [Header("ラウンド数の表示場所")]
    [SerializeField]
    Transform roundPlace;

    GameObject roundObj = null;
    TextMeshProUGUI roundText; //ラウンド数をテキスト
    public float enlargeScale = 1.5f;
    public float animationDuration = 1.0f;
    private Vector3 originalScale;
    private bool isAnimating = false;
    private float animationStartTime;

    void Start()
    {
        originalScale = roundPrefab.transform.localScale;
    }

    void Update()
    {
        if (isAnimating)
        {
            float timeSinceStart = Time.time - animationStartTime;
            float t = Mathf.Clamp01(timeSinceStart / animationDuration);

            // アニメーションが終了
            if (t >= 1.0f)
            {
                isAnimating = false;
                Invoke("DisappearRoundText", 0.5f);
            }
            else
            {
                // 拡大表示から元の大きさへ徐々に変化させる
                float currentScale = Mathf.Lerp(1.0f, enlargeScale, t);
                roundObj.transform.localScale = originalScale * currentScale;
            }
        }
    }

    /// <summary>
    /// ラウンドテキストの表示
    /// </summary>
    /// <param name="roundCount">表示したいラウンド数のテキスト</param>
    public void StartAnimation(string roundCount)
    {
        if (!isAnimating)
        {
            roundObj = Instantiate(roundPrefab, roundPlace);
            roundObj.transform.SetParent(roundPlace);
            roundText = roundObj.GetComponent<TextMeshProUGUI>();
            roundText.text = roundCount;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }

    /// <summary>
    /// ラウンドテキストを消す処理
    /// </summary>
    public void DisappearRoundText()
    {
        Destroy(roundObj);
    }
}
