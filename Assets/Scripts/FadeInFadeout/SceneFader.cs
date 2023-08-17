using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using SelfMadeNamespace;
using System;

// 基本はMain Cameraにアタッチしてください
public class SceneFader : MonoBehaviour
{
    [SerializeField]
    [Header("生成するFadePrefab")]
    private GameObject fadePrefab;

    [SerializeField]
    [Header("FadePrefabのScript")]
    private FadeEffectController fadeManager;

    private GameObject fade;        // DontDestroyされたFadePrefabを格納

    private float fadeDelay;
    private float fadeDuration;

    void Start()
    {
        fadeDelay = fadeManager.fadeSpeed;
        fadeDuration = fadeManager.fadeDurationMultiplier;

        if (!FadeEffectController.isFadeInstance && gameObject.scene.name == "ManagerScene")        // FadePrefabが生成されていない場合生成
        {
            Instantiate(fadePrefab);
        }

        Invoke("findFadeObject", fadeManager.fadeSpeed / 10f);      // 起動時用にCanvasの生成をちょっと待つ
    }

    void findFadeObject()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");            // Canvasをみつける
        fade.GetComponent<FadeEffectController>().fadeIn();         // フェードインフラグを立てる
    }


    /// <summary>
    /// シーンのロードとアンロードを行い、フェードイン、アウトをします。
    /// <para>シーンのロード、アンロードを同時に行う場合は、</para>
    /// <code>SceneChange("ロードしたいシーンの名前", "アンロードしたいシーンの名前");</code>
    /// <para>シーンのロードだけしたい場合は、</para>
    /// <code>SceneChange("ロードしたいシーンの名前");</code>
    /// <para>シーンのアンロードだけしたい場合は、</para>
    /// <code>SceneChange(unLoadSceneName: "アンロードしたいシーンの名前");</code>
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <param name="unLoadSceneName"></param>
    public async void SceneChange(string loadSceneName = "None", string unLoadSceneName = "None")
    {
        fade.SetActive(true);     // パネルが邪魔で消していたのここで表示させています

        fade.GetComponent<FadeEffectController>().fadeOut();         // フェードアウト
        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // 暗転するまで待つ(ミリ秒単位)

        //if (loadSceneName != "None") SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);    // シーンロード
        //if (unLoadSceneName != "None") StartCoroutine(CoUnload(unLoadSceneName));                           // アンロード

        if (loadSceneName != "None")
            await LoadSceneAsync(loadSceneName);

        if (unLoadSceneName != "None")
            StartCoroutine(CoUnload(unLoadSceneName));

        // アンロードだけ行った場合、またはシーン名が指定されなかった場合
        if (loadSceneName == "None" && (unLoadSceneName != "None" || unLoadSceneName == "None"))
            fade.GetComponent<FadeEffectController>().fadeIn();      // フェードイン
    }

    /// <summary>
    /// 指定したシーンの表示を切り替え、フェードイン、アウトをします。
    /// </summary>
    /// <param name="toggleSceneName"></param>
    /// <param name="isSceneActive"></param>
    public async void ToggleSceneWithFade(string toggleSceneName, bool isSceneActive)
    {
        fade.SetActive(true);     // パネルが邪魔で消していたのここで表示させています

        fade.GetComponent<FadeEffectController>().fadeOut();         // フェードアウト
        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // 暗転するまで待つ(ミリ秒単位)

        toggleSceneName.ToggleSceneDisplay(isSceneActive);      // シーンの表示切替

        fade.GetComponent<FadeEffectController>().fadeIn();      // フェードイン
    }


    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// フェードは行わず、
    /// stringで指定されたシーンをアンロードします。
    /// </summary>
    public void UnLoadScene(string unLoadSceneName)
    {
        StartCoroutine(CoUnload(unLoadSceneName));
    }

    IEnumerator CoUnload(string unLoadSceneName)
    {
        // 指定したシーンをアンロード
        var op = SceneManager.UnloadSceneAsync(unLoadSceneName);
        yield return op;

        // アンロード後の処理を書く

        // 必要に応じて不使用アセットをアンロードしてメモリを解放する
        yield return Resources.UnloadUnusedAssets();
    }
}