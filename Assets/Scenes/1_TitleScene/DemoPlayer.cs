using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DemoPlayer : MonoBehaviour
{
    [SerializeField] private GameObject demoDisplay;
    [SerializeField] private VideoPlayer demoVideoPlayer;

    [SerializeField,Header("�f���܂ł̎���(�b)")] private float demoVideoDuration = 60f;
    private float elapsedTime = 0f;
    private bool isDemoVideoPlaying;

    // Start is called before the first frame update
    void Start()
    {
        demoDisplay.SetActive(false);

        //VideoPlayer�̐ݒ�
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
                //��ʂ�\��
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
            //��ʂ��\��
            demoDisplay.SetActive(false);
            isDemoVideoPlaying = false;
        }
    }

    void PlayDemoVideo()
    {
        isDemoVideoPlaying = true;

        // �f��������Đ�
        demoVideoPlayer.Play();
    }

    public void StopDemoVideo()
    {
        isDemoVideoPlaying = false;

        // �f��������~
        demoVideoPlayer.Stop();
        
        elapsedTime = 0;
        demoDisplay.SetActive(false);
    }
}
