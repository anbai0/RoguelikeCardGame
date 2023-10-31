using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DemoPlayer : MonoBehaviour
{
    [SerializeField] private GameObject demoDisplay;
    [SerializeField] private VideoPlayer demoVideoPlayer;

    [SerializeField,Header("デモまでの時間(秒)")] private float demoVideoDuration = 60f;
    private float elapsedTime = 0f;
    private bool isDemoVideoPlaying;

    // Start is called before the first frame update
    void Start()
    {
        demoDisplay.SetActive(false);

        //VideoPlayerの設定
        demoVideoPlayer.isLooping = false;
        demoVideoPlayer.Stop();

        isDemoVideoPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDemoVideoPlaying)
        {
            Debug.Log(elapsedTime.ToString());
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= demoVideoDuration)
            {
                //画面を表示
                demoDisplay.SetActive(true);  
                PlayDemoVideo();
            }
        }

        if (demoVideoPlayer.isPlaying)
        {
            elapsedTime = 0;
        }
        else
        {
            //画面を非表示
            demoDisplay.SetActive(false);
            isDemoVideoPlaying = false;
        }
    }

    void PlayDemoVideo()
    {
        isDemoVideoPlaying = true;

        // デモ動画を再生
        demoVideoPlayer.Play();
    }

    public void StopDemoVideo()
    {
        isDemoVideoPlaying = false;

        // デモ動画を停止
        demoVideoPlayer.Stop();
        
        elapsedTime = 0;
        demoDisplay.SetActive(false);
    }
}
