using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AudioSetting : MonoBehaviour
{
    private AudioManager audioManager;

    [Header("各音量調整スライダー")]
    [SerializeField] private Slider overallVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;

    [Header("音量を表示するテキスト")]
    [SerializeField] private TextMeshProUGUI overallVolumeText;
    [SerializeField] private TextMeshProUGUI seVolumeText;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;

    [Header("最大音量")]
    [SerializeField] private int maxOverallVolume;
    [SerializeField] private int maxSEVolume;
    [SerializeField] private int maxBGMVolume;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        // 最大音量設定
        overallVolumeSlider.maxValue = maxOverallVolume;
        seVolumeSlider.maxValue = maxSEVolume;
        bgmVolumeSlider.maxValue = maxBGMVolume;

        // 現在の音量を設定
        overallVolumeSlider.value = audioManager.overallVolume;
        seVolumeSlider.value = audioManager.seVolume;
        bgmVolumeSlider.value = audioManager.bgmVolume;

        // 現在の音量表示
        overallVolumeText.text = audioManager.overallVolume.ToString();
        seVolumeText.text = audioManager.seVolume.ToString();
        bgmVolumeText.text = audioManager.bgmVolume.ToString();

        // リスナー登録
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // 小数点第二位を切り捨てる
        Debug.Log("全体の音量スライダーの値が変更されました: " + roundedValue);

        audioManager.overallVolume = roundedValue;
        overallVolumeText.text = roundedValue.ToString();
    }

    private void OnSEValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100;
        audioManager.seVolume = roundedValue;
        seVolumeText.text = roundedValue.ToString();
    }

    private void OnBGMValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100;
        audioManager.bgmVolume = roundedValue;
        bgmVolumeText.text = roundedValue.ToString();
    }
}
