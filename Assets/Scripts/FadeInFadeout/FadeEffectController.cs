using UnityEngine;
using UnityEngine.UI;

// Fadeという名前のPrefabにアタッチしてください
public class FadeEffectController : MonoBehaviour
{
    [Header("フェードに掛かる時間")]
    public float fadeSpeed = 0.7f;
    [Header("fadeSpeedの何倍分暗転させ続けるか")]
    public float fadeDurationMultiplier = 1.2f;

    public static bool isFadeInstance = false;      // シングルトンで生成されたか
    private bool isFadeIn = false;                  // フェードインするフラグ
    private bool isFadeOut = false;                 // フェードアウトするフラグ

    private float alpha = 0.0f;     // 透過率
    private Image fadeImage;

    void Start()
    {
        // 生成時にシングルトンを使って複製されないようにする
        if (!isFadeInstance)
        {
            DontDestroyOnLoad(this);
            isFadeInstance = true;
        }
        else
        {
            Destroy(this);
        }

        fadeImage = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (isFadeIn)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            if (alpha <= 0.0f)      // 透明になったら、フェードインを終了
            {
                isFadeIn = false;
                alpha = 0.0f;
                gameObject.SetActive(false);    //パネルが邪魔になるので一時的に消しています
            }
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        else if (isFadeOut)
        {
            alpha += Time.deltaTime / fadeSpeed;
            if (alpha >= 1.0f)      // 真っ黒になったら、フェードアウトを終了
            {
                isFadeOut = false;
                alpha = 1.0f;
            }
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

    public void fadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
    }

    public void fadeOut()
    {
        isFadeOut = true;
        isFadeIn = false;
    }
}