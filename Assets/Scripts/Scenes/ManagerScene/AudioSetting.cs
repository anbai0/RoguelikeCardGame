using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class AudioSetting : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] private Slider overallVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;

    [SerializeField] TextMeshProUGUI overallVolumeText;
    [SerializeField] TextMeshProUGUI seVolumeText;
    [SerializeField] TextMeshProUGUI bgmVolumeText;

    private float maxVolume = 1f;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        // 最大音量設定
        overallVolumeSlider.maxValue = maxVolume;
        seVolumeSlider.maxValue = maxVolume;
        bgmVolumeSlider.maxValue = maxVolume;

        // 現在の音量を設定
        overallVolumeSlider.value = audioManager.overallVolume;
        seVolumeSlider.value = audioManager.SEVolume;
        bgmVolumeSlider.value = audioManager.BGMVolume;

        // 現在の音量表示
        overallVolumeText.text = audioManager.overallVolume.ToString();
        seVolumeText.text = audioManager.SEVolume.ToString();
        bgmVolumeText.text = audioManager.BGMVolume.ToString();

        // リスナー登録
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // 小数点第二位を切り捨てる
        Debug.Log("全体の音量スライダーの値が変更されました: " + roundedValue);
        overallVolumeText.text = roundedValue.ToString();
        audioManager.overallVolume = roundedValue;
    }

    private void OnSEValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // 小数点第二位を切り捨てる
        Debug.Log("SEの音量スライダーの値が変更されました: " + roundedValue);
        seVolumeText.text = roundedValue.ToString();
        audioManager.SEVolume = roundedValue;
    }

    private void OnBGMValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // 小数点第二位を切り捨てる
        Debug.Log("BGMの音量スライダーの値が変更されました: " + roundedValue);
        bgmVolumeText.text = roundedValue.ToString();
        audioManager.BGMVolume = roundedValue;
    }
}
