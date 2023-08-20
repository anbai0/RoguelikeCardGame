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

    public float overallVolume { get; private set; } = 1f;
    public float seVolume { get; private set; } = 0.5f;
    public float bgmVolume { get; private set; } = 0.5f;

    private GameSettings gameSettings;
    //private void Start()
    //{


    //    SaveLoadManager.DataSave(gameSettings);
    //}

    void Start()
    {
        gameSettings = SaveLoadManager.DataLoad();      // Jsonロード
        audioManager = GetComponent<AudioManager>();

        if (gameSettings == null)
        {
            Debug.LogError("Loaded game settings are null.");
        }
        else
        {
            Debug.Log("Loaded game settings: " + JsonUtility.ToJson(gameSettings));
            // 以下の代入処理...
        }

        gameSettings.overallVolume = overallVolume;
        gameSettings.seVolume = seVolume;
        gameSettings.bgmVolume = bgmVolume;

        //SaveLoadManager.DataSave(gameSettings);

        // 最大音量設定
        overallVolumeSlider.maxValue = maxOverallVolume;
        seVolumeSlider.maxValue = maxSEVolume;
        bgmVolumeSlider.maxValue = maxBGMVolume;

        

        // 現在の音量を設定
        //overallVolumeSlider.value = gameSettings.overallVolume;
        //seVolumeSlider.value = gameSettings.bgmVolume;
        //bgmVolumeSlider.value = gameSettings.seVolume;

        // 現在の音量表示
        overallVolumeText.text = (overallVolume * 100).ToString();
        seVolumeText.text = (seVolume * 100).ToString();
        bgmVolumeText.text = (bgmVolume * 100).ToString();

        // リスナー登録
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);

        SaveLoadManager.DataSave(gameSettings);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // 小数点第二位を切り捨てる
        Debug.Log("全体の音量スライダーの値が変更されました: " + roundedValue);

        overallVolume = roundedValue;
        overallVolumeText.text = (roundedValue * 100).ToString();
    }

    private void OnSEValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100;
        seVolume = roundedValue;
        seVolumeText.text = (roundedValue * 100).ToString();
    }

    private void OnBGMValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100;
        bgmVolume = roundedValue;
        bgmVolumeText.text = (roundedValue * 100).ToString();
    }
}
