using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AudioSetting : MonoBehaviour
{
    private AudioManager audioManager;

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
        gameSettings = SaveLoadManager.DataLoad();      // Json���[�h
        audioManager = GetComponent<AudioManager>();

        if (gameSettings == null)
        {
            Debug.LogError("Loaded game settings are null.");
        }
        else
        {
            Debug.Log("Loaded game settings: " + JsonUtility.ToJson(gameSettings));
            // �ȉ��̑������...
        }

        gameSettings.overallVolume = overallVolume;
        gameSettings.seVolume = seVolume;
        gameSettings.bgmVolume = bgmVolume;

        //SaveLoadManager.DataSave(gameSettings);

        // �ő剹�ʐݒ�
        overallVolumeSlider.maxValue = maxOverallVolume;
        seVolumeSlider.maxValue = maxSEVolume;
        bgmVolumeSlider.maxValue = maxBGMVolume;

        

        // ���݂̉��ʂ�ݒ�
        //overallVolumeSlider.value = gameSettings.overallVolume;
        //seVolumeSlider.value = gameSettings.bgmVolume;
        //bgmVolumeSlider.value = gameSettings.seVolume;

        // ���݂̉��ʕ\��
        overallVolumeText.text = (overallVolume * 100).ToString();
        seVolumeText.text = (seVolume * 100).ToString();
        bgmVolumeText.text = (bgmVolume * 100).ToString();

        // ���X�i�[�o�^
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);

        SaveLoadManager.DataSave(gameSettings);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // �����_���ʂ�؂�̂Ă�
        Debug.Log("�S�̂̉��ʃX���C�_�[�̒l���ύX����܂���: " + roundedValue);

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
