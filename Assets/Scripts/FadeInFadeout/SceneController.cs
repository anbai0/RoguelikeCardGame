using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;//追加
using UnityEngine.SceneManagement;//追加
using System.Security.Cryptography.X509Certificates;

//基本はMain Cameraにアタッチしてください
public class SceneController : MonoBehaviour
{
    public GameObject fade;//インスペクタからPrefab化したFadeを入れる
    private GameObject fadeCanvas;//操作するCanvas、タグで探す

    private int fadeDelay;

    [SerializeField]
    private FadeManager fadeManager;

    void Start()
    {
        float tmp = fadeManager.FadeSpeed * 10; //intに変換するために10倍にしてます
        fadeDelay = (int)tmp;
        if (!FadeManager.isFadeInstance)
        {
            Instantiate(fade);
        }

        Invoke("findFadeObject", fadeDelay / 100f);//起動時用にCanvasの召喚をちょっと待つ
    }

    void findFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");//Canvasをみつける
        fadeCanvas.GetComponent<FadeManager>().fadeIn();//フェードインフラグを立てる
    }


    /// <summary>
    /// シーンのロードとアンロードを行います。
    /// <para>シーンのロード、アンロードを同時に行う場合は、</para>
    /// <code>SceneChange("ロードしたいシーンの名前", "アンロードしたいシーンの名前");</code>
    /// <para>シーンのロードだけしたい場合は、</para>
    /// <code>SceneChange("ロードしたいシーンの名前");</code>
    /// <para>シーンのアンロードだけしたい場合は、</para>
    /// <code>SceneChange(unLoadSceneName: "アンロードしたいシーンの名前");</code>
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <param name="unLoadSceneName"></param>
    public async void SceneChange(string loadSceneName = "None", string unLoadSceneName = "None")//ボタン操作などで呼び出す
    {
        fadeCanvas.SetActive(true);     //パネルが邪魔で消していたのここで表示させています
        fadeCanvas.GetComponent<FadeManager>().fadeOut();//フェードアウトフラグを立てる
        await Task.Delay(fadeDelay * 100);//暗転するまで待つ
        if (loadSceneName != "None") SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);    //シーンロード
        if (unLoadSceneName != "None") StartCoroutine(CoUnload(unLoadSceneName));                           //アンロード
    }

    IEnumerator CoUnload(string unLoadSceneName)
    {
        //指定したシーンをアンロード
        var op = SceneManager.UnloadSceneAsync(unLoadSceneName);
        yield return op;

        //アンロード後の処理を書く

        //必要に応じて不使用アセットをアンロードしてメモリを解放する
        //けっこう重い処理なので、別に管理するのも手
        yield return Resources.UnloadUnusedAssets();
    }
}