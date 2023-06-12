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

    public async void sceneChange(string sceneName)//ボタン操作などで呼び出す
    {
        fadeCanvas.SetActive(true);     //パネルが邪魔で消していたのここで表示させています
        fadeCanvas.GetComponent<FadeManager>().fadeOut();//フェードアウトフラグを立てる
        await Task.Delay(fadeDelay * 100);//暗転するまで待つ
        SceneManager.LoadScene(sceneName);//シーンチェンジ
    }
}