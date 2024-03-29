using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using SelfMadeNamespace;
using System;
using Unity.Mathematics;

// 基本はMain Cameraにアタッチしてください
public class SceneFader : MonoBehaviour
{
    [SerializeField]
    [Header("生成するFadePrefab")]
    private GameObject fadePrefab;

    [SerializeField]
    [Header("FadePrefabのScript")]
    private FadeController fadeManager;

    private GameObject fade;        // DontDestroyされたFadePrefabを格納

    private int fadeTime = 1500;       // ミリ秒

    public static SceneFader Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (!FadeController.isFadeInstance)        // FadePrefabが生成されていない場合生成
        {
            Instantiate(fadePrefab);
        }
        Invoke("findFadeObject", 0.08f);      // 起動時用にCanvasの生成をちょっと待つ
    }

    async void findFadeObject()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");            // Canvasをみつける
        await fade.GetComponent<FadeController>().fadeIn();         // フェードインフラグを立てる
    }


    /// <summary>
    /// シーンのロードとアンロードを行い、フェードイン、アウトをします。
    /// <para>シーンのロード、アンロードを同時に行う場合は、</para>
    /// <code>SceneChange("ロードしたいシーンの名前", "アンロードしたいシーンの名前");</code>
    /// <para>シーンのロードだけしたい場合は、</para>
    /// <code>SceneChange("ロードしたいシーンの名前");</code>
    /// <para>シーンのアンロードだけしたい場合は、</para>
    /// <code>SceneChange(unLoadSceneName: "アンロードしたいシーンの名前");</code>
    /// <para>フェードインが終わった時にプレイヤーを動けるようにしたい場合</para>
    /// <code>allowPlayerMove: true</code>
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <param name="unLoadSceneName"></param>
    public async void SceneChange(string loadSceneName = "None", string unLoadSceneName = "None", bool allowPlayerMove = false)
    {
        fade.SetActive(true);   // パネルが邪魔で消していたのここで表示させています

        FadeController fadeController = fade.GetComponent<FadeController>();

        // フェードアウトしフェードアウトが終わるまで待機
        await fadeController.fadeOut();

        await Task.Delay((int)(fadeTime));       // 一定の長さ暗転させる(ミリ秒単位)

        // ロードシーンが指定されていた場合、シーンをロード
        if (loadSceneName != "None") await LoadSceneAsyncTask(loadSceneName);

        // アンロードシーンが指定されていた場合、シーンをアンロード
        if (unLoadSceneName != "None") await UnLoadSceneAsyncTask(unLoadSceneName);    

        // フェードインしフェードインが終わるまで待機
        await fadeController.fadeIn();

        // フェードイン後にプレイヤーを動けるようにする
        if (allowPlayerMove)
            PlayerController.Instance.isEvents = !allowPlayerMove;
    }


    /// <summary>
    /// 指定したシーンの表示を切り替え、フェードイン、アウトをします。
    /// <para>フェードインが終わった時にプレイヤーを動けるようにしたい場合</para>
    /// <code>allowPlayerMove: true</code>
    /// </summary>
    /// <param name="toggleSceneName"></param>
    /// <param name="isSceneActive"></param>
    public async void ToggleSceneWithFade(string toggleSceneName, bool isSceneActive, bool allowPlayerMove = false)
    {
        fade.SetActive(true);     // パネルが邪魔で消していたのここで表示させています

        FadeController fadeController = fade.GetComponent<FadeController>();

        // フェードアウトしフェードアウトが終わるまで待機
        await fadeController.fadeOut();

        await Task.Delay(fadeTime);    // 一定の長さ暗転させる(ミリ秒単位)

        toggleSceneName.ToggleSceneDisplay(isSceneActive);      // シーンの表示切替

        // フェードインしフェードインが終わるまで待機
        await fadeController.fadeIn();

        // フェードイン後にプレイヤーを動けるようにする
        if (allowPlayerMove)
            PlayerController.Instance.isEvents = !allowPlayerMove;
    }

    /// <summary>
    /// すべてのシーンをアンロードするときに使います。
    /// メソッドを受け取りフェードイン、アウトをします。
    /// </summary>
    public async void FadeOutInWrapper(Func<Task> method, bool allowPlayerMove = false)
    {
        fade.SetActive(true);     // パネルが邪魔で消していたのここで表示させています

        FadeController fadeController = fade.GetComponent<FadeController>();

        // フェードアウトしフェードアウトが終わるまで待機
        await fadeController.fadeOut();

        await Task.Delay(fadeTime);    // 一定の長さ暗転させる(ミリ秒単位)

        await method?.Invoke();

        // フェードインしフェードインが終わるまで待機
        await fadeController.fadeIn();

        // フェードイン後にプレイヤーを動けるようにする
        if (allowPlayerMove)
            PlayerController.Instance.isEvents = !allowPlayerMove;
    }

    private async Task LoadSceneAsyncTask(string loadSceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone) await Task.Yield();
    }

    private async Task UnLoadSceneAsyncTask(string unLoadSceneName)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(unLoadSceneName);
        while (!asyncOperation.isDone) await Task.Yield();

        Resources.UnloadUnusedAssets();
    }



}