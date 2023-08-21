using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSetting : MonoBehaviour
{
    private GameSettingsJson gameSettingsJson;
    private GameSettings gameSettings;

    [Header("�e���ʒ����X���C�_�[")]
    [SerializeField] private Slider overallVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;

    [Header("���ʂ�\������e�L�X�g")]
    [SerializeField] private TextMeshProUGUI overallVolumeText;
    [SerializeField] private TextMeshProUGUI seVolumeText;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;

    [Header("�ő剹��")]
    [SerializeField] private int maxOverallVolume;
    [SerializeField] private int maxSEVolume;
    [SerializeField] private int maxBGMVolume;

    // �e����
    public float overallVolume { get; private set; }
    public float seVolume { get; private set; }
    public float bgmVolume { get; private set; }

    void Start()
    {
        gameSettingsJson = GetComponent<GameSettingsJson>();
        gameSettings = gameSettingsJson.loadGameSettingsData();     // �Q�[���ݒ�̃��[�h

        // �ő剹�ʐݒ�
        overallVolumeSlider.maxValue = maxOverallVolume;
        seVolumeSlider.maxValue = maxSEVolume;
        bgmVolumeSlider.maxValue = maxBGMVolume;

        // �Q�[���ݒ�̃f�[�^����l���擾
        overallVolume = gameSettings.overallVolume;
        seVolume = gameSettings.seVolume;
        bgmVolume = gameSettings.bgmVolume;

        // �X���C�_�[�̌��݂̉��ʂ�ݒ�
        overallVolumeSlider.value = overallVolume;
        seVolumeSlider.value = seVolume;
        bgmVolumeSlider.value = bgmVolume;

        // ���݂̉��ʕ\��
        overallVolumeText.text = (overallVolume * 100).ToString();
        seVolumeText.text = (seVolume * 100).ToString();
        bgmVolumeText.text = (bgmVolume * 100).ToString();

        // ���X�i�[�o�^
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
        float roundedValue = Mathf.Floor(value * 100) / 100; // �����_���ʂ�؂�̂Ă�
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

        gameSettingsJson.saveGameSettingsData(gameSettings);      // �Q�[���ݒ�̃Z�[�u
    }
}
