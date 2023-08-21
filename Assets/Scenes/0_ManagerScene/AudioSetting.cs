using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSetting : MonoBehaviour
{
    private GameSettingsJson gameSettingsJson;
    private GameSettings gameSettings;

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

    // 各音量
    public float overallVolume { get; private set; }
    public float seVolume { get; private set; }
    public float bgmVolume { get; private set; }

    void Start()
    {
        gameSettingsJson = GetComponent<GameSettingsJson>();
        gameSettings = gameSettingsJson.loadGameSettingsData();     // ゲーム設定のロード

        // 最大音量設定
        overallVolumeSlider.maxValue = maxOverallVolume;
        seVolumeSlider.maxValue = maxSEVolume;
        bgmVolumeSlider.maxValue = maxBGMVolume;

        // ゲーム設定のデータから値を取得
        overallVolume = gameSettings.overallVolume;
        seVolume = gameSettings.seVolume;
        bgmVolume = gameSettings.bgmVolume;

        // スライダーの現在の音量を設定
        overallVolumeSlider.value = overallVolume;
        seVolumeSlider.value = seVolume;
        bgmVolumeSlider.value = bgmVolume;

        // 現在の音量表示
        overallVolumeText.text = (overallVolume * 100).ToString();
        seVolumeText.text = (seVolume * 100).ToString();
        bgmVolumeText.text = (bgmVolume * 100).ToString();

        // リスナー登録
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);
    }

    private void Update()
    {
        Debug.Log(overallVolume);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // 小数点第二位を切り捨てる
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

    public void SaveAudioSetting()
    {
        gameSettings.overallVolume = overallVolume;
        gameSettings.seVolume = seVolume;
        gameSettings.bgmVolume = bgmVolume;

        gameSettingsJson.saveGameSettingsData(gameSettings);      // ゲーム設定のセーブ
    }
}
