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

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        // �ő剹�ʐݒ�
        overallVolumeSlider.maxValue = maxOverallVolume;
        seVolumeSlider.maxValue = maxSEVolume;
        bgmVolumeSlider.maxValue = maxBGMVolume;

        // ���݂̉��ʂ�ݒ�
        overallVolumeSlider.value = audioManager.overallVolume;
        seVolumeSlider.value = audioManager.seVolume;
        bgmVolumeSlider.value = audioManager.bgmVolume;

        // ���݂̉��ʕ\��
        overallVolumeText.text = audioManager.overallVolume.ToString();
        seVolumeText.text = audioManager.seVolume.ToString();
        bgmVolumeText.text = audioManager.bgmVolume.ToString();

        // ���X�i�[�o�^
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // �����_���ʂ�؂�̂Ă�
        Debug.Log("�S�̂̉��ʃX���C�_�[�̒l���ύX����܂���: " + roundedValue);

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
