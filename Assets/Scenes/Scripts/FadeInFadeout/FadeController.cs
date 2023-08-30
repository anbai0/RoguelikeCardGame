using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

// Fadeという名前のPrefabにアタッチしてください
public class FadeController : MonoBehaviour
{
    [Header("フェードに掛かる時間")]
    public float fadeSpeed = 0.7f;
    [Header("fadeSpeedの何倍分暗転させ続けるか")]
    public float fadeDurationMultiplier = 1.2f;

    public static bool isFadeInstance = false;      // シングルトンで生成されたか
    public static bool isFadeIn = false;                  // フェードインするフラグ
    public static bool isFadeOut = false;                 // フェードアウトするフラグ

    private float alpha = 0.0f;     // 透過率
    private Image fadeImage;

    private bool fadeInDone = false;
    private bool fadeOutDone = false;

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
                fadeInDone = true;
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
                fadeOutDone = true;
            }
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

    public async Task fadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
        while (!fadeInDone) await Task.Yield(); // 同期メソッドの実行を一時的に中断
        fadeInDone = false;
    }

    public async Task fadeOut()
    {
        isFadeOut = true;
        isFadeIn = false;
        while (!fadeOutDone) await Task.Yield();
        fadeOutDone = false;
    }
}